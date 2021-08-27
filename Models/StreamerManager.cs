using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using twitch_notify_v2.Repositories;
using twitch_notify_v2.View;
using twitch_notify_v2.ViewModel;

namespace twitch_notify_v2.Models
{
    class StreamerManager
    {
        private static StreamerManager _instance;

        public Page LivePage;
        public Page StreamerPage;
        public Page ConfigPage;

        public static StreamerManager Instance
        {
            get { if (_instance == null) { _instance = new StreamerManager(); } return _instance; }
        }

        public void AddStreamer(Streamer streamer)
        {
            //Add streamer to repository
            StreamerRepository.Instance.AddStreamer(streamer);

            //Update live monitor to incorporate new streamer
            LiveMonitor.Instance.RefreshStreamerList();

            //Refresh offline and online streamer list 
            StreamerPageVM spVM = (StreamerPageVM)StreamerPage.DataContext;
            spVM.RefreshStreamers();

            //Refresh the live page
            LivePageVM lpVM = (LivePageVM)LivePage.DataContext;
            lpVM.RefreshStreamers();
        }

        public void RemoveStreamer(Streamer streamer)
        {
            //Remove streamer from repository
            StreamerRepository.Instance.RemoveStreamer(streamer);

            //Refresh the live page
            LivePageVM lpVM = (LivePageVM)LivePage.DataContext;
            lpVM.RefreshStreamers();

            //Remove streamer from potential live pool
            LiveMonitor.Instance.RefreshStreamerList();

            //Refresh offline and online streamer list 
            StreamerPageVM spVM = (StreamerPageVM)StreamerPage.DataContext;
            spVM.RefreshStreamers();
        }
    }
}
