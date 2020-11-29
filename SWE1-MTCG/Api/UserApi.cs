using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class UserApi : IApi
    {
        private RequestContext _request;

        public UserApi(RequestContext request)
        {
            _request = request;
        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                case "GET":
                    return GetMethod();

                case "PUT":
                    return PutMethod();

                case "DELETE":
                    return DeleteMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            return "POST Users";
        }

        public string GetMethod()
        {
            return "GET Users";
        }

        public string PutMethod()
        {
            return "PUT Users";
        }

        public string DeleteMethod()
        {
            return "DELETE Users";
        }
    }
}