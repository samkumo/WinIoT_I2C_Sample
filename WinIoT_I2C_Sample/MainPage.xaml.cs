using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinIoT_I2C_Sample;

namespace WinIoT_I2C_Sample
{
    // This is simple Windows IoT application for Raspberry Pi which connects to a I2C device, in this example BME280 sensor.

    public sealed partial class MainPage : Page
    {
        SensorClass sensor = new SensorClass();
        string returnValue; 

        public MainPage()
        {
            this.InitializeComponent();
            sensor.Initialize(); // Establish I2C-connection with the slave device.
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            // This is a simple test function where we read device signature 
            // from BME280 using I2C and display it in TextBox, note that 0x60 = 96d
            returnValue = sensor.Read(0xD0).ToString(); // Hex-value 0xD0 is the address in BME280 that returns device ID
            Bindings.Update();            
        }
 
    }
}

