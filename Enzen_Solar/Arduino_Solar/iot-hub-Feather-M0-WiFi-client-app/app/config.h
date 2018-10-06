// Physical device information for board and sensor
#define DEVICE_ID "c114a1a2-43c5-4b63-bcb2-4efe65c2cc82"

// Pin layout configuration
#define LED_PIN 13
#define BME_CS 5

// Interval time(ms) for sending message to IoT Hub
#define INTERVAL 2000

// If don't have a physical DHT sensor, can send simulated data to IoT hub
#define SIMULATED_DATA true

#define TEMPERATURE_ALERT 30

// SSID and SSID password's length should < 32 bytes
// http://serverfault.com/a/45509
#define SSID_LEN 32
#define PASS_LEN 32
#define CONNECTION_STRING_LEN 256

#define MESSAGE_MAX_LEN 256

#define WINC_EN 2
#define WINC_CS 8
#define WINC_IRQ 7
#define WINC_RST 4
