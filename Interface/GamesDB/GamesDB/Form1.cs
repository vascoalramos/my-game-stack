using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace GamesDB
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        String current_user = "";

        public Form1()
        {
            InitializeComponent();
            this.Text = "GamesDB";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("HERE fired!!!!");
            cn = getSGBDConnection();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=DESKTOP-LB88B3L\\SQLEXPRESS;integrated security=true;initial catalog=Games");
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            string user_name = textBox1.Text;
            string password = textBox2.Text;
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.uspLogin"
            };
            cmd.Parameters.Add(new SqlParameter("@loginName", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@responseMessage", SqlDbType.NVarChar, 250));
            cmd.Parameters["@loginName"].Value = user_name;
            cmd.Parameters["@password"].Value = password;
            cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;
            cmd.ExecuteNonQuery();

            if ("" + cmd.Parameters["@responseMessage"].Value == "User successfully logged in")
            {
                MessageBox.Show("Login sucess!");
                current_user = user_name;
            }
            else if ("" + cmd.Parameters["@responseMessage"].Value == "Incorrect password")
            {
                MessageBox.Show("Wrong password! Please try again");
            }
            else
            {
                MessageBox.Show("Invalid login!");
            }

            cn.Close();
            panel2.Visible = true;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_signUp_Click(object sender, EventArgs e)
        {
            panel_signUp.Visible = true;
        }

        private void button_regist_user_Click(object sender, EventArgs e)
        {
            string mail = textBox_email.Text;
            string first_name = textBox_fName.Text;
            string last_name = textBox_lName.Text;
            string picturePath = textBox_photo.Text;
            string picture = "";
            string username = textBox_userName.Text;
            using (Image image = Image.FromFile(picturePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    picture = Convert.ToBase64String(imageBytes);
                }
            }
            string password = textBox_pass.Text;

            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.uspAddUser"
            };
            cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@fname", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@lname", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
            cmd.Parameters["@mail"].Value = mail;
            cmd.Parameters["@password"].Value = password;
            cmd.Parameters["@fname"].Value = first_name;
            cmd.Parameters["@lname"].Value = last_name;
            cmd.Parameters["@photo"].Value = picture;
            cmd.Parameters["@UserName"].Value = username;
            cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;
            cmd.ExecuteNonQuery();

            current_user = mail;
            panel_signUp.Visible = false;
            panel1.Visible = false;
            cn.Close();

            current_user = username;
            MessageBox.Show("Sign Up succedeed! Welcome" + username + "! You are now loged in");
        }

        private void button_loadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox_photo.Text = fileName;

            }
        }

        public void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            Debug.WriteLine("HERE fired");
            textBox18.Text = current_user;
            MessageBox.Show(current_user);
            if (!verifySGBDConnection())
                return;
            string selectSql = "select * from [Users] where UserName = '" + current_user + "'";
            SqlCommand cmd = new SqlCommand(selectSql, cn);

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBox14.Text = reader["Fname"].ToString();
                        textBox15.Text = reader["Lname"].ToString();
                        byte[] bytes = System.Convert.FromBase64String(reader["Photo"].ToString());
                        var image = new MemoryStream(bytes);
                        Image imgStream = Image.FromStream(image);
                        pictureBox1.Image = imgStream;

                    }
                }
            }
            catch {
                Debug.WriteLine("constructor fired");
            }

            //button11.PerformClick();
            //TableLayoutPanel tableLayoutPanel5 = new TableLayoutPanel();
            //tableLayoutPanel5.AutoScroll = true;
            //tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            //tableLayoutPanel5.Location = new System.Drawing.Point(23, 70);
            //tableLayoutPanel5.MaximumSize = new System.Drawing.Size(600, 1000);
            //tableLayoutPanel5.Name = "tableLayoutPanel5";
            //tableLayoutPanel5.Size = new System.Drawing.Size(600, 304);
            //tableLayoutPanel5.TabIndex = 2;
            //tableLayoutPanel5.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tableLayoutPanel5_CellPaint);
            //tableLayoutPanel5.Controls.Clear();
            //tableLayoutPanel5.RowStyles.Clear();
            //tableLayoutPanel5.ColumnStyles.Clear();
            //tableLayoutPanel5.ColumnCount = 0;
            //tableLayoutPanel5.RowCount = 0;
            //selectSql = "select [ID], [Advertisement_title], [Start_date], [End_date], Comments from ([RENTALS] JOIN PROPERTY ON Property_ID = PROPERTY.ID) LEFT OUTER JOIN REVIEW ON REVIEW.Person_ID = RENTALS.Person_ID AND REVIEW.Rental_Start_date = RENTALS.Start_date WHERE RENTALS.Person_ID = '" + current_id + "'";
            //cmd = new SqlCommand(selectSql, con);
            //try
            //{

            //    using (SqlDataReader read = cmd.ExecuteReader())
            //    {
            //        while (read.Read())
            //        {
            //            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            //            Panel x = new Panel();
            //            Label lb9 = new Label();
            //            Label lb10 = new Label();
            //            Label lb11 = new Label();
            //            MyButton btn1 = new MyButton();
            //            btn1.AutoSize = true;
            //            btn1.Location = new System.Drawing.Point(320, 33);
            //            btn1.TabIndex = 3;
            //            btn1.Text = "WRITE REVIEW";
            //            btn1.PersonID = current_id;
            //            btn1.PropertyID = Int32.Parse(read["ID"].ToString());
            //            btn1.StartDate = read["Start_date"].ToString().Split(' ')[0];
            //            btn1.Click += new EventHandler(writeClick);
            //            lb11.AutoSize = true;
            //            lb11.Location = new System.Drawing.Point(100, 53);
            //            lb11.TabIndex = 3;
            //            lb11.MaximumSize = new System.Drawing.Size(100, 20);
            //            lb10.AutoSize = true;
            //            lb10.Location = new System.Drawing.Point(150, 53);
            //            lb10.TabIndex = 2;
            //            lb10.MaximumSize = new System.Drawing.Size(200, 85);
            //            lb9.AutoSize = true;
            //            lb9.Location = new System.Drawing.Point(100, 28);
            //            lb9.MaximumSize = new System.Drawing.Size(200, 15);
            //            lb9.TabIndex = 1;
            //            lb9.Text = read["Advertisement_title"].ToString();
            //            lb10.Text = read["Start_date"].ToString().Split(' ')[0];
            //            lb11.Text = read["End_date"].ToString().Split(' ')[0];
            //            if (!(read["Comments"].ToString() == ""))
            //            {
            //                btn1.Enabled = false;
            //            }
            //            x.Location = new System.Drawing.Point(45, 65);
            //            x.Size = new System.Drawing.Size(576, 110);
            //            x.TabIndex = 3;
            //            x.Controls.Add(lb9);
            //            x.Controls.Add(lb10);
            //            x.Controls.Add(lb11);
            //            x.Controls.Add(btn1);
            //            tableLayoutPanel5.Controls.Add(x, 0, tableLayoutPanel5.RowCount - 1);

            //        }
            //    }
            //}
            //finally
            //{
            //    this.tabPage4.Controls.Clear();
            //    this.tabPage4.Controls.Add(tableLayoutPanel5);

            //}
            cn.Close();


        }


        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
