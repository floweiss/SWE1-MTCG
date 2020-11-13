using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SWE1_MTCG.Services
{
    public class FileService : IFileService
    {
        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public bool DirectoryExists(string dirname)
        {
            return Directory.Exists(dirname);
        }

        public void CreateDirectory(string dirname)
        {
            if (!DirectoryExists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
        }
        public string[] GetFilesInDir(string dirname)
        {
            return Directory.GetFiles(dirname);
        }

        public string ReadFromFile(string filename)
        {
            return File.ReadAllText(filename);
        }

        public void WriteToFile(string filename, string content)
        {
            File.WriteAllText(filename, content);
        }

        public string ReadAllFilesInDir(string dirname)
        {
            string files = "";

            foreach (var filename in GetFilesInDir(dirname))
            {
                if (!filename.EndsWith("Number.txt"))
                {
                    int lastSlash = filename.LastIndexOf('\\');
                    files = files + filename.Substring(lastSlash + 1) + ": " + ReadFromFile(filename) +
                            "\n";
                }
            }

            if (files == "" || files == null)
            {
                files = "No messages sent yet!";
            }

            return files;
        }

        public void DeleteFile(string filename)
        {
            File.Delete(filename);
        }
    }
}
