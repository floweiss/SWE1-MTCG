using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class StatsApi : IApi
    {
        private RequestContext _request;
        private StatsController _statsController;

        public StatsApi(RequestContext request)
        {
            _request = request;
            IStatsService statsService = new StatsService();
            _statsController = new StatsController(statsService);

        }
        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "GET":
                    return GetMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            throw new NotImplementedException();
        }

        public string GetMethod()
        {
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "GET ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "GET ERR - Not logged in";
            }

            return _statsController.ShowStats(usertoken);
        }

        public string PutMethod()
        {
            throw new NotImplementedException();
        }

        public string DeleteMethod()
        {
            throw new NotImplementedException();
        }
    }
}
