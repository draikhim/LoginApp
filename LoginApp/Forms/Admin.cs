using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginApp.Forms
{
    public partial class Admin : Form
    {
        // show listview of users with email and user type (edit button on right side)
        // Move update logic from New User form to here

        public Admin()
        {
            InitializeComponent();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            this.Hide();

            User user = new User();
            user.Show();
        }


        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            using (var db = new LoginContext())
            {
                
        
               
            }
        }
    }
}
