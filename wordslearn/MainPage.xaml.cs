using System.Collections.ObjectModel;
using System.Linq;

namespace wordslearn
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Word> Words { get; set; }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<EditPage>(this, "WordEdited", (senderPage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LoadWords(); // Reload the words after editing
                });
                MessagingCenter.Unsubscribe<EditPage>(this, "WordEdited");
            });

            MessagingCenter.Subscribe<DeletePage>(this, "WordDeleted", (senderPage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LoadWords(); // Reload the words after deleting
                });
                MessagingCenter.Unsubscribe<DeletePage>(this, "WordDeleted");
            });
        }

        public MainPage()
        {
            InitializeComponent();
            Words = new ObservableCollection<Word>();
            BindingContext = this;
            LoadWords();
            SubscribeToMessages(); // Subscribe to the messages
        }


        // Property to calculate total word count
        public string WordCountText => $"Total words: {Words.Count}";

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
                OnPropertyChanged(nameof(WordCountText)); // Notify the UI about the word count update
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnAddWordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage());

            // Ensure UI updates after the new word is added
            MessagingCenter.Subscribe<AddPage>(this, "WordAdded", async (senderPage) =>
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    LoadWords();
                });
                MessagingCenter.Unsubscribe<AddPage>(this, "WordAdded");
            });
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
