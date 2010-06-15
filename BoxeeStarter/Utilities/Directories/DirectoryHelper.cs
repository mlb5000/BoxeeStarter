using System;
using System.IO;

namespace BoxeeStarter.Utilities.Directories
{
    public class DirectoryHelper
    {
        public virtual string GetProgramDirFor(string program)
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string dirPath = programFilesPath + "\\" + program;
            string dirPath64Bit = programFilesPath + " (x86)" + "\\" + program;

            if (Directory.Exists(dirPath))
            {
                return dirPath;
            }

            return Directory.Exists(dirPath64Bit) ? dirPath64Bit : null;
        }
    }
}