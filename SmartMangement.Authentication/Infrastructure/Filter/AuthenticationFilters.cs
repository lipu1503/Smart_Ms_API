using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartMangement.Authentication.Domain;

namespace SmartMangement.Authentication.Infrastructure.Filter
{
    public class AuthenticationFilters: TypeFilterAttribute
    {
        public AuthenticationFilters(string role): base(typeof(AuthorizeActionFilter)) {
            Arguments = new object[] { };
        }
    }
    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _role;
        private readonly ISession _session;
        public AuthorizeActionFilter(ISession session , string? item)
        {
            _session = session;
            _role = item;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAutherized;
            if (_role == null || _role == string.Empty)
            {
                isAutherized = _session.loggedInUser.IsAuthenticated;
            }
            else
            {
                isAutherized = _session.UserHasRole(_role);
            }
            if (!isAutherized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
