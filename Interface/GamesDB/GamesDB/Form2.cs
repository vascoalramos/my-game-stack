using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamesDB
{
    public partial class Form2 : Form
    {
        private SqlConnection cn;
        private int gameID;
        private String current_user = "";

        public Form2(int gameID, Image img, String user)
        {
            this.current_user = user;
            InitializeComponent();
            this.gameID = gameID;
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd;
            cmd = new SqlCommand("select * from GamesDB.gameInfo ('" + gameID + "')", cn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    label10.Text = reader["Title"].ToString();
                    label11.Text = reader["LauchDate"].ToString().Split(' ')[0];
                    label12.Text = reader["Description"].ToString();
                    label14.Text = reader["PubName"].ToString();
                    pictureBox1.Image = img;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

            cmd = new SqlCommand("select GamesDB.gameAvgScore ('" + gameID + "')", cn);
            double valor = (double)cmd.ExecuteScalar();
            label23.Text = valor.ToString() + " \u2605";

            cmd = new SqlCommand("select GamesDB.gameFranchise ('" + gameID + "')", cn);
            label13.Text = cmd.ExecuteScalar().ToString();

            cmd = new SqlCommand("select * from GamesDB.gameReleases ('" + gameID + "')", cn);
            label25.Text = "";
            Boolean first = true;
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (first)
                        {
                            label25.Text += reader["Name"].ToString();
                            first = false;
                        }
                        else
                            label25.Text += ", " + reader["Name"].ToString();
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select * from GamesDB.gameTournments ('" + gameID + "')", cn);
            label16.Text = "";
            first = true;
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (first)
                        {
                            label16.Text += reader["Name"].ToString();
                            first = false;
                        }
                        else
                            label16.Text += ", " + reader["Name"].ToString();
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select * from GamesDB.gameDevelopers ('" + gameID + "')", cn);
            label15.Text = "";
            first = true;
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (first)
                        {
                            label15.Text += reader["Name"].ToString();
                            first = false;
                        }
                        else
                            label15.Text += ", " + reader["Name"].ToString();
                    }
                }
            }
            catch { }

            cmd = new SqlCommand("select * from GamesDB.gameGenres ('" + gameID + "')", cn);
            label21.Text = "";
            first = true;
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (first)
                        {
                            label21.Text += reader["Name"].ToString();
                            first = false;
                        }
                        else
                            label21.Text += ", " + reader["Name"].ToString();
                    }
                }
            }
            catch { }

            cn.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 new_form = new Form3(this.gameID, this.current_user);
            new_form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string list = comboBox1.Text.ToString();
            if (string.IsNullOrEmpty(list))
            {
                MessageBox.Show("Please Select a List");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select GamesDB.checkGameInList ('" + current_user + "', " + gameID + ", '" + list + "')", cn);
                if (!verifySGBDConnection())
                    return;
                int valor = (int)cmd.ExecuteScalar();
                if (valor == 1)
                {
                    MessageBox.Show("This game already is in your '" + list + "' List!");
                }
                else
                {
                    cmd = new SqlCommand
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "GamesDB.uspAddGameToList"
                    };
                    cmd.Parameters.Add(new SqlParameter("@gameID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@listName", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@responseMsg", SqlDbType.NVarChar, 250));
                    cmd.Parameters["@gameID"].Value = gameID;
                    cmd.Parameters["@userName"].Value = current_user;
                    cmd.Parameters["@listName"].Value = list;
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
                        MessageBox.Show("" + cmd.Parameters["@responseMsg"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                cn.Close();
            }
        }
    }
}
