﻿using SQLite;
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

    // Use LINQ to query and filter data
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

                        // TODO - Do I need to have Id to be able to avoid collision and do saves etc...?
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
