using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureWiz
{
    public interface INamedImage
    {
        string Name
        {
            get;
        }

        string ImagePath
        {
            get;
        }
    }
}
