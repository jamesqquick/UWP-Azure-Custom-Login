using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Custom_Login
{
	class AzureCustomUserHelper
	{

		public static async Task<string> LoginAsync(string username, string password)
		{
			string response = "";
			var results = await App.mobileService.GetTable<User>().Where(User => User.Username == username
		   && User.Password == password).ToListAsync();
			
			if (results.Count == 0)
			{
				response = "Username/Password combination is invalid";
				Debug.WriteLine(response);
			}
			else
			{
				response = "Success";
				Debug.WriteLine(response);
				//Save user credentials
				SaveUserCredentials(username, password);
			}

			return response;
		}

		public static async Task<string> SignUpAsync(string username, string password, string email)
		{
			string response = "";
			User newUser = new User { Password = password, Username = username, Id = username, Email = email};

			try
			{
				//Check to see if username is taken
				var results = await App.mobileService.GetTable<User>()
					.Where(User => User.Username == username)
					.ToListAsync();
				if (results.Count > 0)
				{
					response = "Username already taken";
				}
				else
				{
					await App.mobileService.GetTable<User>().InsertAsync(newUser);
					response = "Success";

					//Save user credentials
					SaveUserCredentials(username, password);
				}
				Debug.WriteLine(response);
				return response;
			}
			catch (Exception ex)
			{
				response  = "Sign Up Failed";
				Debug.WriteLine("Sign Up Exception: " + ex.Message);
			}
			Debug.WriteLine(response);
			return response;
		}

		private static void SaveUserCredentials(string username, string password)
		{
			PasswordVault vault = new PasswordVault();
			vault.Add(new PasswordCredential("Custom", username, password));
			App.mobileService.CurrentUser = new MobileServiceUser(username);
		}

		public static bool CheckLoggedIn()
		{
			PasswordVault vault = new PasswordVault();
			PasswordCredential credential = null;
			try
			{
				credential = vault.FindAllByResource("Custom").FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Vault Exception: " + ex.Message);
			}
			if (credential != null)
			{
				//Update Mobile Service Current User
				App.mobileService.CurrentUser = new MobileServiceUser(credential.UserName);
				return true;
			}
			else
				return false;
		}

		public async static Task<string> UpdateUserAsync(JObject update)
		{
			string response = "";
			try
			{

				await App.mobileService.GetTable<User>().UpdateAsync(update);
				response = "Success";		
			}
			catch (Exception ex)
			{
				response = "Update Failed";
				Debug.WriteLine("Update Failed: " + ex.Message);
			}

			return response;
		}

		public async static Task<string> UpdateUserAsync(User update)
		{
			string response = "";
			try
			{

				await App.mobileService.GetTable<User>().UpdateAsync(update);
				response = "Success";
			}
			catch (Exception ex)
			{
				response = "Update Failed";
				Debug.WriteLine("Update Failed: " + ex.Message);
			}

			return response;
		}
	}
}
