using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class MessageController
    {
        private IFileService _fileService;

        public MessageController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /*
        public string ReadFromFile()*/
    }
}
