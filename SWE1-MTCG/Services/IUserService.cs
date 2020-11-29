using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG.Services
{
    public interface IUserService
    {
        void Register(User user);

        User Login(User user);

        bool isLoggedIn(User user);

        bool IsRegistered(User user);
    }
}
