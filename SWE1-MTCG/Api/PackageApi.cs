using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.Controller;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class PackageApi : IApi
    {
        private RequestContext _request;
        private PackageDTO _package;
        private PackageController _packageController;

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
            IPackageService packageService = new PackageService();
            _packageController = new PackageController(packageService);
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
            if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json")
            {
                return "POST ERR - Request not in JSON Format";
            }
            else if (string.IsNullOrWhiteSpace(_package.PackageId) || _package.CardIds.Count == 0)
            {
                return "POST ERR - No valid Package";
            }

            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "POST ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (usertoken != "admin-mtcgToken")
            {
                return "POST ERR - No admin rights";
            }

            return _packageController.CreatePackage(_package);
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
