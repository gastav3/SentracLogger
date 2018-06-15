using LocalDataAccess.iOS;
using SQLite;
using System;
using System.IO;
[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_iOS))]
namespace LocalDataAccess.iOS
{
  /// <summary>
  /// SQLite connection for iOS.
  /// </summary>
  public class DatabaseConnection_iOS
  {
    public SQLiteConnection DbConnection()
    {
      var dbName = "SentracDb.db3";
      string personalFolder =
        System.Environment.
        GetFolderPath(Environment.SpecialFolder.Personal);
      string libraryFolder =
        Path.Combine(personalFolder, "..", "Library");
      var path = Path.Combine(libraryFolder, dbName);
      return new SQLiteConnection(path);
    }
  }
}