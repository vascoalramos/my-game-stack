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
    }
}
