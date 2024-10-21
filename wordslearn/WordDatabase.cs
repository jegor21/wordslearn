using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;


namespace wordslearn
{
    public class WordDatabase
    {
        private SQLiteAsyncConnection _database;

        public WordDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Word>().Wait();
        }

        public Task<List<Word>> GetWordsAsync()
        {
            return _database.Table<Word>().ToListAsync();
        }

        public Task<int> SaveWordAsync(Word word)
        {
            if (word.ID != 0)
            {
                return _database.UpdateAsync(word);
            }
            else
            {
                return _database.InsertAsync(word);
            }
        }

        public Task<int> DeleteWordAsync(Word word)
        {
            return _database.DeleteAsync(word);
        }

        public Task<Word> GetWordAsync(int id)
        {
            return _database.Table<Word>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
    }
}
