using SentLogger.Resources;
using SentLogger.UWP;
using System.Collections.Generic;
using Xamarin.Forms;
using SentLogger.Models;
using SentLogger.ViewModels;
using Windows.Devices.Enumeration;
using SentLogger.Models.Extra;

[assembly: Dependency(typeof(UsbConnectionSerialPort))]
namespace SentLogger.UWP
{
    public class UsbConnectionSerialPort : IUsbConnectionSerialPort
    {
        private UwpUsbConnection uwpUsbCon;

        public void Start()
        {
            this.uwpUsbCon = new UwpUsbConnection(this);
        }

        public void Start(GraphViewModel graphvm)
        {
            this.uwpUsbCon = new UwpUsbConnection(this);
            StaticValues.graphViewModel = graphvm;
        }

        public void ReceiveNewData(DataDotObject data)
        {
            if (StaticValues.graphViewModel.StreamingPlay) {
                StaticValues.dotList.Add(data);
                StaticValues.graphViewModel.AddNewDot(data.Value, data.Value);
            }
        }

        public List<DataDotObject> GetList()
        {
            return StaticValues.dotList;
        }

        public List<DataDotObject> GetData()
        {
            List<DataDotObject> tempDotList = new List<DataDotObject>(GetList());
            GetList().Clear();

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
