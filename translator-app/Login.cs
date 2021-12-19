using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace translator_app
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = System.Drawing.Color.Transparent;

            label2.Parent = pictureBox1;
            label2.BackColor = System.Drawing.Color.Transparent;

            label3.Parent = pictureBox1;
            label3.BackColor = System.Drawing.Color.Transparent;

            label4.Parent = pictureBox1;
            label4.BackColor = System.Drawing.Color.Transparent;

            label5.Parent = pictureBox1;
            label5.BackColor = System.Drawing.Color.Transparent;

            button2.Parent = pictureBox1;
            button2.BackColor = System.Drawing.Color.Transparent;

            button3.Parent = pictureBox1;
            button3.BackColor = System.Drawing.Color.Transparent;

            button4.Parent = pictureBox1;
            button4.BackColor = System.Drawing.Color.Transparent;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public static string userID = "";
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text;
            string password = textBox3.Text;
            if(username != "" && password != "")
            {


                using (var conn = new SqlConnection())
                {
                    var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    conn.ConnectionString = connString;
                    conn.Open();
                    using (var cmd = new SqlCommand("Select * from [users] Where username = @username and password = @password", conn))
                    {

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    userID = reader["Id"].ToString().Trim();
                                }
                                catch (Exception ex)
                                {
                                    label5.Text = ex.Message;
                                }
                            }
                            if (userID != "")
                            {
                                this.Hide();
                                var pr = new ProjectExplorer();
                                pr.Closed += (s, args) => this.Close();
                                pr.Show();


                            }
                            else
                            {
                                label5.Text = "Wrong username or password!";
                                textBox2.Text = "";
                                textBox3.Text = "";

                            }

                        }
                    }
                }
               
            }
            else
            {
                label5.Text = "Enter Username and Password!";
            }

            if (userID != "")
            {
                textBox2.Text = userID;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pr = new Register();
            pr.Closed += (s, args) => this.Close();
            pr.Show();
        }
    }
    }

