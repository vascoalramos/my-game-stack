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

        private void button_loadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFile.FileName;
                textBox_photo.Text = fileName;

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = true;
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

            if (string.IsNullOrEmpty(platformName))
            {
                MessageBox.Show("Genre Name has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (string.IsNullOrEmpty(owner))
            {
                MessageBox.Show("Platform manufactor has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (string.IsNullOrEmpty(releaseDate))
            {
                MessageBox.Show("Release Date has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
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
    }

}
