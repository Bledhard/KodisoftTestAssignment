using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Models
{
    public partial class IpInfo
    {
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }
        
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        
        public string Continent { get; set; }

        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; }
        
        public string City { get; set; }
        
        public string County { get; set; }
        
        public string Region { get; set; }

        [JsonProperty("region_code")]
        public int RegionCode { get; set; }
        
        public string TimeZone { get; set; }
        
        public string Owner { get; set; }
        
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        
        public string Currency { get; set; }
        
        public List<string> Languages { get; set; }

        public static IpInfo DeserialiseJson(string json)
        {
            IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(json);

            System.Diagnostics.Debug.WriteLine(ipInfo);

            return ipInfo;
        }
    }
}
