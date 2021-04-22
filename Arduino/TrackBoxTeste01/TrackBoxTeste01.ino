#include <GY_85.h>
#include <Wire.h>

GY_85 GY85;     //Criando objeto

#define TO_DEG(x) (x * 57.2957795131)  // *180/pi
  
void setup()
{
    pinMode(9, INPUT_PULLUP);
    pinMode(10, INPUT_PULLUP);
    pinMode(11, INPUT_PULLUP);
    pinMode(12, INPUT_PULLUP);
    Wire.begin();
    delay(10);
    Serial.begin(115200);
    delay(10);
    GY85.init();
    delay(10);
}

void loop()
{
    //int* axis;
    int* aX = GY85.readFromAccelerometer();
    int ax = aX[0];
    int ay = aX[1];
    int az = aX[2];

    float* gX = GY85.readGyro();
    float gx = gX[0] / 14.375; // GY85[i].gyro_x( GY85[i].readGyro() );
    float gy = gX[1] / 14.375; // GY85[i].gyro_y( GY85[i].readGyro() );
    float gz = gX[2] / 14.375; // GY85[i].gyro_z( GY85[i].readGyro() );
    float gt = GY85.temp  ( GY85.readGyro() );
    
    int* cX = GY85.readFromCompass();
    int cx = GY85.compass_x( GY85.readFromCompass() );
    int cy = GY85.compass_y( GY85.readFromCompass() );
    int cz = GY85.compass_z( GY85.readFromCompass() );

    int b0 = digitalRead(9);
    int b1 = digitalRead(10);
    int b2 = digitalRead(11);
    int b3 = digitalRead(12);

    String msg = String(aX[0]) + "," + String(aX[1]) + "," + String(aX[2]) + ","
               + String(gx) + "," + String(gy) + "," + String(gz) + ","
               + String(b0) + "," + String(b1) + "," + String(b2) + ","
               + String(b3) + "," + "0" + ";";
               
    Serial.print(msg);
    delay(100);
}
