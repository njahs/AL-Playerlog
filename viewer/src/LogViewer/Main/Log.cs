using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Main
{
    class Log
    {
        public int uid { get; set; }
        public int cashdiff { get; set; }
        public int bankdiff { get; set; }
        public int totalcash { get; set; }

        public DateTime updatetime { get; set; }
        public string logtype { get; set; }

        public static Log New(int _uid, DateTime _updatetime, string _logtype, int _cashdiff, int _bankdiff, int _cashTotal)
        {
            return new Log() { uid = _uid, updatetime = _updatetime, logtype = _logtype, cashdiff = _cashdiff, bankdiff = _bankdiff,
                               totalcash = _cashTotal
            }; 
        }
    }

    class LogCollection : List<Log>
    { }
}
