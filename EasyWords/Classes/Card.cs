using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWords
{
    class Card
    {
        public int Key { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }

        public Card()
        {
            Key = default(int);
            Word1 = string.Empty;
            Word2 = string.Empty;
        }
    }
}
