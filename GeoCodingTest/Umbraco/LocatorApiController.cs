using SeekAndDestroy.GeoCoding;
using SeekAndDestroy.GeoCoding.Services.Google;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace SeekAndDestroy.Umbraco
{
    public class LocatorApiController : UmbracoApiController
    {

        public IEnumerable<ExpandoObject> GetNearest(string address, int parentId, int docTypeFilter, string locationPropAlias, int numberOfSearchResults)
        {
            UmbracoHelper help = new UmbracoHelper(UmbracoContext);

            var nodesToSearchThrough = help.TypedContent(parentId)
                .Children
                .Where(c => c.DocumentTypeId == docTypeFilter).Where("Visible");

            var distanceunit = DistanceUnits.Kilometers;

            var items = new List<GeoItem>();
            var searchLocation = new Location();

            var r = GoogleGeoCoder.CallGeoWS(address);

            searchLocation = new Location(r.Results[0].Geometry.Location.Lat, r.Results[0].Geometry.Location.Lng);


            foreach (var node in nodesToSearchThrough) {
                         
              var itemLocation = new Location(
                    Convert.ToDouble(node.GetProperty(locationPropAlias).Value.ToString().Split(',')[0], Utility.NumberFormatInfo),
                    Convert.ToDouble(node.GetProperty(locationPropAlias).Value.ToString().Split(',')[1], Utility.NumberFormatInfo));  
              
              items.Add(new GeoItem(node,itemLocation,searchLocation.DistanceBetween(itemLocation,distanceunit)));                                                                           
           }

            items.Sort(new GeoItemComparer());

            var retval = new List<ExpandoObject>();
            foreach (GeoItem geo in items.Take(numberOfSearchResults))
            {
                IPublishedContent node = ((IPublishedContent)geo.Node);

                var obj = new ExpandoObject() as IDictionary<string, Object>;

                obj.Add("Distance", geo.Distance);
                obj.Add("Latitude", geo.Location.Latitude);
                obj.Add("Longitude", geo.Location.Longitude);

                obj.Add("Name", node.Name);
                obj.Add("Url", node.Url);

                foreach (var nodeProp in node.Properties)
                {
                    obj.Add(nodeProp.Alias, nodeProp.Value);
                }

                retval.Add((ExpandoObject)obj);
            }
            return retval;
        }

    }
}