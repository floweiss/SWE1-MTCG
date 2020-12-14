using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class UserController
    {
        private IUserService _userService;
        public User User { get; set; }

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public string Register(string username, string password)
        {
            User user = new User(username, password);
            return _userService.Register(user);
        }

        public string Login(string username, string password)
        {
            User user = new User(username, password);

            if (_userService.IsRegistered(user))
            {
                return _userService.Login(user);
            }

            return "POST ERR - Login failed";
        }

        public string AquirePackage(string usertoken, AquirePackageDTO package)
        {
            return _userService.AquirePackage(usertoken, package.PackageId);
        }

        public string ShowBio(string username)
        {
            return _userService.ShowBio(username);
        }

        public string EditBio(UserBioDTO userBio, string user)
        {
            return _userService.EditBio(userBio, user);
        }
    }
}
