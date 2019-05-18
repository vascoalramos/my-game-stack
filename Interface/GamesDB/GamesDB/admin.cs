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
    }
}
