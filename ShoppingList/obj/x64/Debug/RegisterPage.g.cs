﻿#pragma checksum "C:\Users\ulrla\source\repos\ConsoleDatabaseContext\ConsoleDatabaseContext\ShoppingList\RegisterPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ED73523F0D1E056B983CDFE227951C2C"
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
    partial class RegisterPage : 
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
            case 2: // RegisterPage.xaml line 12
                {
                    this.RegisterPageFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 3: // RegisterPage.xaml line 24
                {
                    this.txtUserName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // RegisterPage.xaml line 25
                {
                    this.txtFirstName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // RegisterPage.xaml line 26
                {
                    this.txtLastName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // RegisterPage.xaml line 27
                {
                    this.txtPassword = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 7: // RegisterPage.xaml line 28
                {
                    this.txtRePassword = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 8: // RegisterPage.xaml line 37
                {
                    this.statusMessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // RegisterPage.xaml line 34
                {
                    this.btnConfirm = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnConfirm).Click += this.ConfirmBtnClicked;
                }
                break;
            case 10: // RegisterPage.xaml line 35
                {
                    this.btnBack = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnBack).Click += this.BackBtnClicked;
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
