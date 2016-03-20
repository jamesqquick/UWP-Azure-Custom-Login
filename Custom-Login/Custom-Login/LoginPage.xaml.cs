using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Custom_Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
		private bool isLoggingin = true;

        public LoginPage()
        {
            this.InitializeComponent();
			this.Loaded += LoginPage_Loaded;		
        }

		private async void LoginPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (AzureCustomUserHelper.CheckLoggedIn())
			{

				Debug.WriteLine("User is logged in");
				NavigateHome();
			}

		}

	
		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			ToggleProgressRing();
			if(UsernameText.Text.Equals("") || PasswordText.Password.Equals(""))
			{
				await new Windows.UI.Popups.MessageDialog("Please ensure Username and Password are entered").ShowAsync();
			}
			else
			{
				string response = "";
				if (isLoggingin)
					response = await AzureCustomUserHelper.LoginAsync(UsernameText.Text, PasswordText.Password);
				else
				{
					if (!PasswordText.Password.Equals(RePasswordText.Password))
					{
						response = "Ensure that your passwords match";
					}
					else if (EmailText.Text.Equals(""))
						response = "Please enter your email address";
					else
						response = await AzureCustomUserHelper.SignUpAsync(UsernameText.Text, PasswordText.Password, EmailText.Text);
				}
				if (response.Equals("Success"))
					NavigateHome();
				else
					await new Windows.UI.Popups.MessageDialog(response).ShowAsync();
			}
			ToggleProgressRing();
		}

		private void ToggleProgressRing()
		{
			MyProgress.IsActive = !MyProgress.IsActive;
		}


		private void ToggleText_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (isLoggingin)
			{
				TitleText.Text = "Sign Up";
				LoginButton.Content = "Sign Up";
				ToggleText.Text = "Login";
				isLoggingin = false;
				EmailText.Visibility = Visibility.Visible;
				RePasswordText.Visibility = Visibility.Visible;
			}
			else
			{
				TitleText.Text = "Login";
				LoginButton.Content = "Login";
				ToggleText.Text = "Sign Up";
				isLoggingin = true;
				EmailText.Visibility = Visibility.Collapsed;
				RePasswordText.Visibility = Visibility.Collapsed;
			}
		}

		public void NavigateHome()
		{
			//Navigate to homescreen
			this.Frame.Navigate(typeof(Home));
		}

		protected override void OnKeyUp(KeyRoutedEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				LoginButton_Click(null, null);
			}
		}
	}
}
