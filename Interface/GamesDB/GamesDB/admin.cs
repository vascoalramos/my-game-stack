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
    public partial class admin : Form
    {
        private SqlConnection cn;
        public admin()
        {
            InitializeComponent();
        }

        
        private void admin_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Admin Loaded!!!!");
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

        private void button2_Click(object sender, EventArgs e)
        {
            string userName = richTextBox1.Text;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("UserName has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.promoteToAdmin"
                };
                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@username"].Value = userName;
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Promote To Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox1.Text = "";
                cn.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string userName = richTextBox2.Text;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("UserName has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeUser"
                };
                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@username"].Value = userName;
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox2.Text = "";
                cn.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string gameID = richTextBox3.Text;
            if (string.IsNullOrEmpty(gameID))
            {
                MessageBox.Show("Game ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeGame"
                };
                cmd.Parameters.Add(new SqlParameter("@gameID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@gameID"].Value = int.Parse(gameID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox3.Text = "";
                cn.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string franID = richTextBox4.Text;
            if (string.IsNullOrEmpty(franID))
            {
                MessageBox.Show("Franchise ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeFranchise"
                };
                cmd.Parameters.Add(new SqlParameter("@franID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@franID"].Value = int.Parse(franID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Franchise", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox4.Text = "";
                cn.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string devID = richTextBox5.Text;
            if (string.IsNullOrEmpty(devID))
            {
                MessageBox.Show("Developer ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeDeveloper"
                };
                cmd.Parameters.Add(new SqlParameter("@devID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@devID"].Value = int.Parse(devID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Developer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox5.Text = "";
                cn.Close();
            }
        }


        private void button11_Click(object sender, EventArgs e)
        {
            string pubID = richTextBox6.Text;
            if (string.IsNullOrEmpty(pubID))
            {
                MessageBox.Show("Publisher ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removePublisher"
                };
                cmd.Parameters.Add(new SqlParameter("@pubID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@pubID"].Value = int.Parse(pubID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox6.Text = "";
                cn.Close();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string tourID = richTextBox7.Text;
            if (string.IsNullOrEmpty(tourID))
            {
                MessageBox.Show("Tournment ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeTournment"
                };
                cmd.Parameters.Add(new SqlParameter("@tourID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@tourID"].Value = int.Parse(tourID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Tournment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox7.Text = "";
                cn.Close();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string genreID = richTextBox8.Text;
            if (string.IsNullOrEmpty(genreID))
            {
                MessageBox.Show("Genre ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removeGenre"
                };
                cmd.Parameters.Add(new SqlParameter("@genreID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@genreID"].Value = int.Parse(genreID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Genre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox8.Text = "";
                cn.Close();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string platID = richTextBox9.Text;
            if (string.IsNullOrEmpty(platID))
            {
                MessageBox.Show("Platform ID has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.removePlatform"
                };
                cmd.Parameters.Add(new SqlParameter("@platID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@platID"].Value = int.Parse(platID);
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value);
                }
                else
                {
                    MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Remove Platform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                richTextBox9.Text = "";
                cn.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = true;
        }

        //Register Genre
        private void button_regist_user_Click(object sender, EventArgs e)
        {      
            string genreName = textBox_userName.Text;

            if (string.IsNullOrEmpty(genreName))
            {
                MessageBox.Show("Genre Name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddGenre"
                };

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@Name"].Value = genreName;
                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully created new genre: " + genreName);
                    panel1.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error: Genre name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private void button20_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
        }

        //Register Developer
        private void button_regist_developer_Click(object sender, EventArgs e)
        {
            string mail = textBox_email.Text;
            string dev_name = textBox1.Text;
            string phone = textBox_fName.Text;
            string picturePath = textBox_photo.Text;
            string picture = "";
            string website = textBox_lName.Text;
            string city = textBox3.Text;
            string country = textBox2.Text;

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


            if (string.IsNullOrEmpty(dev_name))
            {
                MessageBox.Show("Developer name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddDeveloper"
                };
                cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@dev_name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@website", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@mail"].Value = mail;
                cmd.Parameters["@dev_name"].Value = dev_name;
                cmd.Parameters["@phone"].Value = phone;
                cmd.Parameters["@photo"].Value = picture;
                cmd.Parameters["@website"].Value = website;
                cmd.Parameters["@city"].Value = city;
                cmd.Parameters["@country"].Value = country;

                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new developer: " + dev_name);
                    panel2.Visible = false;
                }
                else
                {
                    MessageBox.Show("Developer Name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = true;
        }

        private void button_loadImage_Developer(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox_photo.Text = fileName;

            }
        }

        //Register Publisher
        private void button_regist_Publisher_Click(object sender, EventArgs e)
        {
            string mail = textBox8.Text;
            string pub_name = textBox9.Text;
            string phone = textBox7.Text;
            string picturePath = textBox5.Text;
            string picture = "";
            string website = textBox6.Text;
            string city = textBox10.Text;
            string country = textBox4.Text;

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


            if (string.IsNullOrEmpty(pub_name))
            {
                MessageBox.Show("Publisher name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddPublisher"
                };
                cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@pub_name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@website", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@mail"].Value = mail;
                cmd.Parameters["@pub_name"].Value = pub_name;
                cmd.Parameters["@phone"].Value = phone;
                cmd.Parameters["@photo"].Value = picture;
                cmd.Parameters["@website"].Value = website;
                cmd.Parameters["@city"].Value = city;
                cmd.Parameters["@country"].Value = country;

                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new publisher: " + pub_name);
                    panel3.Visible = false;
                }
                else
                {
                    MessageBox.Show("Publisher Name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = true;
        }

        private void button_loadImage_ClickPublisher(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox5.Text = fileName;

            }
        }


        private void button23_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
        }


        //Register Platform

        private void button_regist_platfrom_Click(object sender, EventArgs e)
        {
            string platformName = textBox16.Text;
            string owner = textBox15.Text;
            string releaseDate = textBox14.Text;

            Boolean error = false;


            if (!string.IsNullOrEmpty(releaseDate))
            {
                Boolean errorFound = false;
                if (releaseDate.Split('-').Length < 3)
                {
                    MessageBox.Show("Invalid date! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }
                else if (!errorFound && (releaseDate.Split('-')[0].Length < 4 || Convert.ToInt16(releaseDate.Split('-')[0]) < 0))
                {
                    MessageBox.Show("Invalid date: invalid YEAR! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }

                else if (!errorFound && (releaseDate.Split('-')[1].Length < 2 || Convert.ToInt16(releaseDate.Split('-')[1]) < 1 || Convert.ToInt16(releaseDate.Split('-')[1]) > 12))
                {
                    MessageBox.Show("Invalid date: invalid MONTH! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }

                else if (!errorFound && releaseDate.Split('-')[2].Length < 2)
                {
                    MessageBox.Show("Invalid date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }
                else
                {
                    int month = Convert.ToInt16(releaseDate.Split('-')[1]);

                    if (!errorFound && (releaseDate.Split('-')[2].Length < 2 || Convert.ToInt16(releaseDate.Split('-')[2]) < 1 || Convert.ToInt16(releaseDate.Split('-')[1]) > 12))
                    {
                        MessageBox.Show("Invalid date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!errorFound && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && Convert.ToInt16(releaseDate.Split('-')[2]) > 31)
                    {

                        MessageBox.Show("Invalid date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else if (!errorFound && (month == 4 || month == 6 || month == 9 || month == 11) && Convert.ToInt16(releaseDate.Split('-')[2]) > 30)
                    {
                        MessageBox.Show("Invalid date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else if (!errorFound && month == 4 && Convert.ToInt16(releaseDate.Split('-')[2]) > 28)
                        MessageBox.Show("Invalid date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if(errorFound)
                        error = true;

                }

            }

            if (string.IsNullOrEmpty(platformName))
            {
                MessageBox.Show("Platform Name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (!error)
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddPlatform"
                };

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@Owner", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@ReleaseDate", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@Name"].Value = platformName;
                cmd.Parameters["@Owner"].Value = owner;
                cmd.Parameters["@ReleaseDate"].Value = releaseDate;

                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new platform: " + platformName);
                    panel5.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error: Platform name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void admin_Load_1(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }


        //Franchise

        private void button4_Click(object sender, EventArgs e)
        {
            panel7.Visible = true;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }

        private void button_loadImage_Click_Franchise(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox12.Text = fileName;

            }
        }


        private void button_regist_franchise_Click(object sender, EventArgs e)
        {
            string franchiseName = textBox19.Text;
            string picturePath = textBox12.Text;
            string picture = "";

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

                if (string.IsNullOrEmpty(franchiseName))
            {
                MessageBox.Show("Franchise Name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddFranchise"
                };

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@Name"].Value = franchiseName;
                cmd.Parameters["@photo"].Value = picture;

                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new franchise: " + franchiseName);
                    panel7.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error: Franchise name already exists", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }
        }


        //Tournment
        private void button31_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel9.Visible = true;
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("select GameID, Title from GamesDB.Games", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["GameID"].ToString();
                        String name = reader["Title"].ToString();
                        comboBox7.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }
            cn.Close();
        }

        private void regist_tournment(object sender, EventArgs e)
        {
            string tourn_name = textBox21.Text;
            string prize = textBox20.Text;
            string location = textBox18.Text;
            string start_date = textBox17.Text;
            string end_date = textBox13.Text;
            string game_title = comboBox7.Text;

            Boolean error = false;

            if (string.IsNullOrEmpty(tourn_name))
            {
                MessageBox.Show("Tournment name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (!string.IsNullOrEmpty(prize))
            {
                try
                {
                    Convert.ToInt64(prize);
                }
                catch
                {
                    MessageBox.Show("Prize pool has to be a number!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }
            }

            if (string.IsNullOrEmpty(game_title) || game_title == "None")
            {
                MessageBox.Show("Tournment's game has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (!string.IsNullOrEmpty(end_date))
            {
                Boolean errorFound2 = false;
                if (end_date.Split('-').Length < 3)
                {
                    MessageBox.Show("Invalid end date! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound2 = true;
                }
                else if (!errorFound2 && (end_date.Split('-')[0].Length < 4 || Convert.ToInt16(end_date.Split('-')[0]) < 0))
                {
                    MessageBox.Show("Invalid end date: invalid YEAR! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound2 = true;
                }

                else if (!errorFound2 && (end_date.Split('-')[1].Length < 2 || Convert.ToInt16(end_date.Split('-')[1]) < 1 || Convert.ToInt16(end_date.Split('-')[1]) > 12))
                {
                    MessageBox.Show("Invalid end date: invalid MONTH! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound2 = true;
                }

                else if (!errorFound2 && end_date.Split('-')[2].Length < 2)
                {
                    MessageBox.Show("Invalid end date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound2 = true;
                }
                else
                {
                    int month = Convert.ToInt16(end_date.Split('-')[1]);

                    if (!errorFound2 && (end_date.Split('-')[2].Length < 2 || Convert.ToInt16(end_date.Split('-')[2]) < 1 || Convert.ToInt16(end_date.Split('-')[1]) > 12))
                    {
                        MessageBox.Show("Invalid end date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!errorFound2 && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && Convert.ToInt16(end_date.Split('-')[2]) > 31)
                    {

                        MessageBox.Show("Invalid end date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound2 = true;
                    }
                    else if (!errorFound2 && (month == 4 || month == 6 || month == 9 || month == 11) && Convert.ToInt16(end_date.Split('-')[2]) > 30)
                    {
                        MessageBox.Show("Invalid end date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound2 = true;
                    }
                    else if (!errorFound2 && month == 4 && Convert.ToInt16(end_date.Split('-')[2]) > 28)
                        MessageBox.Show("Invalid end date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (errorFound2)
                        error = true;

                }
            }

            if (!string.IsNullOrEmpty(start_date))
                {
                Boolean errorFound = false;
                    if (start_date.Split('-').Length < 3)
                    {
                        MessageBox.Show("Invalid start date! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else if (!errorFound && (start_date.Split('-')[0].Length < 4 || Convert.ToInt16(start_date.Split('-')[0]) < 0))
                    {
                        MessageBox.Show("Invalid start date: invalid YEAR! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }

                    else if (!errorFound && (start_date.Split('-')[1].Length < 2 || Convert.ToInt16(start_date.Split('-')[1]) < 1 || Convert.ToInt16(start_date.Split('-')[1]) > 12))
                    {
                        MessageBox.Show("Invalid start date: invalid MONTH! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }

                    else if (!errorFound && start_date.Split('-')[2].Length < 2)
                    {
                        MessageBox.Show("Invalid start date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else
                    {
                        int month = Convert.ToInt16(start_date.Split('-')[1]);

                        if (!errorFound && (start_date.Split('-')[2].Length < 2 || Convert.ToInt16(start_date.Split('-')[2]) < 1 || Convert.ToInt16(start_date.Split('-')[1]) > 12))
                        {
                            MessageBox.Show("Invalid start date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (!errorFound && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && Convert.ToInt16(start_date.Split('-')[2]) > 31)
                        {

                            MessageBox.Show("Invalid start date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errorFound = true;
                        }
                        else if (!errorFound && (month == 4 || month == 6 || month == 9 || month == 11) && Convert.ToInt16(start_date.Split('-')[2]) > 30)
                        {
                            MessageBox.Show("Invalid start date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errorFound = true;
                        }
                        else if (!errorFound && month == 4 && Convert.ToInt16(start_date.Split('-')[2]) > 28)
                            MessageBox.Show("Invalid start date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        if (errorFound)
                            error = true;
                    }
                }

            if (!error)
            {
                game_title = game_title.Split('-')[0].ToString();
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddTournment"
                };
                cmd.Parameters.Add(new SqlParameter("@tourn_name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@prize", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@location", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@start_date", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@end_date", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@game_title", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters["@tourn_name"].Value = tourn_name;
                cmd.Parameters["@prize"].Value = prize;
                cmd.Parameters["@location"].Value = location;
                cmd.Parameters["@start_date"].Value = start_date;
                cmd.Parameters["@end_date"].Value = end_date;
                cmd.Parameters["@game_title"].Value = game_title;

                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new tournment: " + tourn_name);
                    panel9.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error registering tournment!", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }
        }


        //Game

        private void button_loadImage_Click_Games(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox22.Text = fileName;

            }
        }


        private void button33_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel11.Visible = true;
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("select PublisherID, Name from GamesDB.Publishers", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["PublisherID"].ToString();
                        String name = reader["Name"].ToString();
                        comboBox2.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select DeveloperID, Name from GamesDB.Developers", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["DeveloperID"].ToString();
                        String name = reader["Name"].ToString();
                        comboBox3.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select GenreID, Name from GamesDB.Genres order by GenreID", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["GenreID"].ToString();
                        String name = reader["Name"].ToString();
                        comboBox4.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select PlatformID, Name from GamesDB.Platforms order by PlatformID", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["PlatformID"].ToString();
                        String name = reader["Name"].ToString();
                        comboBox6.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select FranchiseID, Name from GamesDB.Franchises", cn);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String id = reader["FranchiseID"].ToString();
                        String name = reader["Name"].ToString();
                        comboBox5.Items.Add(id + " - " + name);
                    }
                }
            }
            catch { }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text != "None"){
                textBox27.Text = textBox27.Text + comboBox3.Text + " ; ";
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text != "None")
            {
                textBox23.Text = textBox23.Text + comboBox4.Text + " ; ";
            }
        }


        private void button34_Click(object sender, EventArgs e)
        {
            textBox27.Text = "";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            textBox23.Text = "";
        }



        private void button_regist_Game(object sender, EventArgs e)
        {
            string game_name = textBox26.Text;
            string launch_date = textBox25.Text;
            string publisher = comboBox2.Text;
            string picturePath = textBox22.Text;
            string picture = "";
            string developers = textBox27.Text;
            string genres = textBox23.Text;
            string franchise = comboBox5.Text;
            string description = textBox24.Text;
            string platforms = textBox11.Text;


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

            Boolean error = false;
            if (string.IsNullOrEmpty(game_name))
            {
                MessageBox.Show("Game name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (string.IsNullOrEmpty(publisher))
            {
                MessageBox.Show("Publisher has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (string.IsNullOrEmpty(developers))
            {
                MessageBox.Show("Developer(s) has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;

            }

            if (string.IsNullOrEmpty(genres))
            {
                MessageBox.Show("Genre has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                error = true;
            }

            if (!string.IsNullOrEmpty(launch_date))
            {
                Boolean errorFound = false;
                if (launch_date.Split('-').Length < 3)
                {
                    MessageBox.Show("Invalid launch date! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }
                else if (!errorFound && (launch_date.Split('-')[0].Length < 4 || Convert.ToInt16(launch_date.Split('-')[0]) < 0))
                {
                    MessageBox.Show("Invalid launch date: invalid YEAR! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }

                else if (!errorFound && (launch_date.Split('-')[1].Length < 2 || Convert.ToInt16(launch_date.Split('-')[1]) < 1 || Convert.ToInt16(launch_date.Split('-')[1]) > 12))
                {
                    MessageBox.Show("Invalid launch date: invalid MONTH! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }

                else if (!errorFound && launch_date.Split('-')[2].Length < 2)
                {
                    MessageBox.Show("Invalid launch date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorFound = true;
                }
                else
                {
                    int month = Convert.ToInt16(launch_date.Split('-')[1]);

                    if (!errorFound && (launch_date.Split('-')[2].Length < 2 || Convert.ToInt16(launch_date.Split('-')[2]) < 1 || Convert.ToInt16(launch_date.Split('-')[1]) > 12))
                    {
                        MessageBox.Show("Invalid launch date: invalid DAY! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!errorFound && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && Convert.ToInt16(launch_date.Split('-')[2]) > 31)
                    {

                        MessageBox.Show("Invalid launch date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else if (!errorFound && (month == 4 || month == 6 || month == 9 || month == 11) && Convert.ToInt16(launch_date.Split('-')[2]) > 30)
                    {
                        MessageBox.Show("Invalid launch date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorFound = true;
                    }
                    else if (!errorFound && month == 4 && Convert.ToInt16(launch_date.Split('-')[2]) > 28)
                        MessageBox.Show("Invalid launch date: invalid DAY (month doesn't have that many days)! Format should be: yyyy-mm-dd", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (errorFound)
                        error = true;
                }
            }


            if (string.IsNullOrEmpty(platforms))
            {
                MessageBox.Show("Platform has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;

            }


            if (!error)
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddGame"
                };

                String temp = "";
                Debug.WriteLine("OIOIOIOI\n\n" + textBox27.Text);

                for (int i = 0; i < developers.Split(';').Length; i++)
                {
                    String dev = developers.Split(';')[i].Split('-')[0].Replace(" ", "");
                    if (dev.Length > 0)
                        temp = temp + dev + ";";
                }
                developers = temp;
                Debug.WriteLine("OIOIOIOI\n\n" + developers);

                temp = "";
                for (int i = 0; i < genres.Split(';').Length; i++)
                {
                    String gen = genres.Split(';')[i].Split('-')[0].Replace(" ", "");
                    if (gen.Length > 0)
                        temp = temp + gen + ";";
                }
                genres = temp;
                Debug.WriteLine("OIOIOIOI\n\n" + genres);

                temp = "";
                for (int i = 0; i < platforms.Split(';').Length; i++)
                {
                    String plat = platforms.Split(';')[i].Split('-')[0].Replace(" ", "");
                    if (plat.Length > 0)
                        plat = temp + plat + ";";
                }
                platforms = temp;

                if (string.IsNullOrEmpty(franchise))
                {
                    franchise = "None";
                }

                cmd.Parameters.Add(new SqlParameter("@game_name", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@launch_date", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@publisher", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@franchise", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@developers", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@genres", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@platforms", SqlDbType.VarChar));

                cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                cmd.Parameters.Add(new SqlParameter("@addedGameID", SqlDbType.Int, 250));

                cmd.Parameters["@game_name"].Value = game_name;
                cmd.Parameters["@launch_date"].Value = launch_date;
                cmd.Parameters["@publisher"].Value = publisher.Split('-')[0];
                cmd.Parameters["@photo"].Value = picture;
                cmd.Parameters["@developers"].Value = developers;
                cmd.Parameters["@genres"].Value = genres;
                cmd.Parameters["@platforms"].Value = platforms;
                cmd.Parameters["@description"].Value = description;

                if (franchise != "None")
                {
                    cmd.Parameters["@franchise"].Value = franchise.Split('-')[0];
                }
                else
                {
                    cmd.Parameters["@franchise"].Value = franchise;
                }


                cmd.Parameters["@responseMsg"].Direction = ParameterDirection.Output;
                cmd.Parameters["@addedGameID"].Direction = ParameterDirection.Output;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                if ("" + cmd.Parameters["@responseMsg"].Value == "Success")
                {
                    MessageBox.Show("Successfully registered new game: " + game_name);
                    panel3.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error registering game", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
                panel11.Visible = false;

            }
        
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if(comboBox6.Text != "None")
            {
                textBox11.Text = textBox23.Text + comboBox6.Text + " ; ";
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            textBox11.Text = "";
        }
    }
}
