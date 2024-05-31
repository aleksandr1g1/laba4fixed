using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiService
{
    public class Trip
    {
        public string CarNumber { get; set; }
        public decimal TripCost { get; set; }

        public Trip(string carNumber, decimal tripCost)
        {
            CarNumber = ValidateCarNumber(carNumber);
            TripCost = ValidateTripCost(tripCost);
        }

        private string ValidateCarNumber(string carNumber)
        {
            if (string.IsNullOrWhiteSpace(carNumber) || carNumber.Length != 7)
                throw new ArgumentException("Car number must be a non-empty string of exactly 7 characters.");
            return carNumber;
        }

        private decimal ValidateTripCost(decimal tripCost)
        {
            if (tripCost <= 0)
                throw new ArgumentOutOfRangeException(nameof(tripCost), "Trip cost must be a positive decimal number.");
            return tripCost;
        }
    }

    public class TaxiService
    {
        private List<Trip> trips = new List<Trip>();

        public void AddTrip(Trip trip)
        {
            trips.Add(trip);
        }

        public IEnumerable<KeyValuePair<string, decimal>> CalculateTotalIncomeByCar()
        {
            return trips
                .GroupBy(trip => trip.CarNumber)
                .Select(group => new KeyValuePair<string, decimal>(group.Key, group.Sum(trip => trip.TripCost)));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var taxiService = new TaxiService();

            taxiService.AddTrip(new Trip("AB12345", 100.5m));
            taxiService.AddTrip(new Trip("CD23456", 150.0m));
            taxiService.AddTrip(new Trip("AB12345", 120.0m));
            taxiService.AddTrip(new Trip("EF34567", 200.0m));
            taxiService.AddTrip(new Trip("AB12345", 90.0m));

            var incomes = taxiService.CalculateTotalIncomeByCar();
            foreach (var income in incomes)
            {
                Console.WriteLine($"{income.Key}: {income.Value:C}");
            }
        }
    }
}