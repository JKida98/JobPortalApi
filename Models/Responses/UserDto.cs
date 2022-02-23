using System;

namespace JobPortalApi.Models.Responses
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string ProfileDescription { get; set; }
    }
}
