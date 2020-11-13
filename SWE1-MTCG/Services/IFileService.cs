using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SWE1_MTCG.Services
{
    public interface IFileService
    {
        bool FileExists(string filename);
        bool DirectoryExists(string dirname);
        void CreateDirectory(string dirname);
        string ReadFromFile(string filename);
        void WriteToFile(string filename, string content);
        string ReadAllFilesInDir(string dirname);
        void DeleteFile(string filename);
    }
}
