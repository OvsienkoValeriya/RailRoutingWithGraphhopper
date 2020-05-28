using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;


namespace RailRouting
{

    public class FileMaker
    {
        public static void MakeRouts() 
        {
            ReadAndWrite(@"input.csv", @"output.csv");

        }
        public static void ReadAndWrite(string filename, string outputFilename)
        {
            using (StreamWriter file = new StreamWriter(outputFilename))
            {
                foreach (string line in File.ReadLines(filename, Encoding.UTF8))
                {
                    var stations = ExtractStationCode(line);
                    if (stations != null)
                    {
                        file.WriteLine($"{stations[0]} {stations[1]}");
                    }
                }
            }
        }
        public static List<string> ExtractStationCode(string input)
        {
            var regexp = new Regex(@"(\d{4})\s*(\d*)\s*(\d{4})");
            var matches = regexp.Matches(input);
            if (matches.Count > 0)
            {
                var match = matches[0];
                var groups = match.Groups;
                var routeStart = groups[1].Value;
                var routeEnd = groups[3].Value;
                return new List<string>() { routeStart, routeEnd };
            }
            else
            {
                return null;
            }
        }
        public static List<float> GetCoordinates(string station)
        {
            var regex = new Regex(@"(\S*)\s+(\S*)\s*(\S+)");
            foreach (string line in File.ReadLines("stations.csv", Encoding.UTF8))
            {
                var match = regex.Matches(line);
                if (match.Count > 0)
                {
                    var groups = match[0].Groups;
                    var esr = groups[1].Value;
                    var lat = groups[2].Value;
                    var lon = groups[3].Value;
                    if (esr == station)
                    {
                        return new List<float> { float.Parse(lat), float.Parse(lon) };
                    }
                }
            }
            return null;
        }
    }
}
