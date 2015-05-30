using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playerlog.Main
{
    class Log
    {
        public int uid { get; set; }
        public double cash { get; set; }
        public double bank { get; set; }
        public double totalcash { get; set; }

        public DateTime updatetime { get; set; }
        public string logtype { get; set; }

        public static Log New(int _uid, DateTime _updatetime, string _logtype, double _cash, double _bank, double _cashTotal)
        {
            return new Log() { uid = _uid, updatetime = _updatetime, logtype = _logtype, cash = _cash, bank = _bank,
                               totalcash = _cashTotal
            }; 
        }
    }

    class LogCollection : List<Log>
    {
        public long maxCash { get; set; } 
    }
}
