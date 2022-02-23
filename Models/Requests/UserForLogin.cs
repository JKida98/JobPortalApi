using System.ComponentModel.DataAnnotations;

namespace JobPortalApi.Models.Requests
{
    public class UserForLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
