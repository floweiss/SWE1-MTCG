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
    public class ScoreApi : IApi
    {
        private RequestContext _request;
        private ScoreController _scoreController;

        public ScoreApi(RequestContext request)
        {
            _request = request;
            IScoreService scoreService = new ScoreService();
            _scoreController = new ScoreController(scoreService);

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

            return _scoreController.ShowScore();
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
