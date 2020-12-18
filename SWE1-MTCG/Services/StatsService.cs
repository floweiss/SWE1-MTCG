using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class StatsService : IStatsService
    {
        public string ShowStats(string usertoken)
        {
            User user = null;
            if (ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            }

            return "Your current ELO is " + user.ELO.ToString();
        }
    }
}
