using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace SentLogger.Resources.Data
{
    public class CSV
    {
        public CSV()
        {
            CreateCSVFile();
            LoadDataFrom(@"C:\Users\Sensor\Desktop\Sentrac_Data.csv");
        }


        public void CreateCSVFile()
        {
            //Create Table
            DataTable data = new DataTable("Data");
            data.Columns.Add("Date", typeof(DateTime));
            data.Columns.Add("Time", typeof(TimeSpan));
            data.Columns.Add("Value", typeof(Double));
            data.Columns.Add("Result", typeof(String));

            //Add rows
            data.Rows.Add(DateTime.Now, new TimeSpan(), 1.0, "Accepted");

            using (StreamWriter sw = new StreamWriter(new FileStream(@"C:\Users\Sensor\Desktop\CsvData\Sentrac_Data.csv", FileMode.Create)))
            {
                for (Int32 i = 0; i < data.Rows.Count; i++)
                {
                    string[] s = new string[data.Columns.Count];
                    for (Int32 j = 0; j < data.Columns.Count; j++)
                    {
                        s[j] = data.Rows[i].ItemArray[j].ToString();
                    }
                    sw.WriteLine(string.Join(",", s));
                }
            }
        }

            public void LoadDataFrom(string path)
        {
            using (var reader = new StreamReader(path))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }
            }
        }

    }
}
