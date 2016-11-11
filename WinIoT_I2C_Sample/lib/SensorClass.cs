using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.UI.Xaml;

namespace WinIoT_I2C_Sample
{
    public class SensorClass
    {
        // This example is for Bosch BME280 sensor but can be easily adapted for BMP280.
        // SDA = PIN_03 (GPIO2)
        // SCL = PIN_05 (GPIO03)                 

        const byte BME280_Address = 0x76; // Addresses for BME280 are 0x76 and 0x77
        const byte BME280_Signature = 0x60; // Bosch BME280 device signature
        const string I2cControllerName = "I2C1"; //Name of the I2C Controller on RPi

        I2cDevice i2cDevice;
        bool i2cDeviceInitialized = false;
        I2cConnectionSettings i2cConnectionSettings;
        string hexInput { get; set; }
        string hexOutput { get; set; }

        public async void Initialize()
        {
            //Initializes I2C-bus
            await InitI2C();
        }

        public bool IsInitialized()
        {
            // Returns true/false on whether device has been initialized.
            return i2cDeviceInitialized;
        }

        private async Task InitI2C()
        {
            i2cConnectionSettings = new I2cConnectionSettings(BME280_Address); //Create a I2C connection to this address.
            i2cConnectionSettings.SharingMode = I2cSharingMode.Exclusive; //I2C-bus mode. If you are connecting multiple devices, use Shared mode.
            i2cConnectionSettings.BusSpeed = I2cBusSpeed.FastMode; // If you are using wires that are 2 meters or longer, use StandardMode.

            string aqs = I2cDevice.GetDeviceSelector(I2cControllerName);
            DeviceInformationCollection dis = await DeviceInformation.FindAllAsync(aqs);
            InitBME280(dis[0].Id, i2cConnectionSettings);
        }
        private async void InitBME280(string dis, I2cConnectionSettings i2cConnection)
        {
            i2cDevice = await I2cDevice.FromIdAsync(dis, i2cConnection); //Create I2C-device object

            if (i2cDevice == null) //If there is no I2C-device in given address, report an error.
            {
                Debug.WriteLine("InitBME280: Device not found");
            }
            else //...else, test that communication works and device signature matches.
            {
                try
                {
                    CheckDevice();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
                }
            }
        }
        private void CheckDevice()
        {
            // This function reads device signature and verifies that device is BME280.

            byte[] WriteBuffer = new byte[] { 0xD0 }; //We want to read from registry 0xD0 which contains device signature
            byte[] ReadBuffer = new byte[] { 0xFF }; // ...and return value will be saved into this ReadBuffer variable.

            try
            {
                // Try to read device id
                i2cDevice.WriteRead(WriteBuffer, ReadBuffer);
            }
            catch (Exception e)
            {
                Debug.WriteLine("!EX BME280.Begin(): " + e.Message);
            }

            if (ReadBuffer[0] == BME280_Signature) // Comparing read device signature to known BME280 signature
            {
                Debug.WriteLine("!BME280.Begin(): Signature matched!");
                i2cDeviceInitialized = true;
            }
            else
            {
                Debug.WriteLine("!BME280.Begin(): Signature mismatch!");
                i2cDeviceInitialized = false;
            }
        }

        public byte Read(byte address)
        {
            if (IsInitialized()) // Checking that connection with the device has been established.
            {
                // This function performs read operation from parameter address and returns read value.

                byte[] writeBuffer = new byte[] { address };
                byte[] readBuffer = new byte[] { 0xFF };

                i2cDevice.WriteRead(writeBuffer, readBuffer);
                return readBuffer[0];
            }
            else
            {
                Debug.WriteLine("!BME280.Read(): Read failed, device/bus not initialized");
                return 0;
            }
        }

    }

}

