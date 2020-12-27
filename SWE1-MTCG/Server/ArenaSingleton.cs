using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Server
{
    public sealed class ArenaSingleton
    {
        private static ArenaSingleton _singleton;
        private static readonly object Lock = new object();

        private ArenaSingleton()
        {
            BattleLogs = new ConcurrentStack<string>();
        }

        public static ArenaSingleton GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (Lock)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new ArenaSingleton();
                        }
                    }
                }

                return _singleton;
            }
        }

        public ConcurrentStack<string> BattleLogs { get; }
    }
}