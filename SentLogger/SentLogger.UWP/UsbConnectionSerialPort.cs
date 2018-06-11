using SentLogger.Resources;
using SentLogger.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using SentLogger.Models;
using SentLogger.ViewModels;
using Windows.Devices.Enumeration;
using SentLogger.Models.Extra;

[assembly: Dependency(typeof(UsbConnectionSerialPort))]
namespace SentLogger.UWP
{
    public class UsbConnectionSerialPort : IUsbConnectionSerialPort
    {
        private List<DataDotObject> dotList;
        private UwpUsbConnection uwpUsbCon;
        private GraphViewModel graphVM;

        public void Start()
        {
            this.dotList = new List<DataDotObject>();
            this.uwpUsbCon = new UwpUsbConnection(this);
        }

        public void Start(GraphViewModel graphvm)
        {
            this.dotList = new List<DataDotObject>();
            this.uwpUsbCon = new UwpUsbConnection(this);
            this.graphVM = graphvm;
        }

        public void ReceiveNewData(DataDotObject data)
        {
            this.graphVM.AddNewDot(data.Value, data.Value);
        }

        public List<DataDotObject> GetList()
        {
            return this.dotList;
        }

        public List<DataDotObject> GetData()
        {
            List<DataDotObject> tempDotList = new List<DataDotObject>(this.dotList);
            this.dotList.Clear();

            return tempDotList;
        }

        public List<ComPort> GetPorts()
        {
            uwpUsbCon.ListAvailablePorts();
            List<ComPort> ports = new List<ComPort>();

            foreach (DeviceInformation dInfo in uwpUsbCon.listOfDevices)
            {
                ComPort port = new ComPort(dInfo.Name, dInfo.Id);
                port.Name = dInfo.Name; // Had to assign it again dident work otherwise.
                port.Id = dInfo.Id; // Had to assign it again dident work otherwise.
                ports.Add(port);
            }

            return ports;
        }

        public void SetPort(string port, bool restart)
        {
            this.uwpUsbCon.ChangePort(port, restart);
        }
    }
}
