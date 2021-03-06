#include <Adafruit_MCP23017.h>
#include <Wire.h>
#include <Servo.h>

#define P_U2_RX       0
#define P_U2_TX       1

#define P_U2_US1_P    2
#define P_U2_US1_E    3
#define P_U2_US2_P    4
#define P_U2_US2_E    5
#define P_U2_US3_P    6
#define P_U2_US3_E    7
#define P_U2_US4_P    8
#define P_U2_US4_E    9
#define P_U2_US5_P    12
#define P_U2_US5_E    13

#define P_U2_PWM1     10
#define P_U2_PWM2     11

#define P_U2_BattVolt A0
#define P_U2_ADC1     A1
#define P_U2_ADC2     A2
#define P_U2_ADC3     A3
#define P_U2_SDA      A4
#define P_U2_SCL      A5

#define P_Hall_H1     0
#define P_Hall_H2     1
#define P_Hall_H3     2
#define P_Hall_H4     3
#define P_Hall_H5     4
#define P_Hall_H6     5
#define P_Hall_H7     6
#define P_Hall_H8     7
#define P_Hall_H9     8
#define P_Hall_H10    9
#define P_Hall_H11    10
#define P_Hall_H12    11
#define P_Hall_H13    12
#define P_Hall_H14    13
#define P_Hall_H15    14
#define P_Hall_H16    15

#define U_NUM            5
#define H_NUM            16
#define SERIAL_COMM_INIT 1000
#define UPDATE_LOOP      100
#define US_SENSOR_LOOP   10
#define HE_SENSOR_LOOP   1
#define HALL_LIFE        10

#define VOLTAGE_FACTOR   .0291986456

Adafruit_MCP23017 mcp;

Servo Lift_Servo;

//pins
const int HEpins[H_NUM] = {P_Hall_H1, P_Hall_H2, P_Hall_H3, P_Hall_H4, P_Hall_H5, P_Hall_H6, P_Hall_H7,
                           P_Hall_H8, P_Hall_H9, P_Hall_H10, P_Hall_H11, P_Hall_H12, P_Hall_H13, P_Hall_H14,
                           P_Hall_H15, P_Hall_H16
                          }; // pins on expansion board
const int USping[U_NUM] = {P_U2_US1_P, P_U2_US2_P, P_U2_US3_P, P_U2_US4_P, P_U2_US5_P};
const int USecho[U_NUM] = {P_U2_US1_E, P_U2_US2_E, P_U2_US3_E, P_U2_US4_E, P_U2_US5_E};

//variables
int i = 0;
int USprev[U_NUM] = {0, 0, 0, 0, 0};
int hallValuesLife[16];
int j = 0;
int SmallestDist[5];

//serial
String msg2pc = "", bBuff = "";
int serialCount = SERIAL_COMM_INIT;
const char USkey[U_NUM] = {'V', 'W', 'X', 'Y', 'Z'};
String prevHe = "", prevVolt = "";
bool pcConnect = false;
bool Servo_On = false;
String Servo_Engaged= "";


void setup() {
  Serial.begin(115200);
  for (int i = 0; i < U_NUM; i++)
  {
    pinMode(USping[i], OUTPUT);
    pinMode(USecho[i], INPUT);
  }
  mcp.begin();
  for (int i = 0; i < H_NUM; i++)
  {
    mcp.pinMode(HEpins[i], INPUT);
    mcp.pullUp(HEpins[i], HIGH);
  }
  
  Lift_Servo.attach(P_U2_PWM2);
  Servo_Lift(Servo_On);
}

void loop() {
  if (Serial.available() > 0)
  {
    SerialIn();
  }
  SerialOut();
  
  delay(10);

}
