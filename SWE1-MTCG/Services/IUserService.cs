using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public interface IUserService
    {
        string Register(User user);

        string Login(User user);

        bool isLoggedIn(RequestContext request);

        int IsRegistered(User user);

        string AquirePackage(string usertoken, string packageId);

        string ShowBio(string username);

        string EditBio(UserBioDTO userBio, string user);
    }
}
