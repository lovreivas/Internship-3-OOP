using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom.Classes
{
    class CrewMember
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public Position Position { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CrewMember(string firstn, string lastn, int year, Position pos)
        {
            Id = Guid.NewGuid();
            FirstName = firstn;
            LastName = lastn;
            BirthYear = year;
            Position = pos;
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        public string ShortInfo() => $"{Id} | {FirstName} {LastName} | {Position}";
    }
}
