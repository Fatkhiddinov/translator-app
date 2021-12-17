using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace translator_app
{
    public partial class VideoFromURL : Form
    {
        Subtitle result;
        int resultIDX=0;
        List<Subtitle> subtitles = new List<Subtitle>();
        double videoLocation = 0.0;
        public bool isPlaying = true;
        public VideoFromURL()
        {
            InitializeComponent();

            textBox2.Text = "https://r6---sn-cvb7ln7e.googlevideo.com/videoplayback?expire=1635790013&ei=Xdh_YaKTFO6NxgL3n4qIDA&ip=201.236.248.250&id=o-AGe8St-qJhPlsor18DB0m79HqqzNaXzp9f7VNP687xJC&itag=22&source=youtube&requiressl=yes&mh=xo&mm=31%2C26&mn=sn-cvb7ln7e%2Csn-hp57yne7&ms=au%2Conr&mv=m&mvi=6&pl=18&initcwndbps=895000&vprv=1&mime=video%2Fmp4&ns=IZKKkBC03oQzgfqKqBbfc7wG&cnr=14&ratebypass=yes&dur=1386.463&lmt=1537177253872686&mt=1635767815&fvip=4&fexp=24001373%2C24007246&c=WEB&n=ANq4JcL1iVkfew&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cns%2Ccnr%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRAIgBvOY1WyR1LqSoGVz_iUx68MFtEAi7JanjacILQ2c5nECIErvWqivaZrkiutLIuUJGDmW1SnSwg70dNncd_O-T8R3&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRQIgaDhM3WuHJpsvXqpa_HGjitVioErCV5uiC7EtyHcTER0CIQCcXGD51leIVbrXcjdGu7gfqGi8Unfm5cp3zZD6A7bYLA%3D%3D&title=Bu%20Duay%C4%B1%20Eden%20Ne%20%C4%B0sterse%20Elde%20Eder%20%7C%20%20Mehmet%20Y%C4%B1ld%C4%B1z";

            textBox1.Text = "https://www.youtube.com/watch?v=dLngvFZ1pQs";


            Thread newThread = new Thread(new ThreadStart(UpdateLabelThreadProc));

            newThread.Start();
        }

        class Subtitle
        {
            public string start { get; set; }
            public string dur { get; set; }
            public string text { get; set; }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var embed = "<html><head>" +
    "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
    "</head><body>" +
    "<iframe width=\"500\" height=\"280\" src=\"{0}\"" +
    "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
    "</body></html>";
            var url = textBox1.Text;
            string videoID = url.Split('=')[1];
            var embedUrl = "https://www.youtube.com/embed/" + videoID;


            this.webBrowser1.DocumentText = string.Format(embed, embedUrl);
            
            WebClient wc = new WebClient();
            //string theTextFile = wc.DownloadString("http://video.google.com/timedtext?lang=tr&v=" + videoID);
            byte[] filebytes = wc.DownloadData("http://video.google.com/timedtext?lang=tr&v=" + videoID);

            string textFile = Encoding.UTF8.GetString(filebytes);
            XDocument xmlDoc = XDocument.Parse(textFile);




            


            


            

            var startList = xmlDoc.Root.Elements("text")
                                       .Select(element => element.Attribute("start").Value)
                                       .ToList();
            var durList = xmlDoc.Root.Elements("text")
                                       .Select(element => element.Attribute("dur").Value)
                                       .ToList();
            var textList = xmlDoc.Root.Elements("text")
                                       .Select(element => element.Value)
                                       .ToList();

            for (int i = 0; i < textList.Count; i++)
            {
                Subtitle temp = new Subtitle();

                temp.start = startList[i];
                temp.dur = durList[i];
                temp.text = textList[i];

                subtitles.Add(temp);
            }

            foreach (var item in subtitles)
            {
                richTextBox1.Text += "start = " + item.start + " " + "dur = " + item.dur + " " + "text = " + item.text + "\n";
            }
        }

        
        void UpdateLabelThreadProc()
        {
            while (isPlaying)
            {
                this.BeginInvoke(new MethodInvoker(UpdateLabel));
                System.Threading.Thread.Sleep(10);
            }
        }

        string dur = "";
        string start = "";
        double durStop = 0.0;
        private void UpdateLabel()
        {
            videoLocation = Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 2);
            label2.Text = videoLocation.ToString();


            
            
            result = subtitles.Find(x => x.start == videoLocation.ToString().Replace(',', '.'));

            //if(subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.'))!=-1)
            //{
            //resultIDX = subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.'));
            //}

            for (int i = 0; i < subtitles.Count; i++)
            {

                double start = double.Parse(subtitles[i].start.Replace('.', ','));
                double dur = double.Parse(subtitles[i].dur.Replace('.', ','));
                double stop = start + dur;
                if (videoLocation > start && videoLocation < stop)
                {
                    resultIDX = i;
                }
            }

            if (result != null) {


                label3.Text = result.text.ToString();

                label3.Location = new Point(axWindowsMediaPlayer1.Location.X + axWindowsMediaPlayer1.Width / 2 - label3.Width / 2, axWindowsMediaPlayer1.Location.Y + axWindowsMediaPlayer1.Height - 100);
                label3.BackColor = Color.Black;
                label3.ForeColor = Color.White;
                start = result.start;
                dur = result.dur;
            }
            if (dur != "" && start != "")
            {
            
            durStop = double.Parse(dur.Replace('.',',')) + double.Parse(start.Replace('.',','));
            }
            if (durStop == videoLocation)
            {
                label3.Text = "";
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = textBox2.Text;

        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            isPlaying = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (resultIDX >= 0 && resultIDX <= subtitles.Count())
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = double.Parse(subtitles[resultIDX + 1].start.Replace('.', ','));
                axWindowsMediaPlayer1.Ctlcontrols.play();
                videoLocation = Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 3);
                if (subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.')) != -1)
                {
                    resultIDX = subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.'));
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (resultIDX >= 0 && resultIDX <= subtitles.Count())
            {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = double.Parse(subtitles[resultIDX - 1].start.Replace('.', ','));
            axWindowsMediaPlayer1.Ctlcontrols.play();
                videoLocation = Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 3);
                if (subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.')) != -1)
                {
                    resultIDX = subtitles.FindIndex(x => x.start == videoLocation.ToString().Replace(',', '.'));
                }
            }
        }

        private void axWindowsMediaPlayer1_PositionChange(object sender, AxWMPLib._WMPOCXEvents_PositionChangeEvent e)
        {
            
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
           
        }
    }
}
