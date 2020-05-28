using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace RailRouting
{
    public class FindTheStation
    {
        public static double THRESHOLD = 0.1;
        [DataContract]
        public class RailwayStationDTO
        {
            [DataMember]
            public string esr { get; set; }

            [DataMember]
            public string status { get; set; }

            [DataMember]
            public string type { get; set; }

            [DataMember]
            public string osm_id { get; set; }

            [DataMember]
            public double lat { get; set; }

            [DataMember]
            public double lon { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string alt_name { get; set; }

            [DataMember]
            public string old_name { get; set; }

            [DataMember]
            public string official_name { get; set; }

            [DataMember]
            public string railway { get; set; }

            [DataMember]
            public string user { get; set; }
        }

        public class Route
        {
            public string Start { get; set; }

            public string End { get; set; }
        }

        public static Route[] GetRailwayStations(string filename) 
        {
            var lines = File.ReadAllLines(filename).Where(l => l != "").ToArray();
            var routes = new Route[lines.Length];
            for (var i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];

                var stations = line.Split(' ');
                routes[i] = new Route()
                {
                    Start = stations[0],
                    End = stations[1]
                };
            }
            return routes;
        }
        public static RailwayStationDTO[] GetRailwayStationsFromJson(string filename) 
        {
            var json = File.ReadAllText(filename);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var deserializer = new DataContractJsonSerializer(typeof(RailwayStationDTO[]));
                return (RailwayStationDTO[])deserializer.ReadObject(ms);
            }

        }

        public static HashSet <string> FindNearestStations(RailwayStationDTO[] stations, List<double> coords)
        {
            var esrSet = new HashSet<string>();
            var pointLat = coords[0];
            var pointLon = coords[1];
            foreach (var station in stations)
            {
                if (Math.Sqrt((Math.Pow(station.lat - pointLat, 2)) + (Math.Pow(station.lon - pointLon, 2))) < THRESHOLD)
                {
                    esrSet.Add(station.esr);
                }
            }
            return esrSet;
        }

        public static (List<List<double>>, List<List<double>>) GetStationsCoordinates(RailwayStationDTO[] stations, Route[] routes) 
        {
            var StartDot = new List<List<double>>();
            var EndDot = new List<List<double>>();
            foreach (var route in routes)
            {
                var stationStart = stations.FirstOrDefault(s => s.esr == route.Start);  
                var stationEnd = stations.FirstOrDefault(s => s.esr == route.End);
                if (stationStart != null && stationEnd != null)
                {
                    StartDot.Add(new List<double>() { stationStart.lat, stationStart.lon });
                    EndDot.Add(new List<double>() { stationEnd.lat, stationEnd.lon });
                }
            }
            return (StartDot, EndDot);
        }
    }
}
