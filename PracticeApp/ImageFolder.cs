using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeApp
{
    public class ImageFolder : IImageFolder
    {
        public ImageFolder(string name, string path)
        {
            Name = name;
            Folder = path;
        }
        public string Name { get; set; }

        public string Folder { get; set; }
    }
}
