using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using twitch_notify_v2.Config;
using twitch_notify_v2.Models;
using twitch_notify_v2.Repositories;
using twitch_notify_v2.View;
using TwitchLib.Api;

namespace twitch_notify_v2.ViewModel
{
    class MainViewModel : ViewModelBase
    {

        private Page LivePage = new LivePage();
        private Page StreamerPage = new StreamerPage();
        private Page ConfigPage = new ConfigPage();

        public MainViewModel()
        {
            StreamerManager.Instance.LivePage = LivePage;
            StreamerManager.Instance.StreamerPage = StreamerPage;
            StreamerManager.Instance.ConfigPage = ConfigPage;
        }

        private RelayCommand<string> _navigationCommand;
        public RelayCommand<string> NavigationCommand
        {
            get
            {
                if (_navigationCommand == null)
                    _navigationCommand = new RelayCommand<string>(Navigate);

                return _navigationCommand;
            }
        }

        private void Navigate(string target)
        {
            Console.WriteLine("Navigating to target: " + target);

            switch (target?.ToLower())
            {
                case "live":
                    CurrentPage = LivePage;
                    break;
                case "streamers":
                    CurrentPage = StreamerPage;
                    break;
                case "config":
                    CurrentPage = ConfigPage;
                    break;
            }

            RaisePropertyChanged("CurrentPage");
        }

        private Page _currentPage = new LivePage();
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }
    }
}
