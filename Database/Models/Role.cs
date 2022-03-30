using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobPortalApi.Database.Models;

public class Role : IdentityRole<Guid>
{
}