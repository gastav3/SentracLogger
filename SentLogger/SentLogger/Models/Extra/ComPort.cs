using System;
using System.Collections.Generic;
using System.Text;

namespace SentLogger.Models.Extra
{
    public class ComPort
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public ComPort(string name, string id)
        {
            this.Name = Name;
            this.Id = Id;
        }
    }
}
