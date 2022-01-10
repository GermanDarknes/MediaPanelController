using System;
using System.Windows.Forms;

namespace MediaPanelController.UI
{
    class TrayIconContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayContextMenu;

        public TrayIconContext(string trayIconHoverText)
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = trayIconHoverText;
            trayIcon.Icon = Resources.IconData.Radio_White;

            trayContextMenu = new ContextMenu();
            trayContextMenu.MenuItems.Add(new MenuItem("Close", Close));

            trayIcon.ContextMenu = trayContextMenu;

            trayIcon.Visible = true;
        }

        void Close(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
