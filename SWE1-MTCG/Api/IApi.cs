using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SWE1_MTCG.Api
{
    public interface IApi
    {
        String Interaction();
        String PostMethod(String workingDir);
        String GetMethod(String workingDir);
        String PutMethod(String workingDir);
        String DeleteMethod(String workingDir);
    }
}
