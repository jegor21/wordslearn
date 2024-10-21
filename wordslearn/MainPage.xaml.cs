using System.Collections.ObjectModel;

namespace wordslearn
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Word> Words { get; set; }

        // Define LearnedCount and NotLearnedCount as public properties
        private int _learnedCount;
        public int LearnedCount
        {
            get => _learnedCount;
            set
            {
                _learnedCount = value;
                OnPropertyChanged(nameof(LearnedCount));  // Notify that the value has changed
            }
        }

        private int _notLearnedCount;
        public int NotLearnedCount
        {
            get => _notLearnedCount;
            set
            {
                _notLearnedCount = value;
                OnPropertyChanged(nameof(NotLearnedCount));  // Notify that the value has changed
            }
        }
        public MainPage()
        {
            InitializeComponent();
            Words = new ObservableCollection<Word>();
            BindingContext = this;
            // Initial load
            LoadWords();
        }

        // Called every time MainPage appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadWords();  // Ensure the word list is reloaded every time the page appears
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

                UpdateWordCounts();  // Update the learned/unlearned word counts
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void UpdateWordCounts()
        {
            int totalWordsCount = Words.Count;
            int learnedCount = Words.Count(w => w.Category == "Learned");
            int notLearnedCount = Words.Count(w => w.Category == "Not learned");

            totalWordsCountLabel.Text = totalWordsCount.ToString();
            LearnedCount = learnedCount;  // Update the property
            NotLearnedCount = notLearnedCount;  // Update the property
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
                bool confirm = await DisplayAlert("Confirmation", $"Delete '{currentWord.Name}'?", "Yes", "No");
                if (confirm)
                {
                    await App.Database.DeleteWordAsync(currentWord);
                    Words.Remove(currentWord); // Immediately remove from list
                    UpdateWordCounts();        // Update the word counts
                }
            }
        }
    }

}
