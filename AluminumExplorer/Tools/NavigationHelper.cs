using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AluminumExplorer.Tools
{
    class NavigationHelper
    {
        public string ProcessURL(string address)
        {
            bool validUri = Uri.IsWellFormedUriString(address, UriKind.RelativeOrAbsolute);
            string processedAddress = address;
            if (!validUri)
            {
                Uri protocolUri;
                if (Uri.TryCreate(processedAddress, UriKind.Absolute, out protocolUri) || Uri.TryCreate("http://" + processedAddress, UriKind.Absolute, out protocolUri))
                {
                    processedAddress = protocolUri.ToString();
                }
            }
            return processedAddress;
        }
        public static Uri GetValidUri(string s)
        {
            return new UriBuilder(s).Uri;
        }
    }
}
