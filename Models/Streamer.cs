using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Repositories;

namespace twitch_notify_v2.Models
{
    class Streamer : ObservableObject
    {
        /// <summary>
        /// Properties written to json
        /// </summary>

        private string _name;
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _avatarUrl;
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { _avatarUrl = value; }
        }

        private bool _notifyGameChange;
        [JsonProperty(PropertyName = "NotifyGameChange")]
        public bool NotifyGameChange
        {
            get { return _notifyGameChange; }
            set { _notifyGameChange = value; RaisePropertyChanged("ButtonGameChangeStyle"); }
        }

        private bool _showPopup;
        [JsonProperty(PropertyName = "ShowPopup")]
        public bool ShowPopup
        {
            get { return _showPopup; }
            set { _showPopup = value; RaisePropertyChanged("PopupStyle"); }
        }

        /// <summary>
        /// Ignored properties for json
        /// </summary>

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _game;
        [JsonIgnore]
        public string Game
        {
            get { return _game; }
            set { _game = value; }
        }

        private DateTime _startedAt;
        [JsonIgnore]
        public DateTime StartedAt
        {
            get { return _startedAt; }
            set { _startedAt = value; }
        }

        private bool _onlineStatus = false;
        [JsonIgnore]
        public bool OnlineStatus
        {
            get { return _onlineStatus; }
            set { _onlineStatus = value; }
        }

        [JsonIgnore]
        public string ButtonGameChangeStyle
        {
            get
            {
                if (NotifyGameChange)
                    return "pack://application:,,,/Resources/Icons/Checkmark.png";
                return "pack://application:,,,/Resources/Icons/Checkcross.png";
            }
        }

        [JsonIgnore]
        public string PopupStyle
        {
            get
            {
                if (ShowPopup)
                    return "pack://application:,,,/Resources/Icons/Checkmark.png";
                return "pack://application:,,,/Resources/Icons/Checkcross.png";
            }
        }

        private RelayCommand _buttonGameChangeCommand;
        [JsonIgnore]
        public RelayCommand ButtonGameChangeCommand
        {
            get
            {
                if (_buttonGameChangeCommand == null)
                    _buttonGameChangeCommand = new RelayCommand(ButtonGameChange);
                return _buttonGameChangeCommand;
            }
        }

        private RelayCommand _popupAlertCommand;
        [JsonIgnore]
        public RelayCommand PopupAlertCommand
        {
            get
            {
                if (_popupAlertCommand == null)
                    _popupAlertCommand = new RelayCommand(PopupChange);
                return _popupAlertCommand;
            }
        }

        private RelayCommand _removeStreamerCommand;
        [JsonIgnore]
        public RelayCommand RemoveStreamerCommand
        {
            get
            {
                if (_removeStreamerCommand == null)
                    _removeStreamerCommand = new RelayCommand(RemoveStreamer);
                return _removeStreamerCommand;
            }
        }

        private void ButtonGameChange()
        {
            NotifyGameChange = !NotifyGameChange;
            StreamerRepository.Instance.SaveStreamers();
        }
        private void PopupChange()
        {
            ShowPopup = !ShowPopup;
            StreamerRepository.Instance.SaveStreamers();
        }
        private void RemoveStreamer()
        {
            Console.WriteLine("Removing streamer with name: " + Name);
            StreamerManager.Instance.RemoveStreamer(this);
        }
    }
}
