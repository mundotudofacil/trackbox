using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Importações
using System.IO.Ports;
using System.Windows;
using System.Diagnostics;
using System.Globalization;



namespace TrackBoxTeste01
{
    public partial class Form1 : Form
    {
        bool isButton0Pressed = false;
        bool isButton1Pressed = false;
        bool isButton2Pressed = false;
        bool isButton3Pressed = false;

        public Form1()
        {
            InitializeComponent();
            _timer.Tick += _timer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitCube();
            _timer.Start();
        }



        #region "Gráfico"



        private Pen[] _pen = { new Pen(Color.Red, 2), new Pen(Color.Blue, 2), new Pen(Color.Green, 2) };
        private Timer _timer = new Timer() { Interval = 33 };
        private Point3D[] _vertices;
        private int[,] _faces;
        private int _angle;
        private int _angleX;
        private int _angleY;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.Black);

            var projected = new Point3D[8];
            //private Pen _penY = new Pen(Color.Blue, 2);
            //private Pen _penZ = new Pen(Color.Red, 2);


            for (var i = 0; i < _vertices.Length; i++)
            {
                var vertex = _vertices[i];
                
                var transformed = vertex.RotateX(-_angleX).RotateY(30).RotateZ(_angleY);
                //var transformed = vertex.RotateX(_angle).RotateY(_angle).RotateZ(_angle);
                projected[i] = transformed.Project(panel1.ClientSize.Width, panel1.ClientSize.Height, 256, 4);
            }
            for (var j = 0; j < 3; j++)
            {
                e.Graphics.DrawLine(_pen[j],
                    (int)projected[_faces[j, 0]].X,
                    (int)projected[_faces[j, 0]].Y,
                    (int)projected[_faces[j, 1]].X,
                    (int)projected[_faces[j, 1]].Y);
            }
        }


        private void InitCube()
        {
            _vertices = new Point3D[]
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(0, -1, 0),
                new Point3D(0, 0, 1)
            };

            _faces = new int[,]
            {

                { 0, 1 },
                { 0, 2 },
                { 0, 3 }

                //{ 0, 1, 2, 3 },
                //{ 1, 5, 6, 2 },
                //{ 5, 4, 7, 6 },
                //{ 4, 0, 3, 7 },
                //{ 0, 4, 5, 1 },
                //{ 3, 2, 6, 7 }
            };
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            panel1.Invalidate();
            _angle++;
        }

        #endregion

        private void btoIniciar_Click(object sender, EventArgs e)
        {
            abrirPortaSerial();
        }



        SerialPort _serialPort;
        void abrirPortaSerial()
        {

            if (_serialPort == null)
            {
                _serialPort = new SerialPort();
            }
            if (_serialPort.IsOpen == false)
            {
                try
                {

                    //Abre a porta serial
                    _serialPort.PortName = "COM9";
                    _serialPort.BaudRate = 115200;
                    _serialPort.DataBits = 8;
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    _serialPort.Open();
                }
                catch
                {
                    MessageBox.Show("Erro ao abrir porta seria.");
                }
            }
            else
            {
                MessageBox.Show("Porta seria já está aberta.");
            }

        }


        //Evendo de chegada de informação na porta serial
        static string _message;
        string _message_buffer;
        string _msgOK_old = "0,0,0,0,0,0,0,0,0";
        Point currentMousePoint;
        Point currentIMUPoint;
        //KalmanFilter2D filter2D = new KalmanFilter2D();
        double dt;
        System.DateTime timeOld = System.DateTime.Now;
        double[] phiG = { 0, 0, 0, 0, 0, 0 };
        double[] chiG = { 0, 0, 0, 0, 0, 0 };
        double[] psiG = { 0, 0, 0, 0, 0, 0 };
        double[] phi = { 0, 0, 0, 0, 0, 0 };
        double[] chi = { 0, 0, 0, 0, 0, 0 };
        double[] psi = { 0, 0, 0, 0, 0, 0 };
        Point pt2_old;
        bool isMouseLeftPressed;
        bool isMouseRightPressed;
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            

            try
            {
                string _msgOK = "0,0,0,0,0,0,0,0,0,0,0";
                string[] _msgPices;
                _message = _serialPort.ReadExisting();

                _message_buffer += _message;

                _msgPices = _message_buffer.Split(';');

                int iMsgOk = _msgPices.Length - 2;
                if (iMsgOk >= 0)
                {
                    _msgOK = _msgPices[iMsgOk];
                    _msgOK_old = _msgOK;

                    _message_buffer = "";
                    for (int i = iMsgOk + 1; i < _msgPices.Length - 1; i++)
                    {
                        _message_buffer += _msgPices[i] + ";";
                    }
                    _message_buffer += _msgPices[_msgPices.Length - 1];
                }
                else
                {
                    _msgOK = _msgOK_old;
                }

            //txtRead.Text += _msgOK + "\n";
            //Debug.WriteLine(_msgOK);
            //if (txtRead.Text.Length > 2000) txtRead.Text = txtRead.Text.Substring(txtRead.Text.Length - 1000);
            
            string[] strMsg = _msgOK.Split(',');
                IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
                if (strMsg.Length == 11) // Número de variáveis
                {
                    int accX = int.Parse(strMsg[0]);
                    int accY = int.Parse(strMsg[1]);
                    int accZ = int.Parse(strMsg[2]);
                    double gyrX = double.Parse(strMsg[3], provider) * (Math.PI / 180);
                    double gyrY = double.Parse(strMsg[4], provider) * (Math.PI / 180);
                    double gyrZ = double.Parse(strMsg[5], provider) * (Math.PI / 180);

                    isButton0Pressed = !Convert.ToBoolean(int.Parse(strMsg[6]));
                    isButton1Pressed = !Convert.ToBoolean(int.Parse(strMsg[7]));
                    isButton2Pressed = !Convert.ToBoolean(int.Parse(strMsg[8]));
                    isButton3Pressed = !Convert.ToBoolean(int.Parse(strMsg[9]));

                    

                    //int magX = int.Parse(strMsg[6]);
                    //int magY = int.Parse(strMsg[7]);
                    //int magZ = int.Parse(strMsg[8]);

                    int iSys = int.Parse(strMsg[10]);

                    //phiM = -atan2(acc.y() / 9.8, acc.z() / 9.8) / 2 / 3.141592654 * 360;
                    double phiA = Math.Atan2(accY, accZ);
                    double chiA = -Math.Atan2(accX, accZ);
                    double psiA = Math.Atan2(accX, accY);
                    _angleY = (int)(phiA * 180 / Math.PI);
                    _angleX = (int)(chiA * 180 / Math.PI);
                    Debug.WriteLine(phiA);

                    dt = (System.DateTime.Now.Subtract(timeOld).TotalMilliseconds) / 1000;
                    timeOld = System.DateTime.Now;

                    phiG[iSys] = phiG[iSys] + gyrX * dt;
                    chiG[iSys] = chiG[iSys] + gyrY * dt;
                    psiG[iSys] = psiG[iSys] + gyrZ * dt;

                    phi[iSys] = phi[iSys] * 0.5 + phiA * 0.5;
                    chi[iSys] = chi[iSys] * 0.5 + chiA * 0.5;
                    psi[iSys] = 0;// psiA;

                    phi[2] = phi[0] - phi[1];
                    chi[2] = 0;
                    psi[2] = 0;


                    if (isButton1Pressed)
                    {
                        Win32.MouseLeftDown((uint)currentMousePoint.X, (uint)currentMousePoint.Y);
                        isMouseLeftPressed = true;
                    }
                    else
                    {
                        if (isMouseLeftPressed){
                            Win32.MouseLeftUp((uint)currentMousePoint.X, (uint)currentMousePoint.Y);
                            isMouseLeftPressed = false;
                        }
                    }
                    if (isButton2Pressed)
                    {
                        Win32.MouseRightDown((uint)currentMousePoint.X, (uint)currentMousePoint.Y);
                        isMouseRightPressed = true;
                    }
                    else
                    {
                        if (isMouseRightPressed)
                        {
                            Win32.MouseRightUp((uint)currentMousePoint.X, (uint)currentMousePoint.Y);
                            isMouseRightPressed = false;
                        }
                    }
                    if (isButton0Pressed)
                    {
                        int sense = 3;
                        Point pt = new Point(sense * (accX - currentIMUPoint.X), sense * (accY - currentIMUPoint.Y));
                        Point pt2;
                        pt2 = new Point((int)(.2 * pt.X + .8 * pt2_old.X), (int)(.2 * pt.Y + .8 * pt2_old.Y));
                        pt2_old = pt2;
                        Win32.SetMousePoint((int)(currentMousePoint.X + pt2.Y), (int)(currentMousePoint.Y - pt2.X));
                    }
                    else
                    {
                        currentIMUPoint = new Point(accX, accY);
                        currentMousePoint = Win32.GetMousePoint();
                        pt2_old = new Point();
                        
                    }
                }
            Application.DoEvents();
            }
            catch
            {
                Debug.WriteLine("Erro!!");
            }
            //txtRead.ScrollToEnd();
        }





    }
}
