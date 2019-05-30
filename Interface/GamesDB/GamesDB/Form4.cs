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

        public Form4(String user)
        {
            InitializeComponent();
            this.current_user = user;
        }
    }
}
