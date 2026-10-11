namespace TrafficFineManagement.Web.Models.Entities
{
    public class Payment : BaseEntity
    {
        public int FineId { get; set; }
        public Fine Fine { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        
        public decimal Amount { get; set; }
        public string LastSixCardNumber { get; set; } = string.Empty;
    }
}