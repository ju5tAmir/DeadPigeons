using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Balance { get; set; } = 0; 
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true; 
    public bool IsAutoPlay { get; set; } = false; 
}