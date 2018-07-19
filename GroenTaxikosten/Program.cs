using System;


namespace GroenTaxikosten
{
    class Program
    {
        static String VraagStartTijd()
        {
            Console.WriteLine("Geef starttijd in formaat jjjj/mm/dd uu:mm:00");
            return Console.ReadLine();
        }

        static String VraagEindTijd()
        {
            Console.WriteLine("Geef eindtijd in formaat jjjj/mm/dd uu:mm:00");
            return Console.ReadLine();
        }

        static bool Toeslag(DateTime st)
        {
            if (st.DayOfWeek == DayOfWeek.Saturday) { return true; }
            else if (st.DayOfWeek == DayOfWeek.Sunday) { return true; }
            else if ((st.DayOfWeek == DayOfWeek.Friday) && (st.Hour >= 22)) { return true; }
            else if ((st.DayOfWeek == DayOfWeek.Monday) && (st.Hour <= 6)) { return true; }
            else { return false; }
        }

        static void ZelfdeDag(DateTime st, DateTime et)
        {
            if (st.Date != et.Date)
            {
                Console.WriteLine("Een taxi rit kun je alleen op dezelfde dag maken");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Goed zo! Deze taxi rit maak je op de dezelfde dag!");
            }
        }

        // tarief berekenen zonder toeslag
        static Double TariefZT(DateTime st, DateTime et, int afstand)
        {
            DateTime vooracht = st.Date;
            TimeSpan ts = new TimeSpan(8, 0, 0);
            vooracht = vooracht.Add(ts);
            
            DateTime nazes = st.Date;
            TimeSpan tsa = new TimeSpan(18, 0, 0);
            nazes = nazes.Add(tsa);

            // rit vindt plaats voor 8 's morgens
            if ((st.Hour < 8) && (et.Hour < 8))
            {
                Console.WriteLine("Je rit vindt plaats voor acht uur 's morgens.");
                TimeSpan td = et - st;
                return (td.TotalMinutes * 0.45) + afstand;
            }
            // rit vindt plaats na 18:00 uur 's avonds
            else if ((st.Hour > 18) && (et.Hour > 18))
            {
                Console.WriteLine("Je rit vindt plaats na zes uur 's avonds.");
                TimeSpan td = et - st;
                return (td.TotalMinutes * 0.45) + afstand;
            }
            // rit vindt plaats tussen 8 uur en 18 uur.
            else if ((st.Hour >= 8) && (et.Hour < 18))
            {
                Console.WriteLine("Je rit vindt plaats tussen acht uur 's morgens en zes uur 's middags.");
                TimeSpan td = et - st;
                return (td.TotalMinutes * 0.25) + afstand;
            }
            // rit begint voor 8 uur 's morgens en eindigt na 18:00 uur 's avond
            else if ((st.Hour < 8) && (et.Hour >= 18))
            {
                Double Temp;
                TimeSpan td = vooracht - st;
                Console.WriteLine("Voor acht uur: {0}", td.TotalMinutes);
                Temp = td.TotalMinutes * 0.45 + 600 * 0.25;
                TimeSpan tda = et - nazes;
                Console.WriteLine("Na zes uur: {0}", tda.TotalMinutes);
                Temp = Temp + tda.TotalMinutes * 0.45;
                return Temp + afstand;
            }
            //rit begint voor 8 uur 's morgens en eindig na 8 uur maar voor 18 uur
            else if ((st.Hour < 8) && (et.Hour >= 8) && (et.Hour < 18))
            {
                Double Temp;
                TimeSpan td = vooracht - st;
                Temp = td.TotalMinutes * 0.45;
                TimeSpan tda = et - vooracht;
                Temp = Temp + tda.TotalMinutes * 0.25;
                return Temp + afstand;
            }
            //rit begint na 8 uur 's morgens, maar voor 18 uur 's middags en eindigt na 18 uur 
            else if ((st.Hour >= 8) && (st.Hour < 18) && (et.Hour >= 18))
            {
                Double Temp;
                TimeSpan td = nazes - st;
                Temp = td.TotalMinutes * 0.25;
                TimeSpan tda = et - nazes;
                Temp = Temp + tda.TotalMinutes * 0.45;
                return Temp + afstand;
            }
            else
            {
                return (0.0);
            }
        }

        static void Main(string[] args)
        {
            String starttijd;
            String eindtijd;
            String afstandstr;
            int afstand;
            bool toeslag;
            Double tariefZT;

            Console.WriteLine("Geef het aantal (gehele) kilometers op");
            afstandstr = Console.ReadLine();
            afstand = int.Parse(afstandstr);

            starttijd = VraagStartTijd();
            eindtijd = VraagEindTijd();
            DateTime st = DateTime.Parse(starttijd);
            DateTime et = DateTime.Parse(eindtijd);

            if (DateTime.Compare(et, st) < 0)
            {
                Console.WriteLine("Tijdreizen is niet mogelijk!!! Deze taxi is geen DeLorean DMC-12! Eindtijd ligt vroeger dan starttijd");
                Environment.Exit(0);
            }

            ZelfdeDag(st, et);

            toeslag = Toeslag(st);
            tariefZT = TariefZT(st, et, afstand);
            if (toeslag)
            {
                Console.WriteLine("Je moet toeslag betalen");
                Console.WriteLine("De kosten voor dit taxiritje bedragen: {0}", tariefZT * 1.15);
            }
            else if (!toeslag)
            {
                Console.WriteLine("Je hoeft geen toeslag te betalen");
                Console.WriteLine("De kosten voor dit taxiritje bedragen: {0}", tariefZT);
            }
        }
    }
}
