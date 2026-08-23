using Microsoft.AspNetCore.Identity;
namespace AutoGestion.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = false; // Inactivo por defecto
}
