using SQLite;
using Xamarin.Forms;
using LocalDataAccess.UWP;
using Windows.Storage;
using System.IO;


[assembly: Dependency(typeof(DatabaseConnection_UWP))]
namespace LocalDataAccess.UWP
{
  /// <summary>
  /// SQLite connection for UWP.
  /// </summary>
  public class DatabaseConnection_UWP : SentLogger.Resources.IDatabaseConnection
  {
    public SQLiteConnection DbConnection()
    {
      var dbName = "SentracDb.db3";
      var path = Path.Combine(ApplicationData.
        Current.LocalFolder.Path, dbName);
      return new SQLiteConnection(path);
    }
  }
}
