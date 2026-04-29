using System;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class DiscoverPage : ContentPage
    {
        private readonly DiscoverViewModel viewModel;

        public DiscoverPage(DiscoverViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            BindingContext = viewModel;
        }

        private async void Search_Clicked(object? sender, EventArgs e)
        {
            await viewModel.RunSearchAsync();
        }
    }
}
