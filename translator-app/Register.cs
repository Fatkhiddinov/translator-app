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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            transparentLabel(label1);
            transparentLabel(label2);
            transparentLabel(label3);
            transparentLabel(label4);
            transparentLabel(label5);
            transparentLabel(label6);
            transparentLabel(label7);
            transparentLabel(label8);
            transparentLabel(label9);
            transparentLabel(label10);

            transparentButton(button2);
            transparentButton(button4);
            transparentButton(button3);
            transparentRadioButton(radioButton1);
            transparentRadioButton(radioButton2);






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
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pr = new Login();
            pr.Closed += (s, args) => this.Close();
            pr.Show();
        }
        string gender = "Male";
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text;
            string surname = textBox1.Text;
            string day = comboBox1.Text;
            string month = comboBox2.Text;
            string year = comboBox3.Text;
            string date = day + " " + month + " " + year;
            string username = textBox4.Text;
            string email = textBox3.Text;
            string pass1 = textBox5.Text;
            string pass2 = textBox6.Text;

            if(name!="" && surname!="" && day != "" && month != "" && year != "" && username != "" && email != "" && pass1 != "" && pass2 != "")
            {
                if(pass1 == pass2)
                {

                    var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [users] ([username], [password], [name], [surname], [bdate], [email], [gender]) VALUES ( @username, @password, @name, @surname, @bdate, @email, @gender); SELECT SCOPE_IDENTITY();", con))
                        {

                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", pass1);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@bdate", date);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@gender", gender);

                            try
                            {

                                int modified = Convert.ToInt32(cmd.ExecuteScalar());

                                if (con.State == System.Data.ConnectionState.Open) con.Close();
                                Login.userID = modified.ToString();

                                this.Hide();
                                var pr = new ProjectExplorer();
                                pr.Closed += (s, args) => this.Close();
                                pr.Show();

                            }
                            catch (Exception ex)
                            {
                                label9.Text = ex.Message;
                            }
                        }
                    }
                }
                else
                {
                    label9.Text = "Passwords are not the same!";
                }
            }
            else
            {
                label9.Text = "Fill all required fields!";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
        }
    }
}
