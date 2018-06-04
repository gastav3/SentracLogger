using System;
using System.Collections.Generic;
using System.Text;
using SentLogger.Models;

namespace SentLogger.Resources
{
   public interface IUsbConnectionSerialPort
    {
        List<GraphDot> GetData();
        void Start();
    }
}
