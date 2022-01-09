using System;
using Windows.Media.Control;
using WindowsMediaController;

namespace MediaPanelController.Communication
{
    internal class MediaCommunicator
    {
        private MediaManager mediaManager = null;
        private Action<GlobalSystemMediaTransportControlsSessionMediaProperties> mediaPropertyCallback;

        public MediaCommunicator(Action<GlobalSystemMediaTransportControlsSessionMediaProperties> mediaPropertyCallback)
        {
            this.mediaPropertyCallback = mediaPropertyCallback;

            mediaManager = new MediaManager();

            mediaManager.OnAnyMediaPropertyChanged += MediaManagerOnAnyMediaPropertyChanged;

            mediaManager.Start();
        }

        private void MediaManagerOnAnyMediaPropertyChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            mediaPropertyCallback(mediaProperties);
        }
    }
}