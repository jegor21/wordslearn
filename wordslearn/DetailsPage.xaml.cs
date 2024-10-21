namespace wordslearn
{
    public partial class DetailsPage : ContentPage
    {
        private Word _word;

        public DetailsPage(Word word)
        {
            InitializeComponent();
            _word = word;
            BindingContext = _word;
        }

        // This is the missing method causing the error
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Ensure you are saving the selected category
            _word.Category = categoryPicker.SelectedItem.ToString();
            await App.Database.SaveWordAsync(_word);

            // Navigate back after saving
            await Navigation.PopAsync();
        }
    }
}
