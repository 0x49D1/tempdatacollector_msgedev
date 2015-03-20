using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPTermoClient.Common
{
    
public class RootobjectLocal
{
public double c { get; set; }
public double h { get; set; }
}



    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public int pressure { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

}
