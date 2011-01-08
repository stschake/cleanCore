using System;
using System.Windows.Forms;
using cleanCore;
using cleanCore.D3D;

namespace TeleportBook
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Offsets.Initialize();
            Pulse.OnFrame += OnFrame;

            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        static void OnFrame(object sender, EventArgs e)
        {
            Teleporter.Pulse();
        }
    }
}
