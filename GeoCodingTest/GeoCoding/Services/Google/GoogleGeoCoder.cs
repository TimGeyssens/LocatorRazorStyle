using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Locator.GeoCoding.Services.Google
{
    public class GoogleGeoCoder
    {
        public static GeoResponse CallGeoWS(string address)
        {
            string url = string.Format(
                    "http://maps.google.com/maps/api/geocode/json?address={0}&sensor=false",
                    HttpUtility.UrlEncode(address)
                    );
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoResponse));
            var res = (GeoResponse)serializer.ReadObject(request.GetResponse().GetResponseStream());
            return res;
        }

    }
}