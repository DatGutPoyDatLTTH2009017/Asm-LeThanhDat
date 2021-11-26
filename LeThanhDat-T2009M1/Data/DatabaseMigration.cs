using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LeThanhDat_T2009M1.Data
{
    public class DatabaseMigration
    {
        private static string _databaseFile = "mynote.db";
        private static string _databasePath;
        private static string _createContactTable = "CREATE TABLE IF NOT EXISTS notes" +
            "(PhoneNumber NVARCHAR(100) FRIMARY KEY," +
            "Name NAVARCHAR(255) NOT NULL)";
        public async static void UpdateDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(_databaseFile, CreationCollisionOption.OpenIfExists);
            _databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _databaseFile);
            using(SqliteConnection cnn = new SqliteConnection($"Filename={_databasePath}"))
            {
                cnn.Open();
                SqliteCommand command = new SqliteCommand(_createContactTable, cnn);
                command.ExecuteNonQuery();
            }
        }
    }
}
