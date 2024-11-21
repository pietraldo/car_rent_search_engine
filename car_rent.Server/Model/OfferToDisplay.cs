﻿namespace car_rent.Server.Model
{
    public class OfferToDisplay
    {
        public Guid Id { get; set; }
        public Car Car { get; set; }
        public int ClientId { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}