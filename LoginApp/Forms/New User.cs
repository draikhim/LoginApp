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
using System.Text.RegularExpressions;
using System.Globalization;

namespace LoginApp
{
    public partial class User : Form
    {
        //TO DOs:
        // Only allow type to be dropdown, no typing allowed
        // Allow user to reset password on own (sending reset link to user's email) --> later

        public Admin admin;
        Admin admin1 = new Admin();

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
                if (!((String.IsNullOrEmpty(txtEmail.Text)) || (String.IsNullOrEmpty(txtPassword.Text)) || (String.IsNullOrEmpty(comboBox1.Text))))
                {                   
                    var result = db.Users
                           .Where(u => u.Email == txtEmail.Text)
                           .FirstOrDefault<User>();
                
                    if (result == null) 
                    {
                        if (IsValidEmail(txtEmail.Text))
                        {                           
                            // Adding new user to the listview
                            var r = Enumerable.Empty<ListViewItem>();

                            if (admin.listView1.Items.Count > 0)
                                r = admin.listView1.Items.OfType<ListViewItem>();

                            int lastId = Convert.ToInt32(r.LastOrDefault().SubItems[0].Text);
                            int id = lastId + 1;

                            if (lastId != null)
                            {
                                string[] row = { id.ToString(), txtEmail.Text, comboBox1.Text };
                                var listViewItem = new ListViewItem(row);
                                admin.listView1.Items.Add(listViewItem);
                            }

                            // Creating new user and saving to db
                            User newUser = new User();
                            newUser.Email = txtEmail.Text.ToLower();
                            newUser.Password = Encrypt(txtPassword.Text);
                            newUser.Type = comboBox1.Text;
                            db.Users.Add(newUser);
                            db.SaveChanges();

                            // Clears form after creating new user
                            this.Controls.Clear();
                            this.InitializeComponent();

                            MessageBox.Show("User with email: " + newUser.Email + " created successfully!");                            
                        }
                        else
                        {
                            MessageBox.Show("Invalid email. Try again.");
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("User exists already.");
                    }
                }
                else
                {
                    MessageBox.Show("Either one or more fields are empty. Make correction and try again!");
                }
            }
        }

        // Check if email is in valid format
        bool IsValidEmail(string email)
        {            
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
