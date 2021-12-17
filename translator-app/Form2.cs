using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace translator_app
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\translator project\translators.mdf;Integrated Security=True;Connect Timeout=30");
            string query = "Select * from dbo.users Where username = @username and password = @password";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
            cmd.Parameters.AddWithValue("@password", textBoxPassword.Text);
            cmd.Connection = sqlcon;

            sqlcon.Open();
            cmd.ExecuteScalar();

            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            sqlcon.Close();

            bool loginSuccessful = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));

            if (loginSuccessful)
            {
                new Form1().Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Invalid username or password");
                textBoxPassword.Clear();
                textBoxUsername.Clear();
                textBoxUsername.Focus();
            }
        }

    }
}
