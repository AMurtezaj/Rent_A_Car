using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DatePosted { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
