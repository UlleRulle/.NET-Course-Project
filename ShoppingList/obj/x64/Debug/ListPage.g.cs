﻿#pragma checksum "C:\Users\ulrla\source\repos\ConsoleDatabaseContext\ConsoleDatabaseContext\ShoppingList\ListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D0528B43C26BC838CE115D5AC538650"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingList
{
    partial class ListPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // ListPage.xaml line 12
                {
                    this.ViewListPageFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 3: // ListPage.xaml line 40
                {
                    this.MyList = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.MyList).SelectionChanged += this.OpenSavedShoppingList;
                }
                break;
            case 4: // ListPage.xaml line 50
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.SaveShoppingList;
                }
                break;
            case 5: // ListPage.xaml line 51
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.AddNewItemBox;
                }
                break;
            case 6: // ListPage.xaml line 52
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.ClickEventNewList;
                }
                break;
            case 7: // ListPage.xaml line 53
                {
                    this.BtnDelete = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnDelete).Click += this.ClickEventDeleteList;
                }
                break;
            case 8: // ListPage.xaml line 54
                {
                    this.ListTitle = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9: // ListPage.xaml line 56
                {
                    this.ListOfItems = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 11: // ListPage.xaml line 26
                {
                    this.BtnLogout = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnLogout).Click += this.ClickEventLogOut;
                }
                break;
            case 12: // ListPage.xaml line 27
                {
                    this.topText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13: // ListPage.xaml line 28
                {
                    this.BtnRefresh = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnRefresh).Click += this.ClickEventPingServer;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

