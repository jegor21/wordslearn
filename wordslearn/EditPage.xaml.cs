﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordslearn
{
    public partial class EditPage : ContentPage
    {
        private Word _word;

        public EditPage(Word word)
        {
            InitializeComponent();
            _word = word;
            entryName.Text = word.Name;
            entryTranslation.Text = word.Translation;
            entryExplanation.Text = word.Explanation;
            entryCategory.Text = word.Category;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _word.Name = entryName.Text;
            _word.Translation = entryTranslation.Text;
            _word.Explanation = entryExplanation.Text;
            _word.Category = entryCategory.Text;

            await App.Database.SaveWordAsync(_word);
            await Navigation.PopAsync();
        }
    }

}