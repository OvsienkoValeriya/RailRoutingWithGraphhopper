using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Flurl;
using Flurl.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace RailRouting
{
    public class Routing
    {
        public static void Main(string[] args)
        {
            FileMaker.MakeRouts();
            var routes = FindTheStation.GetRailwayStations("output.csv");
            var stationDots = FindTheStation.GetRailwayStationsFromJson("stations.json");
            var routeCoords = FindTheStation.GetStationsCoordinates(stationDots, routes);
            var routeCoordsZipped = Enumerable.Zip(routeCoords.Item1, routeCoords.Item2, (startCoord, endCoord) => (startCoord, endCoord));

            var stationsPopularity = new Dictionary<string, int>();
            foreach (var route in routeCoordsZipped)
            {
                var routePoints = RouteMaker.HttpRequest(route.Item1, route.Item2);
                var stationsVisited = new HashSet<string>();
                foreach (var waypoint in routePoints)
                {
                    var waypointStation = FindTheStation.FindNearestStations(stationDots, waypoint);
                    stationsVisited.UnionWith(waypointStation);
                }
                foreach (var station in stationsVisited)
                {
                    int counter;
                    stationsPopularity[station] = stationsPopularity.TryGetValue(station, out counter) ? counter + 1 : 1;
                   
                }

            }
            using (StreamWriter file = new StreamWriter ("final.csv"))
            {
                var esrToPopularity = stationsPopularity.ToList().OrderBy((kw) => kw.Value);
                foreach(var entry in esrToPopularity)
                {
                    file.WriteLine($"{entry.Key} {entry.Value}");
                }
            }
        }
    }
}





       