using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMvvmLib;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Threading;

namespace PictureWiz
{
    public class NamedImageVM : ViewModelBase<NamedImageModel>
    {
        private MediaPlayer _player = null;

        public NamedImageVM(NamedImageModel imageModel)
        {
            Model = imageModel;
            
            LoadCurrentDirectory();
            if (ImageFolders.Count() > 0)
            {
                CurrentFolder = ImageFolders[0];
            }
            else
            {
                SetDefaultImage();
                // need to add the path to the Current folder and update the collection
            }
        }

        private ObservableCollection<IImageFolder> _imageFolders;

        public ObservableCollection<IImageFolder> ImageFolders
        {
            get
            {
                return _imageFolders;
            }

            set
            {
                _imageFolders = value;
            }
        }

        private IImageFolder _currentFolder = null;

        public IImageFolder CurrentFolder 
        {
            get
            {
                return _currentFolder;
            }

            set
            {
                SetProperty(ref _currentFolder, value, () => CurrentFolder);
                LoadNewFolder(CurrentFolder.Folder);
            }
        }

        // New version
        private ObservableCollection<INamedImage> _theNamedImages;

        public ObservableCollection<INamedImage> TheNamedImages
        {
            get
            {
                if(_theNamedImages == null)
                {
                    var theNamedImages = new ObservableCollection<INamedImage>();
                    foreach (string name in Model.NamedThings.Keys)
                    {
                        string imagePath;
                        if (Model.NamedThings.TryGetValue(name, out imagePath))
                        {
                            theNamedImages.Add(new NamedImage(name, imagePath));
                        }
                    }

                    SetProperty(ref _theNamedImages, theNamedImages, () => TheNamedImages);
                }

                return _theNamedImages;
            }
        }

        private INamedImage _currentNamedImage;

        public INamedImage CurrentNamedImage
        {
            get
            {
                return _currentNamedImage;
            }

            set
            {
                if (value != null)
                {
                    KillTimer();

                    _currentNamedImage = value;
                    CurrentName = _currentNamedImage.Name;
                    
                    SetCurrentImage(_currentNamedImage.ImagePath);

                    SetProperty(ref _currentNamedImage, value, () => CurrentNamedImage);

                    if (!IsSpelling())
                    {
                        Spelling = _currentNamedImage.Name;
                        TryToPlaySound(_currentNamedImage.ImagePath);
                    }           
                }
            }
        }

        private BitmapImage _currentImage;

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }

            private set
            {
                SetProperty(ref _currentImage, value, () => CurrentImage);
            }
        }

        private string _currentName;

        public string CurrentName
        {
            get
            {
                return _currentName;
            }

            set
            {
 //               if (value == null || value.Count() == 0)
  //                  return;

                SetProperty(ref _currentName, value, () => CurrentName);
            }
        }

        private void SetDefaultImage()
        {
            var images = TheNamedImages;
            if (images == null || images.Count == 0)
                CurrentNamedImage = null;
            else
                CurrentNamedImage = images[0];
        }

        private void SetCurrentImage(string path)
        {
            try
            {
                var image = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                CurrentImage = image;
            }
            catch (System.Exception)
            {
            }
        }

        private string _currentFolderName;

        public string CurrentFolderName
        {
            get { return _currentFolderName; }

            private set
            {
                SetProperty(ref _currentFolderName, value, () => CurrentFolderName);
            }
        }

        private ICommand _loadCommand = null;

        public ICommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand =
                    new RelayCommand(() =>
                    {
                        var dialog = new System.Windows.Forms.FolderBrowserDialog();
                        dialog.SelectedPath = Directory.GetCurrentDirectory();
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            string path = dialog.SelectedPath;
                            CurrentFolderName = Model.LoadFromPath(path);
                            AddFolderToCollection(path, CurrentFolderName);
                        }
                    }));
            }
        }

        // This isn't currently used
        private ICommand _soundCommand = null;

        public ICommand SoundCommand
        {
            get
            {
                return _soundCommand ?? (_soundCommand =
                    new RelayCommand(
                        () =>
                        {
                            if (_currentNamedImage != null)
                            {
                                KillTimer();
                                Spelling = _currentNamedImage.Name;
                                TryToPlaySound(_currentNamedImage.ImagePath);
                            }
                        }
                    , 
                        () =>
                        {
                            return _currentNamedImage != null;
                        }
                       ));
            }
        }


        private string GetSoundFilePath(string path)
        {
            string folder;
            
            if(Path.GetPathRoot(path).Count()==0)
            {
                folder = Directory.GetCurrentDirectory();
            }
            else
                folder = Path.GetDirectoryName(path);

            string filename = Path.GetFileNameWithoutExtension(path);

            string newPath = folder;
            newPath += @"\";
            newPath += filename;
            newPath += ".wma";

            return newPath;
        }

        private void TryToPlaySound(string filePath)
        {
            try
            {
                // remove extension and add .wma
                string soundFile = GetSoundFilePath(filePath);

                // see if file exists
                if (File.Exists(soundFile))
                {
                    // play
                    Uri uri = new Uri(soundFile, UriKind.RelativeOrAbsolute);

                    if (_player == null)
                        _player = new MediaPlayer();

                    if (_player != null)
                    {
                        _player.Open(uri);
                        _player.Play();
                    }
                 }

                SpellName();
            }
            catch (System.Exception)
            {
            }
        }

        // Method to call on startup that looks for MyGroups in current folder and call load folders

        void LoadCurrentDirectory()
        {
            var currentFolder = Directory.GetCurrentDirectory();
            LoadFolders(currentFolder);
        }

        void LoadFolders(string path)
        {
             ObservableCollection<IImageFolder> folders = new ObservableCollection<IImageFolder>();

             ProcessFolder(ref folders, path);

             ImageFolders = folders;
        }

        void ProcessFolder(ref ObservableCollection<IImageFolder> folders, string path)
        {
            // If path has image files - add to folders
            string folderName = ReturnFolderNameIfItContainsImages(path);
            if (folderName != null && folderName.Count() > 0)
            {
                IImageFolder newFolder = new ImageFolder(folderName, path);
                folders.Add(newFolder);
            }

            // For each subfolder
            var directories = Directory.EnumerateDirectories(path);
            foreach(string directory in directories)
            {
                // call Process Folder
                ProcessFolder(ref folders, directory);
            }
        }

        string ReturnFolderNameIfItContainsImages(string path)
        {
            if (NamedImageModel.FolderHasImageFiles(path))
            {
                return Path.GetFileName(path);
            }

            return "";
        }

        void LoadNewFolder(string path)
        {
            CurrentFolderName = Model.LoadFromPath(path);
            _theNamedImages = null;
            SetDefaultImage();
        }

        void AddFolderToCollection(string path, string foldername)
        {
            foreach (IImageFolder oldFolder in ImageFolders)
            {
                if (oldFolder.Folder == path)
                {
                    CurrentFolder = oldFolder;
                    return;
                }
            }

            var newFolder = new ImageFolder(foldername, path);
            ImageFolders.Add(newFolder);
            CurrentFolder = newFolder;
        }

        // Spelling
        private string _spelling = "";
        private string _word = "";
        private int _letterCount = 0;
        private bool _KillingTimer = false;

        private System.Windows.Threading.DispatcherTimer _timer = null;

        public string Spelling
        {
            get
            {
                return _spelling;
            }

            set
            {
                SetProperty(ref _spelling, value, () => Spelling);
            }
        }

        bool IsSpelling()
        {
            return _timer != null;
        }

        void SpellName()
        {
            if (_timer != null)
                return;

            // make timer
            _timer = new System.Windows.Threading.DispatcherTimer();

            // set current letter
            _letterCount = 0;
            
            // set full spelling
            _word = _currentName;

            // start
            _timer.Tick += new EventHandler(dispatcherTimer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 1, 500);
            _KillingTimer = false;

            _timer.Start();
        }

        void KillTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
                _KillingTimer = true;
            }
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_KillingTimer)
            {
                _KillingTimer = false;
                return;
            }

            if (_letterCount == 0)
            {
                Spelling = "";
            }
            else if (_letterCount >= _word.Count() + 1)
            {
                KillTimer();
                return;
            }
            else
            {
                char letter = _word[_letterCount - 1];
                Spelling += letter;
                SayLetter(letter);
            }

            _letterCount++;
        }

        string GetLetterPath()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exePath = Path.GetDirectoryName(exePath);
            exePath += "/Letters/";

            return exePath;
        }

        void SayLetter(char letter)
        {
            char upperLetter = Char.ToUpper(letter);
            if (upperLetter >= 'A' && upperLetter <= 'Z')
            {
                string filename = GetLetterPath() + upperLetter + ".wma";

                // play
                Uri uri = new Uri(filename, UriKind.RelativeOrAbsolute);

                if (_player == null)
                    _player = new MediaPlayer();

                if (_player != null)
                {
                    _player.Open(uri);
                    _player.Play();
                }
            }

        }
    }
}
