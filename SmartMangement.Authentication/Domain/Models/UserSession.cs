namespace SmartMangement.Authentication.Domain.Models
{
    public class UserSession: UserContext
    {
        public Guid? SessionId { get; set; }
        public string AppName { get; set; }
        public string AppFullName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserProfileImage { get; set; }
        public List<UserRole> Roles { get; set; }
        public string AuthTokens { get; set; }
        public bool IsAuthenticated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClientName { get; set; }
        public List<View> MenuItems { get; set; }
        public UserSession()
        {


        }
        public UserSession(UserContext userContext)
        {
            SessionId = userContext.SessionId;
            AppName = userContext.AppName;
            AppFullName = userContext.AppFullName;
            UserId = userContext.UserId;
            UserName = userContext.UserName;
            UserFullName = userContext.UserFullName;
            UserProfileImage = userContext.UserProfileImage;
            Roles = userContext.Roles;
            AuthTokens = userContext.AuthTokens;
            IsAuthenticated = userContext.IsAuthenticated;
            FirstName = userContext.FirstName;
            LastName = userContext.LastName;
            Email = userContext.Email;
            Phone = userContext.Phone;
            ClientName = userContext.ClientName;
            MenuItems = userContext.MenuItems;
        }

    }
}
