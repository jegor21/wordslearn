using System.Collections.ObjectModel;

namespace wordslearn
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Word> Words { get; set; }

        private int _learnedCount;
        public int LearnedCount
        {
            get => _learnedCount;
            set
            {
                _learnedCount = value;
                OnPropertyChanged(nameof(LearnedCount));
            }
        }

        private int _notLearnedCount;
        public int NotLearnedCount
        {
            get => _notLearnedCount;
            set
            {
                _notLearnedCount = value;
                OnPropertyChanged(nameof(NotLearnedCount));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Words = new ObservableCollection<Word>();
            BindingContext = this;
            LoadWords();

            // Subscribe to messages for word modifications
            MessagingCenter.Subscribe<AddPage>(this, "WordModified", (sender) =>
            {
                LoadWords();
            });
            MessagingCenter.Subscribe<EditPage>(this, "WordModified", (sender) =>
            {
                LoadWords();
            });
            MessagingCenter.Subscribe<DeletePage>(this, "WordModified", (sender) =>
            {
                LoadWords();
            });
        }

        private async void LoadWords()
        {
            try
            {
                var words = await App.Database.GetWordsAsync();
                Words.Clear();
                foreach (var word in words)
                {
                    Words.Add(word);
                }

                // Update the learned/unlearned word counts
                UpdateWordCounts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        // Method to update the labels with word counts
        private void UpdateWordCounts()
        {
            int totalWordsCount = Words.Count;
            LearnedCount = Words.Count(w => w.Category == "Выучено");
            NotLearnedCount = Words.Count(w => w.Category == "Не выучено");

            totalWordsCountLabel.Text = totalWordsCount.ToString(); // Only this needs label reference
        }

        private async void OnWordSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Word selectedWord)
            {
                await Navigation.PushAsync(new DetailsPage(selectedWord));
            }
        }

        private async void OnAddWordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage());
        }

        private async void OnEditWordClicked(object sender, EventArgs e)
        {
            if (WordsCarousel.CurrentItem is Word currentWord)
            {
                await Navigation.PushAsync(new EditPage(currentWord));
            }
        }

        private async void OnDeleteWordClicked(object sender, EventArgs e)
        {
            if (WordsCarousel.CurrentItem is Word currentWord)
            {
                await Navigation.PushAsync(new DeletePage(currentWord));
            }
        }
    }
}
