using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Wiz
{
    public class WordSoundFile
    {
        public WordSoundFile(string name, string path)
        {
            Name = name;
            Path = path;
            Chosen = false;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool Chosen { get; set; }
    }
}
