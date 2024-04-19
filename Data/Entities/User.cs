using Microsoft.AspNetCore.Identity;
namespace Data.Entities;
public class User : IdentityUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public bool IsEmailVerified { get; set; }
    public string AccountVerificationToken { get; set; }
    public IList<Booking> Bookings { get; set; }
    public IList<Review> Reviews { get; set; }
    public IList<Payment> Payments { get; set; }
}
