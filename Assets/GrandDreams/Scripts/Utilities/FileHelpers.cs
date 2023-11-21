using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace GrandDreams.Core.Utilities
{
    public static class FileHelpers
    {

        public static void CreateDirectoryIfNotExist(this string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static long CalculateFileSize(this List<string> fileNames)
        {
            long sumSize = 0;

            for (int index = 0; index < fileNames.Count; index++)
            {
                string fileName = fileNames[index];
                if (File.Exists(fileName))
                {
                    FileInfo fi = new FileInfo(fileName);
                    sumSize += fi.Length;
                }
            }

            return sumSize;
        }

    }
}