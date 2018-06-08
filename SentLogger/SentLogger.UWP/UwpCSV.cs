using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Xamarin.Forms;
using SentLogger.Resources.Data;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.AccessCache;
using System.Threading.Tasks;
using System.Diagnostics;
using SentLogger.Models;

[assembly: Dependency(typeof(UwpCSV))]
namespace SentLogger.Resources.Data
{
    public class UwpCSV : ICsv
    {
       public StorageFile file; // selected file

        public UwpCSV(){}

        public void Start()
        {
           // SelectFile();
        }

        public async void SelectFile()
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("file style", new string[] { ".csv" });
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "Data.csv";
            file = await picker.PickSaveFileAsync();

           await ReadCsvFile(file);
        }

        public async void WriteToFile(StorageFile file, DataTable data)
        {
            if (file != null)
            {   
                StringBuilder sB = ConvertDataTableToCsvFile(data);

                string sep = "\n"; // split between the \n
                string[] splitContent = sB.ToString().Split(sep.ToCharArray());

                await FileIO.AppendLinesAsync(file, splitContent);
            }
        }

        public StringBuilder ConvertDataTableToCsvFile(DataTable dtData)
        {
            StringBuilder data = new StringBuilder();

            //Taking the column names.
            for (int column = 0; column < dtData.Columns.Count; column++)
            {
                //Making sure that end of the line, shoould not have comma delimiter.
                if (column == dtData.Columns.Count - 1)
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";"));
                else
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";") + ',');
            }

            data.Append(Environment.NewLine);//New line after appending columns.

            for (int row = 0; row < dtData.Rows.Count; row++)
            {
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    ////Making sure that end of the line, shoould not have comma delimiter.
                    if (column == dtData.Columns.Count - 1)
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";"));
                    else
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";") + ',');
                }

                //Making sure that end of the file, should not have a new line.
                if (row != dtData.Rows.Count - 1)
                    data.Append(Environment.NewLine);
            }
            return data;
        }

        public async Task<List<DataDotObject>> ReadCsvFile(StorageFile file)
        {
            List<DataDotObject> dataObjects = new List<DataDotObject>();
            DataTranslator dataTranslator = new DataTranslator();

                if (file != null)
                {
                    string fileContent = await FileIO.ReadTextAsync(file);
                    string newString = fileContent.Replace(",", "\t");

                    string sep = "\n"; // split between the \n
                    string[] splitContent = newString.ToString().Split(sep.ToCharArray());

                    foreach(string s in splitContent)
                    {
                        try
                        {
                            dataObjects.Add(dataTranslator.TranslateIntoOneDot(s));

                        }catch(IndexOutOfRangeException e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                 return dataObjects;
                }
            return dataObjects;
            }

        public DataTable DataDotObjectsToDataTable(List<DataDotObject> objs)
        {
            //Create Table
            DataTable data = new DataTable("Data");
            data.Columns.Add("Date", typeof(DateTime));
            data.Columns.Add("Time", typeof(TimeSpan));
            data.Columns.Add("Value", typeof(Double));
            data.Columns.Add("Result", typeof(String));

            foreach(DataDotObject obj in objs)
            {
                data.Rows.Add(obj.Date, obj.Time, obj.Value, obj.Accepted);
            }
            return data;
        }


    }
}
