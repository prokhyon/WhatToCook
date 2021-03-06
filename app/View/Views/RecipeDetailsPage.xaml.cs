﻿using WhatToCook.Common;
using WhatToCook.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhatToCook.ViewModels;
using WhatToCook.DataModel;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace WhatToCook.Views
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class RecipeDetailsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public RecipeDetailsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        private void AddToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var recipe = this.DefaultViewModel["Recipe"] as RecipeViewModel;
            if(!recipe.IsInMenu)
            {
                recipe.SetIsInMenu(true);
            }
            this.Frame.Navigate(typeof(FoodMenuPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarButton;
            var msg = "TODO: open " + button.Label + " page";
            switch (button.Label)
            {
                case "Menü":
                    this.Frame.Navigate(typeof(FoodMenuPage));
                    return;
                case "Kezdőoldal":
                    this.Frame.Navigate(typeof(StartPage));
                    return;
                case "Kedvencek":
                    this.Frame.Navigate(typeof(RecipesPage), new RecipesPageViewModel()
                    {
                        Title = "Kedvencek",
                        Recipes = AppCache.GetFavourites()
                    });
                    return;
                case "Mi van itthon?":
                    //TODO: 
                    break;
                case "Kedvenc":
                    //TODO
                    break;
            }
            //TODO: remove this 
            new Windows.UI.Popups.MessageDialog(msg).ShowAsync();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var recipeViewModel = e.NavigationParameter as RecipeViewModel;
            recipeViewModel.LoadDetails();
            this.DefaultViewModel["Recipe"] = recipeViewModel;
            if (recipeViewModel.IsFavourite)
            {
                FavoriteButton.Icon = new SymbolIcon(Symbol.SolidStar);

            }
            else
            {
                FavoriteButton.Icon = new SymbolIcon(Symbol.OutlineStar);
            }

        }




        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var recipe = this.DefaultViewModel["Recipe"] as RecipeViewModel;
            recipe.IsFavourite = !recipe.IsFavourite;

            if (recipe.IsFavourite)
            {
                var button = sender as AppBarButton;
                button.Icon = new SymbolIcon(Symbol.SolidStar);

            } else
            {
                var button = sender as AppBarButton;
                button.Icon = new SymbolIcon(Symbol.OutlineStar);
            }
        }

       
    }
}