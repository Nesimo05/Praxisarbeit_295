namespace Ski_Service_Backend.Dto
{
    public class RegistrationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Priority { get; set; }
        public string Service { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PickupDate { get; set; }
    }
}
