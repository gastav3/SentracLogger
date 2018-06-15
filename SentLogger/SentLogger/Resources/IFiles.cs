using SentLogger.Models.Extra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SentLogger.Resources
{
    public interface IFiles
    {
        Task<List<FileObject>> GetFoldersInPath(string path);
        Task<List<FileObject>> GetFilesInPath(string path);
    }
}
