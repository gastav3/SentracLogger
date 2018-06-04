using System;
using System.Collections.Generic;
using System.Text;
using SentLogger.Models;
using System.Diagnostics;

namespace SentLogger.Resources
{
    public class DataTranslator
    {

        public List<GraphDot> TranslateIntoDots(List<String> stringList)
        {
            foreach(string s in stringList)
            {
                Debug.WriteLine(s);
            }

            return null;
        }
    }
}
