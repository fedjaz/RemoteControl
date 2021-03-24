using ServerConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static ManualResetEvent waitHandle = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            AppSettings appSettings = LoadSettings();
            KeySender keySender = new KeySender();
            Connection connection = new Connection(appSettings.ServerURI);
            connection.Connected += () => connection.Login(appSettings.User);
            connection.RecievedKey += (c, keyData) => keySender.PressKey(keyData.KeyCode, keyData.Iskeyboard);
            connection.RecievedMouse += (c, mouseData) => keySender.MoveMouse(mouseData.MouseX, mouseData.MouseY);

            Connect(connection);
            waitHandle.WaitOne();
        }

        static async void Connect(Connection connection)
        {
            await connection.SetupConnection();
            if(connection.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
            {
                await connection.Reconnect();
            }
        }

        static AppSettings LoadSettings()
        {

            string path = AppContext.BaseDirectory;
            if(File.Exists($"{path}\\appsettings.json"))
            {
                using(StreamReader sr = new StreamReader($"{path}\\appsettings.json"))
                {
                    string json = sr.ReadToEnd();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings>(json);
                }
            }
            else
            {
                AppSettings appSettings = new AppSettings(new ServerConnector.User("admin", "admin"), "http://127.0.0.1:2754/remotecontrol");
                using(StreamWriter sw = new StreamWriter($"{path}\\appsettings.json"))
                {
                    sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(appSettings));
                }
                return appSettings;
            }
        }
    }
}
