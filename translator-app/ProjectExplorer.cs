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
    public partial class ProjectExplorer : Form
    {
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        private Dictionary<string,string> getProjects()
        {
            var list = new Dictionary<string,string>();
            if(userID != "")
            {
                using (var conn = new SqlConnection())
                {
                    var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    conn.ConnectionString = connString;
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM projects WHERE user_id=@userID", conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(Convert.ToString(reader["id"]).Trim(), Convert.ToString(reader["name"]).Trim());
                              
                            }



                        }
                    }
                }
            
            }
            return list;



        }

        private void createProjectButtons()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var project in getProjects())
            {
               
                Button b = new Button();
                b.Size = new Size(200, 50);
                b.Text = project.Value;
                b.ForeColor = Color.White;
                b.Parent = pictureBox1;
                b.BackColor = Color.MidnightBlue;
                
                b.Click += (sender, EventArgs) => { goToProject(sender, EventArgs, project.Key); };
                flowLayoutPanel1.Controls.Add(b);
            }
        }
        public static string project_id = "";
        private void goToProject(object sender, EventArgs e, string id)
        {
            project_id = id;
            
            string project_type = "";
            if (project_id != "")
            {
                using (var conn = new SqlConnection())
                {
                    var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    conn.ConnectionString = connString;
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM projects WHERE id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                project_type = reader["type"].ToString().Trim();

                            }



                        }
                    }
                }

            }

            if(project_type != "")
            {
                FileProject.project_id = id;
                this.Hide();
                var pr = new FileProject();
                pr.Closed += (s, args) => this.Close();
                pr.Show();
            }
           

        }
        private void ProjectExplorer_Load(object sender, EventArgs e)
        {
            
            transparentButton(button1);
            transparentButton(button4);
            transparentLabel(label2);
            transparentLabel(label3);

            groupBox1.Parent = pictureBox1;
            groupBox2.Parent = pictureBox1;

            getUserData();
            label3.Text = name + " " + surname;

            //label1.Text = a.Count.ToString();
            createProjectButtons();

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        string name = "";
        string surname = "";

        string userID = Login.userID;
        
        private void getUserData()
        {
           
            if(userID != "")
            {
            
                using (var conn = new SqlConnection())
                {
                    var connString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    conn.ConnectionString = connString;
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM users Where id=" + userID, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                name = reader["name"].ToString().Trim();
                                surname = reader["surname"].ToString().Trim();


                            }
                        }
                    }
                }
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pr = new FileProjectCreate();
            pr.Closed += (s, args) => this.Close();
            pr.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pr = new UrlProjectCreate();
            pr.Closed += (s, args) => this.Close();
            pr.Show();
        }
    }
}
