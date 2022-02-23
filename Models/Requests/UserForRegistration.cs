using System.ComponentModel.DataAnnotations;

namespace JobPortalApi.Models.Requests
{
    public class UserForRegistration
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
