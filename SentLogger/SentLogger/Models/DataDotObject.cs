using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;

namespace SentLogger.Models
{
    public class DataDotObject
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public double Value { get; set; }
        public string Accepted { get; set; }

        public DataDotObject(DateTime date, TimeSpan time, double val, string acc)
        {
            this.Date = date;
            this.Time = time;

            this.Value = val;
            this.Accepted = acc;

            Debug.WriteLine("Created new dot object with values" +
                "\nDate: " + this.Date.ToString("dd/MM/yyyy") +
                "\nTime: " + this.Time +
                "\nValue: " + this.Value +
                "\nAccepted: " +  this.Accepted);
        }
    }
}
