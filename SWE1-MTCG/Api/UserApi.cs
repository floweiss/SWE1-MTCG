using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SWE1_MTCG.Controller;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class UserApi : IApi
    {
        private RequestContext _request;
        private UserController _userController;

        public UserApi(RequestContext request)
        {
            _request = request;
            IUserService userService = new UserService();
            _userController = new UserController(userService);
        }

        public string Interaction()
        {
            UserDTO userdata = JsonSerializer.Deserialize<UserDTO>(_request.Content);
            switch (_request.HttpMethod)
            {
                case "POST":
                    if (!_request.CustomHeader.ContainsKey("Content-Type") || _request.CustomHeader["Content-Type"] != "application/json")
                    {
                        return "POST ERR - Request not in JSON Format";
                    }
                    else if (string.IsNullOrWhiteSpace(userdata.Username) || string.IsNullOrWhiteSpace(userdata.Password))
                    {
                        return "POST ERR - Username or Password are empty";
                    }
                    return _userController.Register(userdata.Username, userdata.Password);

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