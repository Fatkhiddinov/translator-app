﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace translator_app
{
    public partial class FileProjectCreate : Form
    {
        public FileProjectCreate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label7.Text = getVideoFile();
            axWindowsMediaPlayer1.URL = label7.Text;
        }
        private string getVideoFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Video File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "mp4",
                Filter = "video files (*.mp4)|*.mp4",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return "File not selected!";
        }

        private string getSubFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Subtitle File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "mp4",
                Filter = "subtitle files (*.srt)|*.srt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return "File not selected!";
        }

        private string getFilePath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
                return "File not selected";
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            label8.Text = getFilePath();
        }

        private void FileProjectCreate_Load(object sender, EventArgs e)
        {
            getLanguages(comboBox1);
            getLanguages(comboBox2);
            
            groupBox1.Parent = pictureBox1;
            groupBox1.BackColor = Color.Transparent;

            groupBox2.Parent = pictureBox1;

            pictureBox2.Parent = pictureBox1;

            transparentLabel(label10);
            transparentButton(button5);
            transparentButton(button6);

        }
        
        private void saveData()
        {   
            

            string user = Login.userID;
            DateTime today = DateTime.Now;

            string date = today.ToString("dd/MM/yyyy HH:mm:ss");
            string name = textBox1.Text;
            string fromLang = comboBox1.Text;
            string toLang = comboBox2.Text;
            string videoPath = label7.Text;
            string subPath = label9.Text;
            string folderPath = label8.Text;
            string type = "file";

            if(user!="" && date != "" && name != "" && fromLang != "" && toLang != "" && videoPath != "" && subPath != "" && folderPath != "")
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [projects] ([user_id], [date], [type], [name], [from_lang], [to_lang], [video_file], [sub_file], [folder]) VALUES ( @user, @date, @type, @name, @from_lang, @to_lang, @video_file, @sub_file, @folder); SELECT SCOPE_IDENTITY();", con))
                    {

                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@from_lang", fromLang);
                        cmd.Parameters.AddWithValue("@to_lang", toLang);
                        cmd.Parameters.AddWithValue("@video_file", videoPath);
                        cmd.Parameters.AddWithValue("@sub_file", subPath);
                        cmd.Parameters.AddWithValue("@folder", folderPath);

                        try
                        {
                            int modified = Convert.ToInt32(cmd.ExecuteScalar());

                            if (con.State == System.Data.ConnectionState.Open) con.Close();
                            FileProject.project_id = modified.ToString();

                            this.Hide();
                            var pr = new FileProject();
                            pr.Closed += (s, args) => this.Close();
                            pr.Show();

                        }
                        catch (Exception ex)
                        {
                            label11.Text = ex.Message;
                        }


                    }
                }
            }
            else
            {
                label11.Text = "Fill all required fields!";
            }

            
        }
        private void getLanguages(ComboBox comboBox)
        {
            var list = new List<string>();
            using (var conn = new SqlConnection())
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                conn.ConnectionString = connString;
                conn.Open();
                using (var cmd = new SqlCommand("SELECT lang FROM languages", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader["lang"]));
                        }
                    }
                }
            }
            comboBox.DataSource = new BindingSource(list, null);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.close();
            saveData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label9.Text = getSubFile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        private void transparentLabel(Label label)
        {
            label.Parent = pictureBox1;
            label.BackColor = System.Drawing.Color.Transparent;
        }

        private void transparentButton(Button button)
        {
            button.Parent = pictureBox1;
            button.BackColor = System.Drawing.Color.Transparent;
        }
        private void transparentRadioButton(RadioButton button)
        {
            button.Parent = pictureBox1;
            button.BackColor = System.Drawing.Color.Transparent;
        }
    }
}
