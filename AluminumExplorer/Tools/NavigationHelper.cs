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
        public string ProcessURL(Uri address)
        {
            bool absoluteUri = address.IsAbsoluteUri;
            string processedAddress = address.ToString();
            if (!absoluteUri)
            {
                processedAddress = address.AbsoluteUri;
            }
            return processedAddress;
        }
    }
}
