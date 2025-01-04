namespace SmartMangement.Authentication.Domain.Models
{
    public class View
    {
        public int ViewId { get; set; }
        public string ViewName { get; set; }
        public string ViewUrl { get; set; }
        public string ViewIcon { get; set; }
        public bool IsMenuItem { get; set; }
        public string DisableControls { get; set; }
        public string HiddenControls { get; set; }
        public string AccessKey { get; set; }
        public List<View> items { get; set; }
    }
}
