using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using LoginApp.Forms;

namespace LoginApp
{
    public partial class User : Form
    {
        //TO DOs:
        // Clear form after creating new user.
        // Refresh listview every create, delete or update event
        // Handle exceptions
        // Allow user to reset password on own (sending reset link to user's email) --> later


        public Admin admin;

        public User()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        // Hash an input string and return the hash as a 32 character hexadecimal string.
        public string Encrypt(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        private void Create_Click(object sender, EventArgs e)
        {
            using (var db = new LoginContext())
            {
                // Add code to handle empty or null email, password and type
                User newUser = new User();
                newUser.Email = txtEmail.Text.ToLower();
                newUser.Password = Encrypt(txtPassword.Text);
                newUser.Type = comboBox1.Text;

                var result = db.Users
                       .Where(u => u.Email == txtEmail.Text)
                       .FirstOrDefault<User>();                
                
                if (result == null)
                {                    
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("User: " + newUser.Email + " created successfully ");
                }
                else
                {
                    MessageBox.Show("User exists already.");

                }
            }
        }     
    }
}
