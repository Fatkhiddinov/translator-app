using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Threading;

namespace translator_app
{
    public partial class WorkSpace : Form
    {

        String Text;
        int p, c, n;
        int btn_counter = 0;
        List<String> textParts = new List<String>();

        public WorkSpace()
        {
            InitializeComponent();


        }

        

        private void axWindowsMediaPlayer1_Resize(object sender, EventArgs e)
        {
            





        }

        private void WorkSpace_Resize(object sender, EventArgs e)
        {
            Size formSize = new Size(this.Width/2, this.Height/2);
            Point mediaPlayerLocation = new Point(this.Width / 4, 0);
            axWindowsMediaPlayer1.Location = mediaPlayerLocation;
            axWindowsMediaPlayer1.Size = formSize;

            Point zeroPoint = new Point(0, 0);

            richTextBox1.Location = new Point(this.Width / 4, axWindowsMediaPlayer1.Height);
            richTextBox1.Size = new Size(axWindowsMediaPlayer1.Width, this.Height / 6);


            richTextBox2.Location = new Point(this.Width / 4, axWindowsMediaPlayer1.Height+richTextBox1.Height);
            richTextBox2.Size = new Size(axWindowsMediaPlayer1.Width, this.Height / 6);

            richTextBox3.Location = new Point(this.Width / 4, axWindowsMediaPlayer1.Height + richTextBox1.Height + richTextBox2.Height);
            richTextBox3.Size = new Size(axWindowsMediaPlayer1.Width, this.Height / 6);

            panel1.Size = new Size(axWindowsMediaPlayer1.Width, 46);
            panel1.Location = new Point(axWindowsMediaPlayer1.Location.X, axWindowsMediaPlayer1.Height-46);
            panel1.BackColor = Color.White;



            button1.Width = this.Width/4;
            button1.Location= zeroPoint;
            button1.Text = (button1.Size).ToString() ;


            button2.Location = new Point(axWindowsMediaPlayer1.Size.Width + button1.Size.Width);
            button2.Width = this.Width - button1.Width - axWindowsMediaPlayer1.Width;
            button2.Text = (button2.Size).ToString();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textParts.Count() > n)
            {
                textParts[p] = richTextBox1.Text;
                textParts[c] = richTextBox2.Text;
                textParts[n] = richTextBox3.Text;

                p += 4; c += 4; n += 4;


                richTextBox1.Text = textParts[p];
                richTextBox2.Text = textParts[c];
                richTextBox3.Text = textParts[n];

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
                Button b = new Button();
                b.Text = textBox1.Text;
                b.Size = button7.Size;
                b.Name = "btn_" + textBox1.Text;
                b.Location = new Point(button7.Location.X, button7.Location.Y + button7.Height * (btn_counter+1) );
                b.Click += new EventHandler(this.characterButtonFunct);
                Controls.Add(b);
                btn_counter++;
            

        }

        public void characterButtonFunct(object sender, EventArgs e)
        {
            var button = sender as Button;
            richTextBox2.Text = button.Text + "--" + richTextBox2.Text;
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            
        }

        ToolTip tip = new ToolTip();

        private void richTextBox2_MouseMove(object sender, MouseEventArgs e)
        {

            string word = GetWord(richTextBox2.Text, richTextBox2.GetCharIndexFromPosition(e.Location));
            
            //var chr = richTextBox2.GetCharIndexFromPosition(GetMousePositionWindowsForms());
            tip.ToolTipTitle = word;
            Point p = richTextBox2.Location;
            tip.Show(Translate(word), this, p.X + e.X,
                        p.Y + e.Y + 32, //You can change it (the 35) to the tooltip's height - controls the tooltips position.
                        1000);
            Thread.Sleep(1000);
        }

        public static string GetWord(string input, int position) //Extracts the whole word the mouse is currently focused on.
        {
            char s = input[position];
            int sp1 = 0, sp2 = input.Length;
            for (int i = position; i > 0; i--)
            {
                char ch = input[i];
                if (ch == ' ' || ch == '\n')
                {
                    sp1 = i;
                    break;
                }
            }

            for (int i = position; i < input.Length; i++)
            {
                char ch = input[i];
                if (ch == ' ' || ch == '\n')
                {
                    sp2 = i;
                    break;
                }
            }

            return input.Substring(sp1, sp2 - sp1).Replace("\n", "");
        }

        public String Translate(String word)
        {
            var toLanguage = "tr";//English
            var fromLanguage = "en";//Turkish
            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + fromLanguage + "&tl=" + toLanguage + "&dt=t&q=" + HttpUtility.UrlEncode(word);
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                return result;
            }
            catch
            {
                return "Error";
            }
        }

        public static Point GetMousePositionWindowsForms()
        {
            var point = Control.MousePosition;
            return new Point(point.X, point.Y);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            
            
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               
                var OpenFile = new System.IO.StreamReader(openFileDialog2.FileName);


                string[] splittedText = File.ReadAllLines(openFileDialog2.FileName);

                textParts.AddRange(splittedText);
                Text = OpenFile.ReadToEnd();
            }

            p = 2;
            c = 6;
            n = 10;
            richTextBox1.Text = textParts[p];
            richTextBox2.Text = textParts[c];
            richTextBox3.Text = textParts[n];

            

            int blockStart = 2; //arbitrary numbers to test
            int blockLength = 4;
            richTextBox1.SelectionStart = blockStart;
            richTextBox1.SelectionLength = blockLength;
            richTextBox1.SelectionBackColor = Color.Yellow;


        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (p > 4)
            {

                textParts[p] = richTextBox1.Text;
                textParts[c] = richTextBox2.Text;
                textParts[n] = richTextBox3.Text;

                p -= 4; c -= 4; n -= 4;
                richTextBox1.Text = textParts[p];
                richTextBox2.Text = textParts[c];
                richTextBox3.Text = textParts[n];


            }

        }
    }
    }

