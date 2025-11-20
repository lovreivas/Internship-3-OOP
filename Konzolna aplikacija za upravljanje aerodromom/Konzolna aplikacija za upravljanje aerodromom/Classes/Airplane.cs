using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom.Classes
{
    class Airplane
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int SeatsStandard { get; set; }
        public int SeatsBusiness { get; set; }
        public int SeatsVIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Airplane(string name, int year, int seatstand, int seatbuiss, int seatvip)
        {
            Id = Guid.NewGuid();
            Name = name;
            Year = year;
            SeatsStandard = seatstand;
            SeatsBusiness = seatbuiss;
            SeatsVIP = seatvip;
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        public string ShortInfo() => $"{Id} | {Name} | {Year} | Kapacitet: {SeatsStandard + SeatsBusiness + SeatsVIP}";
        public string FullInfo() => $"Id: {Id}\nNaziv: {Name}\nGodina: {Year}\nStandard: {SeatsStandard}\nBusiness: {SeatsBusiness}\nVIP: {SeatsVIP}\nKreirano: {CreatedAt}\nAžurirano: {UpdatedAt}";
    }
}
