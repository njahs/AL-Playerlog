using System;

namespace Playerlog.Main
{
    public class Player
    {
        public int uid { get; set; }
        public int ucash { get; set; }
        public int ubank { get; set; }

        public string pid { get; set; }
        public string uname { get; set; }

        public static Player New(int _uid, string _pid, string _uname, int _ucash, int _ubank)
        {
            return new Player() { uid = _uid, pid = _pid, uname = _uname, ucash = _ucash, ubank = _ubank };
        }
    }

    public class PlayerCollection : System.Collections.Generic.List<Player>
    { }
}
