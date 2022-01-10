using System;
using Windows.Media.Control;
using WindowsMediaController;

namespace MediaPanelController.Communication
{
    internal class MediaCommunicator
    {
        private MediaManager mediaManager = null;
        private Action<MediaManager.MediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties> mediaCallback;

        public MediaCommunicator(Action<MediaManager.MediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties> mediaPropertyCallback)
        {
            this.mediaCallback = mediaPropertyCallback;

            mediaManager = new MediaManager();

            mediaManager.OnAnyMediaPropertyChanged += MediaManagerOnAnyMediaPropertyChanged;

            mediaManager.Start();
        }

        private void MediaManagerOnAnyMediaPropertyChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            mediaCallback(mediaSession, mediaProperties);
        }
    }
}