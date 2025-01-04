using SmartMangement.Authentication.Domain.Models;

namespace SmartMangement.Authentication.Domain
{
    public interface ISession
    {
        void SetLoggedInUser(UserSession user);
        void SetLoggedInUser(UserContext user);
        bool UserHasRole(string roleCode);
        public UserSession loggedInUser { get; }
        public DateTime GetEST();

    }
}
