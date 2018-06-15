using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;
using SentLogger.Resources;

namespace LocalDataAccess

{
  class SentracDataAccess
  {
    private SQLiteConnection database;
    private static object collisionLock = new object();

    public ObservableCollection<SentracSQLiteData> SentracData { get; set; }

    public SentracDataAccess()
    {
      database =
        DependencyService.Get<IDatabaseConnection>().
        DbConnection();
      database.CreateTable<SentracSQLiteData>();
      this.SentracData =
        new ObservableCollection<SentracSQLiteData>(database.Table<SentracSQLiteData>());
      // If the table is empty, initialize the collection
      if (!database.Table<SentracSQLiteData>().Any())
      {
        AddNewSentracSQLiteData();
      }
    }

    public void AddNewSentracSQLiteData()
    {
      this.SentracData.
      Add(new SentracSQLiteData
      {
        Date = (DateTime.Now.Date), //'yyyy-MM-dd'
        Time = (DateTime.Now.TimeOfDay), //'HH:mm:ss'
        Value = 0.0,
        Result = "Accept" // Hämta in värdena från sentracen här...
      });
    }

    /// <summary>
    /// TODO-Se om det behövs mer sql hantering. Kolla fig 12 i artikeln.
    /// https://msdn.microsoft.com/en-us/magazine/mt736454.aspx?f=255&MSPPError=-2147217396
    /// </summary>

  }
}
