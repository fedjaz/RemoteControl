using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage(this);
        }

        public void Login(AppSettings appSettings)
        {
            MainPage = new ControlPage(this, appSettings);
        }

        public void BackToLogin()
        {
            MainPage = new LoginPage(this);
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
