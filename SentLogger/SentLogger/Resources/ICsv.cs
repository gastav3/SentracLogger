using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker.Abstractions;
using SentLogger.Models;
using SentLogger.ViewModels;

namespace SentLogger.Resources
{
    public interface ICsv
    {
        void Start();
        void SaveFile();
        Task<List<DataDotObject>> LoadFile();
    }
}
