using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows;
using System.Diagnostics;

namespace SentLogger.Resources
{
    public class SerialPortExample
    {
        // Create the serial port with basic settings
        private SerialPort port = new SerialPort("COM8",
          9600, Parity.None, 8, StopBits.One);



        public SerialPortExample()
        {
            while (true)
            {

                try
                {
                    Debug.WriteLine("Incomming data");

                    // Attach a method to be called when there
                    // is data waiting in the port's buffer
                    port.DataReceived += new
                      SerialDataReceivedEventHandler(Port_DataReceived);

                    // Begin communications
                    port.Open();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString() + " Error Reading Data");
                }
                // Enter an application loop to keep this thread alive
                // Application.Run();
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.WriteLine("data " + e.ToString());
        }

      }
}
