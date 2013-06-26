using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Locator.GeoCoding;
using umbraco.MacroEngines;

namespace Locator
{
    public class GeoItem
    {
        public GeoItem(object Node, Location Location, Distance Distance)
        {
            this.Node = Node;
            this.Location = Location;
            this.Distance = Distance;
        }

        public object Node { get; set; }
        public Location Location { get; set; }
        public double Distance { get; set; }
        
    }
}