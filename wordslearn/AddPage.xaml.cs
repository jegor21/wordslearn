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
                Category = "Не выучено"
            };

            await App.Database.SaveWordAsync(newWord);

            // Send a message to update the word list and counts
            MessagingCenter.Send(this, "WordModified");

            await Navigation.PopAsync();
        }




    }
}
