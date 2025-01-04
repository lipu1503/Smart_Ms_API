using SmartMangement.Authentication.Domain;
using SmartMangement.Authentication.Domain.Models;

namespace SmartMangement.Authentication.Infrastructure
{
    public class Session : ISession
    {

        public UserSession _loggedInUser;
        public UserSession loggedInUser => GetLoggedInUser();
        readonly static TimeZoneInfo estInfo = TimeZoneInfo.FindSystemTimeZoneById("Estern Standard Time");

        private UserSession GetLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEST()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, estInfo);
        }

        public void SetLoggedInUser(UserSession user)
        {
            _loggedInUser = user;
        }

        public void SetLoggedInUser(UserContext user)
        {
            _loggedInUser = new UserSession(user);
            //_loggedInUser.SetRoleProperties();
        }

        public bool UserHasRole(string roleCode)
        {
            if (_loggedInUser == null) return false;
            bool containsRole = _loggedInUser.Roles.Any(x => x.RoleCode == roleCode);
            return containsRole;
        }
    }
}
