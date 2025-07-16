using Umbraco.Cms.Web.Common.Security;

namespace MahakaleswarumUjjayini.Models
{
    public class UserProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
