using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class ArenaApi : IApi
    {
        private RequestContext _request;
        private ArenaController _arenaController;

        public ArenaApi(RequestContext request)
        {
            _request = request;
            ElementEffectivenessService elementEffectivenessService = new ElementEffectivenessService();
            IArenaService arenaService = new ArenaService(elementEffectivenessService);
            _arenaController = new ArenaController(arenaService);
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
            string usertoken;
            if (!_request.CustomHeader.TryGetValue("Authorization", out usertoken))
            {
                return "POST ERR - No authorization header";
            }

            usertoken = usertoken.Substring(6, usertoken.Length - 6);
            if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                return "POST ERR - Not logged in";
            }

            User user;
            ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);

            if (user.Deck.CardCollection.Count != 4)
            {
                return "POST ERR - Deck not configured";
            }
            user.SetReadyForBattle();
            ClientSingleton.GetInstance.ClientMap.AddOrUpdate(usertoken, user, (key, oldValue) => user);

            bool onlyPlayerReady = true;
            User player2 = null;
            foreach (var searchUser in ClientSingleton.GetInstance.ClientMap)
            {
                if (searchUser.Value.ReadyForBattle && (searchUser.Value.Username != user.Username))
                {
                    onlyPlayerReady = false;
                    player2 = searchUser.Value;
                    searchUser.Value.SetNotReadyForBattle();
                    ClientSingleton.GetInstance.ClientMap.AddOrUpdate(searchUser.Key, searchUser.Value,
                        (key, oldValue) => searchUser.Value);
                    user.SetNotReadyForBattle();
                    ClientSingleton.GetInstance.ClientMap.AddOrUpdate(usertoken, user, (key, oldValue) => user);
                    break;
                }
            }

            string battleLog;
            if (onlyPlayerReady)
            {
                // Waiting for second thread to finish battle
                while (ArenaSingleton.GetInstance.BattleLogs.IsEmpty)
                {
                    Thread.Sleep(100);
                }

                ArenaSingleton.GetInstance.BattleLogs.TryPop(out battleLog);
            }
            else
            {
                Arena arena = new Arena(player2, user);
                battleLog = _arenaController.StartBattle(arena);
                ArenaSingleton.GetInstance.BattleLogs.Push(battleLog);
            }

            return battleLog;
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
