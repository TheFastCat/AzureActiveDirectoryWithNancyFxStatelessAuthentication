using Nancy.Security;
using System.Collections.Generic;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WebApp
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity(UserInfo userInfo)
        {
            UserName = userInfo.UserId;
        }

        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
