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
        User newUser = new User();
        string encryptedPassword;
        string email;

        public Update()
        {
            InitializeComponent();
            txtPassword.Click += TextBoxOnClick;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {               
                var result = db.Users
                       .Where(u => u.Email == email)
                       .FirstOrDefault<User>();

                if (result.Password == txtPassword.Text)
                {
                    txtPassword.Modified = false;
                }
                else
                {
                    txtPassword.Modified = true;
                }

                if (txtPassword.Modified)
                {
                    encryptedPassword = newUser.Encrypt(txtPassword.Text);
                }
                else
                {
                    encryptedPassword = txtPassword.Text;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {                  
                var result = db.Users.First(u => u.Email == txtEmail.Text);

                if (result != null)
                {
                    // Set the txtEmail control's ReadOnly property to true
                    result.Email = txtEmail.Text.ToLower();
                    result.Password = encryptedPassword;
                    result.Type = comboBox1.Text;

                    // Updates selected row of listview
                    admin.listView1.SelectedItems[0].SubItems[2].Text = comboBox1.Text;

                    db.SaveChanges();
                    this.Hide();
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
                email = admin.listView1.SelectedItems[0].SubItems[1].Text;

                using (var db = new LoginContext())
                {
                    var result = db.Users
                            .Where(u => u.Email == email)
                            .FirstOrDefault<User>();

                    if (email == result.Email)
                    {                        
                        txtEmail.Text = email;

                        // Set UseSystemPasswordChar property to true to hide password
                        txtPassword.Text = result.Password;
                        comboBox1.Text = result.Type;                        
                    }
                }
            }
        }

        // Select all text on single click
        private void TextBoxOnClick(object sender, EventArgs e)
        {
            var textbox = (System.Windows.Forms.TextBox)sender;
            textbox.SelectAll();
            textbox.Focus();
        }
    }
}

