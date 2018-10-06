

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


#include <Adafruit_Sensor.h>
#include <ArduinoJson.h>
#include <Adafruit_BME280.h>
#include <time.h>
#include <sys/time.h>
#include <AzureIoTHub.h>
#include <AzureIoTUtility.h>
#include <AzureIoTProtocol_HTTP.h>
#include <Wire.h>
#include <SPI.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#include "NTPClient.h"

#include "config.h"

static bool messagePending = false;

static char *connectionString;
static char *ssid = "akshay";
static char *pass = "G8summit";
float watt;
float energy;
float time1;
float voltage;
float current;

  #define BUTTON_A 9
  #define BUTTON_B 6
  #define BUTTON_C 5

Adafruit_SSD1306 display = Adafruit_SSD1306();
void blinkLED()
{
    digitalWrite(LED_PIN, HIGH);
    delay(500);
    digitalWrite(LED_PIN, LOW);
}


void initWifi()
{
    // Attempt to connect to Wifi network:
    LogInfo("Attempting to connect to SSID: %s", ssid);
display.clearDisplay();
display.setCursor(0,0);
  String temp = String("Attempting to connect to SSID: " +  String(ssid));
  display.print(temp);
  display.setCursor(0,0);
  display.display();

    //Configure pins for Adafruit ATWINC1500 Feather
    WiFi.setPins(8,7,4,2);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:

    while (WiFi.begin(ssid, pass) != WL_CONNECTED)
    {
        // Get Mac Address and show it.
        // WiFi.macAddress(mac) save the mac address into a six length array, but the endian may be different. The M0 WiFi board should
        // start from mac[5] to mac[0], but some other kinds of board run in the oppsite direction.
        uint8_t mac[6];
        WiFi.macAddress(mac);
        LogInfo("You device with MAC address %02x:%02x:%02x:%02x:%02x:%02x connects to %s failed! Waiting 10 seconds to retry.",
                mac[5], mac[4], mac[3], mac[2], mac[1], mac[0], ssid);
        WiFi.begin(ssid, pass);
        delay(10000);
    }

    LogInfo("Connected to wifi %s", ssid);

 display.setCursor(0,0);
   display.clearDisplay();
   display.display();
  display.println("connected!");
  display.setCursor(0,0);
  display.display(); // actually display all of the above


    
}

void initTime()
{
    WiFiUDP _udp;

    time_t epochTime = (time_t)-1;

    NTPClient ntpClient;
    ntpClient.begin();

    while (true)
    {
        epochTime = ntpClient.getEpochTime("pool.ntp.org");

        if (epochTime == (time_t)-1)
        {
            LogInfo("Fetching NTP epoch time failed! Waiting 2 seconds to retry.");
            delay(2000);
        }
        else
        {
            LogInfo("Fetched NTP epoch time is: %lu", epochTime);
            break;
        }
    }

    ntpClient.end();

    struct timeval tv;
    tv.tv_sec = epochTime;
    tv.tv_usec = 0;

    settimeofday(&tv, NULL);
}

static void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *userContextCallback)
{
    if (IOTHUB_CLIENT_CONFIRMATION_OK == result)
    {
        LogInfo("Message sent to Azure IoT Hub");
        blinkLED();
    }
    else
    {
        LogInfo("Failed to send message to Azure IoT Hub");
    }
    messagePending = false;
}

static void sendMessage(IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle, char *buffer, bool temperatureAlert)
{
    IOTHUB_MESSAGE_HANDLE messageHandle = IoTHubMessage_CreateFromByteArray((const unsigned char *)buffer, strlen(buffer));
    if (messageHandle == NULL)
    {
        LogInfo("unable to create a new IoTHubMessage");
    }
    else
    {
        MAP_HANDLE properties = IoTHubMessage_Properties(messageHandle);
        Map_Add(properties, "temperatureAlert", temperatureAlert ? "true" : "false");
        LogInfo("Sending message: %s", buffer);
        if (IoTHubClient_LL_SendEventAsync(iotHubClientHandle, messageHandle, sendCallback, NULL) != IOTHUB_CLIENT_OK)
        {
            LogInfo("Failed to hand over the message to IoTHubClient");
        }
        else
        {
            messagePending = true;
            LogInfo("IoTHubClient accepted the message for delivery");
        }

        IoTHubMessage_Destroy(messageHandle);
    }
}

IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message, void *userContextCallback)
{
    IOTHUBMESSAGE_DISPOSITION_RESULT result;
    const unsigned char *buffer;
    size_t size;
    if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
    {
        LogInfo("unable to IoTHubMessage_GetByteArray\r\n");
        result = IOTHUBMESSAGE_REJECTED;
    }
    else
    {
        /*buffer is not zero terminated*/
        char temp[size + 1];
        memcpy(temp, buffer, size);
        temp[size] = '\0';
        LogInfo("Receiving message: %s", temp);
        result = IOTHUBMESSAGE_ACCEPTED;
    }

    return result;
}

void initSensor()
{
  // todo : Add init sensor code
    // use SIMULATED_DATA, no sensor need to be inited
}

float readVolatge()
{
    voltage =220;
    return voltage;
}

float readCurrent()
{
    current = 3;
    return current;
}



bool readMessage(int messageId, char *payload)
{
    float voltage = readVolatge();
    float current = readCurrent();
    long milisec = millis();
    long time2=milisec/1000; 
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.createObject();
    root["deviceId"] = DEVICE_ID;
    //root["messageId"] = messageId;
    bool result = false;
    watt =voltage*current;
    float timer = time2-time1;
    time1 = time2;
    // NAN is not the valid json, change it to NULL
    if (std::isnan(timer*watt/1000))
    {
        root["coins"] = NULL;
    }
    else
    {
        root["coins"] = watt*timer/1000;
        energy = energy+(watt*timer/1000);
        
    }

   
    root.printTo(payload, MESSAGE_MAX_LEN);
    result = false;
    return result;
}

void initSerial()
{
    // Start serial and initialize stdout
    //Serial.begin(115200);
    //while (!Serial)
      //  ;
    //LogInfo("Serial successfully inited");
}

// Read parameters from Serial
void readCredentials()
{
    // malloc for parameters
    //ssid = (char *)malloc(SSID_LEN);
    //pass = (char *)malloc(PASS_LEN);
    //connectionString = (char *)malloc(CONNECTION_STRING_LEN);
connectionString = "HostName=SolarRoof.azure-devices.net;DeviceId=GarvDeice;SharedAccessKey=BjEWByzwlMMVvBh43viLhfA81WHImPYcZHAjd4U6ybM=";
    // read from Serial and save to EEPROM
    //readFromSerial("Input your Wi-Fi SSID: ", ssid, SSID_LEN, 0);
    //readFromSerial("Input your Wi-Fi password: ", pass, PASS_LEN, 0);
    //readFromSerial("Input your Azure IoT hub device connection string: ", connectionString, CONNECTION_STRING_LEN, 0);
}

/* Read a string whose length should in (0, lengthLimit) from Serial and save it into buf.
 *
 *        prompt   - the interact message and buf should be allocaled already and return true.
 *        buf      - a part of memory which is already allocated to save string read from serial
 *        maxLen   - the buf's len, which should be upper bound of the read-in string's length, this should > 0
 *        timeout  - If after timeout(ms), return false with nothing saved to buf.
 *                   If no timeout <= 0, this function will not return until there is something read.
 */
bool readFromSerial(char *prompt, char *buf, int maxLen, int timeout)
{
    int timer = 0, delayTime = 1000;
    String input = "";
    if (maxLen <= 0)
    {
        // nothing can be read
        return false;
    }

    LogInfo("%s", prompt);
    while (1)
    {
        input = Serial.readString();
        int len = input.length();
        if (len > maxLen)
        {
            LogInfo("Your input should less than %d character(s), now you input %d characters\n", maxLen, len);
        }
        else if (len > 0)
        {
            // save the input into the buf
            sprintf(buf, "%s", input.c_str());
            return true;
        }

        // if timeout, return false directly
        timer += delayTime;
        if (timeout > 0 && timer >= timeout)
        {
            LogInfo("You input nothing, skip...");
            return false;
        }
        // delay a time before next read
        delay(delayTime);
    }
}

static IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;
void setup()
{

   energy = 0;
   time1=0;
    // enable red LED GPIO for writing
    pinMode(LED_PIN, OUTPUT);
    initSerial();

//display initiating
LogInfo("OLED FeatherWing test");
    display.begin(SSD1306_SWITCHCAPVCC, 0x3C);

LogInfo("OLED on");
display.display();
  delay(1000);
 
  // Clear the buffer.
  display.clearDisplay();
  display.display();
  
  LogInfo("IO test");
 
  pinMode(BUTTON_A, INPUT_PULLUP);
  pinMode(BUTTON_B, INPUT_PULLUP);
  pinMode(BUTTON_C, INPUT_PULLUP);
 
  // text display tests
  display.setTextSize(1.4);
  display.setTextColor(WHITE);
 



  
#ifdef WINC_EN
    pinMode(WINC_EN, OUTPUT);
    digitalWrite(WINC_EN, HIGH);
#endif

    readCredentials();

    initWifi();
    initTime();

    initSensor();
    // setup iot hub client which will diliver your message

    /*
    * Break changes in version 1.0.34: AzureIoTHub library removed AzureIoTClient class.
    * So we remove the code below to avoid compile error.
    */

    /*
    beginIoThubClient();
    struct timeval tv;
    gettimeofday(&tv, NULL);
    iotHubClient.setEpochTime(tv.tv_sec);
    */
    iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(connectionString, HTTP_Protocol);
    if (iotHubClientHandle == NULL)
    {
        LogInfo("Failed on IoTHubClient_CreateFromConnectionString");
        while (1);
    }

    // Because it can poll "after 2 seconds" polls will happen
    // effectively at ~3 seconds.
    // Note that for scalabilty, the default value of minimumPollingTime
    // is 25 minutes. For more information, see:
    // https://azure.microsoft.com/documentation/articles/iot-hub-devguide/#messaging
    int minimumPollingTime = 2;
    if (IoTHubClient_LL_SetOption(iotHubClientHandle, "MinimumPollingTime", &minimumPollingTime) != IOTHUB_CLIENT_OK)
    {
        LogInfo("failure to set option \"MinimumPollingTime\"\r\n");
    }

    IoTHubClient_LL_SetOption(iotHubClientHandle, "product_info", "HappyPath_FeatherM0WiFi-C");
    IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, receiveMessageCallback, NULL);




    
}

int messageCount = 1;
void loop()
{
 
  if (! digitalRead(BUTTON_A))
  
  {
    display.clearDisplay();
    LogInfo("A");
    String t = DEVICE_ID;
    String temp = String("Id: " +  t);
    display.println(temp);
  }
  if (! digitalRead(BUTTON_B))
  {
    display.clearDisplay();
    LogInfo("B");
     String temp = String("Present Volatage: " +  String(voltage));
    display.println(temp);
    temp = String("Present Current: " +  String(current));
    display.println(temp);
    
    temp = String("Present Watt: " +  String(watt));
    display.println(temp);
  }
  if (! digitalRead(BUTTON_C)) 
  {
    display.clearDisplay();
    LogInfo("C");
    String temp = String("Coins per second: "+  String(watt/1000));
    display.println(temp);
    temp = String("Total coins mined: "+  String(energy));
    display.println(temp);
  }
  delay(10);
  yield();
  display.setCursor(0,0);
  display.display();

  
    if (!messagePending)
    {
        char messagePayload[MESSAGE_MAX_LEN];
        bool temperatureAlert = readMessage(messageCount, messagePayload);
        LogInfo(messagePayload);
        sendMessage(iotHubClientHandle, messagePayload,false);
        messageCount++;
        delay(INTERVAL);
    }
    IoTHubClient_LL_DoWork(iotHubClientHandle);
}
