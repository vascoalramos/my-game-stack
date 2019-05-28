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

        public Form2(int gameID, Image img)
        {
            InitializeComponent();

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
    }
}
