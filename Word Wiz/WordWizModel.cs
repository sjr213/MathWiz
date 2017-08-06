using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Wiz
{
    public class WordWizModel
    {
        static public bool IsSoundFile(string path)
        {
            string ext = Path.GetExtension(path);
            if ((ext != null) && ext.Equals(".wma", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        public List<WordSoundFile> LoadFromPath(string newPath)
        {
            List<WordSoundFile> soundFile = new List<WordSoundFile>();

            try
            {
                var files = Directory.EnumerateFiles(newPath);

                // Set the new path
                Directory.SetCurrentDirectory(newPath);

                foreach (string currentFile in files)
                {
                    string filePath = currentFile;
                    if (IsSoundFile(filePath))
                    {
                        string name = Path.GetFileNameWithoutExtension(filePath);
                        soundFile.Add(new WordSoundFile(name, filePath));
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return soundFile;
        }

        public WordSoundFile Chose(ObservableCollection<WordSoundFile> values)
        {
            int nChoices = values.Count();
            Random ran = new Random();

            int nChoice = ran.Next(nChoices);

            return values[nChoice];
        }

        protected bool IsSoundFileAlreadyInCollection(List<WordSoundFile> results, WordSoundFile newFile)
        {
            foreach (var file in results)
            {
                if (file.Name == newFile.Name)
                    return true;
            }

            return false;
        }

        public List<WordSoundFile> GetChoices(ObservableCollection<WordSoundFile> values, WordSoundFile choice, int numberOfChoices)
        {
            List<WordSoundFile> results = new List<WordSoundFile>();

            if (choice == null || choice.Name.Count() == 0 || values.Count() == 0)
                return results;

            // Get the first letter
            char letter = choice.Name[0];

            choice.Chosen = true;

            // Add the choice
            results.Add(choice);
            int nCount = 1;

            // look for other choices that start with the same letter
            for (int i = 0; i < values.Count() && nCount < numberOfChoices; ++i)
            {
                if (values[i].Name == choice.Name)
                    continue;

                if (values[i].Name.Count() > 0 && values[i].Name[0] == letter)
                {
                    results.Add(values[i]);
                    ++nCount;
                }
            }

            // If we don't have enough add more
            // make sure we don't end up in an infinite loop for some reason
            int nTrials = 0; 
            Random ran = new Random();

            while (nCount < numberOfChoices && nTrials < 1000)
            {
                ++nTrials;

                int index = ran.Next(values.Count());

                var test = values[index];
                if (test.Name.Count() == 0 || test.Name[0] == letter)
                    continue;

                if (! IsSoundFileAlreadyInCollection(results, test))
                {
                    results.Add(test);
                    ++nCount;
                }
            }

            // sort alphabetically
            results = results.OrderBy(f => f.Name).ToList();

            return results;
        }
    }
}
