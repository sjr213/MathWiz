using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMvvmLib;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Word_Wiz
{
    public class WordWizVM : ViewModelBase<WordWizModel>
    {
        public WordWizVM(WordWizModel model)
        {
            Model = model;
        }

        private MediaPlayer _player = null;

        private ObservableCollection<WordSoundFile> _soundFiles = new ObservableCollection<WordSoundFile>();

        public ObservableCollection<WordSoundFile> SoundFiles
        {
            get
            {
                return _soundFiles;
            }

            set
            {
                _soundFiles = value;
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
                            var soundFiles = Model.LoadFromPath(path);
                            SoundFiles.Clear();

                            foreach (var file in soundFiles)
                                SoundFiles.Add(file);
                        }
                    }));
            }
        }

        private int _numberOfWords = 5;

        public int NumberOfWords
        {
            get { return _numberOfWords; }

            set
            {
                if (value < 2)
                    value = 2;

                SetProperty(ref _numberOfWords, value, () => NumberOfWords);
            }
        }

        // Word group and selection

        private ObservableCollection<WordSoundFile> _choiceFiles = new ObservableCollection<WordSoundFile>();

        public ObservableCollection<WordSoundFile> ChoiceFiles
        {
            get
            {
                return _choiceFiles;
            }

            set
            {
                SetProperty(ref _choiceFiles, value, () => ChoiceFiles);
            }
        }

        private WordSoundFile _chosen;

        public WordSoundFile Chosen
        {
            get
            {
                return _chosen;
            }

            set
            {
                SetProperty(ref _chosen, value, () => Chosen);
            }
        }

        private ICommand _nextCommand = null;

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand =
                    new RelayCommand(() =>
                    {
                        if(_soundFiles.Count() == 0)
                            return;
                        Chosen = Model.Chose(_soundFiles);
                        var choices = Model.GetChoices(_soundFiles, Chosen, NumberOfWords);

                        ChoiceFiles.Clear();
                        foreach (var file in choices)
                            ChoiceFiles.Add(file);

                        SelectedWord = null;

                        TryToPlaySound(Chosen.Path);
                    }));
            }
        }

        private ICommand _repeatCommand = null;

        public ICommand RepeatCommand
        {
            get
            {
                return _repeatCommand ?? (_repeatCommand =
                    new RelayCommand(() =>
                    {
                        if(Chosen != null)
                            TryToPlaySound(Chosen.Path);
                    }));
            }
        }

        private void TryToPlaySound(string filePath)
        {
            try
            {
                // see if file exists
                if (File.Exists(filePath))
                {
                    // play
                    Uri uri = new Uri(filePath, UriKind.RelativeOrAbsolute);

                    if (_player == null)
                        _player = new MediaPlayer();

                    if (_player != null)
                    {
                        _player.Open(uri);
                        _player.Play();
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        const string CorrectImageFile = "happyClown3.jpg";
        const string IncorrectImageFile = "sadClown3.jpg";
        const string NeutralImageFile = "books.png";
        private string _imagePathName = NeutralImageFile;

        public string ImagePathName
        {
            get
            {
                return _imagePathName;
            }

            set
            {
                SetProperty(ref _imagePathName, value, () => ImagePathName);
            }
        }

        private WordSoundFile _selectedWord = null;

        public WordSoundFile SelectedWord
        {
            get
            {
                return _selectedWord;
            }

            set
            {
                SetProperty(ref _selectedWord, value, () => SelectedWord);

                if (Chosen == null || _selectedWord == null)
                    ImagePathName = NeutralImageFile;
                else if (_selectedWord == Chosen)
                {
                    ImagePathName = CorrectImageFile;
                    _correctCount++;
                    _totalCount++;
                }
                else
                {
                    ImagePathName = IncorrectImageFile;
                    _totalCount++;
                }
                CalculatePercentCorrect();
            }
        }

        private int _correctCount = 0;
        private int _totalCount = 0;
        private int _percentCorrect = 0;

        void CalculatePercentCorrect()
        {
            if (_totalCount == 0)
                PercentCorrect = 0;
            else
            {
                PercentCorrect = (int)(100.0 * _correctCount) / _totalCount;
            }
        }

        public int PercentCorrect
        {
            get
            {
                return _percentCorrect;
            }

            set
            {
                SetProperty(ref _percentCorrect, value, () => PercentCorrect);
            }
        }

        private ICommand _resetPercentCorrectCommand = null;

        public ICommand ResetPercentCorrectCommand
        {
            get
            {
                return _resetPercentCorrectCommand ?? (_repeatCommand =
                    new RelayCommand(() =>
                    {
                        _correctCount = 0;
                        _totalCount = 0;
                        PercentCorrect = 0;
                    }));
            }
        }
    }
}
