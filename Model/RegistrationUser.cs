using System.ComponentModel.DataAnnotations;

namespace Ski_Service_Backend.Model
{
    public class RegistrationUser
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Priority { get; internal set; }
        public string Service { get; internal set; }
        public DateTime CreateDate { get; internal set; }
        public DateTime PickupDate { get; internal set; }
    }
}
