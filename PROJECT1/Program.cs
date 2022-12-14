using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

internal class TextFieldParser
{
    static void Main(string[] args)
    {
        using (var client = new WebClient())
        {
            client.DownloadFile("https://raw.githubusercontent.com/vilnius/mparking/master/Violations_2019.csv", "Violations_2019.csv");
        }

        using (var reader = new StreamReader(@".\Violations_2019.csv"))
        {
            List<string> date = new List<string>();
            List<string> street = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                var streets = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                var date_extract = values[11].Split(' ');
                var street_name = streets[5];
                date.Add(date_extract[0]);
                street.Add(street_name);
            }

            int num = 0;
            var counts = date
            .GroupBy(w => w)
            .Select(g => new { Most_Day_Tickets = g.Key, Numbers_Of_Tickets = g.Count(), Street_Name = street[date.IndexOf(g.Key)]})
            .ToList();

            List<int> Ticket_Num = new List<int>();
            List<int> Formating = new List<int>();

            foreach (var p in counts)
            {
                Ticket_Num.Add(p.Numbers_Of_Tickets);
            }

            var order = Ticket_Num.OrderByDescending(i => i);

            foreach (var i in order)
            {
                Formating.Add(i);
            }

            for (int i = 0; i < 3; i++)
            {
                foreach (var f in counts)
                {

                    if (f.Numbers_Of_Tickets == Formating[i])
                    {
                        Console.WriteLine(f);
                    }
                }

            }

        }

    }
}