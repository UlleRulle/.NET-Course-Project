using System;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;


namespace ShoppingList
{
    /// <summary>
    /// Register new user to DB
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public string InputUserName { get; private set; }
        public string InputFirstName { get; private set; }
        public string InputLastName { get; private set; }
        public string InputPassword { get; private set; }
        public string InputRePassword { get; private set; }

        static readonly HttpClient client = new HttpClient();

        public RegisterPage()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("http://localhost:54493/");
            }
            this.InitializeComponent();
        }

        private void SetInputStrings()
        {
            InputUserName = txtUserName.Text;
            InputFirstName = txtFirstName.Text;
            InputLastName = txtLastName.Text;
            InputPassword = txtPassword.Password.ToString();
            InputRePassword = txtRePassword.Password.ToString();
        }

        /// <summary>
        /// Fetch user details. If new user details meets requirements, register new user.
        /// </summary>
        /// <param name="sender">Object that sends the event</param>
        /// <param name="e">The event</param>
        private async void ConfirmBtnClicked(object sender, RoutedEventArgs e)
        {
            SetInputStrings();
            statusMessage.Text = "";
            if(InputUserName == string.Empty || InputFirstName == string.Empty || InputLastName == string.Empty || InputPassword == string.Empty || InputRePassword == string.Empty)
            {
                statusMessage.Text = "Please fill out all fields.";
                return;
            }
            else if (!InputUserName.All(Char.IsLetterOrDigit))
            {
                statusMessage.Text = "Username can only contain letters and numbers.";
                return;
            }
            else if (!InputFirstName.All(Char.IsLetter))
            {
                statusMessage.Text = "Your name can only contain letters.";
                return;
            }
            else if (!InputLastName.All(Char.IsLetter))
            {
                statusMessage.Text = "Your name can only contain letters.";
                return;
            }

            if (InputPassword != InputRePassword)
            {
                Console.Write("Password doesnt match");
                Console.ReadLine();
                statusMessage.Text = "Password does not match. Please re-enter your password";
                return;
            }
            await CheckUserNameAvailability();
        }

        private void BackBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Creates new UserProfile and post to database
        /// </summary>
        private void CreateNewUserProfile()
        {
            InputPassword = HashPassword(InputPassword);
            UserProfile NewUserProfile = new UserProfile { UserName = InputUserName, FirstName = InputFirstName, LastName = InputLastName, Password = InputPassword };
            string output = JsonConvert.SerializeObject(NewUserProfile);
            var content = new StringContent(output, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/UserProfiles", content).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.Write("Success");
                statusMessage.Text = "Success! Welcome, " + NewUserProfile.UserName + "!";
            }
            else
            {
                Console.Write("Error");
                statusMessage.Text = "Unable to create new profile.";
            }
        }

        /// <summary>
        /// Creates a hash for the password string input
        /// </summary>
        /// <param name="password">Password string</param>
        private string HashPassword(string password)
        {
            using (SHA512 sha256Hash = SHA512.Create())
            {
                password = GetHash(sha256Hash, password);
            }
            return password;
        }

        /// <summary>
        /// Hash the input string to a hexadecimal string
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm</param>
        /// <param name="input">The string</param>
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Checks if input username already exists in database
        /// </summary>
        private async Task CheckUserNameAvailability()
        {
            string httpResponseBody;
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            Uri requestUri = new Uri($"http://localhost:54493/api/UserProfiles/user/{InputUserName}");

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            httpResponseBody = "";

            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponseBody == "")
            {
                CreateNewUserProfile();
            }
            else
            {
                statusMessage.Text = "Username is taken";
            }
        }
    }
}
