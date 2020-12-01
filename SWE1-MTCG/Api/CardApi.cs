using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Api
{
    public class CardApi : IApi
    {
        private RequestContext _request;
        private CardDTO _card;

        public CardApi(RequestContext request)
        {
            _request = request;
            try
            {
                _card = JsonSerializer.Deserialize<CardDTO>(_request.Content);
            }
            catch (Exception e)
            {
                _card = null;
            }
        }

        public string Interaction()
        {
            switch (_request.HttpMethod)
            {
                case "POST":
                    return PostMethod();

                default:
                    return "Method ERR";
            }
        }

        public string PostMethod()
        {
            return _card.Name;
        }

        public string GetMethod()
        {
            throw new NotImplementedException();
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
