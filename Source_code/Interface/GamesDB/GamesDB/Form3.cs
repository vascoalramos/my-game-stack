using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamesDB
{
    public partial class Form3 : Form
    {
        private SqlConnection cn;
        private String current_user = "";
        private int gameID;


        public Form3(int gameID, String user)
        {
            InitializeComponent();
            this.current_user = user;
            this.gameID = gameID;
            load_reviews(gameID);
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

        private void load_reviews(int gameID)
        {
            SqlCommand cmd;
            cmd = new SqlCommand("select * from GamesDB.gameReviews ('" + gameID + "')", cn);
            if (!verifySGBDConnection())
                return;
            cmd.Connection = cn;

            listView1.Items.Clear();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                
                while (reader.Read())
                {
                    string userName = reader["UserName"].ToString();
                    string title = reader["Title"].ToString();
                    string score = reader["Score"].ToString();
                    string description = reader["Description"].ToString();
                    string date = reader["Date"].ToString().Split(' ')[0].ToString();
                    var row = new string[] { userName, title, score, description, date };
                    var lvi = new ListViewItem(row);
                    listView1.View = View.Details;
                    listView1.Items.Add(lvi);
                }
            }
            cmd = new SqlCommand("select GamesDB.checkGameInUser ('" + current_user + "', '" + gameID + "')", cn);
            int valor = (int)cmd.ExecuteScalar();
            if (valor == 1)
            {
                button1.Enabled = true;
            }
            
            cn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 newForm = new Form4(this.current_user, this.gameID);
            newForm.ShowDialog();
        }
    }
}
