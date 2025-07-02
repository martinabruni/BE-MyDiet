using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyDiet.Auth.Business.Authorizations
{
    internal class AuthorizeUser : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; }

        public AuthorizeUser(string permissions)
        {
            Permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
