# WinIoT_I2C_Sample
Simple I2C app for Windows IoT and Raspberry Pi.

This demonstrates how to get the basic I2C-bus functionality working when running Windows IoT on Raspberry Pi.

- MainPage represents basic user interface with a button and a textbox which are used to send a query for device ID
and display it to the user.

- SensorClass contains the essentials to establish connection between Raspberry Pi and I2C-device, verifying it and reading data
from registry. Example uses Bosch BME280 sensor but it can be easily modified for other I2C devices as well. To do that,
would need to change the Address and Signature constants to match what is shown in your device's datasheet.

-Samuli Kumo
