using System;
using System.Collections.Generic;
using System.Text;

namespace SentLogger.Resources
{
  /// <summary>
  /// SQLite database connection.
  /// </summary>
  public interface IDatabaseConnection
  {
    SQLite.SQLiteConnection DbConnection();
  }
}
