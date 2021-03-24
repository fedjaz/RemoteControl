using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        App mainPage;
        public LoginPage(App mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            if(LoadUser(out AppSettings appSettings))
            {
                loginEntry.Text = appSettings.User.Login;
                passwordEntry.Text = appSettings.User.Password;
                serverURIEntry.Text = appSettings.serverURI;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string login = loginEntry.Text;
            string password = passwordEntry.Text;
            string serverURI = serverURIEntry.Text;
            User user = new User(login, password);
            AppSettings appSettings = new AppSettings(user, serverURI);

            SaveUser(appSettings);
            mainPage.Login(appSettings);
        }

        void SaveUser(AppSettings appSettings)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(appSettings);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/appsettings.json";
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }

        bool LoadUser(out AppSettings appSettings)
        {  
            appSettings = new AppSettings();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/appsettings.json";
            if(File.Exists(path))
            {
                try
                {
                    string json;
                    using(StreamReader sr = new StreamReader(path))
                    {
                        json = sr.ReadToEnd();
                    }
                    appSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings>(json);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}