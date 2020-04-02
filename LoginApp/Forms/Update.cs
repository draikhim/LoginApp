using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Globalization;

namespace LoginApp.Forms
{
    public partial class Update : Form
    {
        public Admin admin;
        string encryptedPassword;

        public Update()
        {
            InitializeComponent();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {
                //string lowerEmail = txtEmail.Text.ToLower(new CultureInfo("en-US", false));
                string email = admin.listView1.SelectedItems[0].SubItems[1].Text;

                var result = db.Users
                       .Where(u => u.Email == email)
                       .FirstOrDefault<User>();

                if (result.Password == txtPassword.Text)
                    txtPassword.Modified = false;
                else
                    txtPassword.Modified = true;
                User newUser = new User();

                if (txtPassword.Modified)
                    encryptedPassword = newUser.Encrypt(txtPassword.Text);
                else
                    encryptedPassword = txtPassword.Text;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {                  
                var result = db.Users.First(u => u.Email == txtEmail.Text);

                if (result != null)
                {
                    result.Email = txtEmail.Text.ToLower();
                    result.Password = encryptedPassword;
                    result.Type = comboBox1.Text;

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
            txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);

            if (admin != null)
            {
                string email = admin.listView1.SelectedItems[0].SubItems[1].Text;

                using (var db = new LoginContext())
                {
                    var result = db.Users
                            .Where(u => u.Email == email)
                            .FirstOrDefault<User>();

                    //string hidePassword = new string('*', result.Password.Length);

                    if (email == result.Email)
                    {
                        txtEmail.Text = email;
                        txtPassword.Text = result.Password;
                        comboBox1.Text = result.Type;
                    }

                }
            }
        }        
    }
}

