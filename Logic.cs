using System;
using Windows.Media.Control;
using MediaPanelController.Communication;
using MediaPanelController.UI;
using MediaPanelController.Utility;
using WindowsMediaController;
using Windows.Media;

namespace MediaPanelController
{
    internal class Logic
    {
        private TrayIconContext trayContext = null;

        private SerialCommunicator serialCommunicator = null;
        private MediaCommunicator mediaCommunicator = null;

        private const string trayIconHoverText = "Media Panel Controller";

        private const string deviceName = "MediaPanel";
        private const string mediaPlayerID = "sonixd";
        private const bool mediaPlayerFilter = true;

        private const MediaPlaybackType mediaTypeID = MediaPlaybackType.Music;
        private const bool mediaTypeFilter = true;

        internal Logic()
        {
            trayContext = new TrayIconContext(trayIconHoverText);
            serialCommunicator = new SerialCommunicator(deviceName);
            mediaCommunicator = new MediaCommunicator(SendMediaPropertyToSerial);
        }
        internal TrayIconContext getContext()
        {
            return trayContext;
        }

        private void SendMediaPropertyToSerial(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            if (mediaTypeFilter && !mediaProperties.PlaybackType.Equals(mediaTypeID))
            {
                return;
            }

            if (mediaPlayerFilter && !mediaSession.Id.ToLower().Contains(mediaPlayerID))
            {
                return;
            }

            String songArtist = String.IsNullOrEmpty(mediaProperties.Artist)
                ? "Unknown Artist"
                : Utilities.RemoveDiacritics(mediaProperties.Artist.Trim());
            String songTitle = String.IsNullOrEmpty(mediaProperties.Title)
                ? "Unknown Title"
                : Utilities.RemoveDiacritics(mediaProperties.Title.Trim());

            serialCommunicator.Write(songArtist + ";" + songTitle);
        }
    }
}
