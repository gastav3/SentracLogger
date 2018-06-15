using System;
using System.Collections.Generic;
using System.Text;
using SentLogger.Models;
using System.Diagnostics;
using System.Linq;
using System.Data;

namespace SentLogger.Resources
{
    public class DataTranslator
    {

        public DataTable DataDotObjectsToDataTable(List<DataDotObject> objs)
        {
            //Create Table
            DataTable data = new DataTable("Data");
            data.Columns.Add("Date", typeof(DateTime));
            data.Columns.Add("Time", typeof(TimeSpan));
            data.Columns.Add("Value", typeof(Double));
            data.Columns.Add("Result", typeof(String));

            foreach (DataDotObject obj in objs)
            {
                if (obj != null) {
                    data.Rows.Add(obj.Date, obj.Time, obj.Value, obj.Accepted);
                }
            }
            return data;
        }

        public DataDotObject TranslateIntoOneDot(string s)
        {
            string sep = "\t"; // split between the tabs
            string[] splitContent = s.Split(sep.ToCharArray()); // string parts

            DateTime date = new DateTime().Date;
            TimeSpan time = new DateTime().TimeOfDay;

            double value = -1.0; // -1.0 to check if later
            string acceptedString = ""; // maybe better name on this?

            if (DateTime.TryParse(splitContent[0], out DateTime dateValue)) { 
                date = dateValue.Date;
            }

            if (TimeSpan.TryParse(splitContent[1], out TimeSpan timeValue)) { 
                time = timeValue;
            }

            bool result = Double.TryParse(splitContent[2], out double val);
            if (result)
            {
                value = val;
            }

            try
            {
                acceptedString = splitContent[3];
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine(e.Message);
            }

            if (date != null && time != null && value != -1.0 && acceptedString != null) {
                DataDotObject newDot = new DataDotObject(date, time, value, acceptedString);
                return newDot;
            }
            return null;
        }

        public List<DataDotObject> TranslateListIntoDots(List<String> stringList)
        {
            List<DataDotObject> tempList = new List<DataDotObject>();

            foreach (string s in stringList)
            {
                DataDotObject tempObj = TranslateIntoOneDot(s);

                if (tempObj != null) { 
                    tempList.Add(TranslateIntoOneDot(s));
                     }
                }

            return tempList;
        }
    }
}
