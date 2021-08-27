using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using twitch_notify_v2.Models;

namespace twitch_notify_v2.ViewModel
{
    class ConfigPageVM : ViewModelBase
    {
        public ConfigPageVM()
        {
            LoadConfig();
        }

        public string ClientId { get { return Models.Config.Instance.ClientId; } set { Models.Config.Instance.ClientId = value; } }
        public string AccessToken { get { return Models.Config.Instance.AccessToken; } set { Models.Config.Instance.AccessToken = value; } }

        private RelayCommand _verifyCommand;
        public RelayCommand VerifyCommand
        {
            get
            {
                if (_verifyCommand == null)
                    _verifyCommand = new RelayCommand(VerifyButton);
                return _verifyCommand;
            }
        }

        private RelayCommand _helpCommand;
        public RelayCommand HelpCommand
        {
            get
            {
                if (_helpCommand == null)
                    _helpCommand = new RelayCommand(ShowHelp);
                return _helpCommand;
            }
        }

        private RelayCommand _startWithWindowsCheckboxCommand;
        public RelayCommand StartWithWindowsCheckboxCommand
        {
            get
            {
                if (_startWithWindowsCheckboxCommand == null)
                    _startWithWindowsCheckboxCommand = new RelayCommand(ToggleStartWithWindows);
                return _startWithWindowsCheckboxCommand;
            }
        }

        private RelayCommand _minimizeToTrayCheckboxCommand;
        public RelayCommand MinimizeToTrayCheckboxCommand
        {
            get
            {
                if (_minimizeToTrayCheckboxCommand == null)
                    _minimizeToTrayCheckboxCommand = new RelayCommand(ToggleMinimizeToTray);
                return _minimizeToTrayCheckboxCommand;
            }
        }

        private RelayCommand _clickAlertToOpenTwitchCheckboxCommand;
        public RelayCommand ClickAlertToOpenTwitchCheckboxCommand
        {
            get
            {
                if (_clickAlertToOpenTwitchCheckboxCommand == null)
                    _clickAlertToOpenTwitchCheckboxCommand = new RelayCommand(ToggleClickAlertToOpenTwitch);
                return _clickAlertToOpenTwitchCheckboxCommand;
            }
        }

        private RelayCommand<string> _alertDurationCommand;
        public RelayCommand<string> AlertDurationCommand
        {
            get
            {
                if (_alertDurationCommand == null)
                    _alertDurationCommand = new RelayCommand<string>(SetAlertDuration);
                return _alertDurationCommand;
            }
        }

        public string StartWithWindowsCheckboxPath
        {
            get
            {
                if (Models.Config.Instance.StartWithWindows)
                    return "pack://application:,,,/Resources/Icons/Checkmark.png";
                return "pack://application:,,,/Resources/Icons/Checkcross.png";
            }
        }

        public string MinimizeToTrayCheckboxPath
        {
            get
            {
                if (Models.Config.Instance.MinimizeToTray)
                    return "pack://application:,,,/Resources/Icons/Checkmark.png";
                return "pack://application:,,,/Resources/Icons/Checkcross.png";
            }
        }

        public string ClickAlertToOpenTwitchCheckboxPath
        {
            get
            {
                if (Models.Config.Instance.ClickAlertToShowTwitch)
                    return "pack://application:,,,/Resources/Icons/Checkmark.png";
                return "pack://application:,,,/Resources/Icons/Checkcross.png";
            }
        }

        private Brush _passiveButtonBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#32495f");
        private Brush _ActiveButtonBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#21d1b4");

        public Brush Alert1BackgroundColor
        {
            get
            {
                if (Models.Config.Instance.AlertDuration == 1)
                    return _ActiveButtonBrush;
                return _passiveButtonBrush;
            }
        }
        public Brush Alert2BackgroundColor
        {
            get
            {
                if (Models.Config.Instance.AlertDuration == 2)
                    return _ActiveButtonBrush;
                return _passiveButtonBrush;
            }
        }
        public Brush Alert3BackgroundColor
        {
            get
            {
                if (Models.Config.Instance.AlertDuration == 3)
                    return _ActiveButtonBrush;
                return _passiveButtonBrush;
            }
        }
        public Brush Alert4BackgroundColor
        {
            get
            {
                if (Models.Config.Instance.AlertDuration == 4)
                    return _ActiveButtonBrush;
                return _passiveButtonBrush;
            }
        }

        private Brush SuccessBorderBrush = new SolidColorBrush(Color.FromArgb(1, 0, 0, 1));
        private Brush FailureBorderBrush = new SolidColorBrush(Color.FromArgb(0, 1, 0, 1));
        public Brush ClientIdBorderBrush { get; set; }
        public Brush AccessTokenBorderBrush { get; set; }
        private async void VerifyButton()
        {
            Config.TwitchAPIConfig.Instance.TwitchAPI.Settings.ClientId = ClientId;
            Config.TwitchAPIConfig.Instance.TwitchAPI.Settings.AccessToken = AccessToken;

            bool result = await VerifyCredentials();
            if (result)
            {
                SaveConfig();
                LiveMonitor.Instance.StartMonitor();
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine("Showing help");
        }

        private void ToggleStartWithWindows()
        {
            Models.Config.Instance.StartWithWindows = !Models.Config.Instance.StartWithWindows;
            RaisePropertyChanged("StartWithWindowsCheckboxPath");
            SaveConfig();
        }

        private void ToggleMinimizeToTray()
        {
            Models.Config.Instance.MinimizeToTray = !Models.Config.Instance.MinimizeToTray;
            RaisePropertyChanged("MinimizeToTrayCheckboxPath");
            SaveConfig();
        }

        private void ToggleClickAlertToOpenTwitch()
        {
            Models.Config.Instance.ClickAlertToShowTwitch = !Models.Config.Instance.ClickAlertToShowTwitch;
            RaisePropertyChanged("ClickAlertToOpenTwitchCheckboxPath");
            SaveConfig();
        }

        private void SetAlertDuration(string duration)
        {
            int dur = Int32.Parse(duration);
            Models.Config.Instance.AlertDuration = dur;
            SaveConfig();

            RaisePropertyChanged("Alert1BackgroundColor");
            RaisePropertyChanged("Alert2BackgroundColor");
            RaisePropertyChanged("Alert3BackgroundColor");
            RaisePropertyChanged("Alert4BackgroundColor");
        }

        public async void LoadConfig()
        {
            // Determine resource paths
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "TwitchNotifier");
            string resourceName = "config.json";
            string resourceLocation = Path.Combine(specificFolder, resourceName);

            if (File.Exists(resourceLocation))
            {
                using (StreamReader file = File.OpenText(resourceLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    Models.Config credential = (Models.Config)serializer.Deserialize(file, typeof(Models.Config));

                    Models.Config.Instance.ClientId = credential.ClientId;
                    Models.Config.Instance.AccessToken = credential.AccessToken;
                    Models.Config.Instance.StartWithWindows = credential.StartWithWindows;
                    Models.Config.Instance.MinimizeToTray = credential.MinimizeToTray;
                    Models.Config.Instance.ClickAlertToShowTwitch = credential.ClickAlertToShowTwitch;
                    Models.Config.Instance.AlertDuration = credential.AlertDuration;

                    Config.TwitchAPIConfig.Instance.TwitchAPI.Settings.ClientId = credential.ClientId;
                    Config.TwitchAPIConfig.Instance.TwitchAPI.Settings.AccessToken = credential.AccessToken;

                    bool verifyResult = await VerifyCredentials();
                    if (verifyResult)
                    {
                        LiveMonitor.Instance.StartMonitor();
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: File at location: " + resourceLocation + " does not exist!");
            }
        }
        private async Task<bool> VerifyCredentials()
        {
            Console.WriteLine("ConfigPageVM: Verifying credentials...");

            //Correct credentials
            if (await Config.TwitchAPIConfig.Instance.VerifyCredentials())
            {
                Console.WriteLine("ConfigPageVM: Verify success");
                ClientIdBorderBrush = SuccessBorderBrush;
                AccessTokenBorderBrush = SuccessBorderBrush;
                return true;
            }

            Console.WriteLine("ConfigPageVM: Verify failed");
            //Incorrect credentials
            ClientIdBorderBrush = FailureBorderBrush;
            AccessTokenBorderBrush = FailureBorderBrush;
            return false;
        }

        private void SaveConfig()
        {
            // Determine resource paths
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "TwitchNotifier");
            string resourceName = "config.json";
            string resourceLocation = Path.Combine(specificFolder, resourceName);

            // Make sure the directory exists
            Directory.CreateDirectory(specificFolder);

            // Write to file
            using (StreamWriter file = File.CreateText(resourceLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Models.Config.Instance);
                file.Close();
                file.Dispose();
            }
        }
    }
}
