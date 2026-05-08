using System;
using System.Collections.Generic;
using System.Text;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class LibraryPage : ContentPage
    {
        private readonly LibraryViewModel viewModel;

        public LibraryPage(LibraryViewModel viewModel)
        {
            InitializeComponent();

            // FLOW:
            // LibraryPage receives LibraryViewModel from MauiProgram.
            // BindingContext connects LibraryPage.xaml labels/lists to LibraryViewModel.
            this.viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // FLOW:
            // Every time the Library page opens, reload saved games from SQLite.
            await viewModel.LoadLibraryAsync();
        }
    }
}
