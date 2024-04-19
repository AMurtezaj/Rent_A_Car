namespace Data.Entities
{
    public class InsuranceOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
