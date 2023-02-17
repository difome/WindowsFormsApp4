using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private int streamHandle;

        public Form1()
        {
            InitializeComponent();

            if (!Bass.BASS_Init(-1, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                MessageBox.Show("Error initializing BASS");
                Close();
            }
        }

        private async Task PlayButton_ClickAsync()
        {
            string url = "http://178.88.167.62:8080/DALA_320";
            var streamCreationTask = Task.Run(() => Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_STATUS, null, IntPtr.Zero));

            streamHandle = await streamCreationTask;
            var tagsHandle = Bass.BASS_ChannelGetTags(streamHandle, BASSTag.BASS_TAG_META);

            if (tagsHandle != IntPtr.Zero)
            {
                var tags = Utils.IntPtrAsStringAnsi(tagsHandle);

                var streamTitleMatch = Regex.Match(tags, @"StreamTitle='(?<title>.+?)';");


                if (streamTitleMatch.Success)
                {
                    var streamTitle = streamTitleMatch.Groups["title"].Value;
                    label1.Text = streamTitle;
                }
            }


            if (!Bass.BASS_ChannelPlay(streamHandle, false))
            {
                MessageBox.Show("Error initializing BASS");

                return;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await PlayButton_ClickAsync();
        }

    }
}
