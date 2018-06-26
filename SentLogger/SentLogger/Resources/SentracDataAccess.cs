using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;
using SentLogger.Resources;
using SentLogger.Models;
using SentLogger.Models.Extra;
using System.Diagnostics;

namespace LocalDataAccess

{
  class SentracDataAccess
  {
    private SQLiteConnection database;
    private static object collisionLock = new object();

    public ObservableCollection<SentracSQLiteData> SentracData { get; set; }

    public SentracDataAccess()
    {
        database = DependencyService.Get<IDatabaseConnection>().DbConnection();
        database.CreateTable<SentracSQLiteData>();
        this.SentracData = new ObservableCollection<SentracSQLiteData>(database.Table<SentracSQLiteData>());

            Debug.WriteLine("--------------------------");
            Debug.WriteLine(database.DatabasePath);
            Debug.WriteLine("--------------------------");

            // If the table is empty, initialize the collection
            if (!database.Table<SentracSQLiteData>().Any())
      {
     //   AddNewSentracSQLiteData();
      }
    }

    private void AddDataFromDotListToSQLList()
    {
            foreach (var dot in StaticValues.dotList)
            {
                if (dot != null)
                {
                    AddNewSentracSQLiteData(dot);
                }
            }
    }

    public void SaveAllSQLData()
    {
            AddDataFromDotListToSQLList();

            foreach (var data in this.SentracData)
            {
                if (data != null)
                {
                    SaveSentracSQLiteData(data);
                }
            }
    }
    
    
    public void AddNewSentracSQLiteData(DataDotObject dot)
    {
      this.SentracData.
      Add(new SentracSQLiteData
      {
        Date = dot.Date,
        Time = dot.Time,
        Value = dot.Value,
        Result = dot.Result
      });
    }

    // Use LINQ to query and filter data in other words to load SQLite Data
    public IEnumerable<SentracSQLiteData> GetFilteredSentracData(DateTime theDate)
    {
      // Use locks to avoid database collitions
      lock (collisionLock)
      {
        var query = from SentracData in database.Table<SentracSQLiteData>()
                    where SentracData.Date == theDate
                    select SentracData;
        return query.AsEnumerable();
      }
    }

    public int SaveSentracSQLiteData(SentracSQLiteData sentracSQLiteDataInstance)
    {
      lock (collisionLock)
      {
        if (sentracSQLiteDataInstance.Id != 0)
        {
          database.Update(sentracSQLiteDataInstance);
          return sentracSQLiteDataInstance.Id;
        }
        else
        {
          database.Insert(sentracSQLiteDataInstance);
          return sentracSQLiteDataInstance.Id;
        }
      }
    }

    public void SaveAllSentracData()
    {
      lock (collisionLock)
      {
        foreach (var sentracSQLiteDataInstance in this.SentracData)
        {
          if (sentracSQLiteDataInstance.Id != 0)
          {
            database.Update(sentracSQLiteDataInstance);
          }
          else
          {
            database.Insert(sentracSQLiteDataInstance);
          }
        }
      }
    }

    public int DeleteSentracSQLiteData(SentracSQLiteData sentracSQLiteDataInstance)
    {
      var id = sentracSQLiteDataInstance.Id;
      if (id != 0)
      {
        lock (collisionLock)
        {
          database.Delete<SentracSQLiteData>(id);
        }
      }
      this.SentracData.Remove(sentracSQLiteDataInstance);
      return id;
    }

  }
}
