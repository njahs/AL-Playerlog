using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LogViewer.Main
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
                Console.WriteLine(ex.Message);
                switch (ex.Number)
                {
                    case 0:
                        _return = Connection.ERROR_Connection;
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
                    cmd.CommandText = query;
                    cmd.Connection = connection;

                    MySqlDataReader dataReader = cmd.ExecuteReader();

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
                    cmd.CommandText = query;
                    cmd.Connection = connection;

                    MySqlDataReader dataReader = cmd.ExecuteReader();

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
                            (int)dataReader["cashdiff"],
                            (int)dataReader["bankdiff"],
                            (int)dataReader["totalcash"]));
                    }
                }

                this.CloseConnection();
            }
            return _return;
        }

        public enum Connection
        {
            Unknown,
            ERROR_Connection,
            ERROR_Credentials,
            Successfull
        }
    }
}
