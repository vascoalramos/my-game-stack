using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;


namespace GamesDB
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        private int pageSize = 10;
        private int pageNumber = 1;
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
            if (pageNumber == 1)
            {
                button20.Enabled = false;
                button1.Enabled = false;
            }
            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    SqlCommand cmd;
                    pageNumber = 1;
                    textBox3.Text = "";
                    comboBox1.Text = "None";
                    comboBox2.Text = "None";
                    comboBox6.Text = "None";
                    comboBox7.Text = "None";
                    load_games(pageNumber);
                    if (!verifySGBDConnection())
                        return;
                    string id, name;

                    cmd = new SqlCommand("select * from GamesDB.Genres", cn);
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader["GenreID"].ToString();
                                name = reader["Name"].ToString();
                                comboBox2.Items.Add(id + " - " + name);
                            }
                        }
                    }
                    catch { }

                    if (!verifySGBDConnection())
                        return;
                    cmd = new SqlCommand("select * from GamesDB.Franchises", cn);
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader["FranchiseID"].ToString();
                                name = reader["Name"].ToString();
                                comboBox6.Items.Add(id + " - " + name);
                                Debug.WriteLine(id + " - " + name);
                            }
                        }
                    }
                    catch { }

                    if (!verifySGBDConnection())
                        return;
                    cmd = new SqlCommand("select * from GamesDB.Publishers", cn);
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader["PublisherID"].ToString();
                                name = reader["Name"].ToString();
                                comboBox7.Items.Add(id + " - " + name);
                            }
                        }
                    }
                    catch { }
                    cn.Close();
                    break;

                case 1:
                    pageNumber = 1;
                    textBox4.Text = "";
                    comboBox11.Text = "None";
                    load_franchises(pageNumber);
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 5:
                    textBox18.Text = current_user;
                    if (!verifySGBDConnection())
                        return;
                    cmd = new SqlCommand("select * from GamesDB.userInfo ('" + current_user + "')", cn);

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                textBox14.Text = reader["Fname"].ToString();
                                textBox15.Text = reader["Lname"].ToString();
                                textBox7.Text = reader["Email"].ToString();
                                byte[] bytes = System.Convert.FromBase64String(reader["Photo"].ToString());
                                var image = new MemoryStream(bytes);
                                Image imgStream = Image.FromStream(image);
                                pictureBox1.Image = imgStream;

                            }
                        }
                    }
                    catch { }

                    cn.Close();
                    break;

                case 6:
                    break;

                case 7:
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

        private void load_games(int paginacao)
        {
            int nGames = 0;
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.RowCount = 0;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspSearchGames"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel2.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    try
                    {
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
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

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

                    tableLayoutPanel2.Controls.Add(x, 0, tableLayoutPanel2.RowCount - 1);
                    nGames++;
                }
                if (nGames < pageSize)
                {
                    button19.Enabled = false;
                }
            }

            cn.Close();
        }

        private void pic_Click(object sender, EventArgs e)
        {
            myPicture temp = (myPicture)sender;
            MessageBox.Show("image click");
            ;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            pageNumber++;
            string option = comboBox1.Text;
            string title = textBox3.Text;
            string genreID = comboBox2.Text;
            string franID = comboBox6.Text;
            string pubID = comboBox7.Text;
            if (genreID != "None")
            {
                genreID = genreID.Split('-')[0].ToString();
            }
            if (franID != "None")
            {
                franID = franID.Split('-')[0].ToString();
            }
            if (pubID != "None")
            {
                pubID = pubID.Split('-')[0].ToString();
            }
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_games(pageNumber, option, title, genreID, franID, pubID);

            if (pageNumber > 1)
            {
                button20.Enabled = true;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            pageNumber--;
            string option = comboBox1.Text;
            string title = textBox3.Text;
            string genreID = comboBox2.Text;
            string franID = comboBox6.Text;
            string pubID = comboBox7.Text;
            if (genreID != "None")
            {
                genreID = genreID.Split('-')[0].ToString();
            }
            if (franID != "None")
            {
                franID = franID.Split('-')[0].ToString();
            }
            if (pubID != "None")
            {
                pubID = pubID.Split('-')[0].ToString();
            }
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_games(pageNumber, option, title, genreID, franID, pubID);

            if (pageNumber == 1)
            {
                button20.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            this.button20.Enabled = false;
            string option = comboBox1.Text;
            string title = textBox3.Text;
            string genreID = comboBox2.Text;
            string franID = comboBox6.Text;
            string pubID = comboBox7.Text;
            if (genreID != "None")
            {
                genreID = genreID.Split('-')[0].ToString();
            }
            if (franID != "None")
            {
                franID = franID.Split('-')[0].ToString();
            }
            if (pubID != "None")
            {
                pubID = pubID.Split('-')[0].ToString();
            }
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_games(pageNumber, option, title, genreID, franID, pubID);
        }

        private void filter_games(int paginacao, string opt, string title, string genreID, string franID, string pubID)
        {
            int nGames = 0;

            if (paginacao == 1)
            {
                button19.Enabled = true;
            }
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.RowCount = 0;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspFilterGames"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@opt", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@genreID", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@franID", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@pubID", SqlDbType.VarChar));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;
            cmd.Parameters["@opt"].Value = opt;
            cmd.Parameters["@name"].Value = title;
            cmd.Parameters["@genreID"].Value = genreID;
            cmd.Parameters["@franID"].Value = franID;
            cmd.Parameters["@pubID"].Value = pubID;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel2.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    try
                    {
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
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

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
                    lb12.Text = reader["LaunchDate"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb10);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel2.Controls.Add(x, 0, tableLayoutPanel2.RowCount - 1);
                    nGames++;
                }
            }
            if (nGames < pageSize)
            {
                button19.Enabled = false;
            }

            cn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            button1.Enabled = false;
            string option = comboBox11.Text;
            string title = textBox4.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_franchises(pageNumber, option, title);
        }

        private void load_franchises(int paginacao)
        {
            int nFranchises = 0;

            if (paginacao == 1)
            {
                button2.Enabled = true;
            }
            tableLayoutPanel5.Controls.Clear();
            tableLayoutPanel5.RowStyles.Clear();
            tableLayoutPanel5.ColumnStyles.Clear();
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.RowCount = 0;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspSearchFranchises"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel5.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    try
                    {
                        byte[] bytes = System.Convert.FromBase64String(reader["Logo"].ToString());
                        var image = new MemoryStream(bytes);
                        Image imgStream = Image.FromStream(image);
                        pic.Image = imgStream;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(100, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(200, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel5.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel5.RowCount;


                    lb9.Text = reader["FranchiseID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Number of Games:";
                    lb12.Text = reader["NoOfGames"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel5.Controls.Add(x, 0, tableLayoutPanel5.RowCount - 1);
                    nFranchises++;
                }
            }
            if (nFranchises < pageSize)
            {
                button2.Enabled = false;
            }

            cn.Close();
        }

        private void filter_franchises(int paginacao, string opt, string title)
        {
            int nFranchises = 0;

            if (paginacao == 1)
            {
                button2.Enabled = true;
            }
            tableLayoutPanel5.Controls.Clear();
            tableLayoutPanel5.RowStyles.Clear();
            tableLayoutPanel5.ColumnStyles.Clear();
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.RowCount = 0;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspfilterFranchises"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@opt", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;
            cmd.Parameters["@opt"].Value = opt;
            cmd.Parameters["@name"].Value = title;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel5.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    try
                    {
                        byte[] bytes = System.Convert.FromBase64String(reader["Logo"].ToString());
                        var image = new MemoryStream(bytes);
                        Image imgStream = Image.FromStream(image);
                        pic.Image = imgStream;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(100, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(200, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel5.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel5.RowCount;


                    lb9.Text = reader["FranchiseID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Number of Games:";
                    lb12.Text = reader["NoOfGames"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel5.Controls.Add(x, 0, tableLayoutPanel5.RowCount - 1);
                    nFranchises++;
                }
            }
            if (nFranchises < pageSize)
            {
                button2.Enabled = false;
            }

            cn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pageNumber++;
            string option = comboBox11.Text;
            string title = textBox4.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_franchises(pageNumber, option, title);

            if (pageNumber > 1)
            {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pageNumber--;
            string option = comboBox11.Text;
            string title = textBox4.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_franchises(pageNumber, option, title);

            if (pageNumber == 1)
            {
                button1.Enabled = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox6.Text = fileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string first_name = textBox14.Text;
            string last_name = textBox15.Text;
            string email = textBox7.Text;
            string picturePath = textBox6.Text;
            string picture = "";
            string userName = textBox18.Text;
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
                password = "None";
            }
            if (string.IsNullOrEmpty(first_name))
            {
                first_name = "None";
            }
            if (string.IsNullOrEmpty(last_name))
            {
                last_name = "None";
            }
            if (string.IsNullOrEmpty(email))
            {
                email = "None";
            }
            if (picture == "")
            {
                picture = "None";
            }

            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspUpdateUser"
            };
            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@fname", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@lname", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
            cmd.Parameters["@password"].Value = password;
            cmd.Parameters["@UserName"].Value = userName;
            cmd.Parameters["@fname"].Value = first_name;
            cmd.Parameters["@lname"].Value = last_name;
            cmd.Parameters["@photo"].Value = picture;
            cmd.Parameters["@mail"].Value = email;
            cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;
            cmd.ExecuteNonQuery();  

            if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }

            cn.Close();

        }
    }
}
