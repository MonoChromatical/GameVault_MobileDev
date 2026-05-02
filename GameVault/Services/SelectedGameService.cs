using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;

namespace GameVault.Services
{
    // FLOW:
    // HomePage or DiscoverPage stores the tapped Game here.
    // GameDetailsPage reads the selected Game from here.

    public class SelectedGameService
    {
        public Game? SelectedGame {  get; set; }
    }
}
