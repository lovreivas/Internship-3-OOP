using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom.Classes
{
    class Passenger
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Passenger(string first, string last, int year, string email, string password)
        {
            Id = Guid.NewGuid();
            FirstName = first;
            LastName = last;
            BirthYear = year;
            Email = email;
            Password = password;
            CreatedAt = UpdatedAt = DateTime.Now;
        }
    }
}
