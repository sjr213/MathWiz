using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureWiz
{
    public class NamedImageModel
    {
        private Dictionary<string, string> _NamedThings = new Dictionary<string, string>();

        public Dictionary<string, string> NamedThings
        {
            get { return _NamedThings; }
            set { _NamedThings = value; }
        }

        public NamedImageModel()
        {
            _NamedThings.Add("Car", "Images/Car.jpg");
            _NamedThings.Add("Airplane", "Images/Airplane.png");
            _NamedThings.Add("Sailboat", "Images/Sailboat.jpg");
            _NamedThings.Add("Train", "Images/Train.jpg");
            _NamedThings.Add("Bicycle", "Images/Bicycle.jpg");
            _NamedThings.Add("Truck", "Images/Truck.jpg");
            _NamedThings.Add("Helicopter", "Images/Helicopter.jpg");
            _NamedThings.Add("Motorcycle", "Images/Motorcycle.png");
            _NamedThings.Add("School bus", "Images/School Bus.jpg");
            _NamedThings.Add("Ship", "Images/Ship.jpg");
            _NamedThings.Add("Tricycle", "Images/Tricycle.jpg");
            _NamedThings.Add("Tugboat", "Images/Tugboat.jpg");
        }

        public string LoadFromPath(string newPath)
        {
            try
            {
                _NamedThings.Clear();

                var files = Directory.EnumerateFiles(newPath);

                // Set the new path
                Directory.SetCurrentDirectory(newPath);

                string folderPath = "";

                foreach (string currentFile in files)
                {
                    string filePath = currentFile;
                    if( IsImageFile(filePath) )
                    {
                        string name = Path.GetFileNameWithoutExtension(filePath);
                        _NamedThings.Add(name, filePath);
                    }

                    folderPath = filePath;
                }

                string folderName = "";
                if(folderPath.Count() > 0)
                    folderName = Path.GetFileName(Path.GetDirectoryName(folderPath));
                return folderName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }


        static public bool IsImageFile(string path)
        {
            string ext = Path.GetExtension(path);
            if ((ext != null) && ext.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
                        || ext.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
                        || ext.Equals(".bmp", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        static public bool FolderHasImageFiles(string folder)
        {
            var files = Directory.EnumerateFiles(folder);

            foreach (string currentFile in files)
            {
                if (IsImageFile(currentFile))
                    return true;
            }

            return false;
        }

    }
}
