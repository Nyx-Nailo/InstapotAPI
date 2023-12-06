using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InstapotAPI.Entity
{
    public class Profile
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePicture {  get; set; }
        public bool IsVerified { get; set; }
        public List<int> Images { get; set; } = new List<int>();
        public List<int> Comments { get; set; } = new List<int>(); 
    }
}
