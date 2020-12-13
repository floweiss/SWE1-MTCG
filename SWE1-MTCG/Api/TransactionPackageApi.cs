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
    public class TransactionPackageApi : IApi
    {
        private RequestContext _request;
        private AquirePackageDTO _packageID;
        private UserController _userController;
        private string _usertoken;

        public TransactionPackageApi(RequestContext request)
        {
            _request = request;
            try
            {
                _packageID = JsonSerializer.Deserialize<AquirePackageDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _packageID = null;
            }
            IUserService userService = new UserService();
            _userController = new UserController(userService);
            _usertoken = null;
        }

        public string Interaction()
        {
            if (!_request.CustomHeader.TryGetValue("Authorization", out _usertoken))
            {
                return "POST ERR - No authorization header";
            }

            _usertoken = _usertoken.Substring(6, _usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(_usertoken))
            {
                return "POST ERR - Not logged in";
            }
            
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
            else if (string.IsNullOrWhiteSpace(_packageID.PackageId))
            {
                return "POST ERR - No valid PackageId";
            }

            return _userController.AquirePackage(_usertoken, _packageID);
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
