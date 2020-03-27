using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginApp
{
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
                Application.Exit();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {
                // Clear form after creating new user. 
                // Handle exception that pops up sometimes

                User newUser = new User();
                newUser.Email = txtEmail.Text.ToLower();
                newUser.Password = txtPassword.Text;
                newUser.Type = txtUserType.Text.First().ToString().ToUpper() + txtUserType.Text.Substring(1).ToLower();

                var result = db.Users
                       .Where(u => u.Email == txtEmail.Text)
                       .FirstOrDefault<User>();                
                
                if (result == null)
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("User: " + newUser.Email + " created successfully");
                }
                else
                {
                    MessageBox.Show("User exists already.");

                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            
            using (var db = new LoginContext())
            {

                var result = db.Users.First(u => u.Email == txtEmail.Text);

                if(result != null)
                {
                    result.Email = txtEmail.Text.ToLower();
                    result.Password = txtPassword.Text;
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
    }
}
