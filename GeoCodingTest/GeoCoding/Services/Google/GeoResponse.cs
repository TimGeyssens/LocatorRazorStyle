﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SeekAndDestroy.GeoCoding.Services.Google
{
    [DataContract]
    public class GeoResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "results")]
        public CResult[] Results { get; set; }

        [DataContract]
        public class CResult
        {
            [DataMember(Name = "formatted_address")]
            public string formatted_address { get; set; }
            [DataMember(Name = "address_components")]
            public GeocodeAddressComponent[] AddressComponents { get; set; }

            [DataContract]
            public class GeocodeAddressComponent
            {
                [DataMember(Name = "long_name")]
                public string LongName { get; set; }
                [DataMember(Name = "short_name")]
                public string ShortName { get; set; }
                [DataMember(Name = "types")]
                public string[] Type { get; set; }
            }

            [DataMember(Name = "geometry")]
            public CGeometry Geometry { get; set; }

            [DataContract]
            public class CGeometry
            {
                [DataMember(Name = "location")]
                public CLocation Location { get; set; }

                [DataContract]
                public class CLocation
                {
                    [DataMember(Name = "lat")]
                    public double Lat { get; set; }
                    [DataMember(Name = "lng")]
                    public double Lng { get; set; }
                }
            }
        }
    }
}