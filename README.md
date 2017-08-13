# MathWiz
WPF programs to help young kids learn arithetic and spelling. It's aimed for kids between about 3 and 7.

The solution contains the following projects

Draw Wiz - Like MS Paint but simpler and could use more features.

Math Wiz - Provides random addition, subtraction, multiplication and division problems. 

MyMvvmLib - Contains some common code used by the other projects.

Picture Wiz - Shows pictures and when one is selected it displays the spelling and plays an audio file with pronunciation. 

Word Wiz - Displays a list of words and plays an audio file for one of the words. The user has the select the matching word.


Draw Wiz and Math Wiz can run as they are. Picture Wiz and Word Wiz need data files to do anything useful. Some default files are in the Data folder.

Picture Wiz expects pairs of files for each word/image. The image file should be in .jpg or .png format and the audiofile as .wma. The paired files should have the same name and that name is the word that will be displayed (i.e. cow.png and cow.wma). You can put a folder named MyGroups/ in the same directory as the .exe and it will load at start up or you can run the app and navigate to the folder after clicking the Load button.

Word Wiz - expects .wma files for each pronounced word. The name of the file will be displayed in the app. You can load a bunch of files by navigating to a folder by clicking the Words button. Sample files are found in \Data\WordWiz\MyWords.

You will probably want to record your own audio files. Mine sound dull and have a New York accent.
