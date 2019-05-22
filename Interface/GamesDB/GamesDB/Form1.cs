using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace GamesDB
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        private int pageSize = 10;
        String current_user = "";

        public Form1()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=mednat.ieeta.pt\\SQLSERVER,8101;Initial Catalog=p5g4;user id=p5g4; password=urMomGay69");
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
                CommandText = "GamesDB.uspLogin"
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

                SqlCommand comand = new SqlCommand("select GamesDB.checkAdmin ('" + user_name + "')", cn);
                int valor = (int)comand.ExecuteScalar();
                if (valor == 1)
                {
                    admin admin = new admin();
                    admin.ShowDialog();
                }
                else
                {
                    panel2.Visible = true;
                    this.tabControl1.SelectTab(6);
                }
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
            if (!(string.IsNullOrEmpty(picturePath)))
            {
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
            }
            string password = textBox_pass.Text;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("UserName has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(mail))
            {
                MessageBox.Show("Email has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!(IsEmailValid(mail)))
            {
                MessageBox.Show("Email inserted is invalid!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddUser"
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

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    current_user = username;
                    MessageBox.Show("Sign Up succedeed!\nWelcome " + username + "!\nYou are now loged in");
                    panel_signUp.Visible = false;
                    panel2.Visible = true;
                }
                else
                {
                    MessageBox.Show("User Name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
                cn.Close();
            }
        }

        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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
            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    load_game(1);
                    break;
                case 5:
                    textBox18.Text = current_user;
                    if (!verifySGBDConnection())
                        return;
                    SqlCommand cmd = new SqlCommand("select * from GamesDB.userInfo ('" + current_user + "')", cn);

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
                    catch
                    {
                    }

                    cn.Close();
                    break;
            }
        }

        private void button_register_exit_Click(object sender, EventArgs e)
        {
            this.panel_signUp.Visible = false;
            this.panel1.Visible = true;
        }

        class myPicture : PictureBox
        {
            private int property, total_people;
            private Image property_Image;
            private bool bed;

            public Image Property_Image { get => property_Image; set => property_Image = value; }
            public int Property { get => property; set => property = value; }
            public int Total_people { get => total_people; set => total_people = value; }
            public bool Bed { get => bed; set => bed = value; }
        }

        private void load_game(int paginacao)
        {
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.RowCount = 0 ;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand("exec GamesDB.uspSearchGames " + pageSize + ", " + paginacao, cn);
        
            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {             
                    tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel2.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    byte[] bytes = System.Convert.FromBase64String(reader["CoverImage"].ToString());
                    var image = new MemoryStream(bytes);
                    Image imgStream = Image.FromStream(image);
                    pic.Image = imgStream;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Location = new System.Drawing.Point(20, 28);
                    pic.Size = new System.Drawing.Size(180, 150);
                    pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                    pic.TabIndex = 3;
                    pic.TabStop = false;

                    Label lb9 = new Label();
                    Label lb10 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(305, 140);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(100, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(220, 140);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb10.AutoSize = true;
                    lb10.Location = new System.Drawing.Point(220, 53);
                    lb10.TabIndex = 2;
                    lb10.MaximumSize = new System.Drawing.Size(425, 80);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(220, 28);
                    lb9.MaximumSize = new System.Drawing.Size(200, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel2.RowCount;
                    lb10.Name = "label10_" + tableLayoutPanel2.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel2.RowCount;
                    lb12.Name = "label12_" + tableLayoutPanel2.RowCount;

                    lb9.Text = reader["GameID"].ToString() + " - " + reader["Title"].ToString();
                    lb10.Text = reader["Description"].ToString();
                    lb11.Font = new Font(lb11.Font, FontStyle.Bold);
                    lb11.Text = "Launch Date:";
                    lb12.Text = reader["LauchDate"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb10);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel2.Controls.Add(x, 0, tableLayoutPanel2.RowCount-1);
                    string ola = reader["GameID"].ToString();
                }
            }

            cn.Close();
        }

        private void pic_Click(object sender, EventArgs e)
        {
            myPicture temp = (myPicture)sender;
            MessageBox.Show("image click");
;        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            load_game(10);
        }
    }
}
