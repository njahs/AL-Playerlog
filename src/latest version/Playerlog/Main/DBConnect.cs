using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Playerlog.Main
{
    class DBConnect
    {
        public static DBConnect instance { get; set; }
        //public static DBConnect instance { get { return _instance; } set { if (_instance != value) _instance = value; } }

        private MySqlConnection connection;
        protected string server { private get; set; }
        protected string database { private get; set; }
        protected string uid { private get; set; }
        protected string password { private get; set; }
        protected string connection_string 
        {
            private get { return string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};",
                server, database, uid, password); }
            set { }
        }

        public DBConnect(string srv, string db, string un, string pw)
        {
            instance = this;

            server = srv; database = db; uid = un; password = pw;

            connection = new MySqlConnection(connection_string);
        }

        public Connection CheckConnection() { return OpenConnection(); }

        private Connection OpenConnection()
        {
            if (connection != null &&
                connection.State != System.Data.ConnectionState.Closed &&
                connection.State != System.Data.ConnectionState.Broken) return Connection.Successfull;
            Connection _return = Connection.Unknown;

            try
            {
                connection.Open();
                _return = Connection.Successfull;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("[{0}]: {1}", ex.Number, ex.Message);

                switch (ex.Number)
                {
                    case 0:
                        _return = Connection.ERROR_Connection;
                        break;
                    case 1042:
                        _return = Connection.ERROR_Servername;
                        break;
                    case 1045:
                        _return = Connection.ERROR_Credentials;
                        break;
                }
            }

            return _return;
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public PlayerCollection FetchPlayers()
        {
            string query = "SELECT uid, name, playerid, cash, bankacc FROM players";
            PlayerCollection _return = new PlayerCollection();

            if (this.OpenConnection() == Connection.Successfull)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    MySqlDataReader dataReader = null;
                    cmd.CommandText = query;
                    cmd.Connection = connection;

                    try { dataReader = cmd.ExecuteReader(); }
                    catch { _return = null; }

                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            _return.Add(Player.New(
                                (int)dataReader["uid"],
                                (string)dataReader["playerid"],
                                (string)dataReader["name"],
                                (int)dataReader["cash"],
                                (int)dataReader["bankacc"]));
                        }
                    }
                }

                this.CloseConnection();
            }
            return _return;
        }

        public LogCollection FetchPlayerLog(int uid)
        {
            string query = String.Format("SELECT * FROM players_log WHERE uid='{0}'", uid);
            LogCollection _return = new LogCollection();

            if (this.OpenConnection() == Connection.Successfull)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    MySqlDataReader dataReader = null;
                    cmd.CommandText = query;
                    cmd.Connection = connection;

                    try { dataReader = cmd.ExecuteReader(); }
                    catch { _return = null; }
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            DateTime _dt = DateTime.MinValue;
                            if (dataReader["updatetime"].GetType().Equals(typeof(DateTime)))
                                _dt = (DateTime)dataReader["updatetime"];
                            else
                                if (!dataReader["updatetime"].GetType().Equals(typeof(DBNull))) DateTime.TryParse((string)dataReader["updatetime"], out _dt);

                            _return.Add(Log.New(
                                (int)dataReader["uid"],
                                _dt,
                                (string)dataReader["logtype"],
                                (double)(int)dataReader["cash"],
                                (double)(int)dataReader["bank"],
                                (double)(int)dataReader["totalcash"]));
                        }

                        if (!dataReader.IsClosed) dataReader.Close();

                        cmd.CommandText = String.Format("SELECT MAX(totalcash) FROM players_log WHERE uid = '{0}'", uid);
                        try { long n; if (long.TryParse(cmd.ExecuteScalar().ToString(), out n)) _return.maxCash = n; }
                        catch { _return.maxCash = 0; }
                    }
                }

                this.CloseConnection();
            }
            return _return;
        }

        public StatusResult GetStatus()
        {
            StatusResult @return = new StatusResult();

            if (this.OpenConnection() == Connection.Successfull)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    MySqlDataReader reader = null;

                    cmd.Connection = connection;

                    // TABLE STATUS
                    cmd.CommandText = "SELECT uid FROM players_log LIMIT 1";
                    @return.tableStatus = true;

                    try { reader = cmd.ExecuteReader(); }
                    catch { @return.tableStatus = false; }
                    if (reader != null && reader.IsClosed == false) reader.Close();

                    // TRIGGER STATUS
                    cmd.CommandText = "SELECT 0 FROM INFORMATION_SCHEMA.TRIGGERS WHERE TRIGGER_NAME like 'player%'"; /*"SHOW TRIGGERS LIKE 'player%'";*/
                    @return.triggerStatus = true;

                    try { reader = cmd.ExecuteReader(); int n = 0; while (reader.Read()) { n++; } if (n != 2) @return.triggerStatus = false; }
                    catch { @return.triggerStatus = false; }
                    if (reader != null && reader.IsClosed == false) reader.Close();

                    // LOG ENTRIES / COUNT
                    cmd.CommandText = "SELECT COUNT(*) FROM players_log";
                    @return.logEntries = 0;

                    if (@return.tableStatus == true)
                    {
                        try { @return.logEntries = Convert.ToInt32(cmd.ExecuteScalar().ToString()); }
                        catch { @return.logEntries = 0; }
                    }
                }

                this.CloseConnection();
            }
            return @return;
        }
        
        public bool Query(string query)
        {
            bool @return = true;

            if (this.OpenConnection() == Connection.Successfull)
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = query;

                    try { cmd.ExecuteNonQuery(); }
                    catch { @return = false; }
                }

                this.CloseConnection();
            }

            return @return;
        }

        public struct StatusResult
        {
            public bool tableStatus { get; set; }
            public bool triggerStatus { get; set; }
            public int logEntries { get; set; }
        }

        public enum Connection
        {
            Unknown,
            ERROR_Connection,
            ERROR_Credentials,
            ERROR_Servername,
            Successfull
        }
    }
}
