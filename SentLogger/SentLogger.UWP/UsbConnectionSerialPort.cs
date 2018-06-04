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

[assembly: Dependency(typeof(UsbConnectionSerialPort))]
namespace SentLogger.UWP
{
    public class UsbConnectionSerialPort : IUsbConnectionSerialPort
    {
        private DataTranslator translator;
        private List<GraphDot> dotList;
        private UwpUsbConnection uwpUsbCon;

        public void Start()
        {
            this.translator = new DataTranslator();
            this.dotList = new List<GraphDot>();
            this.uwpUsbCon = new UwpUsbConnection(this);
        }

        public List<GraphDot> GetData()
        {
         //   translator.TranslateIntoDots();


            List<GraphDot> temoDotList = new List<GraphDot>(this.dotList);
            this.dotList.Clear();

            return temoDotList;
        }
    }
}
