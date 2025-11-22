using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom.Classes
{
    class Reservation
    {
        public Guid PassengerId { get; set; }
        public SeatCategory Category { get; set; }
    }

    class Flight
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public double DurationHours => (Arrival - Departure).TotalHours;
        public int DistanceKm { get; set; }
        public Guid AirplaneId { get; set; }
        public Guid CrewId { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Flight(string name, DateTime dep, DateTime arr, int distance, Guid airplaneId, Guid crewId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Departure = dep;
            Arrival = arr;
            DistanceKm = distance;
            AirplaneId = airplaneId;
            CrewId = crewId;
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        public string ShortInfo()
        {
            return $"{Id} | {Name} | Polazak: {Departure} | Dolazak: {Arrival} | Trajanje: {DurationHours:F1}h | Dist: {DistanceKm}km";
        }


        public string FullInfo(DataStore store)
        {
            Airplane ap = null;
            foreach (var a in store.Airplanes)
            {
                if (a.Id == AirplaneId)
                {
                    ap = a;
                    break;
                }
            }

            Crew cr = null;
            foreach (var c in store.Crews)
            {
                if (c.Id == CrewId)
                {
                    cr = c;
                    break;
                }
            }

            return $"Id: {Id}\nNaziv: {Name}\nPolazak: {Departure}\nDolazak: {Arrival}\nTrajanje: {DurationHours:F1}h\nUdaljenost: {DistanceKm}km\nAvion: {ap?.Name}\nPosada: {cr?.Id}\nBroj rezervacija: {Reservations.Count}";
        }

        public bool IsFull(DataStore store)
        {
            int cap = store.GetTotalCapacityForFlight(this);
            return Reservations.Count >= cap;
        }

        public int GetAvailableSeatsByCategory(DataStore store, SeatCategory cat)
        {
            Airplane ap = null;
            foreach (var a in store.Airplanes)
            {
                if (a.Id == AirplaneId)
                {
                    ap = a;
                    break;
                }
            }

            if (ap == null) return 0;

            int cap = 0;
            switch (cat)
            {
                case SeatCategory.Standard:
                    cap = ap.SeatsStandard;
                    break;
                case SeatCategory.Business:
                    cap = ap.SeatsBusiness;
                    break;
                case SeatCategory.VIP:
                    cap = ap.SeatsVIP;
                    break;
                default:
                    cap = 0;
                    break;
            }

            int taken = 0;
            foreach (var r in Reservations)
            {
                if (r.Category == cat)
                    taken++;
            }

            return cap - taken;
        }
    }
}
