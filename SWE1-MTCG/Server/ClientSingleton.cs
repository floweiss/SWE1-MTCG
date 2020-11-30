using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Server
{
    public sealed class ClientSingleton
    {
        private static ClientSingleton _singleton;
        private static readonly object Lock = new object();

        private ClientSingleton()
        {
            ClientMap = new ConcurrentDictionary<string, User>();
        }

        public static ClientSingleton GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (Lock)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new ClientSingleton();
                        }
                    }
                }

                return _singleton;
            }
        }

        public ConcurrentDictionary<string, User> ClientMap { get; }
    }
}