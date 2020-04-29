using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PassLock
{   
    public partial class Login : Form
    {
        public static SqlConnection sqlCon;        
        public Login()
        {
            InitializeComponent();
        }        
        /*
         * Login Functionality
         */
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //login button to get into Home page
            if (UsernameTextBox.Text.Length == 0)//to check if username field is empty
            {
                MessageBox.Show("Please enter your username");
            }
            else if (PasswordTextBox.Text.Length == 0)//to check if password field is empty
            {
                MessageBox.Show("Please enter your password");
            }
            else
            {
                try
                {
                    sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                    sqlCon.Open();
                    SqlDataAdapter sdata = new SqlDataAdapter("Select [Password],[userID],[FirstName],[LastName] from [testdb].[dbo].[Users] " +
                        "where [Username] = '" + UsernameTextBox.Text + "'", sqlCon);
                    DataTable dataTable = new DataTable();
                    sdata.Fill(dataTable);
                    string answer1 = dataTable.Rows[0][0].ToString();//retrieve encrypted password from db
                    string answer1Hash = BCrypt.Net.BCrypt.HashPassword(PasswordTextBox.Text);
                    if (BCrypt.Net.BCrypt.Verify(PasswordTextBox.Text, answer1) == true)//verify retreived password with user entered password
                    {
                        this.Hide();
                        HomePage home = new HomePage();
                        home.userID = dataTable.Rows[0][1].ToString();//Assign userID to the user to further use in home page
                        home.label5.Text = "Welcome " + dataTable.Rows[0][2].ToString() + " " + dataTable.Rows[0][3].ToString();//it will pick up user name to show in Home page.
                        home.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Pasword");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No username found!!");
                    UsernameTextBox.Text = null;
                    PasswordTextBox.Text = null;
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }
        /*
         * It will take user to the username forgot page to retrieve the username
         */
        private void UsernameBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Username username = new Username();
            username.Show();//Shows username page.
        }
        /*
         * It will take user to the pasword forgot page to reset the password
         */    
        private void PasswordBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Password password = new Password();
            password.Show();//shows password reset page.
        }
        /* 
         * Will take user to the signup page
         */
        private void SignupBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.Show();//shows signup page
        }
        /*
         * it will shows and hide the password with checkbox response
         */
        private void ShowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowCheckBox.Checked)
            {
                PasswordTextBox.UseSystemPasswordChar = false;//will show the password
            }
            else
            {
                PasswordTextBox.UseSystemPasswordChar = true;//will hide password
            }
        }
    }
}
