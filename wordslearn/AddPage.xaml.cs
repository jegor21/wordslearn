namespace wordslearn
{
    public partial class AddPage : ContentPage
    {
        public AddPage()
        {
            InitializeComponent(); // Make sure this is called to initialize XAML components.
        }

        private async void OnSaveWordClicked(object sender, EventArgs e)
        {
            var newWord = new Word
            {
                Name = entryName.Text,
                Translation = entryTranslation.Text,
                Explanation = entryExplanation.Text,
                Category = entryCategory.Text
            };

            await App.Database.SaveWordAsync(newWord);

            // Notify MainPage that a new word has been added
            MessagingCenter.Send(this, "WordAdded");

            await Navigation.PopAsync();
        }


    }
}
