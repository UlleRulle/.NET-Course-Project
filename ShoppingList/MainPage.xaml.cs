using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingList
{
    /// <summary>
    /// MainPage is where user login or send user to registration
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public UserProfile ActiveUser;
        public static MainPage Instance;

        public MainPage()
        {
            this.InitializeComponent();
            Instance = this;
        }

        /// <summary>
        /// Checks if password input matches the users hashed password from database
        /// </summary>
        /// <param name="userProfile">The UserProfile object</param>
        private void CheckUserPassword(UserProfile userProfile)
        {
            var passwordBoxPassword = txtPassword as PasswordBox;
            var passwordInput = passwordBoxPassword.Password.ToString();
            using (SHA512 sha256Hash = SHA512.Create())
            {
                if(VerifyHash(sha256Hash, passwordInput, ActiveUser.Password))
                {
                    this.Frame.Navigate(typeof(ListPage));
                }
                else
                {
                    ActiveUser = null;
                    statusMessage.Text = "Invalid password";
                }
            }
        }

        /// <summary>
        /// Compares the password input with the hashed password
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm</param>
        /// <param name="input">The password input</param>
        /// /// <param name="hash">The hashed password</param>
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        /// <summary>
        /// Hashes the password input
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm</param>
        /// <param name="input">The password input</param>
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
        /// Checks if the username exists in the database
        /// </summary>
        /// <param name="userName">The username string</param>
        private async Task GetUserByUserName(string userName)
        {
            string httpResponseBody;

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

            Uri requestUri = new Uri($"http://localhost:54493/api/UserProfiles/user/{userName}");

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            httpResponseBody = "";

            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponseBody == "")
            {
                statusMessage.Text = "This user does not exist";
                return;
            }
            ActiveUser = JsonConvert.DeserializeObject<UserProfile>(httpResponseBody);
            CheckUserPassword(ActiveUser);
        }

        /// <summary>
        /// Fetch username input, checks if username exists
        /// </summary>
        public async Task UserLogIn()
        {
            statusMessage.Text = "Logging in...";
            var textBoxUserName = txtUserName as TextBox;
            var userNameInput = textBoxUserName.Text;
            try
            {
                await GetUserByUserName(userNameInput);
            }
            catch (Exception ex)
            {
                statusMessage.Text = "Connection error" + "\n" + "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }

        /// <summary>
        /// Log in button click event
        /// </summary>
        /// <param name="sender">The object (Button)</param>
        /// <param name="e">The event</param>
        private async void LogInBtnClicked(object sender, RoutedEventArgs e)
        {
            statusMessage.Text = "";
            await UserLogIn();
        }

        /// <summary>
        /// Navigate to RegisterPage
        /// </summary>
        /// <param name="sender">The object (Button)</param>
        /// <param name="e">The event</param>
        private void RegisterBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }
    }
}
