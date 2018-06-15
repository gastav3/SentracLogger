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
using SentLogger.Models.Extra;

[assembly: Dependency(typeof(UwpCSV))]
namespace SentLogger.Resources.Data
{
    public class UwpCSV : ICsv
    {
       public StorageFile file; // selected file

        public UwpCSV(){}

        public void Start()
        {
        }

        public async Task<List<DataDotObject>> LoadFile()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                return await ReadCsvFile(file);
            }
            return null;
        }

        public async void SaveFile()
        {
            DataTranslator translator = new DataTranslator();

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("CSV", new List<string>() { ".csv" });
            savePicker.SuggestedFileName = "Sentrac Data";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                WriteToFile(file, translator.DataDotObjectsToDataTable(StaticValues.dotList));
            }
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
                    //Making sure that end of the line, shoould not have comma delimiter.
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
                    if (!string.Equals(s, "") || s != null) {
                        try
                        {
                            dataObjects.Add(dataTranslator.TranslateIntoOneDot(s));
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }
              return dataObjects;
            }
        return dataObjects;
        }
    }
}
