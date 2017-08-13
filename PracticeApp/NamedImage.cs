using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureWiz
{
    public class NamedImage : INamedImage
    {
        public NamedImage(string name, string path)
        {
            Name = name;
            ImagePath = path;
        }
        public string Name { get; set; }

        public string ImagePath { get; set; }
    }
}
