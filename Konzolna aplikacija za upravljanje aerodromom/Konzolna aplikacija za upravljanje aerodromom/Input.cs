using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konzolna_aplikacija_za_upravljanje_aerodromom
{
    static class Input
    {
        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                Console.WriteLine("Polje ne smije biti prazno.");
            }
        }

        public static string ReadName(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s))
                {
                    Console.WriteLine("Polje ne smije biti prazno.");
                    continue;
                }
                s = s.Trim();
                bool ok = true;
                foreach (var ch in s)
                {
                    if (!char.IsLetter(ch))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok) return s;
                Console.WriteLine("Vrijednost smije sadržavati samo slova.");
            }
        }

        public static int ReadIntRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var v) && v >= min && v <= max) return v;
                Console.WriteLine($"Unesite broj između {min} i {max}.");
            }
        }

        public static string ReadEmail(string prompt)
        {
            while (true)
            {
                var s = ReadNonEmpty(prompt);
                if (s.Contains("@") && s.Contains(".")) return s;
                Console.WriteLine("Neispravan email.");
            }
        }

        public static DateTime ReadDateTime(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    if (dt.Year >= 1900) return dt;
                    Console.WriteLine("Godina ne smije biti manja od 1900.");
                }
                else
                {
                    Console.WriteLine("Neispravan format. Koristite yyyy-MM-dd HH:mm");
                }
            }
        }

        public static Guid ReadGuid(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (Guid.TryParse(s, out var id)) return id;
                Console.WriteLine("Neispravan id (GUID).");
            }
        }
        public static void Pause()
        {
            Console.WriteLine("Pritisnite Enter za nastavak...");
            Console.ReadLine();
        }
        public static string ReadRoute(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s))
                {
                    Console.WriteLine("Polje ne smije biti prazno.");
                    continue;
                }
                s = s.Trim();
                if (!s.Contains("-"))
                {
                    Console.WriteLine("Format mora biti start-end, npr. Split-Zagreb.");
                    continue;
                }
                var parts = s.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine("Format mora biti točno start-end.");
                    continue;
                }
                var a = parts[0].Trim();
                var b = parts[1].Trim();
                if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
                {
                    Console.WriteLine("Oba dijela moraju biti ne-prazna.");
                    continue;
                }
                bool ok = true;
                foreach (var ch in a)
                {
                    if (!char.IsLetter(ch))
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                {
                    Console.WriteLine("Start smije sadržavati samo slova.");
                    continue;
                }
                ok = true;
                foreach (var ch in b)
                {
                    if (!char.IsLetter(ch))
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                {
                    Console.WriteLine("End smije sadržavati samo slova.");
                    continue;
                }
                return $"{a}-{b}";
            }
        }
    }
}
