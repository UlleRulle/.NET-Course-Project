using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace ShoppingList
{
    /// <summary>
    /// The page where the user process its ShoppingLists
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public static ObservableCollection<ShoppingList> UserShoppingLists;
        UserProfile ActiveUser = new UserProfile();
        MainPage MainPageReference;
        static readonly HttpClient client = new HttpClient();
        Boolean OfflineModus = false;

        public ListPage()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("http://localhost:54493/");
            }
            this.InitializeComponent();
            UserShoppingLists = new ObservableCollection<ShoppingList>();
            MainPageReference = MainPage.Instance;
            ActiveUser = MainPageReference.ActiveUser;
            PopulateListOfShoppingLists();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = this;
        }

        /// <summary>
        /// Fetch the user's shopping lists from database by UserID
        /// </summary>
        private async Task PopulateListOfShoppingLists()
        {
            var httpResponseBody = await GetResultFromAPI($"http://localhost:54493/api/ShoppingLists/user/{ActiveUser.UserID}");
            var listOfShoppingList = JsonConvert.DeserializeObject<List<ShoppingList>>(httpResponseBody);
            UserShoppingLists = new ObservableCollection<ShoppingList>(listOfShoppingList as List<ShoppingList>);
            MyList.ItemsSource = UserShoppingLists;
        }

        /// <summary>
        /// Add new item box to list.
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private void AddNewItemBox(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            Button button = new Button();
            button.Click += new RoutedEventHandler(DeletePanelBtn_Click);
            button.Content = "Delete";
            TextBox newItemBox = new TextBox();
            newItemBox.MaxLength = 50;
            panel.Children.Add(newItemBox);
            panel.Children.Add(button);
            ListOfItems.Items.Add(panel);
        }

        /// <summary>
        /// Delete TextBox from ListView
        /// </summary>
        /// <param name="sender">The object (Button)</param>
        /// <param name="e">The event</param>
        private void DeletePanelBtn_Click(object sender, RoutedEventArgs e)
        {
            ListOfItems.Items.Remove(((sender as FrameworkElement).Parent));
        }

        /// <summary>
        /// Returns selected ShoppingList from ListView
        /// </summary>
        private ShoppingList GetSelectedList()
        {
            return MyList.SelectedItem as ShoppingList;
        }

        /// <summary>
        /// Update an already existing shopping list in the database
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private void UpdateShoppingList(ShoppingList shoppingList)
        {
            string output = JsonConvert.SerializeObject(shoppingList);
            var content = new StringContent(output, Encoding.UTF8, "application/json");
            var response = client.PutAsync($"api/ShoppingLists/{shoppingList.ShoppingListID}", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageToast("List could not be updated");
                return;
            }
            UpdateShoppingListItems(shoppingList);
            UpdateMyListView(shoppingList);
        }

        /// <summary>
        /// Delete a shoppinglist's items in the database by using ShoppingListID
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private void DeleteShoppingListItems(ShoppingList shoppingList)
        {
            var response = client.DeleteAsync($"api/ShoppingListItems/ShoppingList/{shoppingList.ShoppingListID}").Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageToast("List item could not be deleted");
                return;
            }
        }

        /// <summary>
        /// Updates the ListView MyList
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private void UpdateMyListView(ShoppingList shoppingList)
        {
            var item = UserShoppingLists.FirstOrDefault(i => i.ShoppingListID == shoppingList.ShoppingListID);
            int j = UserShoppingLists.IndexOf(item);
            UserShoppingLists[j] = shoppingList;
            MyList.SelectedItem = shoppingList;
        }

        /// <summary>
        /// Post a list of ShoppingListItem to database
        /// </summary>
        /// <param name="shoppingListItems">List of ShoppingListItems</param>
        private void InsertShoppingListItems(List<ShoppingListItem> shoppingListItems)
        {
            string output = JsonConvert.SerializeObject(shoppingListItems);
            var content = new StringContent(output, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/ShoppingListItems/batch", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageToast("List item could not be saved");
                return;
            }
        }

        /// <summary>
        /// Update a shopping list's items by deleting all of them, then insert them
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private void UpdateShoppingListItems(ShoppingList shoppingList)
        {
            DeleteShoppingListItems(shoppingList);
            var shoppingListItems = GetShoppingListItemsFromView(shoppingList);
            InsertShoppingListItems(shoppingListItems);
        }

        /// <summary>
        /// Create ShoppingListItem from ListView. Add items to the shopping list object
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private List<ShoppingListItem> GetShoppingListItemsFromView(ShoppingList shoppingList)
        {
            List<ShoppingListItem> shoppingListItems = new List<ShoppingListItem>();
            foreach (var item in ListOfItems.Items)
            {
                var stackPanel = item as StackPanel;
                var itemTextBox = stackPanel.Children[0] as TextBox;
                var itemText = itemTextBox.Text;

                ShoppingListItem NewItem = new ShoppingListItem(itemText, shoppingList.ShoppingListID);
                shoppingListItems.Add(NewItem);
            }
            return shoppingListItems;
        }

        /// <summary>
        /// Save list in database. New lists gets saved to database. If selected list from MyList is selected, update list in database. If internet is not available, just store in UserShoppingLists
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private void SaveShoppingList(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ListTitle.Text))
            {
                ListTitle.Text = "(no title)";
            }
            ShoppingList NewShoppingList = new ShoppingList(ListTitle.Text, DateTime.Now, ActiveUser.UserID);
            ShoppingList selectedList = GetSelectedList();
            if (OfflineModus)
            {
                UserShoppingLists.Add(CreateOfflineShoppingList(NewShoppingList));
            }
            else
            {
                if (selectedList != null)
                {
                    NewShoppingList.ShoppingListID = selectedList.ShoppingListID;
                    UpdateShoppingList(NewShoppingList);
                }
                else
                {
                    NewShoppingList = CreateShoppingList(NewShoppingList);
                    if (OfflineModus == false)
                    {
                        var shoppingListItems = GetShoppingListItemsFromView(NewShoppingList);
                        InsertShoppingListItems(shoppingListItems);
                    }
                    UserShoppingLists.Add(NewShoppingList);
                }
            }
        }

        /// <summary>
        /// Return ShoppingList object without UserID and ShoppingListID
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private ShoppingList CreateOfflineShoppingList(ShoppingList shoppingList)
        {
            shoppingList.UserProfileUserID = 0;
            shoppingList.ShoppingListID = 0;
            shoppingList.Items = GetShoppingListItemsFromView(shoppingList);
            return shoppingList;
        }

        /// <summary>
        /// Post a new ShoppingList to database
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private ShoppingList CreateShoppingList(ShoppingList shoppingList)
        {
            try
            {
                string output = JsonConvert.SerializeObject(shoppingList);
                var content = new StringContent(output, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                response = client.PostAsync("api/ShoppingLists", content).Result;
                var jsonResponse = response.Content.ReadAsStringAsync();
                shoppingList = JsonConvert.DeserializeObject<ShoppingList>(jsonResponse.Result);
                return shoppingList;
            }
            catch (Exception ex)
            {
                MessageToast("Failed to save list in DB. Stored locally.");
                OfflineModus = true;
                topText.Text = "OFFLINE MODE";
                BtnRefresh.Visibility = Visibility.Visible;
                return CreateOfflineShoppingList(shoppingList);
            }
        }


        /// <summary>
        /// When list is clicked, an event is sent.
        /// </summary>
        /// <param name="sender">Object that sends the event</param>
        /// <param name="e">The event</param>
        private async void OpenSavedShoppingList(object sender, SelectionChangedEventArgs e)
        {
            BtnDelete.Visibility = Visibility.Visible;
            var view = sender as ListView;
            var selectedList = view.SelectedValue as ShoppingList;
            if(selectedList != null)
            {
                try
                {
                    await PopulateSelectedShoppingList(selectedList);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    OfflineModus = true;
                    topText.Text = "OFFLINE MODE";
                    BtnRefresh.Visibility = Visibility.Visible;
                    MessageToast("Unable to open saved list");
                }
            }
        }


        /// <summary>
        /// Populate the selected list with its items
        /// </summary>
        /// <param name="selectedList">The selected list</param>
        private async Task PopulateSelectedShoppingList(ShoppingList selectedList)
        {
            ListOfItems.Items.Clear();
            if (OfflineModus == false)
            {
                var httpResponseBody = await GetResultFromAPI($"http://localhost:54493/api/ShoppingListItems/ShoppingList/{selectedList.ShoppingListID}");
                var listOfShoppingListItems = JsonConvert.DeserializeObject<List<ShoppingListItem>>(httpResponseBody);
                selectedList.Items = listOfShoppingListItems;
            }
            ListTitle.Text = selectedList.Title;
            if (selectedList.Items == null || selectedList.Items.Count == 0)
            {
                return;
            }

            foreach (var item in selectedList.Items)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                Button button = new Button();
                button.Click += new RoutedEventHandler(DeletePanelBtn_Click);
                button.Content = "Delete";
                TextBox newItemBox = new TextBox();
                newItemBox.Text = item.Name;
                panel.Children.Add(newItemBox);
                panel.Children.Add(button);
                ListOfItems.Items.Add(panel);
            }
        }

        /// <summary>
        /// Fetch response from API
        /// </summary>
        /// /// <param name="uri">The Uri</param>
        public async Task<string> GetResultFromAPI(string uri)
        {
            try
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                var headers = httpClient.DefaultRequestHeaders;
                Uri requestShoppingListsUri = new Uri(uri);
                Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
                string httpResponseBody = "";

                httpResponse = await httpClient.GetAsync(requestShoppingListsUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                OfflineModus = false;
                topText.Text = "";
                return httpResponseBody;
            }
            catch (Exception ex)
            {
                MessageToast("Couldn't establish DB connection");
                OfflineModus = true;
                topText.Text = "OFFLINE MODE";
                MyList.ItemsSource = UserShoppingLists;
                BtnRefresh.Visibility = Visibility.Visible;
                return null;
            }
        }

        /// <summary>
        /// Show string in a toast
        /// </summary>
        /// <param name="text">The string</param>
        private static void MessageToast(string text)
        {
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Shopping List"
                        },
                        new AdaptiveText()
                        {
                            Text = text
                        }
                    }
                }
            };

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual
            };
            var toast = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        /// Checks if API can connect to the database
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private async void ClickEventPingServer(object sender, RoutedEventArgs e)
        {
            var response = await GetResultFromAPI("http://localhost:54493/api/ShoppingLists/ping");
            if (response == "true")
            {
                OfflineModus = false;
                PostLocalListsToServer();
                await PopulateListOfShoppingLists();
                BtnRefresh.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Post lists who have not been saved in the database
        /// </summary>
        private void PostLocalListsToServer()
        {
            var shoppingLists = UserShoppingLists.Where(t => t.ShoppingListID.Equals(0));
            if (shoppingLists.Count() > 0)
            {
                foreach (var list in shoppingLists)
                {
                    ShoppingList newList = new ShoppingList(list.Title, list.Date, ActiveUser.UserID);
                    newList = CreateShoppingList(newList);
                    newList.Items = list.Items;
                    newList.Items = GetItemsInLocalList(newList);
                    InsertShoppingListItems(newList.Items as List<ShoppingListItem>);
                }
            }
        }

        /// <summary>
        /// Get a list of ShoppingListItem
        /// </summary>
        /// <param name="shoppingList">The ShoppingList object</param>
        private List<ShoppingListItem> GetItemsInLocalList(ShoppingList shoppingList)
        {
            List<ShoppingListItem> newList = new List<ShoppingListItem>();
            foreach (var item in shoppingList.Items)
            {
                ShoppingListItem newItem = new ShoppingListItem(item.Name, shoppingList.ShoppingListID);
                newList.Add(newItem);
            }
            return newList;
        }

        /// <summary>
        /// Clears ListView of items, clears title text and deselects any selected items in MyList
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private void ClickEventNewList(object sender, RoutedEventArgs e)
        {
            ListOfItems.Items.Clear();
            ListTitle.Text = "";
            MyList.SelectedItem = null;
            BtnDelete.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Log out of UserProfile. Navigate back to MainPage
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private void ClickEventLogOut(object sender, RoutedEventArgs e)
        {
            MainPageReference = null;
            ActiveUser = null;
            UserShoppingLists = null;
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Delete selected list and its items
        /// </summary>
        /// <param name="sender">The sender (Button)</param>
        /// <param name="e">The event</param>
        private void ClickEventDeleteList(object sender, RoutedEventArgs e)
        {
            ListOfItems.Items.Clear();
            ListTitle.Text = "";
            var selectedList = GetSelectedList();
            if (OfflineModus == false)
            {
                foreach (var item in selectedList.Items)
                {
                    client.DeleteAsync($"api/ShoppingListItems/{item.ItemID}");
                }
                client.DeleteAsync($"api/ShoppingLists/{selectedList.ShoppingListID}");
            }
            UserShoppingLists.Remove(selectedList);
            BtnDelete.Visibility = Visibility.Collapsed;
        }
    }
}
