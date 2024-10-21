using System;
using System.Collections.ObjectModel;

namespace wordslearn
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Word> Words { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Words = new ObservableCollection<Word>();
            BindingContext = this;
            LoadWords();
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
                OnPropertyChanged(nameof(WordCountText)); // Notify UI of changes
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                Console.WriteLine($"Error loading words: {ex.Message}");
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

        public string WordCountText => $"{Words.Count(w => w.Category == "учим")} of {Words.Count} words learned";
    }
}
