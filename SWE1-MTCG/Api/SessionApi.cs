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
    public class SessionApi : IApi
    {
        private RequestContext _request;
        private UserController _userController;
        private UserDTO _userdata;

        public SessionApi(RequestContext request)
        {
            _request = request;
            try
            {
                _userdata = JsonSerializer.Deserialize<UserDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _userdata = null;
            }
            IUserService userService = new UserService();
            _userController = new UserController(userService);
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
            else if (string.IsNullOrWhiteSpace(_userdata.Username) || string.IsNullOrWhiteSpace(_userdata.Password))
            {
                return "POST ERR - Username or Password are empty";
            }
            return _userController.Login(_userdata.Username, _userdata.Password);
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
