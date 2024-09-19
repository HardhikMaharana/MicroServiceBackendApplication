using CurrentLoggedinUserDetails.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CurrentLoggedinUserDetails
{
    public class LoggedInUser
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public LoggedInUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public UserDetails User()
        {
            var ID= _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var NAME= _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString();
            UserDetails result = new UserDetails
            {
                Id = ID,
                //UserName=NAME
            };
            return result;
        }
    }
}
