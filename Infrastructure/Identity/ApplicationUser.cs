using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; }
    [Required, MaxLength(100)]
    public string LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }
}

