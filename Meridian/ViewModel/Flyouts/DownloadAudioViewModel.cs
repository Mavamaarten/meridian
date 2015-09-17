using System;
using System.Linq;
using System.Net;
using GalaSoft.MvvmLight.Command;
using Meridian.Controls;
using Meridian.Model;
using Meridian.Services;
using Neptune.UI.Extensions;
using Application = System.Windows.Application;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Meridian.ViewModel.Flyouts
{
    public class DownloadAudioViewModel : ViewModelBase
    {
        private VkAudio _track;
        private string _title;
        private string _artist;
        private string _lyrics;
        private int _downloadProgress;
        private WebClient _webClient;
        
        #region Commands

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand CloseCommand { get; private set; }

        #endregion

        public VkAudio Track
        {
            get { return _track; }
            set
            {
                if (Set(ref _track, value))
                    Load();
            }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string Artist
        {
            get { return _artist; }
            set { Set(ref _artist, value); }
        }

        public int DownloadProgress
        {
            get { return _downloadProgress;}
            set { Set(ref _downloadProgress, value); }
        }

        public DownloadAudioViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand(Close);
            SaveCommand = new RelayCommand(Save);
        }

        private void Load()
        {
            Title = _track.Title;
            Artist = _track.Artist;
        }

        private void Save()
        {
            IsWorking = true;

            try
            {
                var savefiledialog = new SaveFileDialog
                {
                    AddExtension = true,
                    CheckFileExists = false,
                    Filter = "mp3 files (*.mp3)|*.mp3",
                    FileName = string.Format("{0} - {1}.mp3", _artist, _title)
                };

                if (savefiledialog.ShowDialog() != true) return;

                _webClient = new WebClient();
                _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
                _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
                _webClient.DownloadFileAsync(new Uri(_track.Source), savefiledialog.FileName);
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }
        }

        void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            IsWorking = false;
            Close();
        }

        void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress = e.ProgressPercentage;
            RaisePropertyChanged("DownloadProgress");
        }

        private void Close()
        {
            var flyout = Application.Current.MainWindow.GetVisualDescendents().FirstOrDefault(c => c is FlyoutControl) as FlyoutControl;
            if (flyout != null)
                flyout.Close();
        }
    }
}
