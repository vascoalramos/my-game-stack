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
    public partial class Form4 : Form
    {
        private SqlConnection cn;
        private String current_user = "";
        private int gameID;

        public Form4(String user, int id)
        {
            InitializeComponent();
            this.current_user = user;
            this.gameID = id;
            load_review();
  
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

        private void load_review()
        {
            SqlCommand cmd;
            cmd = new SqlCommand("select * from GamesDB.getGameUserReview ('" + current_user + "', '" + gameID + "')", cn);
            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    comboBox1.Text = reader["Score"].ToString();
                    textBox1.Text = reader["Title"].ToString();
                    richTextBox1.Text = reader["Description"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text;
            string score = comboBox1.Text;
            string description = richTextBox1.Text;

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Description has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Title has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(score))
            {
                MessageBox.Show("Score has to be defined!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GamesDB.uspAddReview"
                };
                cmd.Parameters.Add(new SqlParameter("@score", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@gameID", SqlDbType.Int));
                cmd.Parameters["@score"].Value = score;
                cmd.Parameters["@title"].Value = title;
                cmd.Parameters["@description"].Value = description;
                cmd.Parameters["@UserName"].Value = current_user;
                cmd.Parameters["@gameID"].Value = gameID;

                if (!verifySGBDConnection())
                    return;
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

                cn.Close();
            }
        }
    }
}
