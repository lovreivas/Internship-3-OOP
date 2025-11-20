using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom.Classes
{
    class Crew
    {
        public Guid Id { get; set; }
        public Guid PilotId { get; set; }
        public Guid CopilotId { get; set; }
        public List<Guid> Stewards { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Crew(Guid pilot, Guid copilot, List<Guid> stewards)
        {
            Id = Guid.NewGuid();
            PilotId = pilot;
            CopilotId = copilot;
            Stewards = stewards;
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        public string ShortInfo() => $"{Id} | Pilot: {PilotId} | Copilot: {CopilotId} | Steward(esa): {Stewards.Count}";

        public string FullInfo()
        {
            return $"Id: {Id}\nPilotId: {PilotId}\nCopilotId: {CopilotId}\nStjuard(esa): {string.Join(", ", Stewards)}\nKreirano: {CreatedAt}";
        }
    }
}
