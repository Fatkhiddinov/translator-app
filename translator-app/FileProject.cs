using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace translator_app
{
    public partial class FileProject : Form
    {
        public FileProject()
        {
            InitializeComponent();
        }
        public static string project_id = "";
        string user = "";
        string date = "";
        string name = "";
        string fromLang = "";
        string toLang = "";
        string videoPath = "";
        string subPath = "";
        string folderPath = "";
        string type = "";
        private void FileProject_Load(object sender, EventArgs e)
        {
            getProject();

            groupBox1.Parent = pictureBox1;
            groupBox2.Parent = pictureBox1;
            groupBox3.Parent = pictureBox1;
            groupBox4.Parent = pictureBox1;
            groupBox5.Parent = pictureBox1;
            groupBox6.Parent = pictureBox1;
            //panel1.Parent = pictureBox1;
            panel1.BackColor = Color.Transparent;
            button3.Parent = pictureBox1;
            button5.Parent = pictureBox1;
            button6.Parent = pictureBox1;



            
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label4.Text = name;
            loadListWithWords(listBox1);
            flowLayoutPanel1.AutoScroll = true;
            createButtons();

            openSRT(subPath);
            subEditor("none");
            
            if(type == "url")
            {
                axWindowsMediaPlayer1.Visible = false;
                WebBrowser web = new WebBrowser();
                web.Size = new Size(560, 300);
                
                playYoutubeVideo(web);
                panel1.Controls.Add(web);

            }
            fillOptions();
            axWindowsMediaPlayer1.URL = videoPath;
        }

        private void playYoutubeVideo(WebBrowser web)
        {
            if (videoPath.Contains("youtube"))
            {
                var embed = "<html><head>" +
    "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
    "</head><body>" +
    "<iframe width=\"535\" height=\"280\" src=\"{0}\" margin=\"0\"" +
    "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
    "</body></html>";
                var url = videoPath;
                string videoID = url.Split('=')[1];
                var embedUrl = "https://www.youtube.com/embed/" + videoID;


                web.DocumentText = string.Format(embed, embedUrl);
            }
        }

        List<Tuple<String, String>> list = new List<Tuple<String, String>>();
        int pos = 0;
        private void openSRT(string textFile)
        {
            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile); 
                string text = File.ReadAllText(textFile);
                int count = 3;
                for (int i = 2; i < lines.Length; i++)
                {
                    if (count == 3)
                    {
                    list.Add(new Tuple<string, string>(lines[i], "-"));
                        count = 0;
                    }
                    else
                    {
                        count += 1;
                    }
                }

                richTextBox1.Text = text;
            }
        }
        private void subEditor(string command)
        {
            if (pos == 0)
            {
                richTextBox2.Text = list[0].Item1;
                label5.Text = list[0].Item2;
                label6.Text = "";
                label7.Text = "";
                label8.Text = list[1].Item2;
                label9.Text = list[1].Item1;
            }
            if (command == "down")
            {
                if (pos <= list.Count)
                {
                    label6.Text = list[pos].Item2;
                    label7.Text = list[pos].Item1;
                    pos += 1;
                    richTextBox2.Text = list[pos].Item1;
                    label5.Text = list[pos].Item2;

                    if(pos == list.Count)
                    {

                        label8.Text = "";
                        label9.Text = "";
                    }
                    else
                    {

                        label8.Text = list[pos + 1].Item2;
                        label9.Text = list[pos + 1].Item1;
                    }
                }
            }
            if (command == "up")
            {
                if (pos > 0)
                {

                    label8.Text = list[pos].Item2;
                    label9.Text = list[pos].Item1;
                    pos -= 1;
                    richTextBox2.Text = list[pos].Item1;
                    label5.Text = list[pos].Item2;

                    if (pos == 0)
                    {

                        label6.Text = "";
                        label7.Text = "";
                    }
                    else
                    {

                        label6.Text = list[pos - 1].Item2;
                        label7.Text = list[pos - 1].Item1;
                    }
                }
            }

        }

        private void getProject()
        {
            var list = new List<string>();
            using (var conn = new SqlConnection())
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                conn.ConnectionString = connString;
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM projects Where id=" + project_id, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            date = reader["date"].ToString().Trim();
                            name = reader["name"].ToString().Trim();
                            fromLang = reader["from_lang"].ToString().Trim();
                            toLang = reader["to_lang"].ToString().Trim();
                            videoPath = reader["video_file"].ToString().Trim();
                            subPath = reader["sub_file"].ToString().Trim();
                            folderPath = reader["folder"].ToString().Trim();
                            type = reader["type"].ToString().Trim();
                            project_id = reader["ID"].ToString().Trim();
                            pos = Int32.Parse(reader["pos"].ToString().Trim());
                            

                        }
                    }
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string word = textBox1.Text;
            string translation = Translate(word);
            textBox2.Text = translation;
            saveTranslation(word, translation);
            loadListWithWords(listBox1);

        }

        private void loadListWithWords(ListBox listbox)
        {
            var list = new List<string>();
            using (var conn = new SqlConnection())
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                conn.ConnectionString = connString;
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM words WHERE project_id="+project_id, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        for (int i = 0; i < 10; i++)
                        {

                            if (reader.Read())
                            {

                            list.Add(Convert.ToString(reader["word"]).Trim() + " -> " + Convert.ToString(reader["translation"]).Trim());
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                }
            }
            listbox.DataSource = new BindingSource(list, null);
            listbox.Enabled = true;
        }
        private void saveTranslation(string word, string translation)
        {
            

            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO [words] ([word], [translation], [project_id]) VALUES (@word, @translation, @project_id)", con))
                {

                    cmd.Parameters.AddWithValue("@word", word);
                    cmd.Parameters.AddWithValue("@translation", translation);
                    cmd.Parameters.AddWithValue("@project_id", project_id);

                    cmd.ExecuteNonQuery();




                }
            }
        }
        public String Translate(String word)
        {
            var fromLanguage = fromLang;
            var toLanguage = toLang;
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private List<string> getCharacters()
        {
            var list = new List<string>();
            using (var conn = new SqlConnection())
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                conn.ConnectionString = connString;
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM characters WHERE project_id=" + project_id, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                                list.Add(Convert.ToString(reader["name"]).Trim());
                        }

                           

                        }
                    }
                }
            return list;
            }
        private void createButtons()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var character in getCharacters())
            {
                Button b = new Button();
                b.Size = new Size(190, 40);
                b.ForeColor = Color.Black;
                b.Text = character;


                b.Click += (sender, EventArgs) => { setCharacter(sender, EventArgs); };
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void setCharacter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            list[pos] = new Tuple<string, string>(list[pos].Item1, btn.Text);
            label5.Text = btn.Text;
        }

        private void saveCharacter(string character)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO [characters] ([name], [project_id]) VALUES (@name, @project_id)", con))
                {

                    cmd.Parameters.AddWithValue("@name", character);
                    cmd.Parameters.AddWithValue("@project_id", project_id);

                    cmd.ExecuteNonQuery();

                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            saveCharacter(textBox3.Text);
            createButtons();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UPDATE [projects] SET [pos]=@pos WHERE id=@id", con))
                    {

                        cmd.Parameters.AddWithValue("@pos", pos);
                        cmd.Parameters.AddWithValue("@id", project_id);


                        try
                        {
                            cmd.ExecuteScalar();



                        }
                        catch (Exception ex)
                        {
                            label20.Text = ex.Message;
                        }


                    }
                }
            
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            subEditor("up");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            subEditor("down");

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            list[pos] = new Tuple<string, string>(richTextBox2.Text,list[pos].Item2);
        }

        private void fillOptions()
        {
            textBox5.Text = name;
            getLanguages(comboBox1);
            getLanguages(comboBox2);
            if (type == "url")
            {
                textBox4.Text = videoPath;
                label13.Text = "";


            }
            else
            {

                label13.Text = videoPath; 
            }
                label15.Text = subPath;
            comboBox1.SelectedItem = fromLang;
            comboBox2.SelectedItem = toLang;

            label19.Text = folderPath;


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
        private void button3_Click(object sender, EventArgs e)
        {
            if (groupBox6.Visible) groupBox6.Visible=false;
            else groupBox6.Visible=true;
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            updateProject();

            this.Hide();
            var pr = new FileProject();
            pr.Closed += (s, args) => this.Close();
            pr.Show();
        }
        private void updateProject()
        {

            //            string user = Login.userID;
            string videoPath = "";
            DateTime today = DateTime.Now;
            if (type == "url")
            {
                videoPath = textBox4.Text;

            }
            else
            {
                videoPath = label13.Text;
            }
            string date = today.ToString("dd/MM/yyyy HH:mm:ss");
            string name = textBox5.Text;
            string fromLang = comboBox1.Text;
            string toLang = comboBox2.Text;
            
            string subPath = label15.Text;
            string folderPath = label19.Text;
            

            if (date != "" && name != "" && fromLang != "" && toLang != "" && subPath != "" && folderPath != "")
            {
                var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UPDATE [projects] SET [date]=@date, [name]=@name, [from_lang]=@from_lang, [to_lang]=@to_lang, [video_file]=@video_file, [sub_file]=@sub_file, [folder]=@folder WHERE id=@id", con))
                    {

                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@from_lang", fromLang);
                        cmd.Parameters.AddWithValue("@to_lang", toLang);
                        cmd.Parameters.AddWithValue("@video_file", videoPath);
                        cmd.Parameters.AddWithValue("@sub_file", subPath);
                        cmd.Parameters.AddWithValue("@folder", folderPath);
                        cmd.Parameters.AddWithValue("@id", project_id);


                        try
                        {
                            cmd.ExecuteScalar();

     

                        }
                        catch (Exception ex)
                        {
                            label20.Text = ex.Message;
                        }


                    }
                }
            }
            else
            {
                label11.Text = "Fill all required fields!";
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            string output = "";
            foreach (var item in list)
            {
                output += item.Item2 + " - " + item.Item1 + "\n";
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = folderPath;
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, output);
                }
            }
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

        private void button10_Click(object sender, EventArgs e)
        {
            label15.Text = getSubFile();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label19.Text = getFilePath();
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

        private void button9_Click(object sender, EventArgs e)
        {
            label13.Text = getVideoFile();
        }
    }
    }

