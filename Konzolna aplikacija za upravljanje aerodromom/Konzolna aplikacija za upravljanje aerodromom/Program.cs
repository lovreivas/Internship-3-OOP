using Konzolna_aplikacija_za_upravljanje_aerodromom.Classes;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Passenger("Ivan", "Ivić", 2001, "ivan@ivic", "1234");
            Console.WriteLine("Ime:{0},Prezime:{1},Godina:{2},Email:{3},Lozinka:{4},id:{5}\n",p.FirstName,p.LastName,p.BirthYear,p.Email,p.Password,p.Id);
            var a = new Airplane("Boing 747", 1990, 50, 20, 10);
            Console.WriteLine(a.FullInfo());
            var cm = new CrewMember("Marko", "Markić", 2000, Position.Pilot);
            var cm1 = new CrewMember("Karlo", "Karlić", 2001, Position.Copilot);
            var cm2 = new CrewMember("Ana", "Anić", 2002, Position.Stewardess);
            var cm3 = new CrewMember("Iva", "Ivić", 2003, Position.Stewardess);
            Console.WriteLine(cm.ShortInfo());
            var c = new Crew(cm.Id, cm1.Id,new List<Guid>{ cm2.Id, cm3.Id });
            Console.WriteLine(c.FullInfo());
            
        }
    }
}
