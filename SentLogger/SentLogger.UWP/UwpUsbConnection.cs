using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using System.Diagnostics;
using SentLogger.Resources;
using SentLogger.Models.Extra;

namespace SentLogger.UWP
{
    public class UwpUsbConnection
    {
        private DataTranslator dataTranslator;
        private UsbConnectionSerialPort usbConnectionClass;

        private SerialDevice serialPort = null;
        private DataWriter dataWriteObject = null;
        private DataReader dataReaderObject = null;
        public ObservableCollection<DeviceInformation> listOfDevices;
        private CancellationTokenSource ReadCancellationTokenSource;

        private string portId;

        public UwpUsbConnection(UsbConnectionSerialPort usbConn)
        {
            if (serialPort == null)
            {
                this.portId = StaticValues.SelectedPort;
                dataTranslator = new DataTranslator();
                usbConnectionClass = usbConn;
                listOfDevices = new ObservableCollection<DeviceInformation>();
                ListAvailablePorts();
                SerialPortConfiguration();
            }
            else
            {
                SerialPortDisconnect();
            }
        }

        public void ChangePort(string id, bool restart)
        {
            this.portId = id;

            if (restart) {
                SerialPortDisconnect();
                dataTranslator = new DataTranslator();
                SerialPortConfiguration();
                Debug.WriteLine("Change Port - UWPUSBCONNECTION");
            }
        }

        public async void ListAvailablePorts()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);
                for (int i = 0; i < dis.Count; i++)
                {
                    listOfDevices.Add(dis[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void SerialPortConfiguration()
        {

            if (portId != null && serialPort == null) {
                try
                {
                    serialPort = await SerialDevice.FromIdAsync(portId);
                    serialPort.WriteTimeout = TimeSpan.FromMilliseconds(100);
                    serialPort.ReadTimeout = TimeSpan.FromMilliseconds(100);
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = SerialParity.None;
                    serialPort.StopBits = SerialStopBitCount.One;
                    serialPort.DataBits = 8;
                    serialPort.Handshake = SerialHandshake.None;
                    ReadCancellationTokenSource = new CancellationTokenSource();
                    Debug.WriteLine("Connection configuration completed");
                    Listen();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " - Failed to connect.");
                }
            }
        }

        private void SerialPortDisconnect()
        {
            try
            {
                CancelReadTask();
                CloseDevice();
                ListAvailablePorts();
                Debug.WriteLine("Disconnect from the connection.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " Failed to disconnect.");
            }
        }

        private async Task ManageLed(string value)
        {
            var accendiLed = value;
            Task<UInt32> storeAsyncTask;
            if (accendiLed.Length != 0)
            {
                dataWriteObject.WriteString(accendiLed);
                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();
                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {
                    Debug.WriteLine("Value sent correctly");
                }
            }
            else
            {
                Debug.WriteLine("No value sent");
            }
        }

        private async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);
                    while (true)
                    {
                        await ReadData(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (ex.GetType().Name == "TaskCanceledException")
                {
                    CloseDevice();
                }
                else
                {
                    Debug.WriteLine("Task canceld");
                }
            }
            finally
            {
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        private async Task ReadData(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;
            uint ReadBufferLength = 128;
            cancellationToken.ThrowIfCancellationRequested();
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                usbConnectionClass.ReceiveNewData(dataTranslator.TranslateIntoOneDot(dataReaderObject.ReadString(bytesRead).ToString()));
            }
        }

        private void CancelReadTask()
        {
            if (ReadCancellationTokenSource != null)
            {
                if (!ReadCancellationTokenSource.IsCancellationRequested)
                {
                    ReadCancellationTokenSource.Cancel();
                }
            }
        }

        private void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;
            listOfDevices.Clear();
        }
    }
}

