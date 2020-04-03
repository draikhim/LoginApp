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
        public Admin()
        {
            InitializeComponent();
        }        
        
        // App exits when this form closed
        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.View = View.Details;

            using (var db = new LoginContext())
            {
                var users = db.Users;

                foreach (var user in users)
                {
                    string[] row = { user.Id.ToString(), user.Email, user.Type };

                    listView1.Items.Add(new ListViewItem(row));

                    // Autosize based on column content size 
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }

                // Autosize based on column header size
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string email = listView1.SelectedItems[0].SubItems[1].Text;

            if(MessageBox.Show("Are you sure you want to delete this record?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)            {
                using (var db = new LoginContext())
                {
                    var user = db.Users
                          .Where(u => u.Email == email)
                          .FirstOrDefault<User>();

                    if (user.Email == email)
                    {
                        // Removes user from listview only
                        listView1.SelectedItems[0].Remove();

                        db.Users.Remove(user);                       
                        db.SaveChanges();
                        MessageBox.Show("User " + email + " deleted successfully!");
                    }
                }
            }                
        }

        // Enables and disables update and delete button based on user selection
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                btnDelete.Enabled = true;
                btnToUpdateForm.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnToUpdateForm.Enabled = false;
            }
        }

        // Takes to Update form
        private void btnToUpdateForm_Click(object sender, EventArgs e)
        {       
            Update update = new Update();
            update.admin = this;
            update.Show();
        }

        // Takes to Create User Form
        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            User newUser = new User();
            newUser.admin = this;
            newUser.Show();            
        }
    }
}
