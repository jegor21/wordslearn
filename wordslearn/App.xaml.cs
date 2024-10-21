namespace wordslearn
{
    public partial class App : Application
    {
        private static WordDatabase _database;

        public static WordDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new WordDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WordsLearnDB.db3"));
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

    }
}
