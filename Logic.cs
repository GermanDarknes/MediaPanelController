using System;
using Windows.Media.Control;
using MediaPanelController.Communication;
using MediaPanelController.UI;
using MediaPanelController.Utility;

namespace MediaPanelController
{
    internal class Logic
    {
        private TrayIconContext trayContext;

        private SerialCommunicator serialCommunicator = null;
        private MediaCommunicator mediaCommunicator = null;
        internal Logic()
        {
            trayContext = new TrayIconContext();
            serialCommunicator = new SerialCommunicator("MediaPanel");
            mediaCommunicator = new MediaCommunicator(SendMediaPropertyToSerial);
        }
        internal TrayIconContext getContext()
        {
            return trayContext;
        }

        private void SendMediaPropertyToSerial(GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
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
