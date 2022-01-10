using System;
using Windows.Media.Control;
using MediaPanelController.Communication;
using MediaPanelController.UI;
using MediaPanelController.Utility;
using WindowsMediaController;

namespace MediaPanelController
{
    internal class Logic
    {
        private TrayIconContext trayContext = null;

        private SerialCommunicator serialCommunicator = null;
        private MediaCommunicator mediaCommunicator = null;

        private const string deviceName = "MediaPanel";
        private const string audioPlayerID = "org.erb.sonixd";
        private const bool audioPlayerFilter = true;

        internal Logic()
        {
            trayContext = new TrayIconContext();
            serialCommunicator = new SerialCommunicator(deviceName);
            mediaCommunicator = new MediaCommunicator(SendMediaPropertyToSerial);
        }
        internal TrayIconContext getContext()
        {
            return trayContext;
        }

        private void SendMediaPropertyToSerial(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            if(audioPlayerFilter && !mediaSession.Id.Equals(audioPlayerID))
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
