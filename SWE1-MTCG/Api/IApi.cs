using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SWE1_MTCG.DataTransferObject;

namespace SWE1_MTCG.Api
{
    public interface IApi
    {
        string Interaction();
        string PostMethod();
        string GetMethod();
        string PutMethod();
        string DeleteMethod();
    }
}
