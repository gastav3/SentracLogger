using SentLogger.Resources;
using SentLogger.UWP;
using System.Collections.Generic;
using Xamarin.Forms;
using SentLogger.Models;
using SentLogger.ViewModels;
using Windows.Devices.Enumeration;
using SentLogger.Models.Extra;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

[assembly : Dependency(typeof(UsbConnectionSerialPort))]
namespace SentLogger.UWP
{
    public class UwpFiles : IFiles
    {
        public Task<List<FileObject>> GetFilesInPath(string path)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<FileObject>> GetFoldersInPath(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
