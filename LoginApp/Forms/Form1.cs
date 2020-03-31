using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using LoginApp.Forms;

namespace LoginApp
{
    // Allow user to reset password on own (sending reset link to user's email)

    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using( var db = new LoginContext())
            {

                // Downcase Email before saving to DB
                // Init cap Type before saving to DB
                // Encrypt Password before saving to DB

                string lowerEmail = txtEmail.Text.ToLower(new CultureInfo("en-US", false));

                var user = db.Users
                       .Where(u => u.Email == lowerEmail)
                       .FirstOrDefault<User>();               

                // Calling method from New User form
                User newUser = new User();
                var encryptedPassword = newUser.Encrypt(txtPassword.Text);

                if ((user != null) && (lowerEmail == user.Email) && (encryptedPassword == user.Password))                
                {
                    if (user.Type == "Admin")
                    {
                        this.Hide();

                        Admin admin = new Admin();
                        admin.Show();
                    }
                    else
                    {                   
                        this.Hide();

                        Main main = new Main();
                        main.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect User ID or Password. Try again!");
                }
            }
           
        }       

        private void EncryptPassword(object sender, EventArgs e)
        {
            // The password character is an asterisk.
            txtPassword.PasswordChar = '*';

            // The control will allow no more than 25 characters.
            txtPassword.MaxLength = 25;
        }
    }
}
