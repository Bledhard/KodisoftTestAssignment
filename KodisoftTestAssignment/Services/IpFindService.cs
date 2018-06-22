using KodisoftTestAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KodisoftTestAssignment.Services
{
    public class IpFindService
    {
        private const string apikey = "780f0261-8e8d-4afa-b1bd-d4c762bfcfde";

        public IpInfo GetIpInfo(string ip)
        {
            var url = "https://ipfind.co/?ip=" + ip + "&auth=" + apikey;
            var json = new WebClient().DownloadString(url);
            return IpInfo.DeserialiseJson(json);
        }
    }
}
