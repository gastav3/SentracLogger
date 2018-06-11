using System;
using System.Collections.Generic;
using System.Text;
using SentLogger.Models;
using SentLogger.Models.Extra;
using SentLogger.ViewModels;

namespace SentLogger.Resources
{
   public interface IUsbConnectionSerialPort
    {
        void Start();
        void Start(GraphViewModel graphVM);
        List<DataDotObject> GetData();
        List<ComPort> GetPorts();
        void SetPort(string port, bool restart);
    }
}
