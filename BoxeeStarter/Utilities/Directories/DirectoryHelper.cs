using System;
using System.IO;
using BoxeeStarter.Utilities.Logging;

namespace BoxeeStarter.Utilities.Directories
{
    public class DirectoryHelper
    {
        public virtual string GetProgramDirFor(string program)
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string dirPath = programFilesPath + "\\" + program;
            EventLogger logger = new EventLogger();
            logger.Log(String.Format("DirPath 1: {0}", dirPath));

            if (Directory.Exists(dirPath))
                return dirPath;

            dirPath = programFilesPath + " (x86)\\" + program;
            logger.Log(String.Format("DirPath2: {0}", dirPath));

            if (Directory.Exists(dirPath))
                return dirPath;

            return null;
        }
    }
}