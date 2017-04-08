﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWords
{
    class DataItem
    {
        public int Key { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }

        public DataItem()
        {
            Key = FileWorker.GetLastIndex + 1;
        }
    }
}
