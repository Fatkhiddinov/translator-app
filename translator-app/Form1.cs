﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Windows.Forms;


namespace translator_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = textBox1.Text;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Boş
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();

            if(openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox2.Text = openFileDialog2.FileName;
                var OpenFile = new System.IO.StreamReader(openFileDialog2.FileName);
                richTextBox1.Text = OpenFile.ReadToEnd();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox3.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += this.listBox1.SelectedItem.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = Translate(textBox4.Text);
        }
        public String Translate(String word)
        {
            var toLanguage = "en";//English
            var fromLanguage = "tr";//Turkish
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
    }
}
