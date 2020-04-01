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
    public partial class Update : Form
    {
        public Admin admin;

        public Update()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {           
            using (var db = new LoginContext())
            {
                // Calling method from New User form
                User newUser = new User();
                var encryptedPassword = newUser.Encrypt(txtPassword.Text);

                var result = db.Users.First(u => u.Email == txtEmail.Text);

                if (result != null)
                {
                    result.Email = txtEmail.Text.ToLower();
                    result.Password = encryptedPassword;
                    result.Type = txtUserType.Text.First().ToString().ToUpper() + txtUserType.Text.Substring(1).ToLower();

                    db.SaveChanges();
                    MessageBox.Show("User updated successfully.");
                }
                else
                {
                    MessageBox.Show("No User with email " + result.Email + " found.");
                }
            }
        }

        private void Update_Load(object sender, EventArgs e)
        {
            // Calling method from New User form
            User newUser = new User();
            var encryptedPassword = newUser.Encrypt(txtPassword.Text);

            if (admin != null)
            {
                string email = admin.listView1.SelectedItems[0].SubItems[1].Text;
                
                using(var db = new LoginContext())
                {
                    var result = db.Users.First(u => u.Email == email);

                    txtEmail.Text = email;
                    txtPassword.Text = result.Password;
                    txtUserType.Text = result.Type;
                }               
            }
        }
    }
}
