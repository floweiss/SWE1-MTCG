using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public interface IApiService
    {
        IApi GetApi(RequestContext request);
    }
}
