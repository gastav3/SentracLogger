using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;

namespace LocalDataAccess
{
  /// <summary>
  /// SQLite, the table form we will store the incoming data in.
  /// </summary>
  [Table("SentracData")]
  public class SentracSQLiteData : INotifyPropertyChanged
  {
    private int _id;
    [PrimaryKey, AutoIncrement]
    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        this._id = value;
        OnPropertyChanged(nameof(Id));
      }
    }

    private DateTime _date;
    [NotNull]
    public DateTime Date
    {
      get
      {
        return _date;
      }
      set
      {
        this._date = value;
        OnPropertyChanged(nameof(Date));
      }
    }

    private TimeSpan _time;
    [NotNull]
    public TimeSpan Time
    {
      get
      {
        return _time;
      }
      set
      {
        this._time = value;
        OnPropertyChanged(nameof(Time));
      }
    }

    private double _value;
    [NotNull]
    public double Value
    {
      get
      {
        return _value;
      }
      set
      {
        _value = value;
        OnPropertyChanged(nameof(Value));
      }
    }

    private string _result;

    public string Result
    {
      get
      {
        return _result;
      }
      set
      {
        _result = value;
        OnPropertyChanged(nameof(Result));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      this.PropertyChanged?.Invoke(this,
        new PropertyChangedEventArgs(propertyName));
    }
  }
}