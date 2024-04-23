namespace Data.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Speed { get; set; }
        public string Color { get; set; }
        public string TransmissionType { get; set; }
        public string FuelType { get; set; }
        public int Seats { get; set; }
        public double PricePerDay { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<CarBooking> CarBookings { get; set; }
        public IList<InsuranceOption> InsuranceOptions { get; set; }
        public IList<Offer> Offers { get; set; }

    }
}
