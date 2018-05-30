using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SentLogger.Models
{
    public class CSV
    {



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
