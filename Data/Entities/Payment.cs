namespace Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
