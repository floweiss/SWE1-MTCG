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
        private UserDTO _userdata;
        private UserBioDTO _userBio;

        public UserApi(RequestContext request)
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

            try
            {
                _userBio = JsonSerializer.Deserialize<UserBioDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _userBio = null;
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

                case "GET":
                    return GetMethod();

                case "PUT":
                    return PutMethod();

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
            return _userController.Register(_userdata.Username, _userdata.Password);
        }

        public string GetMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "GET ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "GET ERR - Not logged in";
            }

            string user = _request.RequestedResource.Substring(7, _request.RequestedResource.Length - 7);
            return _userController.ShowBio(user);
        }

        public string PutMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "PUT ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "PUT ERR - Not logged in";
            }

            string user = _request.RequestedResource.Substring(7, _request.RequestedResource.Length - 7);
            if (usertoken.Substring(0, usertoken.IndexOf('-')) != user)
            {
                return "PUT ERR - Can't edit data from other user";
            }

            return _userController.EditBio(_userBio, user);
        }

        public string DeleteMethod()
        {
            throw new NotImplementedException();
        }
    }
}