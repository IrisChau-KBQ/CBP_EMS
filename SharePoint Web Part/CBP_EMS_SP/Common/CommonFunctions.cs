using Microsoft.ApplicationServer.Caching;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.DistributedCaching.Utilities;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Common
{
    public sealed class CacheManager
    {


        public static readonly object _lock = new object();
        public static DataCache _defaultCache;

        public CacheManager() { }

        public  DataCache DefaultCache
        {
            get
            {
                using (SPMonitoredScope scope = new SPMonitoredScope("CacheManager.DefaultCache"))
                {
                    lock (_lock)
                    {
                        if (_defaultCache == null)
                        {
                            //By default the farm ID is appended to the name of the cluster and caches
                            //created by SharePoint.
                            var farmID = SPFarm.Local.Id;

                            //This is one of the caches created by SharePoint. Run the AppFabric's
                            //Get-Cache PowerShell to see a list of caches created by SharePoint.
                            var defaultCacheName = "DistributedDefaultCache_" + farmID;

                            var factoryConfiguration = new DataCacheFactoryConfiguration()
                            {
                                Servers = GetAllDataCacheServerEndpointsForFarm(farmID)
                            };

                            _defaultCache = new DataCacheFactory(factoryConfiguration).GetCache(defaultCacheName);
                        }
                        return _defaultCache;
                    }
                }
            }
        }

        private static List<DataCacheServerEndpoint> GetAllDataCacheServerEndpointsForFarm(Guid farmID)
        {
            var endpoints = new List<DataCacheServerEndpoint>();

            //By default this is the name of the cluster created by SharePoint.
            string cacheClusterName = "SPDistributedCacheCluster_" + farmID;

            var cacheClusterManager = SPDistributedCacheClusterInfoManager.Local;
            var cacheClusterInfo = cacheClusterManager.GetSPDistributedCacheClusterInfo(cacheClusterName);

            foreach (var cacheHost in cacheClusterInfo.CacheHostsInfoCollection)
            {
                endpoints.Add(new DataCacheServerEndpoint(cacheHost.HostName, cacheHost.CachePort));
            }
            return endpoints;
        }
        public Dictionary<string, object> DcacheDict
        {
            get
            {
                return (DefaultCache.GetObjectsInRegion("TNACache")).ToDictionary(x => x.Key, x => x.Value);
            }
        }

    }
}
