using Data.Enums;

namespace Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public double TotalPrice { get; set; }
        public BookingStatus BookingStatuses { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
