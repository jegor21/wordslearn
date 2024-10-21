using System.IO;
using SQLite;

namespace wordslearn
{
    public static class Constants
    {
        public const string DatabaseFilename = "VoorkeelDB.db3";
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;
    }
}
