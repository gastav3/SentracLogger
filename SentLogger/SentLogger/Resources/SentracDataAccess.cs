using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;
using SentLogger.Resources;
using SentLogger.Models;

namespace LocalDataAccess
{
    /// <summary>
    /// To be able to handle and access SQLite data.
    /// </summary>
    class SentracDataAccess
    {
        private SQLiteConnection database;
        private static object collisionLock = new object();

        public ObservableCollection<SentracSQLiteData> SentracData { get; set; }

        DateTime theDate;
        TimeSpan theTime;
        Double theValue;
        string theResult;

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
                AddNewSentracSQLiteData(theDate, theTime, theValue, theResult);
            }
        }

        /// <summary>
        /// To be able to add a new row of data to the SQLite database, not connected to any entries for 
        /// in parameters at the moment.
        /// </summary>
        /// <param name="theDate"></param>
        /// <param name="theTime"></param>
        /// <param name="theValue"></param>
        /// <param name="theResult"></param>
        public void AddNewSentracSQLiteData(DateTime theDate, TimeSpan theTime, Double theValue, string theResult)
        {
            this.SentracData.
            Add(new SentracSQLiteData
            {
                Date = theDate,
                Time = theTime,
                Value = theValue,
                Result = theResult
            });
        }

    /// <summary>
    /// Use LINQ to query and filter data in other words to load SQLite Data.
    /// </summary>
    /// <param name="theDate">In parameter to be able to load from a specific date.</param>
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
