using Konzolna_aplikacija_za_upravljanje_aerodromom.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom
{
    class DataStore
    {
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public List<Airplane> Airplanes { get; set; } = new List<Airplane>();
        public List<CrewMember> CrewMembers { get; set; } = new List<CrewMember>();
        public List<Crew> Crews { get; set; } = new List<Crew>();
        public List<Flight> Flights { get; set; } = new List<Flight>();

        public void Seed()
        {
            var a1 = new Airplane("Boeing 737", 2015, 120, 20, 6);
            var a2 = new Airplane("Airbus A320", 2018, 140, 30, 10);

            Airplanes.Add(a1);
            Airplanes.Add(a2);

            var cm1 = new CrewMember("Ivan", "Peric", 1978, Position.Pilot);
            var cm2 = new CrewMember("Marko", "Ivic", 1982, Position.Copilot);
            var cm3 = new CrewMember("Ana", "Kovacic", 1990, Position.Stewardess);
            var cm4 = new CrewMember("Luka", "Horvat", 1992, Position.Steward);

            CrewMembers.AddRange(new[] { cm1, cm2, cm3, cm4 });

            var crew1 = new Crew(cm1.Id, cm2.Id, new List<Guid> { cm3.Id, cm4.Id });
            Crews.Add(crew1);

            var f1 = new Flight("Zagreb - London", DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(3), 1200, a1.Id, crew1.Id);
            var f2 = new Flight("Zagreb - Paris", DateTime.Now.AddDays(3), DateTime.Now.AddDays(3).AddHours(2), 900, a2.Id, crew1.Id);

            Flights.AddRange(new[] { f1, f2 });

            var p1 = new Passenger("Petar", "Novak", 1995, "petar@example.com", "pass1");
            var p2 = new Passenger("Maja", "Zoric", 1992, "maja@example.com", "pass2");

            Passengers.AddRange(new[] { p1, p2 });

            f1.Reservations.Add(new Reservation { PassengerId = p1.Id, Category = SeatCategory.Standard });
        }

        public int GetTotalCapacityForFlight(Flight f)
        {
            Airplane ap = null;

            foreach (var a in Airplanes)
            {
                if (a.Id == f.AirplaneId)
                {
                    ap = a;
                    break;
                }
            }

            if (ap == null) return 0;

            return ap.SeatsStandard + ap.SeatsBusiness + ap.SeatsVIP;
        }

        public bool IsMemberAssigned(Guid memberId)
        {
            foreach (var c in Crews)
            {
                if (c.PilotId == memberId) return true;
                if (c.CopilotId == memberId) return true;

                foreach (var st in c.Stewards)
                {
                    if (st == memberId)
                        return true;
                }
            }

            return false;
        }
    }
}
