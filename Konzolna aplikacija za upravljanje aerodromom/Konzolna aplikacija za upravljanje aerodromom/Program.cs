using Konzolna_aplikacija_za_upravljanje_aerodromom.Classes;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var store = new DataStore();
            store.Seed();

            Console.WriteLine("=== PUTNICI ===");
            foreach (var p in store.Passengers)
                Console.WriteLine($"{p.Id} | {p.FirstName} {p.LastName} | {p.Email}");

            Console.WriteLine();

            Console.WriteLine("=== AVIONI ===");
            foreach (var a in store.Airplanes)
                Console.WriteLine(a.FullInfo());

            Console.WriteLine();

            Console.WriteLine("=== CLANOVI POSADE ===");
            foreach (var cm in store.CrewMembers)
                Console.WriteLine(cm.ShortInfo());

            Console.WriteLine();

            Console.WriteLine("=== POSADE ===");
            foreach (var c in store.Crews)
                Console.WriteLine(c.FullInfo());

            Console.WriteLine();

            Console.WriteLine("=== LETOVI ===");
            foreach (var f in store.Flights)
            {
                Console.WriteLine(f.FullInfo(store));
                Console.WriteLine("-----");
            }

            Console.WriteLine("TEST REZERVACIJA NA PRVOM LETU");
            var firstFlight = store.Flights[0];
            Console.WriteLine("Broj zauzetih rezervacija: " + firstFlight.Reservations.Count);
            Console.WriteLine("Dostupnih standard sjedala: " + firstFlight.GetAvailableSeatsByCategory(store, SeatCategory.Standard));
            Console.WriteLine("Let pun? " + firstFlight.IsFull(store));

            Console.ReadLine();
        }
    }
}
