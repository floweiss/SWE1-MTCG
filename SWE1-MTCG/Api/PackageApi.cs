using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class PackageApi : IApi
    {
        private RequestContext _request;
        private PackageDTO _package;

        public PackageApi(RequestContext request)
        {
            _request = request;
            try
            {
                _package = JsonSerializer.Deserialize<PackageDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _package = null;
            }
        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            return _package.Cards.Count.ToString();
        }

        public string GetMethod()
        {
            throw new NotImplementedException();
        }

        public string PutMethod()
        {
            throw new NotImplementedException();
        }

        public string DeleteMethod()
        {
            throw new NotImplementedException();
        }
    }
}
