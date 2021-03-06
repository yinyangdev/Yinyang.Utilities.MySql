namespace Yinyang.Utilities.MySql.Samples
{
    public class Sample
    {
        /// <summary>
        ///     ConnectionString
        /// </summary>
        /// <remarks>
        ///     接続文字列
        /// </remarks>
        private const string ConnectionString =
            "Server=127.0.0.1;Port=3306;Database=db;User Id=user;Password=password;";


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <remarks>
        ///     ConnectionString設定後、省略可能
        /// </remarks>
        public Sample() =>
             // Set default connection string
             MySqlConnect.ConnectionString = ConnectionString;

        /// <summary>
        ///     Select Sample
        /// </summary>
        public void Select()
        {
            // Use default connection string
            using (var db = new MySqlConnect())
            {
                db.Open();
                db.CommandText = "select * from test;";
                var result = db.ExecuteReader<Entity>();
                db.Close();
            }

            using (var db = new MySqlConnect(ConnectionString))
            {
                db.Open();
                db.CommandText = "select * from test;";
                var result = db.ExecuteReader<Entity>();
                db.Close();
            }
        }

        /// <summary>
        ///     Insert Sample
        /// </summary>
        public void Insert()
        {
            using var db = new MySqlConnect(ConnectionString);

            db.Open();

            db.BeginTransaction();

            db.CommandText = "INSERT INTO test2 VALUES(@id, @value)";
            db.AddParameter("@id", 1);
            db.AddParameter("@value", "あいうえお");
            if (1 != db.ExecuteNonQuery())
            {
                db.Rollback();
                return;
            }

            db.Refresh();

            db.CommandText = "select * from test2 where \"id\" = @id;";
            db.AddParameter("@id", 1);
            var result = db.ExecuteReaderFirst<Entity>();

            if (null == result)
            {
                db.Rollback();
                return;
            }

            db.Commit();

            db.Close();
        }
    }
}
