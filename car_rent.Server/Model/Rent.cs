﻿using System.Text.Json.Serialization;

namespace car_rent_api2.Server.Database
{
    public enum RentStatus
    {
        Reserved,
        Rented,
        Returned
    }
    public class Rent
    {
        public string Rent_ID { get; set; }
        public string RentId_in_company { get; set; }
        public DateTime Rent_date { get; set; }
        public DateTime Return_date { get; set; }
        public RentStatus Status { get; set; }
    
        public string Offer_ID { get; set; }
        // Navigation properties
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public Offer Offer { get; set; }
        public Company Company { get; set; }

    }
}
