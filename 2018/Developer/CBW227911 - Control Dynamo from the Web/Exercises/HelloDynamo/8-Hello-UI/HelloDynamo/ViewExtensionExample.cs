﻿using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using HelloDynamo.Stats;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace HelloDynamo
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtensionExample : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Hello Dynamo View Extension";

        private MenuItem extensionMenu;
        private ViewLoadedParams viewLoadedParams;
        private DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        public void Startup(ViewStartupParams vsp) { }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
        /// </summary>
        /// <param name="vlp">
        /// Parameters that provide references to Dynamo commands, settings, events and
        /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
        /// </param>
        public void Loaded(ViewLoadedParams vlp)
        {
            MessageBox.Show("Hello there, viewExtension has loaded!");

            // hold a reference to the Dynamo params to be used later
            viewLoadedParams = vlp;

            // we can now add custom menu items to Dynamo's UI
            MakeMenuItems();
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        public void MakeMenuItems()
        {
            // let's now create a completely top-level new menu item
            extensionMenu = new MenuItem { Header = "Dynamo Hackathon" };

            // and now we add a new sub-menu item that says hello when clicked
            var sayHelloMenuItem = new MenuItem { Header = "Say Hello" };
            sayHelloMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName);
            };

            // and a submenu item that shows the window
            var statsMenuItem = new MenuItem { Header = "Show Statistics" };
            statsMenuItem.Click += (sender, args) =>
            {
                var viewModel = new StatsViewModel(viewLoadedParams);
                var window = new StatsWindow
                {
                    // Set the data context for the main grid in the window.
                    MainGrid = { DataContext = viewModel },

                    // Set the owner of the window to the Dynamo window.
                    Owner = viewLoadedParams.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };

            // now make a hackathon worthy menu item
            var hackMenuItem = new MenuItem { Header = "Hack !" };
            hackMenuItem.Click += (sender, args) =>
            {
                Process.Start("https://giphy.com/search/hackathon");
            };

            // add all menu items to menu
            extensionMenu.Items.Add(sayHelloMenuItem);
            extensionMenu.Items.Add(statsMenuItem);
            extensionMenu.Items.Add(hackMenuItem);

            // finally, we need to add our menu to Dynamo
            viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }
    }
}
