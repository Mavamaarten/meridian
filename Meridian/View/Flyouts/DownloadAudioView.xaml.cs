using System.Windows.Controls;
using Meridian.Model;
using Meridian.ViewModel;
using Meridian.ViewModel.Flyouts;

namespace Meridian.View.Flyouts
{
    /// <summary>
    /// Interaction logic for DownloadAudioView.xaml
    /// </summary>
    public partial class DownloadAudioView : UserControl
    {
        private DownloadAudioViewModel _viewModel;

        public DownloadAudioView(VkAudio audio)
        {
            InitializeComponent();

            _viewModel = new DownloadAudioViewModel();
            this.DataContext = _viewModel;
            _viewModel.Track = audio;
        }
    }
}
