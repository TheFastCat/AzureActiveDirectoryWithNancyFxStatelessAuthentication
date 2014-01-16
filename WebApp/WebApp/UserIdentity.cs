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
            // ignore Claims for now -- haven't received any help from the Nancy boys on how this should be used
            // to map claims provided via ADAL
        }

        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
