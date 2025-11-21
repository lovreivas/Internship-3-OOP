using Konzolna_aplikacija_za_upravljanje_aerodromom.Classes;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom
{
    internal class Program
    {
        static DataStore Store = new DataStore();
        static void Main(string[] args)
        {
            Store.Seed();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Glavni izbornik:\n1 - Putnici\n2 - Letovi\n3 - Avioni\n4 - Posada\n5 - Izlaz iz programa");
                var key = Console.ReadLine();
                if (key == "1") PassengersMenu();
                else if (key == "2") FlightsMenu();
                else if (key == "3") ;// AirplanesMenu();
                else if (key == "4") ;// CrewsMenu();
                else if (key == "5") break;
            }
        }

        static void PassengersMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Putnici:\n1 - Registracija\n2 - Prijava\n3 - Povratak");
                var k = Console.ReadLine();
                if (k == "1") RegisterPassenger();
                else if (k == "2") LoginPassenger();
                else if (k == "3") break;
            }
        }

        static void RegisterPassenger()
        {
            Console.Clear();
            Console.WriteLine("Registracija - unesite podatke:");
            var first = Input.ReadName("Ime: ");
            var last = Input.ReadName("Prezime: ");
            var year = Input.ReadIntRange("Godina rođenja: ", 1900, DateTime.Now.Year);
            var email = Input.ReadEmail("Email: ");
            var pass = Input.ReadNonEmpty("Lozinka: ");
            var p = new Passenger(first, last, year, email, pass);
            Console.Write("Želite li stvarno registrirati korisnika y/n? ");
            if (Confirm())
            {
                Store.Passengers.Add(p);
                Console.WriteLine("Uspješno registrirano. Id: " + p.Id);
            }
            else Console.WriteLine("Prekinuto.");
            Input.Pause();
        }

        static Passenger FindPassengerByEmailAndPassword(string email, string pass)
        {
            foreach (var p in Store.Passengers)
            {
                if (p.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && p.Password == pass) return p;
            }
            return null;
        }

        static void LoginPassenger()
        {
            Console.Clear();
            var email = Input.ReadNonEmpty("Email: ");
            var pass = Input.ReadNonEmpty("Lozinka: ");
            var p = FindPassengerByEmailAndPassword(email, pass);
            while (p == null)
            {
                Console.WriteLine("Prijava neuspješna, pokušajte ponovno.");
                Console.Write("1 - Pokušaj ponovno, 2 - Povratak: ");
                var c = Console.ReadLine();
                if (c == "2") return;
                email = Input.ReadNonEmpty("Email: ");
                pass = Input.ReadNonEmpty("Lozinka: ");
                p = FindPassengerByEmailAndPassword(email, pass);
            }
            PassengerMenu(p);
        }

        static List<Flight> GetFlightsReservedByPassenger(Guid passengerId)
        {
            var result = new List<Flight>();
            foreach (var f in Store.Flights)
            {
                foreach (var r in f.Reservations)
                {
                    if (r.PassengerId == passengerId)
                    {
                        result.Add(f);
                        break;
                    }
                }
            }
            return result;
        }

        static List<Flight> GetAvailableFlights()
        {
            var result = new List<Flight>();
            foreach (var f in Store.Flights)
            {
                if (!f.IsFull(Store)) result.Add(f);
            }
            return result;
        }

        static Flight FindFlightById(Guid id)
        {
            foreach (var f in Store.Flights)
            {
                if (f.Id == id) return f;
            }
            return null;
        }

        static List<Flight> FindFlightsByNameContains(string substring)
        {
            var list = new List<Flight>();
            var user = substring.Replace(" ", "").ToLower();

            foreach (var f in Store.Flights)
            {
                var name = f.Name.Replace(" ", "").ToLower();
                if (name.Contains(user)) list.Add(f);
            }

            return list;
        }


        static void PassengerMenu(Passenger p)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Dobrodošli {p.FirstName} {p.LastName}:\n1 - Prikaz svih rezerviranih letova\n2 - Odabir leta\n3 - Pretraživanje letova\n4 - Otkazivanje leta\n5 - Povratak");
                var k = Console.ReadLine();
                if (k == "1")
                {
                    Console.Clear();
                    var flights = GetFlightsReservedByPassenger(p.Id);
                    if (flights.Count == 0) Console.WriteLine("Nema rezervacija.");
                    else
                    {
                        foreach (var f in flights) Console.WriteLine(f.ShortInfo());
                    }
                    Input.Pause();
                }
                else if (k == "2")
                {
                    Console.Clear();
                    var available = GetAvailableFlights();
                    if (available.Count == 0) { Console.WriteLine("Nema dostupnih letova."); Input.Pause(); continue; }
                    foreach (var f in available) Console.WriteLine(f.ShortInfo());
                    var id = Input.ReadGuid("Unesite id leta za rezervaciju: ");
                    var flight = FindFlightById(id);
                    bool found = false;
                    foreach (var f in available) if (f.Id == id) { found = true; break; }
                    if (!found) { Console.WriteLine("Let nije pronađen ili nije dostupan."); Input.Pause(); continue; }
                    Console.WriteLine("Dostupne kategorije i preostala mjesta:");
                    foreach (SeatCategory cat in Enum.GetValues(typeof(SeatCategory)))
                    {
                        var cap = flight.GetAvailableSeatsByCategory(Store, cat);
                        if (cap > 0) Console.WriteLine($"{cat} - {cap}");
                    }
                    var catStr = Input.ReadNonEmpty("Odaberite kategoriju (Standard/Business/VIP): ");
                    if (!Enum.TryParse<SeatCategory>(catStr, true, out var chosen)) { Console.WriteLine("Kriva kategorija."); Input.Pause(); continue; }
                    if (flight.GetAvailableSeatsByCategory(Store, chosen) <= 0) { Console.WriteLine("Nema mjesta u toj kategoriji."); Input.Pause(); continue; }
                    Console.Write("Želite li stvarno rezervirati y/n? ");
                    if (!Confirm()) { Console.WriteLine("Prekinuto."); Input.Pause(); continue; }
                    flight.Reservations.Add(new Reservation { PassengerId = p.Id, Category = chosen });
                    flight.UpdatedAt = DateTime.Now;
                    Console.WriteLine("Uspješno rezervirano.");
                    Input.Pause();
                }
                else if (k == "3")
                {
                    Console.Clear();
                    Console.WriteLine("1 - Po id-u\n2 - Po nazivu\n3 - Povratak");
                    var c = Console.ReadLine();
                    if (c == "1")
                    {
                        var id = Input.ReadGuid("Unesite id: ");
                        var f = FindFlightById(id);
                        if (f == null) Console.WriteLine("Ne postoji."); else Console.WriteLine(f.FullInfo(Store));
                        Input.Pause();
                    }
                    else if (c == "2")
                    {
                        var name = Input.ReadNonEmpty("Unesite naziv: ");
                        var list = FindFlightsByNameContains(name);
                        if (list.Count == 0) Console.WriteLine("Ne postoji."); else foreach (var f in list) Console.WriteLine(f.FullInfo(Store));
                        Input.Pause();
                    }
                }
                else if (k == "4")
                {
                    Console.Clear();
                    var myFlights = GetFlightsReservedByPassenger(p.Id);
                    if (myFlights.Count == 0) { Console.WriteLine("Nema rezervacija."); Input.Pause(); continue; }
                    foreach (var f in myFlights) Console.WriteLine(f.ShortInfo());
                    var id = Input.ReadGuid("Unesite id leta za otkaz: ");
                    Flight flight = null;
                    foreach (var f in myFlights) if (f.Id == id) { flight = f; break; }
                    if (flight == null) { Console.WriteLine("Let nije pronađen među vašim rezervacijama."); Input.Pause(); continue; }
                    if ((flight.Departure - DateTime.Now).TotalHours < 24) { Console.WriteLine("Ne možete otkazati let koji je u sljedećih 24h."); Input.Pause(); continue; }
                    Console.Write("Želite li stvarno otkazati rezervaciju y/n? ");
                    if (!Confirm()) { Console.WriteLine("Prekinuto."); Input.Pause(); continue; }
                    Reservation foundRes = null;
                    foreach (var r in flight.Reservations) if (r.PassengerId == p.Id) { foundRes = r; break; }
                    if (foundRes != null) flight.Reservations.Remove(foundRes);
                    flight.UpdatedAt = DateTime.Now;
                    Console.WriteLine("Uspješno otkazano.");
                    Input.Pause();
                }
                else if (k == "5") break;
            }
        }
        static void FlightsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Letovi:\n1 - Prikaz svih letova\n2 - Dodavanje leta\n3 - Pretraživanje letova\n4 - Uređivanje leta\n5 - Brisanje leta\n6 - Povratak");
                var k = Console.ReadLine();
                if (k == "1")
                {
                    Console.Clear();
                    foreach (var f in Store.Flights) Console.WriteLine(f.ShortInfo());
                    Input.Pause();
                }
                else if (k == "2")
                {
                    Console.Clear();
                    var name = Input.ReadRoute("Naziv leta (start-end): ");
                    Console.WriteLine("Odaberite avion:");
                    foreach (var a in Store.Airplanes) Console.WriteLine(a.ShortInfo());
                    var aid = Input.ReadGuid("Unesite id aviona: ");
                    Airplane airplane = null;
                    foreach (var a in Store.Airplanes) if (a.Id == aid) { airplane = a; break; }
                    if (airplane == null) { Console.WriteLine("Avion nije pronađen."); Input.Pause(); continue; }
                    DateTime dep;
                    DateTime arr;
                    while (true)
                    {
                        dep = Input.ReadDateTime("Vrijeme polaska (yyyy-MM-dd HH:mm): ");
                        arr = Input.ReadDateTime("Vrijeme dolaska (yyyy-MM-dd HH:mm): ");

                        if (arr > dep) break;

                        Console.WriteLine("Vrijeme dolaska mora biti nakon vremena polaska. Pokušajte ponovno.");
                    }
                    var dist = Input.ReadIntRange("Udaljenost (km): ", 1, 100000);
                    Console.WriteLine("Odaberite posadu: ");
                    foreach (var c in Store.Crews) Console.WriteLine(c.ShortInfo());
                    var cid = Input.ReadGuid("Unesite id posade: ");
                    Crew crew = null;
                    foreach (var c in Store.Crews) if (c.Id == cid) { crew = c; break; }
                    if (crew == null) { Console.WriteLine("Posada nije pronađena."); Input.Pause(); continue; }
                    Console.Write("Želite li stvarno dodati let y/n? ");
                    if (!Confirm()) { Console.WriteLine("Prekinuto."); Input.Pause(); continue; }
                    var flight = new Flight(name, dep, arr, dist, airplane.Id, crew.Id);
                    Store.Flights.Add(flight);
                    Console.WriteLine("Let dodan, id: " + flight.Id);
                    Input.Pause();
                }
                else if (k == "3")
                {
                    Console.Clear();
                    Console.WriteLine("1 - Po id-u\n2 - Po nazivu\n3 - Povratak");
                    var c = Console.ReadLine();
                    if (c == "1")
                    {
                        var id = Input.ReadGuid("Unesite id: ");
                        var f = FindFlightById(id);
                        if (f == null) Console.WriteLine("Ne postoji."); else Console.WriteLine(f.FullInfo(Store));
                        Input.Pause();
                    }
                    else if (c == "2")
                    {
                        var name = Input.ReadNonEmpty("Unesite naziv: ");
                        var list = FindFlightsByNameContains(name);
                        if (list.Count == 0) Console.WriteLine("Ne postoji."); else foreach (var f in list) Console.WriteLine(f.FullInfo(Store));
                        Input.Pause();
                    }
                }
                else if (k == "4")
                {
                    Console.Clear();
                    foreach (var f in Store.Flights) Console.WriteLine(f.ShortInfo());
                    var id = Input.ReadGuid("Unesite id leta za uređivanje: ");
                    var flight = FindFlightById(id);
                    if (flight == null) { Console.WriteLine("Ne postoji."); Input.Pause(); continue; }
                    DateTime dep;
                    DateTime arr;
                    while (true)
                    {
                        dep = Input.ReadDateTime("Novo vrijeme polaska (yyyy-MM-dd HH:mm): ");
                        arr = Input.ReadDateTime("Novo vrijeme dolaska (yyyy-MM-dd HH:mm): ");

                        if (arr > dep) break;

                        Console.WriteLine("Vrijeme dolaska mora biti nakon vremena polaska. Pokušajte ponovno.");
                    }
                    Console.WriteLine("Dostupne posade:");
                    foreach (var c in Store.Crews) Console.WriteLine(c.ShortInfo());
                    var cid = Input.ReadGuid("Unesite id nove posade: ");
                    Crew crew = null;
                    foreach (var c in Store.Crews) if (c.Id == cid) { crew = c; break; }
                    if (crew == null) { Console.WriteLine("Posada nije pronađena."); Input.Pause(); continue; }
                    Console.Write("Želite li stvarno urediti let y/n? ");
                    if (!Confirm()) { Console.WriteLine("Prekinuto."); Input.Pause(); continue; }
                    flight.Departure = dep;
                    flight.Arrival = arr;
                    flight.CrewId = crew.Id;
                    flight.UpdatedAt = DateTime.Now;
                    Console.WriteLine("Uređeno.");
                    Input.Pause();
                }
                else if (k == "5")
                {
                    Console.Clear();
                    foreach (var f in Store.Flights) Console.WriteLine(f.ShortInfo());
                    var id = Input.ReadGuid("Unesite id leta za brisanje: ");
                    var flight = FindFlightById(id);
                    if (flight == null) { Console.WriteLine("Ne postoji."); Input.Pause(); continue; }
                    var totalCap = Store.GetTotalCapacityForFlight(flight);
                    int resCount = flight.Reservations.Count;
                    if (resCount >= totalCap / 2) { Console.WriteLine("Ne možete izbrisati let jer je više od 50% zauzeto."); Input.Pause(); continue; }
                    if ((flight.Departure - DateTime.Now).TotalHours < 24) { Console.WriteLine("Ne možete izbrisati let koji polijeće za manje od 24h."); Input.Pause(); continue; }
                    Console.Write("Želite li stvarno izbrisati let y/n? ");
                    if (!Confirm()) { Console.WriteLine("Prekinuto."); Input.Pause(); continue; }
                    Store.Flights.Remove(flight);
                    Console.WriteLine("Let izbrisan.");
                    Input.Pause();
                }
                else if (k == "6") break;
            }
        }
        static bool Confirm()
        {
            var r = Console.ReadLine();
            return r?.Trim().ToLower() == "y";
        }
    }
}
