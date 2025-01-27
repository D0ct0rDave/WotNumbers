﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Code;

namespace WinApp.Forms
{
    public partial class Replay : FormCloseOnEsc
    {
        private int _battleId { get; set; }
        private string _filename { get; set; }
        public Replay(int battleId)
        {
            InitializeComponent();
            _battleId = battleId;
        }

        private async void Replay_Shown(object sender, EventArgs e)
        {
            // await GetvBAddictUploadInfo();
            FileInfo fi = await ReplayHelper.GetReplayFile(_battleId);
            if (fi != null)
            {
                lblMessage.Text = "Replay file for the current battle is found.";
                txtPath.Text = Path.GetDirectoryName(fi.FullName);
                txtFile.Text = Path.GetFileName(fi.FullName);
                _filename = fi.FullName;
            }
            else
            {
                lblMessage.Text = "Sorry, could not find any replay file for this battle.";
                btnPlayReplay.Enabled = false;
                btnShowFolder.Enabled = false;
            }
        }

        private void btnShowFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", _filename));
        }

        private void btnPlayReplay_Click(object sender, EventArgs e)
        {
            Process[] p = Process.GetProcessesByName("WorldOfTanks");
            if (p.Length > 0 && p[0].ProcessName == "WorldOfTanks")
                MsgBox.Show("It seems like World of Tanks is already running. Shut down WoT to be able to play replay", "WoT is running");
            Process.Start("explorer.exe", _filename);
        }
                
        //private async Task GetvBAddictUploadInfo()
        //{
        //    linkvBAddictUpload.Text = await vBAddictHelper.GetInfoUploadedvBAddict(_battleId);
        //    toolTipvBAddictLink.SetToolTip(linkvBAddictUpload, "Go to battle report at vBAddict");
        //}

        //private void linkvBAddictUpload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    if (linkvBAddictUpload.Text != "")
        //    {
        //        // http://www.vbaddict.net/battlereport/user-server/map-nation-tankname-battleId
        //        string serverURL = string.Format("http://www.vbaddict.net/battlereport/{0}-{1}/{2}",
        //            Config.Settings.playerName.ToLower(), // user
        //            ExternalPlayerProfile.GetServer, // server
        //            vBAddictHelper.GetReplayURLInfo(_battleId) // map - nation - tankname - battleid
        //            );
        //        // string serverURL = string.Format("http://www.vbaddict.net/player/{0}-{1}", Config.Settings.playerName.ToLower(), ExternalPlayerProfile.GetServer);
        //        System.Diagnostics.Process.Start(serverURL);
        //    }
        //}
    }
}
