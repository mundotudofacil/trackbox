
namespace TrackBoxTeste01
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btoIniciar = new System.Windows.Forms.Button();
            this.btoParar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRead = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btoIniciar
            // 
            this.btoIniciar.Location = new System.Drawing.Point(29, 28);
            this.btoIniciar.Name = "btoIniciar";
            this.btoIniciar.Size = new System.Drawing.Size(75, 23);
            this.btoIniciar.TabIndex = 0;
            this.btoIniciar.Text = "Iniciar";
            this.btoIniciar.UseVisualStyleBackColor = true;
            this.btoIniciar.Click += new System.EventHandler(this.btoIniciar_Click);
            // 
            // btoParar
            // 
            this.btoParar.Location = new System.Drawing.Point(29, 57);
            this.btoParar.Name = "btoParar";
            this.btoParar.Size = new System.Drawing.Size(75, 23);
            this.btoParar.TabIndex = 1;
            this.btoParar.Text = "Parar";
            this.btoParar.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(428, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 167);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // txtRead
            // 
            this.txtRead.Location = new System.Drawing.Point(29, 107);
            this.txtRead.Multiline = true;
            this.txtRead.Name = "txtRead";
            this.txtRead.Size = new System.Drawing.Size(248, 89);
            this.txtRead.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 282);
            this.Controls.Add(this.txtRead);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btoParar);
            this.Controls.Add(this.btoIniciar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btoIniciar;
        private System.Windows.Forms.Button btoParar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRead;
    }
}

