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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Custom_Login
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Home : Page
	{
		public Home()
		{
			this.InitializeComponent();
			
		}

		private async void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//Removed stored credentials for user
				PasswordVault vault = new PasswordVault();
				vault.Remove(vault.Retrieve("Custom", App.mobileService.CurrentUser.UserId));
				//Log out of MobileServiceClient
				await App.mobileService.LogoutAsync();
				Debug.WriteLine("Logout Successful");
				//Navigate to Login Page
				this.Frame.Navigate(typeof(LoginPage));
				
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Logout Exception: " + ex.Message);

			}
        }
	}
}
