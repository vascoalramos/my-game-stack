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
                button4.Enabled = false;
                button11.Enabled = false;
                button15.Enabled = false;
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
                    pageNumber = 1;
                    textBox5.Text = "";
                    load_publishers(pageNumber);
                    break;

                case 3:
                    pageNumber = 1;
                    textBox5.Text = "";
                    load_developers(pageNumber);
                    break;

                case 4:
                    pageNumber = 1;
                    textBox9.Text = "";
                    load_tournments(pageNumber);
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
            private int id;
            private Image image;

            public Image MyImage { get => image; set => image = value; }
            public int ID { get => id; set => id = value; }
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
                    pic.ID = (int)reader["GameID"];

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
            int id = (int)temp.GetType().GetProperty("ID").GetValue(obj: temp, index: null);
            Image img = (Image)temp.GetType().GetProperty("Image").GetValue(obj: temp, index: null);
            Form2 new_form = new Form2(id, img);
            new_form.ShowDialog();
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel2.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
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
            string password = textBox19.Text;

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

        private void load_publishers(int paginacao)
        {
            int nPublishers = 0;

            if (paginacao == 1)
            {
                button5.Enabled = true;
            }
            tableLayoutPanel6.Controls.Clear();
            tableLayoutPanel6.RowStyles.Clear();
            tableLayoutPanel6.ColumnStyles.Clear();
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.RowCount = 0;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspSearchPublishers"
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
                    tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel6.RowCount++;
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel6.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel6.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(400, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel6.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel6.RowCount;


                    lb9.Text = reader["PublisherID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Headquarters:";
                    lb12.Text = reader["City"].ToString() + ", " + reader["Country"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel6.Controls.Add(x, 0, tableLayoutPanel6.RowCount - 1);
                    nPublishers++;
                }
            }
            if (nPublishers < pageSize)
            {
                button5.Enabled = false;
            }

            cn.Close();
        }

        private void filter_publishers(int paginacao, string title)
        {
            int nPublishers = 0;

            if (paginacao == 1)
            {
                button5.Enabled = true;
            }
            tableLayoutPanel6.Controls.Clear();
            tableLayoutPanel6.RowStyles.Clear();
            tableLayoutPanel6.ColumnStyles.Clear();
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.RowCount = 0;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspFilterPublishers"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;
            cmd.Parameters["@name"].Value = title;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel6.RowCount++;
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel6.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel6.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(400, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel6.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel6.RowCount;


                    lb9.Text = reader["PublisherID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Headquarters:";
                    lb12.Text = reader["City"].ToString() + ", " + reader["Country"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel6.Controls.Add(x, 0, tableLayoutPanel6.RowCount - 1);
                    nPublishers++;
                }
            }
            if (nPublishers < pageSize)
            {
                button5.Enabled = false;
            }

            cn.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            button4.Enabled = false;
            string title = textBox5.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_publishers(pageNumber, title);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pageNumber++;
            string title = textBox5.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_publishers(pageNumber, title);

            if (pageNumber > 1)
            {
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pageNumber--;
            string title = textBox5.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_publishers(pageNumber, title);

            if (pageNumber == 1)
            {
                button4.Enabled = false;
            }
        }

        private void load_developers(int paginacao)
        {
            int nDevelopers = 0;

            if (paginacao == 1)
            {
                button12.Enabled = true;
            }
            tableLayoutPanel7.Controls.Clear();
            tableLayoutPanel7.RowStyles.Clear();
            tableLayoutPanel7.ColumnStyles.Clear();
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.RowCount = 0;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspSearchDevelopers"
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
                    tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel7.RowCount++;
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel7.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel7.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(400, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel7.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel7.RowCount;


                    lb9.Text = reader["DeveloperID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Headquarters:";
                    lb12.Text = reader["City"].ToString() + ", " + reader["Country"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel7.Controls.Add(x, 0, tableLayoutPanel7.RowCount - 1);
                    nDevelopers++;
                }
            }
            if (nDevelopers < pageSize)
            {
                button12.Enabled = false;
            }

            cn.Close();
        }

        private void filter_developers(int paginacao, string title)
        {
            int nDevelopers = 0;

            if (paginacao == 1)
            {
                button12.Enabled = true;
            }
            tableLayoutPanel7.Controls.Clear();
            tableLayoutPanel7.RowStyles.Clear();
            tableLayoutPanel7.ColumnStyles.Clear();
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.RowCount = 0;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspFilterDevelopers"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;
            cmd.Parameters["@name"].Value = title;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel7.RowCount++;
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
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel7.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }
                    catch
                    {
                        pic.Image = null;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(300, 150);
                        pic.Name = "pic_" + tableLayoutPanel7.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;
                    }

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(510, 100);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(400, 100);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(100, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(400, 75);
                    lb9.MaximumSize = new System.Drawing.Size(400, 15);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel7.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel7.RowCount;


                    lb9.Text = reader["DeveloperID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Headquarters:";
                    lb12.Text = reader["City"].ToString() + ", " + reader["Country"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);

                    tableLayoutPanel7.Controls.Add(x, 0, tableLayoutPanel7.RowCount - 1);
                    nDevelopers++;
                }
            }
            if (nDevelopers < pageSize)
            {
                button12.Enabled = false;
            }

            cn.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pageNumber++;
            string title = textBox8.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_developers(pageNumber, title);

            if (pageNumber > 1)
            {
                button4.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pageNumber--;
            string title = textBox8.Text;

            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_developers(pageNumber, title);

            if (pageNumber == 1)
            {
                button4.Enabled = false;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            button11.Enabled = false;
            string title = textBox8.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            filter_developers(pageNumber, title);
        }

        private void load_tournments(int paginacao)
        {
            int nTournments = 0;

            if (paginacao == 1)
            {
                button16.Enabled = true;
            }
            tableLayoutPanel8.Controls.Clear();
            tableLayoutPanel8.RowStyles.Clear();
            tableLayoutPanel8.ColumnStyles.Clear();
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.RowCount = 0;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspSearchTournments"
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
                    tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel8.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                        byte[] bytes = System.Convert.FromBase64String("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");
                        var image = new MemoryStream(bytes);
                        Image imgStream = Image.FromStream(image);
                        pic.Image = imgStream;
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Location = new System.Drawing.Point(20, 28);
                        pic.Size = new System.Drawing.Size(180, 150);
                        pic.Name = "pic_" + tableLayoutPanel8.RowCount;
                        pic.TabIndex = 3;
                        pic.TabStop = false;

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();
                    Label lb13 = new Label();
                    Label lb14 = new Label();
                    Label lb15 = new Label();
                    Label lb16 = new Label();

                    lb16.AutoSize = true;
                    lb16.Location = new System.Drawing.Point(265, 120);
                    lb16.TabIndex = 3;
                    lb16.MaximumSize = new System.Drawing.Size(300, 20);

                    lb15.AutoSize = true;
                    lb15.Location = new System.Drawing.Point(205, 120);
                    lb15.TabIndex = 3;
                    lb15.MaximumSize = new System.Drawing.Size(265, 20);

                    lb14.AutoSize = true;
                    lb14.Location = new System.Drawing.Point(265, 100);
                    lb14.TabIndex = 3;
                    lb14.MaximumSize = new System.Drawing.Size(300, 20);

                    lb13.AutoSize = true;
                    lb13.Location = new System.Drawing.Point(205, 100);
                    lb13.TabIndex = 3;
                    lb13.MaximumSize = new System.Drawing.Size(60, 20);

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(255, 80);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(205, 80);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(50, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(205, 60);
                    lb9.MaximumSize = new System.Drawing.Size(500, 20);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel8.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel8.RowCount;
                    lb12.Name = "label12_" + tableLayoutPanel8.RowCount;
                    lb13.Name = "label13_" + tableLayoutPanel8.RowCount;
                    lb14.Name = "label14_" + tableLayoutPanel8.RowCount;
                    lb15.Name = "label14_" + tableLayoutPanel8.RowCount;
                    lb16.Name = "label16_" + tableLayoutPanel8.RowCount;


                    lb9.Text = reader["TournmentID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Game:";
                    lb12.Text = reader["Title"].ToString();
                    lb13.Text = "Prize Pool:";
                    lb14.Text = reader["PrizePool"].ToString();
                    lb15.Text = "Location:";
                    lb16.Text = reader["Location"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);
                    x.Controls.Add(lb13);
                    x.Controls.Add(lb14);
                    x.Controls.Add(lb15);
                    x.Controls.Add(lb16);

                    tableLayoutPanel8.Controls.Add(x, 0, tableLayoutPanel8.RowCount - 1);
                    nTournments++;
                }
            }
            if (nTournments < pageSize)
            {
                button16.Enabled = false;
            }

            cn.Close();
        }

        private void filter_tournments(int paginacao, string opt, string name, string title)
        {
            int nTournments = 0;

            if (paginacao == 1)
            {
                button16.Enabled = true;
            }
            tableLayoutPanel8.Controls.Clear();
            tableLayoutPanel8.RowStyles.Clear();
            tableLayoutPanel8.ColumnStyles.Clear();
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.RowCount = 0;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GamesDB.uspFilterTournments"
            };
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@opt", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar));
            cmd.Parameters["@pageSize"].Value = pageSize;
            cmd.Parameters["@pageNumber"].Value = paginacao;
            cmd.Parameters["@opt"].Value = opt;
            cmd.Parameters["@name"].Value = name;
            cmd.Parameters["@title"].Value = title;

            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 206));
                    Panel x = new Panel();
                    myPicture pic = new myPicture();
                    pic.Click += new EventHandler(pic_Click);

                    tableLayoutPanel8.RowCount++;
                    x.Location = new System.Drawing.Point(24, 111);
                    x.Size = new System.Drawing.Size(1010, 204);
                    x.TabIndex = 3;
                    byte[] bytes = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AABjkklEQVR42u2deZgkRZn/a6aru2tqyhouAaGBZqam4n0jso7u6u45YKSA9kJ0FV10PdcL72tFvFAQvNBdUYSfi8e6iCCK4oA9KyIqyCGIwLqDCp4olxwichbMTPv7oyOHpKc7M6sqMzKy6lvPU48+D9WfyXgz4v1+MzLijUwGH3zwwQcffPDBp93P9HRzyfR0c6nnuwQ88MADDzzwwEsXr91/fGD+FzzwwAMPPPDASxevXdeRnZ5uDnq+2U7dB3jggQceeOCBZ57XyT8+OD3dHPJ8B7tsDHjggQceeOCBZ5DXyT8+PD3dzHm+w102BjzwwAMPPPDAM8jr5B/PTU83l3m+uS4bAx544IEHHnjgGeS5zLA/XDo93cxPTzeXe7756enm0g7/YfDAAw888MADzzxviV40uDTsP758erpZ8HyXd9kY8MADDzzwwAPPLM9dQBhsADz/eNHzLXTZmAJ44IEHHnjggWeUt8Sza8DfAOgf5z0XsEL/bzeNcTkrwAMPPPDAAw88Izx3AeGQxwAs8ftxzjP1UESwwQMPPPDAAy+VPHfXwHYDEOQUls1794BggwceeOCBB166eHnProGh6elmNugdQc5jAJYj2OCBBx544IGXOp6r4a4BGPSb+s9qh+AagDyCDR544IEHHnip43l3DSzzLRqkFwUMegxADsEGDzzwwAMPvFTyih4DkAta9Oc1AN2UK8TNC8kTQr2QiG8mou8R8aeJ6HVCOE+dmBgfRfzAAw+8XuER0a7lsjqQiF6nc933iPhmIdQLEb/YeK4ByPvquf6jAc8eQYi/AR4RvYuI/+H9Sind791SqiuUUl8mkm9n5qeVStURxA888MCzlVerTZCU8hlC8NuJ6D+J+FIivmt+nnv8S/+G+MXGK4Zaw+cxAFmIvzkeM39mEfHf/l1g0NxPRD8j4jOI6L1CyH8ionImkxnA/QAPPPAM8AakZFKq8mKl1PFKqa9LKa+VUj6wuNAv/BWCT8b9iI0XbveexwBA/A3yiPjbbYq/3/dRIr5JCN5ERJ+TsnJMtVo5slarTm7YsP7JuB/ggQdeWJ5SaqhcLhMzP1s/zZ8yl1v4JiJ+NIJ8pb/yW7gfCfO6PFEIwe6Qp5/k/xHdYFrUTGwj4j8RyR8R8ReY+T1E9AKlVF0I8STcD/DA6z+eEOJJczlAvXAuJ/AXdI74ExFvayO/dJyrmPlq3A97eAiOQR4R325A/EP8Hd1JxFcy85lEdJwQ8mVEtL5arey3du3kzri/4IGXPt4+++w9ODHR4Gq18nSlKkcppT6ulDqHiK/0fy8fdX7x5d2O+wvx7zve2rVrhqWU25IX/0DeFinVLVI6V0mpzhWCT2LmNxHR4UKIar1e3wn3FzzwzPPq9fpOQogqER3OzG8Sgj9BxGcTycuF4D9LKbemIL9sazY37Ib7C/HvK97YWM1JweAMy/s7Ed/AzP8zt/JXvl8I+TJmfioRjR58cHMI/QU88MLzms0NuzDzKmZ+6tyMnHw/Ef3n3BjjG/SYszUftMUbHx9z0F8g/n3Fq9Vqz+oR8Q/D2yalvEtKdYOUzsVSqrP0TMI7mPlFRHSQEEKUSqUi+gt4vcxbuXLlCiGEIKKDmPlFUvI7pVSfVkqdrcfGDVLyXX7v4HsgHzzh6zi1p6O/QPz7iuc4zmv7RPzb5T1ExH/Q7ym/LQSfxswfJKLXMfNzKxXnoHp9Uq5dO7kH+h94NvCIRL5Uqo4IISaI6HDdVz8oBJ+md/pcqfv0Q30wftvmMfNL0P8g/n3FcxznQxD/rnkPC8F/FoKvF4IvZuZvMPP/Y+YT9ezCy5n5MCnlGqVUqVKp7JzJZJag/4G3yGdJpVLZWSlVklKuYebDdB96BzN/RCn1JaXUeczqEmb1f8zy1oVEHeO3PZ4Q/G70P/PiH3r3H4IdPU8p53SIfyK8rUR8t5TyN1Kqq6R0vqeUc5ZS6v8JwScKwe8mkq8nopfop7mDpJRjSqnSqlXV3UdGRpahP9vJGxkZWbZqVXV3x3HK9XrtwGq18uxKxXmR4zivVarydiI+Rgg+Ya4Al/wKkbyAiK8g4huJ+G7dNzDejPPoFPRnozy39H/oIkEFBDtanhC8EckjtbwtRHzv3DkOvFkbiYuVUt9xHHWmlOo0Zv6IEPwBInoXM7+ZmV8jhHwpER3BzIcJoQ4hovXMPK6UkuVyeaUQYq9qtbLb5OT47s3mgTv10pP1yMjIMqXULkKIvcrl8kqllGTmcWY+0HFqh1erlX+uVJyXKVV5neM4b5aS30JE7xKCP8DMJwrBJzPzl5n5m0TyQi3cm/U9uFffE/TnFPKE4O9AP4yKfzaUAfCcJ1xEsKPl6feCSB7g+fEelVLer+s0/ImIf0vENzLzr+bEj/6XiK8j4muI+Cpdy+EyIr5EF3S5WBuTi6RUFyqlNkmpLiCibwnB5xDJs4jkWXP/n84l4vOYeSMRf3dutbm8kIh/oFmXMPPlUqqrpVTXSKmuk1L9Qkq5mYg362u6UV/jn/Q1/11XqMT9Bc+PdwX0w5j4u+f9+BsA/eO8fvovItjR8nSiRPIADzzw+p33G+iHEfEf1qf9DvqW/tc/zumn/4LnbGEEOyKeEPw3JA/wwAMPPL4X+hE7L6e/2w1AkFNY5jEABQQ7Ol6j0RhE8gAPPPDA438Q8Wyz2cxCP2Lj5bWeuwYgG/SOIOcxAMsR7Gh5zPwUJA/wwAMPvLmvUmpP6EcsPFfDXQMw6Df1n9UOwTUAeQQ7eh4RVZA8wAMPPPC2nwroQD8i57mz964BGPYT/wHtDoY87wsQ7Bh4c1vAkDzAAw888Oa2AoqDoR+R84oeA5ALWvTnNQDDoasEIdht8+bq3yN5gAceeODNGQB5JPQjcp5rAPK+eq7/aMCzRxDiHyOPiN6C5AEeeOCBt/0VwJuhH5HziqHW8HkMQBbiHz+PSB6P5AEeeOCBt70c8PHQj8h5hXbK/Q5A/M3w9ClhSB7ggQceeMT/kFJ9AfqREK9T4UewO+PN1TNH8gAPPPDAk1L+Qyl1HvQjeR6CY4AnBF+M5AEeeOCBN8dgVpdAPyD+fcEjop8heVjL24L4gQeeWZ5S6ufQD4h/X/CI5K+RPOzkKVV5J+IHHnhmeUrJG6EfEP++4Ekpb0PysJGnrp+ebq5gVlcgfuCBZ5JHt0A/IP59wZNS3o/kYeM0ZOWo6elmkZlfgPiBB55R3n3QD4h/X/CklFuRPGzjqdubzQ276Pu7lJl/h/iBB54x3lboh1nxD737D8GOjvfUpx7wFAx2+3iO47zPe3+J5OsRP/DAM8cbGRlZBv0wwnNL/4cuElRAsKPhTU1NrMZgt453z/j42JO8961UKg0T8a2IH3jgmeGtWlXdHfphRPyzoQyA5zzhIoIdDW98fKyOwW4Xj5nfs8h2zbcgfuCBZ4ZXKjmroB+xi7973o+/AdA/zuun/yKCHQ3PcZxxDHareLc1Go38QvdQKTVExH9E/MADL35euaxq0I9YxX9Yn/Y76Fv6X/84p5/+C56zhRHsLnlSyg0Y7PbwmPm1/jUb6F8QP/DAi59XLqsDoR+x8XL6u90ABDmFZR4DUECwo+Ex82EY7Nbwfp7JZILu8xIieTniBx548fLKZfks6EcsvLzWc9cAZIPeEeQ8BmA5gh0dj5lfhMFuBW+bEGIizH0logoRP4b4gQdefDwh5JHQj8h5roa7BmDQb+o/qx2CawDyCHa0PCHkqzDYk+cJwZ9o5/4SyQ8jfuCBFyeP/hX6ESnPnb13DcCwn/gPaHcw5HlfgGBHzCOiozDYE+ddp5Qaauf+Hnpoc2cp1TWIH3jgxcWj10E/IuUVPQYgF7Toz2sAhkNXCUKw2+Ix85sw2BPl3Ru03Wix+zsxMe5IKe/B/QAPvOh5QvAboR+R8lwDkPfVc/1HA549ghD/mHhCyLdhsCfGe4yZp7u5v9Vq5ZlE3ML9AA+8aHnM8m3Qj0h5xVBr+DwGIAvxj5dHRP+GwZ4Ib1YI+dIo7i8RvYCIt+J+gAdedDxdjhv6ER2v0E653wGIf/w8Zj4Gg904b5t3gVFEuzleTMRbcD/AAy8anlLOB6EfCfA6FX4Eu30ekXw/BrtR3kPM/Lw47q+u6fAA7gd44HXPU0p9GPqRLC91jVm92mFmfgeRPItIXi4EX09EP2PmjUTyhFqt+owNG9btbEuwiehDGOzGeDcz83i8azpE1e/oYNwP8MALx3Mc56MWieFSIZynKiU/KqW6QEp1NbP6P6XUlULwmXNruagM8U9uNf3zmfnqcJ1L3aGU/M9qtTKddLCF4BMw2E3w6Nx6vb6TiftbKpWKRHw27gd44HXOU8r5RML5eQkRrSeiU4j49pDtvVII9VyIv6HGKKX2JaIfdtFZ/ygEnxT3k6HPK4CPYbDHyaM7mflFSfRnIjqCiG/H/QAPvE549LEk9IiZx5n5k0R8cxftvahUqo5A/GPkzbkzvjvCznqTEHyCUkqaai8zfwqDPRbeFiI6pZ2n/jjuLxHtpJTzOSnlY7i/4IEXnicEn2RKj1avdljPxt4UYXvvZua1EP8YeFLKNUT8YIyddbMQ/AFvkZg42quUOjXGwTQrpbyBSJ4tBJ9GxGcQ0c/a2bKWwuQxS0Tfavd9XNz9uVodryulzpVSboM4gAdeGJ78jzjHb6nkrBKCP0DE/xdjex8ol9UUxD/aaf89ifgvBjvrz5nluycmGhx1e5WSp8cwmB5zHPX5er1WWej6Vq2q7q4XHz7QQ8ljm5RyIxHVbe7PSqmKXh+AugHggefLo89GPX5LpeoIEb2LiK8x2N7bq9XqU2wX/9C7/yxY8Lcxoc46K6VzleNUjpmcnFgVRXuVck6PdjCpP42N1Q8MWYRolIivTXnyeEQp+V9jY7WxNG3FKZfLK4n41DCzWBAH8PqTR6dEMd5WraruzsxvZubLiHg2mfaqCyzeOuiW/g9dJKiQVGOIqGlJZ906t/iQXqeU2qXT9jqO+nyE4n/L2Fid27kfQognEfHPU5g8bnMc57jJycb+ad6HW6/Xd9JPJH+EOIAH3hPOAji50/FWr9d2ZebXEPEP2p1ti6u91Wrl2ZaKfzaUAfCcJ1xMyskQ8YyFnfUxZv4fZn5FqVQqttNepZzPRXR9WyuV+iGd3A9m3o+I709Z8nhAKfWFarUykeYiHPo95MlEfB/EATzwvF/57+2Mt3Xr1uytVOW1WiMeta+96kILxd8978ffAOgf5/XTfzGZZFl6sp+bs6TzP0LE5zHzixqNRj6ovcz8qSiuTyn5te5Wq/OxKU0es0LwJiHEwWkSfyHEAfpV1izEATzwFpwBOClovK1dO7mH41RfIaVzvpTyYcvbu5WZd7dI/If1ab+DvqX/9Y9z+um/4Dlb2KiT0fXWU9P5heCvBrXXrQPQ7fWNjdUP6OZ+TEyMj/bAVrWrmPkwm8WfmZ8mBP8E4gAeeEF/Rx8NGm9SqnPStbVRHmnJTGVOf7cbgCCnsMxjAAoJHZzzyTR1fmb+ZlB7mfnECK7vjijuh5TOVT2SPH5KRE2bxJ+I1ocRfogDeOBtf4A6IWi8SSk3pmxr48csEP+81nPXAGSD3hHkPAZgeXIH5/DX09X55XeD2kskj4/g+i6O4n4o5Xyll5IRM28UQuyfpPg7jrMPM38D4gAeeO3y6Lig8Sal+l662ivPSlj8XQ13DcCg39R/VjsE1wDkkz04R16Qrs6vfhjUXmb+YLfXx8wbo7gfSsnP9mAyepiZj9lnn70HDYv/0rkDQoJPB4Q4gAfegjMAxwaNN73KPzXtFYK/k6D4u7P3rgEY9hP/Ae0OhjzvCxJdvejdAZCOzq+uCFHO+H0RXN93o7gfQvBJvZuM1NVjY/WKCfGfKzbClyCZgwdeNzx6b9B4C/tazZb2hnlYi3GmsugxALmgRX9eAzAcukpQjI1xDUB6Or+6JkRRo2MiqJj1vSjuBzN/pMeT0d8cp/aCOMVfCHVImPMpIA7ggRc4A/DuEGuork5Te4MMQMyvKV0DkPfVc/1HA549gomLvzYAm1LW+TcH1zWgd0VwfT+I4n60ux4hpclom5T8lnjEX7567hAiJHPwwOuWx0zvCl5Dxb9IU3v9DICBNUrFUGv4PAYga4v4T083l0qpLkxX51c3BbVXvyfu9vp+HMX9EIKP7Zdk5H2/GI15on9DMgcPvCh5lWOCd9fwjWlq72IGwNAC5UI75X4HbBL/6elmcb4BSEHn/2OIrY2vjWBa6bKItqq9t7+SkTw6iv7MzG9GMgcPvGh5lYrzluA1VHxzmtq7kAGw7pTAToU/7sZ4DUBKOv/twa816CVR7HuP4n4QyaP7LBnNEtFLunzyPyLsASMQB/DAC89zHOc1wWfD8B1p25pstfh384m7Ma4BSFHnvzeojULIf4rg+q6J4n4w8zv6MBk9QkSNTuInpVRhT/ODOIAHXns8x6n9S5B+CMF/S1N7vQYA4t8mT0p1Yco6/0MhXgFMR1Ax6/oo7kfQVHavJiNm/p0Q4kntxK/RaOSJ5K+RzMEDLx5erVZ9TvCR5vxwmtrrGgCIfwc8It6Uss6/NcQrgHURXN/mKO4HkXx9Hxcd+VI78SPiU5HMwQMvPp6Ucn2InWHbUlYefiPEv0MeBRwFbGPnbzYXqbP8uAGod3998tcRnbXwmn5ORsw8HSZ+5bI6sJ33/kjm4IHXCY8qfvmq0WgMpm9rI58P8e94wVX7BiD5Iy2fOLU8v73j42P1CK7vt1HcDyHky/o8Gd20YcP6XQPiN0DEm5HMwQMvXl6p5Kzyy1elUqmYtvYqpWYg/h3y2jUANnQGx3H28WvvxMS4iOD6bo3ifjDz8/s9GSlVOTbg7IY3I5mDB178PKXUnn75Sim1b/ryi9pkq/iH3v2X3JGq4Q2APZ1hx2ksb3snJxsjEVzffdGctUBPRzKSf1u7dmq/heKnlCoQ0Z1I5uCBZ+LgnMVnT/WruFra2usxANaIv6f0f+giQYUkpjHCGgDL9n0+1a+969ZN7RLB9W2J5qwFWo9kJP/hOOpTC8VPCP4Akjl44JnhZTKZgEXLdFDa2qsNgG3inw1lADznCReTmMYIYwDsW12unhtid8Mj3V7f2rWTe3R/1kIUCxJ7Ihndr5Ta5YniL55ExPcimYMHnhHeI0H5Skr5/LS1V68BsEn83fN+/A2A/nFeP/0Xk5jGCDIAlu77fEWI3Q13dHt9k5ON/bu9H5XKeA3JaPurm+PmPf2/G8kcPPCM8W4PyleOo96YtvYy8/kWif+wPu130Lf0v/5xTj/9FzxnCxt1Mn4GwN4jLeXbQtSS/1W31zc+Xlfd3o+pqYnVSEbbv/c0Go28fvc/RMS3I5mDB54x3i+D8pXjOO9PW3uDjgM2uOYup7/bDUCQU1jmMQCFJKYxFjMAdnd+Oi7E7oYrur0+pVSl2/uxbt2avZGMnlAc6I361cgrkczBA88kT14elK+Uck5KW3vbNQAxiX9e67lrALJB7whyHgOwPKl3GAsZgJhu3qOVivMGpdRFUsrHuuOp00Lsbvhut52rXFZT3d6PZvPAnaSUs0hGTyiwtISIr0MyBw88kzx5QVC+Ukqe3uX1PaaUuqhScd4gpXzURHvbMQAxib+r4a4BGPSb+s9qh+AagHySCxjmG4AYO2vLvT6l5JOJ6Cgi+aPFyk767/t0zgpqrxD81W47lxDqkIjKLT+IZPSERHQ8kjl44BnnnRGUr5jV1zu4rm1E9ENmPmpqamK/x8+ZkS0T7Q1rAGISf3f23jUAw37iP6DdwZDnfUGiqxe9BiDmztpaZB/4nkLIt+kp+9kwPObtlZ/8TuH7TLedy7vboMtyy39BMgIPPPASPpvj5BAPK2HrwswSycuJ5FuVUnsufNCcbJlobxgDEGOdnaLHAOSCFv15DcBw6CpBMTbGveEGOmsr6PqUUvvqleHX+vPUZcG7G+i47juX/7n2bZRbvhHJCDzwwEuWRx8KylfMfFkA5+dE8mhvNVYfM9Ey0d4gAxBzkT3XAOR99Vz/0YBnj2Di4q/FaZOhztpq57qYeTUzf5CIf7kAb3NQe/WsQiQL1rovtywvRzICDzzwkuXJt4Z4WLlhgb/dLAR/YKFzBAJmPlsm2utnAAxU2C2GWsPnMQBZW8R/erq5VEp1oaHO2uq0vfV6bZ3jOJ+SUv1e827u5BCeDpzlB6Mpt0znIxmBBx54yfLUy0LsnrpV//43zHyilFJ1MfPZMtHexQyAofL6hXbK/Q7YJP7T083ifAMQY2dtRdFeIpqaX1BmkRmEZ3ffueizUdwPZv4ykhF44IGXJK9Sqbww+GFFHs/M4xEdNNcy0d6FDEBSZ+t0fyqQYSfjNQAxd9aWyfYS0bruOxd9LYrrE4JPQjICDzzwkuTVatVpwwfNtUy0d74BsE78u/nE3RjXABjorC2T7RVC7B/BvtkLo7i+oJK3SG7ggQde3LyxsXrF8EFzLRPt9RoAiH+bPCnVhYY6a8tke0dHR3MRdK5rorg+IeSrkIzAAw+8JHnr16/d3fBBcy0T7XUNAMS/syI1mwx11pb5BY7y7112rj9GU26ZnoNkBB544CXFY5b3JXDQXMtEe5l5I8S/8yI1M4Y6a8v8Akfnpi471/3RlFum9UhG4IEHXlI8peSNCRw01zLRXmY+H+Lf+VaNGUOdtWV+gaPzk2471/j4WC6CBYllJCPwwAMvOR79yLQeLWYAom6vUtsrw0L8O9iqMWOos7ZMt1cp55vddq6xsXq52+urVCo7IxmBBx54yfEW3tEUpx4tZADiaK9SapOt4h96919S7zDaMQBd3ryW6fYq5ZwSwdaZ9RFVXHwEyQg88MBLhkefMi2G8w1AXO31GABrxN9T+j90kaBCEtMYYQ1ABDevZdrsMMt3d9u5arXqc6O4PqXkH5CMwAMPvGR49C7TYug1AHG2VxsA28Q/G8oAeM4TLiYxjRHGAER081qmZzqEkC/ttnM5jvPGiCouXo5kBB544CXDi+Zgs04MQPwLHNWMZeLvnvfjbwD0j/P66b+YxDRGkAGI8Oa1TL/mEEId0n3ncj4cxfW56xGQjMADDzzTPCHEwabFkIhbJtrLzOdbJP7D+rTfQd/S//rHOf30X/CcLWx6q8aMoc7aMr3GoVwuUwSd6/NRXJ/jqM8iGYEHHnhJ8MrlMiVQh6Vlor1BxwEbXHOX09/tBiDIKSzzGIBCEtMYixmAGDpry/Qah0ajkY9g9ez5UVyflJVjkIzAAw+8JHiNRiNvvg7LEw1AXO1t1wDE1N681nPXAGSD3hHkPAZgeVLvMBYyADF11lZCaxzu7nIwXRvNgkR+IZIReOCBlwDvriR2n3kNQJztbccAxNReV8NdAzDoN/Wf1Q7BNQD5JBcwzDcAMXbWVkJrHK7pcjD9JYrrk1KuQTICDzzwzPPoZ0lsPXcNQNztDWsAYmqvO3vvGoBhP/Ef0O5gyPO+INHVi14DEHNnbSWzxoHO7XIwzTYajcFur69Uqo4gGYEHHnimecz8zSTqzkgpWybaG8YAxNjeoscA5IIW/XkNwHDoKkHxbtWYMdRZW8mscZD/3u1gYub9ur2+ZrOZJeKtSG7ggQeeWd4TiwAZPGiuZaK9QQYg5va6BiDvq+f6jwY8ewQTF39tADYZ6qytJNpLRG/pdjAJIQ6IpugS3YJkBB544JnkMfObk6g426kBaLe9fgbAQHuLodbweQxA1hbxn3s3rS401FlbSbSXiA7vdjAJIY+MaEHiJUhu4IEHnkkeMz/btPjPrwQYZ3sXMwCG2ltop9zvgE3iryvUXWios7aSaC8RVbofTDuW0ezk+pj5y0hu4IEHnkkeMzsJHTTXMtHehQxAUmfr+FYJynT4iXerxuMGIObO2kqivUKIJ3U7mITg0yJakPg+JDfwwAPPJK9WqxYTOmiuZaK98w2AdeLfzSf+rRpzBsBAZ20l1V4p5b1dDqbvR3F9QsgjkdzAAw88g7x7khLDsAYggjUOGyH+HW/VUBca6qytpNorpbq+y3dov4vi+ph5HMkNPPDAM8dT1yclhmEMQERrHDZC/DvfqrHJUGdtJdVepdS5XQ6mLQcf3Bzq9vpWrly5AskNPPDAM8VTyvlmUmIYZAAiXOOwEeLf+VaNGUOdtZVUe5VSH+92MNVq1WpEdRfuQXIDDzzwTPCUUh9LSgz9DECU7WXm8yH+nW/VmDHUWVtJtbdScV7V7WCqVivPi6juwlVIbuCBB54JXqVSeVVSYriYAYi6vUqpGYh/51s1Zgx11lZS7VVKNbp30pV3RrMg0fkmkht44IFngjc2Vj8gwYPmWmZec6hNtop/6N1/Sb3DaMcAdHnzWkm1t1qtLifi2W4Gk1LqlCiuTynnJCQ38MADzwBvdmpqspCUGM43ADG+5thkm/h7Sv+HLhJUSGirxoyhztpKdqYjXBnexdurLoji+hzHeQ2SG3jggRc3Twj+c5Ji6DUAcbZXGwDbxD8bygB4zhMuJrRVY8ZQZ20lOdMhBF/c5eD8RRTXV6tVD0ByAw888AzwfpCkGLoGIO726jUANom/e96PvwHQP87rp/9iQls1Zgx11laSWzWE4NO6HEwPZjKZJd1e3/r1a3Zr51RAJDfwwAOvQ96pSYohEbdMtJeZz7dI/If1ab+DvqX/9Y9z+um/4Dlb2PRWjRlDnbWV5FYNIeTbuh2cQccCt7H18kYkN/DAAy9ennxrkgvQpZQtE+0NOg7Y4Jq7nP5uNwBBTmGZxwAUEtqqMWOos7aS3KpBRM/odnC6p2pFsPXy20hu4IEHXrw8enqSC9DnG4C42tuuAYipvXmt564ByAa9I8h5DMDyBLdqzBjqrK0kt2oopfbtdnAy83uiuD4h+AQkN/DAAy9OnuM4+yS5+8xrAOJsbzsGIKb2uhruGoBBv6n/rHYIrgHIJ7mAYb4BiLGztpLeqkHE93UzOIXgr0azIHHhQ4GQ3MADD7woeELw35Leeu4agLjbG9YAxNRed/beNQDDfuI/oN3BkOd9QaKrF70GIObO2kp6qwYzX9bl4LwuiuuTUiokN/DAAy8unhD8k6TrzkgpWybaG8YAxNjeoscA5IIW/XkNwHDoKkHxbtWYMdRZWxaYnVO7HJwPZzKZpd1eX6PRGCTix5DcwAMPvJh4pyZddC7sccDdtjfIAMTcXtcA5H31XP/RgGePYOLir0Vxk6HO2kre7NBR3Q5OZl4d0e6LXyK5gQceePHw6KgkxV/vdmqZaK+fATDQ3mKoNXweA5C1RfzntmqoCw111lbS7ZVSru92cDLz86K4PiH4HCQ38MADLw4eM69NUvznVwKMs72LGQBD7S20U+53wCbxn9uq8UQDEGNnbSXd3vXr1+4ppZztpr1C8LFRXB8zH4PkBh544MXAm61Wq8uTFP9ODEAXZmdj0mYnVJWgTIefeLdqPG4AYu6sLRvaq5T6QzftFYLPieL6hFCHILmBBx54UfOY+Xc2iGE7BqDL9m60Wvy7+cS/VWPOABjorC1L2vvdLtt7QxTXV6/Xd3JPKERyAw888CLknWeDGIY1ABG8lt0I8e94q4a60FBnbdnQXqWcj3fZ3q1TU409I1qA+VskN/DAAy9anjzeBjEMYwCiaK9rACD+nW3V2GSos7ZsaK+U8ohu21urVacjWoD5LSQ38MADL0oeMz/fBjEMMgARtncjxL/zrRozhjpry4b2MvN+3bbXcdS7o7g+pSrHIrmBBx54UfKUUvvbIIZ+BiDK9jLz+RD/zrdqzBjqrC0b2qvbfFd37XW+FsX1VauVZyO5gQceeBHy7rJFDBczAFHHTyk1A/HvXAxnDHXWli1bNYQI/9pjkfZujuL61qyZHJFSziK5gQceeFHwmNX3bRHDhQxAHPFTSm2yVfxD7/5LcKvGjKHO2rJlqwaRPL7L9m5tNBr5iNZg3IjkBh544EUjhs4nbBHD+QYgrvh5DIA14u8p/R+6SFAhoa0aM4Y6a8uWrRpEdHj37aV10azBoK8huYEHHnhR8KrVypG2iKHXAMQZP20AbBP/bCgD4DlPuJjQVo0ZQ521ZctWDcdx9ui+vfKt0azBkG9FcgMPPPCi4I2NTZVsEUPXAMQdP70GwCbxd8/78TcA+sd5/fRfTGirxoyhztqyaauGEPznLtv731Fcn5RyDMkNPPDA656nbrHpSZiIWybix8znWyT+w/q030Hf0v/6xzn99F/wnC1seqvGjKHO2rJpqwYRf7vL9t4Q0fUNEPH9SG7ggQdel7uTNlr0JLxUStkyEb+g44ANrrnL6e92AxDkFJZ5DEAhoa0aM4Y6a8umrRpE9N4u27t1/oEbXbyGuQjJDTzwwOvySfi9Nu0+m28A4opfuwYgpvbmtZ67BiAb9I4g5zEAyxN8Ep4x1FlbNm3VkFIe2m17hRAHR/Mahj6E5AYeeOB1wxNCHWLT1nOvAYgzfu0YgJja62q4awAG/ab+s9ohuAYgn/BWjRlDnbVl01aNlStXriDibV2eQvXBKK5v/smASG7ggQdem7xtK1euXGFT3RnXAMQdv7AGIKb2urP3rgEY9hP/Ae0OhjzvC5LeqjFjqLO2LNuqsVRKeUOX7f1+FNfXaDTyRLwFyQ088MDrkPcL24rOSSlbJuIXxgDE2N6ixwDkghb9eQ3AcOgqQfFu1Zgx1FlbNon/3Dsq9cUu2/v3TCazNKKZmGuQ3MADD7wOeafaJP66yFnLRPyCDEDM7XUNQN5Xz/UfDXj2CC6xQQyJeJOhztqySfynp5srKpXKq7ttr+M441FcnxB8MpIbeOCB1+E0+ItsEn990FzLRPz8DICB9hZDreHzGICsLeKvj6S90FBnbdkk/tPTzRVjY3WO4CCKo6OZiaEXILmBBx54nfDK5fLeNom/txBQ3PFbzAAYam+hnXK/AzaJv54Gv9BQZ23ZJP6eaao/dtde9a0orq9SqeyF5AYeeOB1wPuDbeLfiQHoNH4LGQCbis5tXwOQ6fAT71aNxw1AzJ21ZZv4T083lwrBX+2yvbdFuG/2HiQ38MADr03eGbaJf7sGoMvdWButFv9uPvFv1ZgzAAY6a8s28ddT70d1297x8bqKyIz9DskNPPDAa49Hr7NN/NsxAN3Gz2sAIP5tb9VQFxrqrC3bxD+TyWRWr3a4+/aql0Vkxu5AcgMPPPDa4ZXLZbJN/MMagCji5xoAiH9n78A3GeqsLRunqXRHvbvLWtSf7/b61q6d2k9KOYvkBh544LXBu8tG8Q9jAKKKHzNvhPh3vlVjxlBnbdko/noL3ne6bO8fMpnMkm6uz3HUa5DcwAMPvPbKkfN3bF0A52cAoowfM58P8e98q8aMoc7astWpCsFvj2Af7rO7nIm5AskNPPDAa4cnhHybrQvgFjMAUcdPKTUD8e98q8aMoc7astWpSilVBOUofzU6Oprr5PqY+SVIbuCBB167PKWUtHUB3EIGII74KaU22Sr+oXf/JbhVY8ZQZ23ZvFWDiG+PoCLVmZlMZmk71yeEM0bEf0dyAw888Npk3Gbz6vf5BiCu+HkMgE1nzbil/0MXCSoktABuxlBnbdncWZn5zIjeyZ2jlCqEm/Z3pon4HiQ38MADr12GEPxVm1e/ew1AnPHTBsA28c+GMgCe84SLCW3VmDHU+Vt2d1Z6ZYTt/ZMQ8lWjo6O5ha6vVqs2iOgrRDyL5AYeeOB1uPjtFTavfncNQNzx02sAbBJ/97wffwOgf5zXT//FhLZqzBjq/C2bO2upVB2JobM+IKX8gVLydMdRn1VKnS2l/CWSG3jggdc9j/axefU7EbdMxI+Zz7dI/If1ab+DvqX/9Y9z+um/4Dlb2PRWjRlDnb9lc2edOxhJ/gbJCDzwwLOfJ3+dgnzaMhG/oOOADa65y+nvdgMQ5BSWeQxAIaGtGjOGOn/L5s46Pd0sKqW+gGQEHnjg2c5Tyjnd9nw63wDEFb92DUBM7c1rPXcNQDboHUHOYwCWJ7j6fcZQ52/Z3Fmnp5srqtXKS5CMwAMPPNt5jlN9ie351GsA4oxfOwYgpva6Gu4agEG/qf+sdgiuAcgnvFVjxlDnb9ncWaenmysmJsb3kVJuRTICDzzwLOZtnZxsjNieT10DEHf8whqAmNrrzt67BmDYT/wHtDsY8rwvSHqrxoyhzt+yubOiIh944IGXDp66ynbxdw2AifiFMQAxtrfoMQC5oEV/XgMwHLpKUKxbNeh8Q52/Zbv4T083lwrBH0AyAg888GzlKVU5ISUPU1tMxG+h8xAMttc1AHlfPdd/NODZI7jEhptHxN8x1Pm32S7+mUwmo5SqIxmBBx54tvIqlfr6NDxMmatzQucm2N5iqDV8HgOQtUX8p6ebS5nVt011/mZzkZWRloi/Z1bkFiQj8MADzzYes7w1DeKvlBoyGL+zE2xvoZ1yvwM2ib/e+na2qc6/cuXKFbaL/5wBkKcjGYEHHnj28ej0NMyk1uv1nczFT37FJv1YdA1ApsNPnI1xHPVlU52/VKqO2C7+mUwmI4R6LpIReOCBZx+PnmO7+GcymQwz72cufvJzVot/N5+4G6OUOsVc56eK7eKfyWQy1Wp1+WJnWSMZgQceeAnxHmk0Gvk0vEYVQlTNxU9+HOLf8VYNdYKpzi+EOsR28fe8BrgQyQg88MCzh0ffS4P46xmAaXPxo/dC/DvfqvEmc52f/iUN4q8XAr7FzGBXdyolvyGl8xHHcd5LJE9g5o1E/CCSJXjggefZ7/7mNIj/3AyAfJm5+MnXQ/w73vcu/9lc55dHp0H89RTW/vEOdvVHx6m+ct26qV0Wuj6lVIGI3kXE9yFZggceeEQ0mgbx1zMAx5iKHzO/AOLfIU9KucHgas3PpUH8PTUSfhnPQR7qvKc+9YCnhLk+pdS+RPS/SJbggdfXvF+mRfznHqD4NFPxq9Wqz4D4d75ac7Wpzi8Eb0rTak1m/mgMg33jhg3rdm7n+ur1+k7M/CskS/DA608eM5+YFvE3vYaqWh2v2yr+oXf/JXXzGo1G3mDnvykt4j893Vxar9cOjLiz/nndujV7d3J9zOwQ8WNIluCB1388pVQ9LeKvZ0//YCp+U1ONPS3UD7f0f+giQYWkbh4R32Oo828tlUrDaRD/x4+0VDdH1VkrFef13R3cRP+JZAkeeP3Gk79Pk/iPjo7miHibofjdZan4Z0MZAM95wsWkbh4RX2Oq8zuOM54W8Y+yTgKzvG/DhvVP7ub63L21SJbggdc/PGb+ZFrEX+epCXMHI6mfWyj+7nk//gZA/zivn/6LCS7YOMdU51eq8o60iP/0dHNFrVY9NIrOqpTaFMX1SSlvQLIED7z+4Ukp16RF/PX7/7cajN83LBP/YX3a76Bv6X/945x++i94zhY23hhmPtFU51dKfT0t4j893SzWas4AEd/afWdVn4nm7Abng0iW4IHXLzy6JZPJLEnZ7qmzDZ6NcLxF+pHT3+0GIMgpLPMYgEKCRW9eYq7zq9+nRfxdHpH8XLedVSnnE1Fc3/j4GEkptyFZggdeP/DolDSJvzYAN5uKHzO/2BL9yGs9dw1ANugdQc5jAJYnefOUUnWTnX9srLYyLeKvDVKz284qBP+/qK5PSvUjJEvwwOsHHh2UJvFXSu1tMn5CiKoF+uFquGsABv2m/rPaIbgGIJ/0zdNnN28x1fmZ+Z/SIv76M0DEd3XZWX8a1fVJKV+BZAkeeD0v/ndmMpmlado67TjOSw3G71Gl1FDC+uHO3rsGYNhP/Ae0OxjyvC+wZdrmF+Y6/+OnN6VA/N34fKHLzrpVKbVnFNenTyvEWQHggdfTPHl6msR/bo3S3K4pQ/H7hQX6UfQYgFzQoj+vARgOXSXIQGOkdL5msPP/PE3iP7dTQh3S7WBn5ndGdX3MfCaSJXjg9S5PCHFwmsR/7vWk3GzwbIT/skA/XAOQ99Vz/UcDnj2CS2y6eZWKc7TJzq+U2jct4q8/S4n4ti4H+3VRXR8zPw3JEjzwepZ3ayaTWZom8R8bq1VNxk8I+QYL9KMYag2fxwBkbRN/vd/9ILM3j9+eIvF3XwN8utvBLqVUEV3fooYEyRc88NLOk/+RJvHXRdPe34flkQvtlPsdsFH8p6ebKw49tLmrlPJBg53/kjSJ/9xrgO4rXAnBn4jq+ojoU0iW4IHXezxmnkyT+GsDcKXB+D2QyWQGUqMfnQq/ycYIwT802Pm3lsvl3dIi/p5ZgJu63Nd7i3dlbzfXN9+QIPmCB15P8G5Mm/jX65Mr59f/jzl+30/Tw2PGdvGfnm4uFYKPNdn5mfk16StyQcd3O9iFUIdEdH1LifghJF/wwOsdnlLyo2kS/+npZpGZjzJcIfF9EP+Iecy81nDn/36abt70dHPp2FhtrPvBLr8S1fUR8W+RfMEDr3d4jcb4WJrEf3q6uZSIf2AyfgudjwDx7543QMT3Gez8s6WSsyp9W13UdV0O9vtLpVXLo7g+Zv4dki944PUM79r0iT+ViXjWYPzunf/+H+IfEY+IzjXb+elTaVvt6jhO16tdHUe9OoLrW0rEDyP5ggdeb/Acx3lf2s5KEYJPNhk/Zv4GxD8mnhDy1YY7/z1r1kw+OU3vvBqNsdXdLnhRSl3U7fWVy2oKyRc88HqGt3ViYlykSfwbjUZeCP6byfgx8ysg/jHxVq2q7u4nbnF0fsdx3pjCaa/vRTDYnS4XJJ6C5AseeL3B0w8FxTQdlMbMrzEcv61EtGtaxD/07j+bGsPMl5nt/Oqa9E17qRd2mzyY1RmdXl+1WtlPSvkwki944PUGz3Gcl6VJ/PUi5GsNx+/SlKwZc0v/hy4SVLClMULw2813/trBaer8+gTFu7vs/LNEdHi713fwwc0hKdVPkHzBA69nePesXbtmOE3iL4Q4IIEKiW9NifhnQxkAz3nCRVsaUy6X9/a+BjA0mC5K22sTZv5MBJ3/QSnloWGvb3x8LKeUOg/JFzzweocnBH8mhQvGf2g4ftuUUnunQPzd8378DYD+cV4//Rctq3h3ifnBRAela8GkqEbU+bcIwSc0Go18wKuZSSnV9Ui+4IHXWzxmdtIk/kGno8YTP/pRCsR/WJ/2O+hb+l//OKef/gues4WtaIyU/Drzg0lenqYFk9ooXRNh5797blaBniPmPvuXy2pKCH6jEPxDJEvwwOtFHv0sbQvGifin5iskVt5k+W6xnP5uNwBBTmGZxwAUbGpMozE2IqV8xPRgYubD0rRgUgh+I5IbeOCB1ykvzLG2dok/HZ5A/B5et27N3haLf17ruWsAskHvCHIeA7Dc0op35yQwmK7LZDJL0iD+mUwms3LlyhVuMR4kN/DAA69N3sMrV65ckaKZzyVC8PWm46eUOtti8Xc13DUAg35T/1ntEFwDkLd19XutVj0sicEkhHxZGsTf44i/huQGHnjgtctj5jPT9NqTmV+eTIXE2jMtFX939t41AMN+4j+g3cGQ532B1VvfiPiGBAbTXUqpXdIg/nox4MFIbuCBB177PGqmRfyJaFe/rc8x1on5lcV1YooeA5ALWvTnNQDDoasEJdgZiOTrkxhMQvCX0iD+Lk9K+RskN/DAAy88T/46ZQue/zuJ+FUqzlstLhLnGoC8r57rPxrw7BG0XvwzmUymVCoNE/GtCQymWWZ+aloOCnIc531IbuCBB15YnhD89rSIv9+2v5jjd9uGDet3tXimvBhqDZ/HAGTTIv6e9z7vSGYwyV/rm2/9QUHr16/d1y3Ni+QGHnjgBfAeClr8Z4v4j46O5oj4t8nEr3KM5a/JC+2U+x1Im/hnMnMnPhHxXUkMJqXUx9NyUBCzOgPJDTzwwAviCcFfTMtWZyL5sWTix3eVSquWp00vF10DkOnwY8cCEPn+hAbTlkrFeUYaDgpSSjWQ3MADD7wgnpRyLB11TsTBC50OayJ+zPzenhD/bj62NKZUKhUXO/fZgBO8jZl3T0P8iPgqJEvwwAPP5/vTNIiX4zh7EPEdCcXvXiHEkyD+FjWGmU9MsFzm9/wKBNkSP2Z+BZIleOCBt9iXmV+egny/lIh/kFz85Ich/pY1Ru8DfTCpwSQEf8D2+OldE/cgWYIHHngLnfdRKpWGbc/3RPShBOP3wPw6MBB/a04JlP+e4GDaGqZwRtLxY+ZPIlmCBx54Cyz+O8n2fL/Qe3/Dp8J+CuJvaWMqlcpeUsoHEhxMdzPzapvjJ4TYf6GFM0iW4IHX17xtQoj9LX/yL8+v9mc4fvc7jrMHxN/uojfvT3gw/WGhTmLX1hn+NpIleOCB56lr8i2b871Sak8i/kPCZyO8o5fEP/Tuv7SI//R0c8WhhzZ3lVLdkPDgvNa7StTCabQDkCzBAw88z9T2eovz1ZOI+Lpk40f/m8lkBnpEL93S/6GLBBXSIP7ut1Jxnk7EswkPzosajcagvacEPr4lEMkSPPD6mvdTW8Wr0WgMzl/xn0D8Zr0GqQfEPxvKAHjOEy6mRfw9BwV9JenBycxfazYPXGFjZ2DmFyFZggceeELIIy0VryXMfKYFpyL+Vw+Jv3vej78B0D/O66f/YprEX295ezIR35v04FRKfX3duqldbIvfPvvsPSil/DOSJXjg9TXv5kwmM2CheA0IwV+1IH5/LZfLu/WI+A/r034HfUv/6x/n9NN/wXO2cCrE//F3R/INdgxOdYGNp0YppT6AZAkeeP3Mo3+zcMHfEBGfZ0f85Ot7ZI1cTn+3G4Agp7DMYwAKaRN//VlKRD+zY3DK74+MjCyzKX6Tk40Rd9skkiV44PUd7+9EtJNl7/zzRPx9G+LHzFdnMpmlPSD+ea3nrgHIBr0jyHkMwPKUir+7d7Rhy753IfgnpVKpaFP8pFSnIVmCB15f8k62Sbz0mS4/sSR+24io0QPi72q4awAG/ab+s9ohuAYgn2bx9yx4O9GiwXmd4zj72BI/KeXKuSqGSJbggddHvK3j43Vli3g5jrNP8lv9nlAV8YQeEH939t41AMN+4j+g3cGQ531B6sXfXVBCxJdaNDjvIqKDbIkfEZ+NZAkeeP3EU+daVOHvoLmcaE38fuw39Z+iujhFjwHIBS368xqA4dBVglISHCHEXm4ns2RwbhFCvs2G+BFRpZ26CUi+4IGXat5svV5bZ0eRH/k2It5iT/zoTmZ+Sg+I/wqPAcj76rn+owHPHsGeEv/Hhc45TEo5a9ngPGN0dDSXdPyI5AVIluCB1xe8/0k6P4+OjuaI+AzL4jdLRE/vEfF3WcvDFvwZ0GsAelL8XZ6U6mQLB+e1QlRUkvGTUq5BsgQPvN7nVSr1Q5LMz6tXO0zE19oXP/poD4n/itC79zwGoKfF//GzApyrLBycjyjlfHDDhnU7JxU/IvohkiV44PUuj1ldkmB+XioEv5uIWxbG79LMIrX+Uyr+4XmdCn9agzMx0WAi/qudg11dNTZWG0sifkKoQ5AswQOvd3mVinN4Qg8XZSK+0tL43V0ul/fuS/Hv5pPm4BDR4Z0cGGSosz6sj55cYjp+RPxTJEvwwOtFnro6gfy8hJnfQcQPWxq/WSHUMyH+fST+Hlf6XssH+8/9tgvGET8ieg6SJXjg9R7PcZwjDRdhay70rt+u+NG7IP59KP6PT3vzybYPdmbeSERlQ/FbQsS/QPIFD7ye4m02lZ+FEIKIzrc/fvQpiH8fi//jgifPSsFgf4yITiGiXeOOn1KVVyL5ggdeL/HUi+LOp+VyeTciPnX+vn5L43fGQq9YIf79Jf6ZTCaTaTQag0R8UUoG+31E8uNCiL3iit+GDet2llL9CskXPPB6gre53fVE7eTTcrm8txB80tzhQvbHTwje1GzueCAOxL8Pxd/9KKUK808OtHywP8asvl6t1g+II36VivNyJF/wwOsFHh0RRz5VStWZ+cy52cnUxO+njUYjD/FvY/dfvwSnVCo9mYhvSttgZ1aXVCqVF65atX82qvg1mweuEIKvR/IFD7xU866LeDfRUmY+zK9miK3xY+ZfKaV26Xfx95T+D10kqNAvwZFSrpRS3pHSwX4bkfyPoGMs29gq+RwkX/DASzOPnhNFPiWiKSL+9FyOSWP86JaFTmHtU/HPhjIAnvOEi30SnKXT081irVZdL6W6M+XJ40YiOk4pVeomfsx8NZIveOClUvx/1k0+rddrDaWcj7uzoimO3+1KKQnxby7xnPfjbwD0j/P66b/YL+LvsqrV8bqU6o89kjxuIJKfI6Ij3CmwsPETQj0TyRc88NLHW6zAzWL5b2pqYr9KxXm5UuoLUqobeyR+vxFC7A/xby7Rp/wOeQyA7/HAOf30X/CcLdwX4u/yKpXKXp2+B7c4eWwTgq9XSp1aq1Wnw8SPiK9A8gUPvFTxrgiT+5h5LRGfLKXcPP+k1B6I37WrVlV3h/g3l2o9z3kNQJBTWOYxAIV+E3+XVyqVikR8SY8mj3smJxsjQfFb7IwAJF/wwLOTJ6U8NMSi5yIR39OL8ROCLxZCPAni31yqZ/KXeQxANugdQc5jAJb3q/h7BsowEZ/Xm8lDnRQmfkT8YyRf8MBLBe+SkE//H+nF+DHzN5VSQxD/7Wv4lnsMwKDf1H9WOwTXAOT7Xfw9nwEi/kIPJo8HFpom29EA0DokX/DASwOP1gWN53K5vqeU8qEefPI/LZPJLIX4b9+9V/AYgGE/8R/Q7mDI874A4r+jaz6xB1cLnxImbkT8bSRf8MCzmndemPynlHN678WPjuvnOjYL8IoeA5ALWvTnNQDDoasE9WGwieiVRPxQDyWPR4loNES7y0S8BckXPPCs5G3xOzTMzX/1eq0ipXy0h+L3oBDyZRD/HXiuAcj76rn+owHPHkGIf8BHKSWJ+Je9kjyE4K+Ge3JQX0LyBQ88K08N/XyY/KeU+kYPxW9zuVwmiP+CvGKoNXweA5CF+IfnbdhwwB5KOWf1yGDaxsxOUPympiZWSykfRPIFDzyreA84jrNHuCJnclsvxI+ZvzwyMrIMerQor9BOud8BiH9nvErFeYOU8qH0JyN5QZj2KuV8AskXPPBs4snjw+QrKdWFPdDeh5j5FdCjiHidCj+C/ThPiIqaq7iX9mRE64Pau27d1FOI+C9IvuCBZwXvL0qpQlC+qlScZ/RAe29YqKwv9AhHBCfOazQaeWb+csqT0aVh2svMb0LyBQ88K/a9vylMvmJWV6a8vf+90FG+0COIv1U8Zn4+Ed2S1mRULstnBbW32Wxmifg3SObggZco76Zmc8fKbjuebVL55xS39zYh5D9DjyD+qeEppQpE9Km5rTnpSkZC8PXN5oErgtrLzM9DMgcPvOR4zPy8oHzVbB64k5Rycwrbu4WIP71QSV/oEcQ/FTxmdpj5srQlI8dxXhOmvULwxUjm4IFnnicEXxwmXzmOek0K23uFEKIKPYL49wJvCRH9KxHfnZZkpJT6Q7O5Ybeg9jKzQ8RbkczBA88ob+v8bbsL5atmc8MuzPy7FLX3HiHkqzOZzBLoB8S/p3hTUxP7KeV8xXvspt3JyHlPyIOCTkUyBw88o7xTw+QrKfmdKWnvrBD8RSLaFfphRvxD7/5DsKPl1eu1Q6VUP7Y9GQnBfyuXy7sFtU8ptQsR/xXJHDzwjPD+qpTaJXhrstiDiO+zvb1C8MXMvBb6YYznlv4PXSSogGBHz5NSriHi79qdjOTpYdpJRG9BMgcPPBM8ekuY/BJ0eqkF7f2ulHIN9MO4+GdDGQDPecJFBDs+nlKqTkTnEvE2C5PRNinlWIjmDhDxZiRz8MCLlbc5k8kMhKjTMR4mnyTQ3m3MfK5Sqg79SET83fN+/A2A/nFeP/0XEez4eatXO8zMZy62qC6pZMTMl4Vpb61WfQ6SOXjgxceTUh4aJr/M33lkQXu3MKuvEwmJfJ+Y+A/r034HfUv/6x/n9NN/wXO2MIJtgFcqOavmFsRwy6KDN14cpr3MagbJHDzwoucx88Yw+YWI/sWi9raUcr5Sr9cqyPeJ8nL6u90ABDmFZR4DUECwzfPK5fJuRHyMlPL3yScjumWhMpzz2zs2VqtKKR9GMgcPvEh5j5TL5ZVhypGHqUIaf3vV75RyPthojK9Evk+cl9d67hqAbNA7gpzHACxHsJPlNZsH7lSrVQ9TSp0tpXwgqWQkBJ8Qpr1EfCySOXjgRccTgo8Nk1+Y+cQE23u/lM7XarXqYcj31vBcDXcNwKDf1H9WOwTXAOQRbLt4U1OTBSHkkUT8bSJ+2PBgf4SIRoPaq5QaIuIbkczBAy8S3k3j42O5oPwihNh/bowavb6Hifg8KeWLJyfHd0e+t4rnzt67BmDYT/wHtDsY8rwvQLDt3j1QIKIjiPgMIr7bTDKS3wpzfUKoQ5DMwQOve55S6mlh8oF+KDBxfXcT8X8T0RHVanU58rO1vKLHAOSCFv15DcBw6CpBCLYtvKXlspoSgo8l4kvjXEDoOM6zw1wfEZ+NZA4eeF3xvh4mH7RjuDu4vhYRXyIEH1suq6lMJrMU+TkVPNcA5H31XP/RgGePIMQ/5bzR0dGclLKplPNhKZ3vSanujHCBzy/XrZvaJej6lFJ7BlUigziAB96i3/smJhqlEPlggIg3R3d9dCeRvIBIvp+Znzo6OppDfk4lrxhqDZ/HAGQh/r3Lq9UmiJmfSyTfTyTPIuLriPjBTpJbpeIcHeb6vBUCIQ7ggdfW1P/R4WbaFh9jAdf3kBB8/VwuoPcR0eGlUnUE+bRneIV2yv0OQPz7kzf3pE7rmPnFQvC7heCT9fT9RUT8c2b+nZTybinlw1LKbTp53FutVnYLcTlLifhaiAN44LXDU9dv2LBu5xDivysR3+tW7STih4j4LiL5ez3uLiKSZ0upTlOqcmylUnl1tVp5WqMxthr5D7ztawAyHX4Q7P7jrV07ubPjqGXed4F+n3JZTXrLkkIcwAPPl7etWq00Q47fpaVSaTijywMjX4GHI4LBs45HRKdAHMADL5inlDwd+QU8iD94PcOr1apFKeWfIQ7ggee7wPZPBxywbi/kF/Ag/uD1FK9SqTwf4gAeeIvzqtXK85BfwIP4g9eTPKWcsyAO4IG3I08p+TXkF/Ag/uD18ALCqX2J+A6IA3jgeXnqjvXr1+6L/AIexB+8nubp8sUQB/DAe3zq/6XIL+CZFP/Qu/8QbPCi5hHJb0EcwANP/kNKuRH5BTyDPLf0f+giQQUEG7woeY7j7EHEf4U4gNfnvHsnJydWIb+AZ1D8s6EMgOc84SKCDV7UPGZ+BcQBvH7mVSrOUcgH4BkUf/e8H38DoH+c10//RQQbvDh4c4eOQBzA60veJuQD8AyK/7A+7XfQt/S//nFOP/0XPGcLI9jgRcrTrwLuhjiA12e8u8vl+p7IB+AZ4uX0d7sBCHIKyzwGoIBggxcXb/6uAIgDeL3Ok1IegXwAniFeXuu5awCyQe8Ich4DsBzBBi9unhD8VYgDeH3C+yryAXiGeK6GuwZg0G/qP6sdgmsA8gg2eCZ45XJ5Z2Z5K8QBvB7n/WnlypUrkA/AM8BzZ+9dAzDsJ/4D2h0Med4XINjgGePVatXnSilnITbg9ShvVgh1CPIBeIZ4RY8ByAUt+vMagOHQVYIQbPAiPStAng6xAa83efTZlI7fpchXqeS5BiDvq+f6jwY8ewQh/uAlwpucHN+diG+E2IDXWzz565GRkWVpHL9E9FHkq1TyiqHW8HkMQBbiD17SvHJZTRLxYxAb8HqE91i5rCbTOH6ZeZqItxLRKPJV6njhdu95DADEHzwreET0XogNeL3AY+b3pHX8EvF39QzGfyBf9SivU+FHsMGLkbeEiH8AsQEv5byLMpnMkjSO33K5TEQ8q9vx91KpVES+whHBCDZ4ps4KeAoR3wWxAS+lvLuUUnumdfwKwV/ytpWZ34N8BfFHsMEzxiuX5bM8TyEQG/DSwpstl+Wz0jp+HcfZZ/46HCnlnVNTjT2RryD+CDZ4Jp9ETobYgJcmnhB8cprHLzN/fqH2Oo7zfuQriD+CDZ4xnlJqiIivhdiAlxLedUqpobSOt3K5vNL79D//EKOpqYm9kK8g/gg2eMZ4zLyaiB+A2IBnOe9BIYRI83hj5m/4tZeZP4p8BfFHsMEzynMc9QaIDXg284SQr07zeJNSbgjR3kfm1wVAvkqf+Ife/Ydgg2cLTynnLIgNeJbyzkjzeGs0GoNEfEPI9s4gX6WW55b+D10kqIBgg2cDb2qqsaeUcjPEBjzLeJsbjUY+zeONSB7fTvyEkC9Fvkql+GdDGQDPecJFBBs8W3hKSUHEf4d4gWcJ734iKqdb/GkdEW9pJ35C8N+YeT/kq1SJv3vej78B0D/O66f/IoINnk08IdQLIV7g2cATQv5zysV/VyK+uZP4MfPVpVJpGPkqFeI/rE/7HfQt/a9/nNNP/wXP2cIINnjW8Jj5MxAv8JLkMfNn0jze9BbbS7qM3xnIV9bzcvq73QAEOYVlHgNQQLDBs42nFy1dCfECLyHelY1GYzDF422pEPyNaOInP4Z8ZS0vr/XcNQDZoHcEOY8BWI5gg2crr1SqjhDx3RAv8Azz7iqVqiMpHm8DQvCZUcZPCD4R+co6nqvhrgEY9Jv6z2qH4BqAPIINnu08Ino6EW+DeIFniLeNmZ+W1vE2MjKyjJnPjyN+Sjmnr1s3tQvylRU8d/beNQDDfuI/oN3BkOd9AYINXip4QvCxEC/wTPCE4A+k+J3/vkR8bczx+8HU1MR+yFeJ84oeA5ALWvTnNQDDoasEIdjg2cFbQiS/BfECL14enZvW8UZERxDxXw3F74/lsjoQ+SpRnmsA8r56rv9owLNHEOIPXup4tVq1KKX8JcQLvJh4/1etVpenbXwQ0a7MfGYC8dtGJP89TIEk5L9YeMVQa/g8BiAL8Qcvzbx6vVqTUt4L8QIvYt5fy+XyypSNjwEi+Xoivifh+N1MRC9AvjLOC7d7z2MAIP7gpZ5XrVaeJ6XcCvECLyLeVmaeTtP4IKLnuHX9bbkfzHzZQq8FkP8S5nUq/Ag2eLbymOloiBd40fDo31IyPpbMCT/9zPL7cZFrBJCvcEQweODFwnPfe0IMweuUJwR/1fbxMVfNj/6ViDen6X4w8+VKVV68YcO6nZGvIP7ggRcpb3R0NEfEP4cYgtch7+ejo6M5W8cHMz+FiI4j4jvSfD+UUn9wHOd9jcbYCPIfxB888CLjOY6zz2IJEmIIns/3Dsdx9rF5fDDzfkLwJ9xFfmm9H8zqVqXUccy8O/IfxB888CLllctqkogfhhiCF5L3kBBiIi3jY2RkZJkQ8g1E/Nt03Q91faVSedXatZM7I19B/MEDLzYeEb2AiGchhuAF/M0sER2RxvGxatX+WaUqr5BS/dLu+6GuqFYr/4R8BfEHDzxjPGZ+D8QQvICFacekfXw0mwfuVKlUXiWl/L1d90Nd5xF+5KuExT/07j8EG7xe4QnBX4IYgrfIiv8v9tL4WLt2zTAzv5OI70v4ftymVOW1zeaBOyFfWcFzS/+HLhJUQLDB6wXe2rVrhqWUl0IMwZsn/hc3Go3BXhwfc7sF+NsJ3I9ZIj5tampiL+Qrq8Q/G8oAeM4TLiLY4PUKb/36tftK6dwEMQRPT/v/ql6v79Tr40MI+VIi/ruh+3ErkTONfGWd+Lvn/fgbAP3jvH76LyLY4PUST58ZcA/EsO95dy1U479Xx4dSqrRQwaBo74f8kRBiD+Qr68R/WJ/2O+hb+l//OKef/gues4URbPB6hsfMB/ptD4S49jzvYSJa12/jQwjxJCL6Xhz3Qwj+4sEHN4eQX6zj5fR3uwEIcgrLPAaggGCD14s8IeQ/EfFWiGHf8bYKoZ7br+Oj0WgMMvM3o7wfzPxJ5BcreXmt564ByAa9I8h5DMByBBu8XuYR0VEQ137j0ev6fXwcfHBzSCk1E9GT/8nIL1byXA13DcCg39R/VjsE1wDkEWzw+oFHJD8Mce0Xnjwe42OOt3bt5B5Sqqu7FP9zajVnAPnFOp47e+8agGE/8R/Q7mDI874AwQavb3hE/AWIa8+L/+kYH0/kTU1NrJZS3t7hvbiuVFq1HPnFSl7RYwByQYv+vAZgOHSVIAQbvN7hDRDR+RDX3uQx88ZMJjOA8bEjT0rZbHctDBHfT0QlxM9anmsA8r56rv9owLNHEOIPXl/yRkZGlhHxlRDXnuNdMTIysgzjY3Eekfx4m9v9Xo34Wc0rhlrD5zEAWYg/eP3OU0rtQiR/DXHtmSf/XymldsH48OeVSqVhIr4x5P24CPGznhdu957HAED8wQNvurl0YqLBUqpbIK6p593sOM4+GB/heFLKQ0PEtDU+PlZH/HqE16nwI9jg9TKv0Rgfk1L9BeKaWt4dSqkS+nN7PCI+L+BUv5MRPxwRjGCD1/O8Wq26lojvhbimjvdXZnbQnzvZDUNlIt6yyP3468TE+D6IH8QfwQavL3jlspoi4gcgrqnh3V8uqyn05855zPzlhe6H4zjHIX4QfwQbvL7iEVGTiB+BuFrPe5iIDkJ/7o4nhNjfOwug78E9Bxywbi/ED+KPYIPXdzxmfjYRPwaxtpb3GDMfhv4cDY+Iz3ji/XBORPwg/gg2eH3LE0IeScTbINbW8bYJIY9Ef46Op5SqeO7Dg2vXTu2H+EH8EWzw+prHzK8h4lmItTW8WSHkq9Gfo+dJKX8gpfyHUvI/Eb/eEv/Qu/8QbPDAmz89Skd5TQDEOjnxn3+yH/pzdDzHqb1ASrmtUhmvIX49w3NL/4cuElRAsMEDb4fXAW8g4lmIdZLiL1+P/hwfr9k8cCel1KmIX0+JfzaUAfCcJ1xEsMEDb0eeUs47IdaJTfu/Af05ft4+++w9iPj1jPi75/34GwD947x++i8i2OCBtzCvUnGOhlib5THzm9D/wAOvLfEf1qf9DvqW/tc/zumn/4LnbGEEGzzwFuAppY6GWBsT/zej/4EHXlu8nP5uNwBBTmGZxwAUEGzwwPPnCSHfBrGOmyffiv4HHnht8fJaz10DkA16R5DzGIDlCDZ44IXjCcFvh1jHwxNCvg39Dzzw2uK5Gu4agEG/qf+sdgiuAcgj2OCB13bFwDcvVCcA4t/5gj+88wcPvLZ57uy9awCG/cR/QLuDIc/7AgQbPPA6OlKV/pWIt0L8u+ZtJaJXov+BB17bvKLHAOSCFv15DcBw6CpBCDZ44C340WWDH4P4d17bH+V9wQOvY55rAPK+eq7/aMCzRxDiD17bPGZ+LeK3w0zAc4i4BfFvm9ciosMx3sADr2NeMdQaPo8ByEL8weuEJ4SoEnFr1arq7ojfEz/MPE3ED0L8Q/MelFIeivEGHnhd8cLt3vMYAIg/eB3xmPnLc8mbPoT47cirVJynSynvh/gHfu8TQhyA8QYeeIZ4nQo/gg1eJpPJOI6zh2ea+y+lUmkY8duRV6tVD5JS/hXiv+j3HiJqoL+AB14yPAQHvLZ5RPTRJyZyOgrxW5g3NlafIuJbIf47fG+VUir0F/DAg/iDlxLeypUrVxDxffNKtf4uk8kMIH4L85RS+xLJX0P8t1f3+7VSal/0F/DAg/iDlyIeEX1o4aT++N5txG9HHhHtSsRXQfz5p0S0K8YbeOBB/MFLEU8ptcv8p3/P9w9KqSHEb3Feo9HIM/P/9Kv4C8GbGo1GHuMNPPAg/uCljEfEnw5I8u9A/Px5zWYzKwR/tQ+f/M9oNp94KAnGG3jgQfzBSwFPCCEWqnI3Txz+1miMr0T8Aj9LiOhT/SL+zPzJTCazBOMNPPCSE//Qu/8QbPAWePr/QRhxcBx1JuIX7kMkj3YPEepR8Z8londhvIEHXqI8t/R/6CJBBQQbvMeFiv61HXGo1arPQfzCmgAhKxXnoGq10nS/lYpzEBFNCSEm2v0S0ZQtvHK5TBhv4IGXuPhnQxkAz3nCRQQbPD31vz8R/72dJ0Mh+M/1en0nxA888MADL1Hxd8/78TcA+sd5/fRfRLDBU0oNMfPVnU0L0/mZee99cT/AAw888IyJ/7A+7XfQt/S//nFOP/0XPGcLI9h9zCOSX+nunbD8MO4HeOCBB55xXk5/txuAIKewzGMACgh2v4s/HRfFgrCgI4NxP8ADDzzwIuXltZ67BiAb9I4g5zEAyxHs/uYx83siXA2+jZlfjvsBHnjggRc7z9Vw1wAM+k39Z7VDcA1AHsHua96SHQ/6iWRr2TYiegvuB3jggQdebDx39t41AMN+4j+g3cGQ530Bgt2nvEajkReCz4l3X7n8XLPZzOJ+gAceeOBFzit6DEAuaNGf1wAMh64ShGD34j7/ChHfYKaojLx8fLyucD/AAw888CLluQYg76vn+o8GPHsEIf79yRtg5mOIuGW4otz9lYrzlmbzwJ36+X4w81OI+PZ2vlLKO6RUnq+8o12GSV69XhMYb+CBZ4RXDLWGz2MAshD//uQJIQ4gov9Nspwss7qCiOr9ej9KpepISg/2Cc2rVJxRjDfwwDPCC7d7z2MAIP59xmPm1cz8TYvEZqsQ/MVyubx3v92PdgxAWs8KUErti/ELHngW8ToVfgQ7vTylVImI/ouIt1gqNo8w82eEEHv1y/0NawDSfFBQuwYA4xc88MzxEJwe5xFRQ6/u35oSsXmUiL/QDwfJhDEAaT8lsB0DgPELHngQf/C65w0Q0QuI+NIUv2OeFYI3EdEzMpnMkl68v0EGoBeOCA5rADB+wQMP4g9eBDwp5RgRnT9XgCfV75gfJZJnOY5T7sX762cAekH8wxoAjF/wwIP4gxcxr1wurySizxLxgykT/78y80eY+Sm9fH8XMwC9Iv5hDADGL3jgQfzBi5GnlNpFH+5zr+Vicxszv7NarS7vh/u7kAHoJfEPMgAYv+CBB/EHzxCvVCoVieTxRHy/ZWJzNzO/Y3R0NNdf9+OJBqDXxN/PAGD8ggeeWfEPvfsPwe5tXrlc31NK57+klNsSFptHheBPlEqlYj/eD68B6EXxX8wAYPyCB55Rnlv6P3SRoAKC3fu8SqV+sJRyc0LicIkQQvTz/XANQK+K/0IGAOMXPPCMi382lAHwnCdcRLD7g9dsbtiFmT/azo6BLsXhESH47ZlMZglmYmr79rL4zzcAGG/ggWdc/N3zfvwNgP5xXj/9FxHs/uIJIQ4mojvjFQf5+3JZ1XA/5ni12gT1svh7DQDGG3jgGRf/YX3a76Bv6X/945x++i94zhZGsPuIp5Tal4g3xyEOQvBPlFK74H48zqvXJ2Uvi79rADDewAPPOC+nv9sNQJBTWOYxAAUEuz959Xp9JyL+acTi8F3vCn/cjzme1wD0ovgT8T8qFWcU4w088Izy8lrPXQOQDXpHkPMYgOUIdn/zSqVSkZmvjkgcZhqNxiDux4481wD0qvgT8T8ajXGJ8QYeeMZ4roa7BmDQb+o/qx2CawDyCDZ4mUwmUy6XdyPi33YpDlc1Go087sfCvFptgnpZ/KWU/5iYGHcw3sADzwjPnb13DcCwn/gPaHcw5HlfgGCD5zUB1EXRoFsdx9kD92NxXrlc27eXxX+eAcB4Aw+8eHlFjwHIBS368xqA4dBVghDsvuIRqRd1IA5by2V1IOLnzwtzHHDazwrQBgDjDTzw4ue5BiDvq+f6jwY8ewQh/uAtylNKnd2OOAjBJyB+wbxODEDa6gboNQAYb+CBFz+vGGoNn8cAZCH+4AXx1q9fu6+U6o6Q4rB5oUV/uB878to1AGksGlSpOKMYb+CBZ4QXbveexwBA/MELxatUnFeFEIFZIcQBiF84XjsGIK0VA4OOA8Z4Aw88w7xOhR/B7m+eEPyTgEp/ZyF+4XlhDUCaywW3awDQX8ADzxwPwQEvNK9cVlM+QtBi5v0Qv/C8MAYg7WcFtGMA0F/AAw/iD57FPCI+byEhYObPIH7t8YIMQC8cFBTWAKC/gAcexB88y3lCiCoRz85L9A8rpfZE/NqtuLi4AeiVUwLDGAD0F/DAg/iDlxIekbxgXqI/FfFrn7eYAeilI4KDDAD6C3jgQfzBSxGPiNZ7i/4Q0Sji1z5vIQPQS+IfZADQX8ADD+IPXgp5RHylLvpzDuLXGW++Aeg18fczAOgv4IFnVvxD7/5DsMEL+jDzi/Tiv7WIX2c8rwHoRfFfzACgv4AHnlGeW/o/dJGgAoINnt+n2Wxmieh8xK9znmsAelX8FzIA6C/ggWdc/LOhDIDnPOEigg1e0KdWqxYRv8555XJt314W//kGAOMNPPCMi7973o+/AdA/zuun/yKCDR548fKazQ27VCr1g92v49SbQjhrymU12e5XCGeN49SbtvGUUkPoL+CBl4j4D+vTfgd9S//rH+f003/Bc7Ywgg0eeOCBBx546eLl9He7AQhyCss8BqCAYIMHHnjggQde6nh5reeuAcgGvSPIeQzAcgQbPPDAAw888FLHczXcNQCDflP/We0QXAOQR7DBAw888MADL3U8d/beNQDDfuI/oN3BkOd9AYINHnjggQceeOnjFT0GIBe06M9rAIZDVwlCsMEDDzzwwAPPNp5rAPK+eq7/aMCzRxDiDx544IEHHnjp5RVDreHzGIAsxB888MADDzzwUs8Lt3vPYwAg/uD1Na9arS5XSj1NSudYpdTZUqrLlVI3Ssm3EfFfOv1KKe+UUnm+8k7w4uLRLcz8KyK+hIj/Wwh+t5Ty0Gq1uhzjAzzwdgR0JPwINni9wCuVSk9m5jcTyR8R8WO9Xo63j3mPEckfO47zrvHx+iqMD/DA6+KDYIOXZp6UcoyIzybiRyGufcd7VCl1br1eOxDjAzzwEBzw+oTHzKuJ+NsQQ/D0f/s2M6/G+AAP4o/ggNejvEajMUgkP+x94ocYgqe/jxLJDzcajUGMN/Ag/ggOeD3EU0qViPg6iCF4AbzrlFIljDfwIP4INng9wCOiZwjBf4MYgheGN9dX6BkYb+BB/BFs8FLMY+aXE/EWiCF4bfK2MPPLMd7A61XxD737D8EGL6VP/q8k4lmIIXgd8maZ+RUYb+D1GM8t/R+6SFABwQYvZeJ/OBFvhRiC1yVvq+M4R2K8gddD4p8NZQA85wkXEWzw0sKTUioivh/iBV5EvAfGxuprMd7A6wHxd8/78TcA+sd5/fRfRLDBSwOv0WjkifiXEC/wouWpGzdsOGAPjDfwUiz+w/q030Hf0v/6xzn99F/wnC2MYINnNY+IPgvxAi8eHp2C8QZeSnk5/d1uAIKcwjKPASgg2OClQPwbRLwN4gVeTLxtRNTAeAMvZby81nPXAGSD3hHkPAZgOYINXhp4zHwZxAu8OHnMfBnGG3gp4rka7hqAQb+p/6x2CK4ByCPY4KWBJ4R6JsQLPBM8IdQzMX7BSwHPnb13DcCwn/gPaHcw5HlfgGCDlwoeEf8Y4gWeId6PMX7BSwGv6DEAuaBFf14DMBy6ShCCDV7CPKWUhHiBZ5KnlJIYv+BZznMNQN5Xz/UfDXj2CEL8wUsNTwg+CeIFnkmeEHwSxi94lvOKodbweQxAFuIPXtp4RPL3EC/wTPKY+XcYv+BZzgu3e89jACD+4KVM/KkM8QIvGR6VMX7BSz2vU+FHsMFLmsfMr4d4gZcMj47C+AWvl3gIDnip4jGrMyBe4CXBE4K/jPELHsQfwQYvIZ6U6mqIF3jJ8NTVGL/gQfwRbPAS4kmp7oR4gZcMT/0F4xc8iD+CDV4CvA0b1u0spdwG8QIvId62DRvW7YzxCx7EHzzwDPMmJxsjEC/wkuRNTIzvjfELHsQfPPAM8xqNsf0hXuAlyWPm3TF+wUuj+Ife/Ydgg2cjj5l3h3iBlySvXC7vhvELXsp4bun/0EWCCgg2eLbxyuXybhAv8JLktWsAMH7Bs0D8s6EMgOc84SKCDZ5tvE4MAMQLvCh57RgAjF/wLBB/97wffwOgf5zXT/9FBBs823jtGgCIF3hR88IaAIxf8CwQ/2F92u+gb+l//eOcfvoveM4WRrDBs4bXjgGAeIEXBy+MAcD4Bc8CXk5/txuAIKewzGMACgg2eLbxwhoAiBd4cfGCDADGL3gW8PJaz10DkA16R5DzGIDlCDZ4NvLCGACIF3hx8vwMAMYveBbwXA13DcCg39R/VjsE1wDkEWzwbOUFGQCIF3hx8xYzABi/4FnAc2fvXQMw7Cf+A9odDHneFyDY4FnL8zMAEC/wTPAWMgAYv+BZwit6DEAuaNGf1wAMh64ShGCDlxBvMQMA8QLPFG++AcD4Bc8inmsA8r56rv9owLNHEOIPnvW8hQwAxAs8kzyvAcD4Bc8yXjHUGj6PAchC/MFLC4+IdiXire5XSrnD1/vf2/2CB17wl3bF+AXPUl643XseAwDxBw888MADD7x+4XUq/Ag2eOCBBx544PUGD8EBDzzwwAMPPIg/ggMeeOCBBx54EH8EGzzwwAMPPPAg/gg2eOCBBx544EH8wQMPPPDAAw88iD944IEHHnjggWej+Ife/YdggxcHr1QqPblUqo4Efcvl2r612gTV65PS/dZqE1Qu1/YN8/fggWcrb3y8vgr5ADzDPLf0f+giQQUEG7yoeUT8Y5STBa/PeZciH4BnWPyzoQyA5zzhIoINXtS8MAYAYgNej/MuRT4Az6D4u+f9+BsA/eO8fvovItjgRc0LMgAQB/B6n0c/Qj4Az5D4D+vTfgd9S//rH+f003/Bc7Ywgg1eZDw/AwBxAK8/ePRD5APwDPBy+rvdAAQ5hWUeA1BAsMGLmreYAYA4gNc/vPYMAPILeB3w8lrPXQOQDXpHkPMYgOUINnhx8BYyABAH8PqLF94AIL+A1wHP1XDXAAz6Tf1ntUNwDUAewQYvLt58AwBxAK//eOEMAPILeB3w3Nl71wAM+4n/gHYHQ573BQg2eLHxvAYA4gBef/KCDQDyC3gd8ooeA5ALWvTnNQDDoasEIdjgdchzDQDEAbz+5fkbAOQX8LrguQYg76vn+o8GPHsEIf7gxc4j4ksgDuD1N29xA4D8Al6XvGKoNXweA5CF+INniiel+gnEAbz+5i1sAJBfwIuAF273nscAQPzBM8aTUl0GcQCvv3k7GgDkF/CM8joVfgQbvG54XgMAcQCvP3lPNADIL+AlyUNwwDPGcw0AxAG8/uU9bgCQX8CD+IPXNzwp1U8gDuD1N2/OACAfgAfxB6+veER8CcQBvP7m0Q+RD8CD+IPXd7wwxwFDbMDrbR79CPkAPIg/eH3H68QAQGzA6zHepcgH4EH8wes7XrsGAGIDXg/yLkU+AC8J8Q+9+w/BBi8OXjsGAGIDXo/yLkU+AM8wzy39H7pIUAHBBi9qXlgDALEBr4d5lyIfgGdY/LOhDIDnPOEigg1e1LwwBgBiA16P8y5FPgDPoPi75/34GwD947x++i8i2OBFzQsyABAH8HqfRz9CPgDPkPgP69N+B31L/+sf5/TTf8FztjCCDV5kPD8DAHEArz94/scBI7+AFxEvp7/bDUCQU1jmMQAFBBu8qHmLGQCIA3j9w2vPACC/gNcBL6/13DUA2aB3BDmPAViOYIMXB28hAwBxAK+/eOENAPILeB3wXA13DcCg39R/VjsE1wDkEWzw4uLNNwAQB/D6jxfOACC/gNcBz529dw3AsJ/4D2h3MOR5X4Bggxcbz2sAIA7g9Scv2AAgv4DXIa/oMQC5oEV/XgMwHLpKEIINHnjggQceeLbxXAOQ99Vz/UcDnj2CEH/wwAMPPPDASy+vGGoNn8cAZCH+4IEHHnjggZd6Xrjdex4DAPEHDzzwwAMPvH7hdSr8CDZ44IEHHnjg9QYPwQEPPPDAAw88iD+CAx544IEHHngQ/yf+494zAooRlAsGDzzwwAMPPPAM8jr5x71nBBQiKBcMHnjggQceeOAZ5HXyj+c99YWXR1AuGDzwwAMPPPDAM8hr9x9f4jkjYJnncIEl4IEHHnjggQdeOngus51/fNhzRkCuy3LB4IEHHnjggQdeMryBsEWClnjOCHC/g13+4+CBBx544IEHnnleNpQB8Px40PPNRvCPgwceeOCBBx54yfBCGYCB+d9MFx/wwAMPPPDAA88K3pIgt7DU813S5T8OHnjggQceeOBZwvv/TmcqXxWmLEoAAAAASUVORK5CYII=");
                    var image = new MemoryStream(bytes);
                    Image imgStream = Image.FromStream(image);
                    pic.Image = imgStream;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Location = new System.Drawing.Point(20, 28);
                    pic.Size = new System.Drawing.Size(180, 150);
                    pic.Name = "pic_" + tableLayoutPanel8.RowCount;
                    pic.TabIndex = 3;
                    pic.TabStop = false;

                    Label lb9 = new Label();
                    Label lb11 = new Label();
                    Label lb12 = new Label();
                    Label lb13 = new Label();
                    Label lb14 = new Label();
                    Label lb15 = new Label();
                    Label lb16 = new Label();

                    lb16.AutoSize = true;
                    lb16.Location = new System.Drawing.Point(265, 120);
                    lb16.TabIndex = 3;
                    lb16.MaximumSize = new System.Drawing.Size(300, 20);

                    lb15.AutoSize = true;
                    lb15.Location = new System.Drawing.Point(205, 120);
                    lb15.TabIndex = 3;
                    lb15.MaximumSize = new System.Drawing.Size(265, 20);

                    lb14.AutoSize = true;
                    lb14.Location = new System.Drawing.Point(265, 100);
                    lb14.TabIndex = 3;
                    lb14.MaximumSize = new System.Drawing.Size(300, 20);

                    lb13.AutoSize = true;
                    lb13.Location = new System.Drawing.Point(205, 100);
                    lb13.TabIndex = 3;
                    lb13.MaximumSize = new System.Drawing.Size(60, 20);

                    lb12.AutoSize = true;
                    lb12.Location = new System.Drawing.Point(255, 80);
                    lb12.TabIndex = 3;
                    lb12.MaximumSize = new System.Drawing.Size(300, 20);

                    lb11.AutoSize = true;
                    lb11.Location = new System.Drawing.Point(205, 80);
                    lb11.TabIndex = 3;
                    lb11.MaximumSize = new System.Drawing.Size(50, 20);

                    lb9.AutoSize = true;
                    lb9.Font = new Font(lb9.Font, FontStyle.Bold);
                    lb9.Location = new System.Drawing.Point(205, 60);
                    lb9.MaximumSize = new System.Drawing.Size(500, 20);
                    lb9.TabIndex = 1;

                    lb9.Name = "label9_" + tableLayoutPanel8.RowCount;
                    lb11.Name = "label11_" + tableLayoutPanel8.RowCount;
                    lb12.Name = "label12_" + tableLayoutPanel8.RowCount;
                    lb13.Name = "label13_" + tableLayoutPanel8.RowCount;
                    lb14.Name = "label14_" + tableLayoutPanel8.RowCount;
                    lb15.Name = "label14_" + tableLayoutPanel8.RowCount;
                    lb16.Name = "label16_" + tableLayoutPanel8.RowCount;


                    lb9.Text = reader["TournmentID"].ToString() + " - " + reader["Name"].ToString();
                    lb11.Text = "Game:";
                    lb12.Text = reader["Title"].ToString();
                    lb13.Text = "Prize Pool:";
                    lb14.Text = reader["PrizePool"].ToString();
                    lb15.Text = "Location:";
                    lb16.Text = reader["Location"].ToString();

                    x.Controls.Add(pic);
                    x.Controls.Add(lb9);
                    x.Controls.Add(lb11);
                    x.Controls.Add(lb12);
                    x.Controls.Add(lb13);
                    x.Controls.Add(lb14);
                    x.Controls.Add(lb15);
                    x.Controls.Add(lb16);

                    tableLayoutPanel8.Controls.Add(x, 0, tableLayoutPanel8.RowCount - 1);
                    nTournments++;
                }
            }
            if (nTournments < pageSize)
            {
                button16.Enabled = false;
            }

            cn.Close();
        }


        private void button18_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            button15.Enabled = false;
            string option = comboBox3.Text;
            string title = textBox10.Text;
            string name = textBox9.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "None";
            }
            filter_tournments(pageNumber, option, name, title);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            pageNumber++;
            string option = comboBox3.Text;
            string title = textBox10.Text;
            string name = textBox9.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "None";
            }
            filter_tournments(pageNumber, option, name, title);

            if (pageNumber > 1)
            {
                button15.Enabled = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            pageNumber--;
            string option = comboBox3.Text;
            string title = textBox10.Text;
            string name = textBox9.Text;
            if (string.IsNullOrEmpty(title))
            {
                title = "None";
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "None";
            }
            filter_tournments(pageNumber, option, name, title);

            if (pageNumber == 1)
            {
                button15.Enabled = false;
            }
        }
    }
}
