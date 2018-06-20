using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;

namespace SentLogger.Models
{
  /// <summary>
  /// Part of the data translation from Sentrac.
  /// </summary>
    public class DataDotObject
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public double Value { get; set; }
        public string Result { get; set; }

        public string StringDate { get; set; }
        public string StringTime { get; set; }

        public DataDotObject(DateTime date, TimeSpan time, double val, string res)
        {
            this.Date = date;
            this.Time = time;

            this.Value = val;
            this.Result = res;

            this.StringDate = date.ToString("yyyy/MM/dd");
            this.StringTime = time.ToString();

            Debug.WriteLine("Created new dot object with values" +
                "\nDate: " + this.Date.ToString("dd/MM/yyyy") +
                "\nTime: " + this.Time +
                "\nValue: " + this.Value +
                "\nAccepted: " +  this.Result);
        }
    }
}
