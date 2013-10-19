using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ServiceModel.Channels;

namespace MiniCrm
{
    public partial class MainPage : PhoneApplicationPage
    {

        private sforce.SoapClient _Binding;
        private string _SessionId;
        private string _ServerURL;
        // Url of Home page
        private string MainUri = "/Html/index.html";
        bool HasInit = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Browser.IsScriptEnabled = true;
            
            
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            _Binding = new sforce.SoapClient();

            _Binding.loginCompleted += binding_loginCompleted;

            _Binding.loginAsync("lhenderson@madmagi.homelinux.net", "g0dsgracewEM0nR3A9skygXqRLqgdps9zg");

            // Add your URL here
          //  Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Navigates back in the web browser's navigation stack, not the applications.
        private void BackApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        // Navigates forward in the web browser's navigation stack, not the applications.
        private void ForwardApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
        }

        // Navigates to the initial "home" page.
        private void HomeMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        void binding_loginCompleted(object sender, sforce.loginCompletedEventArgs e)
        {
            //e.Results, e.Error, e.Cancelled, e.UserState

            // We are logged In
            
            sforce.LoginResult lr = (sforce.LoginResult)e.Result;
            _SessionId = lr.sessionId;
            _ServerURL = lr.serverUrl;

            // Now use the SOAP Partner API to get a list of Contacts
            SessionHeader sh = new SessionHeader();
            sh.sessionId = lr.sessionId;

            _Binding = new sforce.SoapClient();           
            
            AddressHeader addressHeader1 = AddressHeader.CreateAddressHeader("SessionHeader","partner.soap.sforce.com" , sh);
            AddressHeader[] addressHeaders1 = new AddressHeader[1] { addressHeader1 };

            _Binding.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(_ServerURL),addressHeader1);

            _Binding.queryCompleted += _Binding_queryCompleted;

            string query = "SELECT Id, Name, Phone from Contact";

            _Binding.queryAsync(query, e.UserState);
           
            
        }

        List<string> dylist = new List<string>();

        void _Binding_queryCompleted(object sender, sforce.queryCompletedEventArgs e)
        {
            var dataset = e.Result.records;
            foreach (MiniCrm.sforce.sObject record in dataset)
            {
                //dylist.Add(new { inner = record.Any[0].Value, name = record.Any[1].Value });
                dylist.Add(record.Any[0].Value + "|" + record.Any[1].Value + "|" + record.Any[2].Value);

                //var inner = record.Any[0].Value;
                //var name = record.Any[1].Value;
                //Console.WriteLine(name);
                //foreach (var item in record.Any)
                //{
                //    var inner = item.InnerText;
                //    var name = item.LocalName;
                //}
            } 

             // navigate to the main index page
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }


        // Handle navigation failures.
        private void Browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            MessageBox.Show("Navigation to this page failed, check your internet connection");

           
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                if (HasInit == false)
                {
                    string[] args = dylist.ToArray();
                    // replace with array of the list items, appended together like  "DFGSDFGRSDRGRKEY|James Jameson"
                    Browser.InvokeScript("SFInit", args);
                    HasInit = true;
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        private void Browser_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }
    }
}
