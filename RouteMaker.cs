using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Flurl;
using Flurl.Http;



namespace RailRouting
{
    public class RouteMaker
    {
        public class Info
        {
            public List<string> copyrights { get; set; }
            public int took { get; set; }
        }

        public class Points
        {
            public string type { get; set; }
            public List<List<double>> coordinates { get; set; }
        }

        public class Instruction
        {
            public double distance { get; set; }
            public double heading { get; set; }
            public int sign { get; set; }
            public List<int> interval { get; set; }
            public string text { get; set; }
            public int time { get; set; }
            public string street_name { get; set; }
            public double last_heading { get; set; }
        }

        public class SnappedWaypoints
        {
            public string type { get; set; }
            public List<List<double>> coordinates { get; set; }
        }

        public class Path
        {
            public double distance { get; set; }
            public double weight { get; set; }
            public int time { get; set; }
            public int transfers { get; set; }
            public bool points_encoded { get; set; }
            public List<double> bbox { get; set; }
            public Points points { get; set; }
            public List<Instruction> instructions { get; set; }
            public List<object> legs { get; set; }
            public double ascend { get; set; }
            public double descend { get; set; }
            public SnappedWaypoints snapped_waypoints { get; set; }
        }

        public class RootObject
        {
            public Info info { get; set; }
            public List<Path> paths { get; set; }
        }

        public static List<List<double>> HttpRequest(List<double> startPoint, List<double> endPoint)  
        {
            var requestUrl = "http://localhost:8989"
            .AppendPathSegment("route")
            .SetQueryParams(new
            {
                point = new[] { $"{startPoint[0]},{startPoint[1]}",$"{endPoint[0]},{endPoint[1]}" },
                type = "json",
                points_encoded = "false",
                key = "",
                locale = "en-GB",
                vehicle = "alltracks"
            });
            var data = requestUrl.GetJsonAsync<RootObject>().Result;
            return data.paths[0].points.coordinates;
        }
    }
}
 