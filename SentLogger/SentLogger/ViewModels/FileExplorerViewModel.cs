using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Plugin.FilePicker.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SentLogger.ViewModels
{
    public class FileExplorerViewModel : INotifyPropertyChanged
    {
        private FileData choosenFile;

        public FileExplorerViewModel()
        {
          
        }

        public void PrintFiles(List<string> list)
        {
            foreach (var s in list)
            {
                Debug.WriteLine(s);
            }
        }

        public List<string> GetFileExplorerFiles(string path)
        {
            string[] files = System.IO.Directory.GetFiles(path);
            List<string> fileList = new List<string>(files);

            PrintFiles(fileList);

            return fileList;
        }

        public FileData GetChoosenFile
        {
            get => this.choosenFile;
            set
            {
                this.choosenFile = value;
                OnPropertyChanged();
            }
        }



        //--------ON PROPERTY CHANGED STUFF-----------

        /// <summary>
        /// A standard PropertyChangedEventHandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
