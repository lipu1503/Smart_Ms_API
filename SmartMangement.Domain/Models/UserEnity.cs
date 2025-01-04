namespace SmartMangement.Domain.Models
{
    public class UserEnity: Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool Active { get; set; }
        public string ClientId { get; set; }
    }
}
