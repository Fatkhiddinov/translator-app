using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace translator_app
{
    public partial class WorkSpace : Form
    {

        String Text;
        int p, c, n;
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

