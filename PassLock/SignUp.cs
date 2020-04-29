using System;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace PassLock
{
    public partial class SignUp : Form
    {
        public static SqlConnection sqlCon;
        public SignUp()
        {
            InitializeComponent();
        }  
        private void BackToLoginBtn_Click(object sender, EventArgs e)
        {
            //Hide current form
            this.Hide();
            Login form1 = new Login();
            //it will take user to the login page
            form1.Show();
        }
        private void CheckUsernameBtn_Click_1(object sender, EventArgs e)
        {
            //To check inserted username only has alphanumeric value or not
            bool HasSpecialChars(string username)
            {
                return username.Any(ch => !Char.IsLetterOrDigit(ch));
            }
            //If user inset alphanumeric value more than 7 characters
            if (UsernameTB.Text.Length > 7 && HasSpecialChars(UsernameTB.Text) == false)
            {
                ShowWarning1.Visible = false;
                using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
                {
                    sqlCon.Open();
                    SqlCommand sqlcmd = new SqlCommand();
                    //To compare if the inserted username already exist
                    sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Username=@uName";
                    sqlcmd.Parameters.AddWithValue("@uName", UsernameTB.Text);
                    sqlcmd.Connection = sqlCon;
                    SqlDataReader sdata = sqlcmd.ExecuteReader();
                    if (sdata.HasRows)
                    {
                        ShowWarning2.Visible = true;
                        ShowWarning2.Text = "Username already exist";
                        ShowWarning2.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        ShowWarning2.Visible = true;
                        ShowWarning2.Text = "Username available!";
                        ShowWarning2.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            //If username insert more than 7 characters but username has special charachter
            else if (UsernameTB.Text.Length > 7 && HasSpecialChars(UsernameTB.Text) == true)
            {
                ShowWarning2.Visible = false;
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "username must be alphanumeric without space";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                ShowWarning2.Visible = false;
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "Username should have minimum 8 characters";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            //To check inserted username only has alphanumeric value or not
            bool HasSpecialChars(string username)
            {
                return username.Any(ch => !Char.IsLetterOrDigit(ch));
            }
            bool hasSpaceInPassword(String password)
            {
                return password.Contains(" ");
            }
            bool isLetter(char c)
            {
                return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
            }

            bool isDigit(char c)
            {
                return c >= '0' && c <= '9';
            }

            bool isSymbol(char c)
            {
                return c > 32 && c < 127 && !isDigit(c) && !isLetter(c);
            }
            bool isValidPassword(string password)
            {
                return
                   password.Any(c => isLetter(c)) &&
                   password.Any(c => isDigit(c)) &&
                   password.Any(c => isSymbol(c));
            }
            bool hasAtInEmail(string dot)
            {
                return dot.Contains("@");
            }
            bool hasDotComIneamil(string dotcom)
            {
                if (dotcom.Substring(dotcom.Length - 4) == ".com")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            bool hasAtDotInEmail(string value)
            {
                return value.Contains("@.");
            }
            bool hasAtInFirstCharEmail(string value)
            {
                char[] firstLeter = value.ToCharArray();
                if (firstLeter[0] == '@')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            bool isSecurityQuestionValid()
            {
                if (Q1TB.Text.Length == 0 || Answer1TB.Text.Length == 0 ||
                    Q2TB.Text.Length == 0 || Answer2TB.Text.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            bool isEmailUnique()
            {
                using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
                {
                    sqlCon.Open();
                    SqlCommand sqlcmd = new SqlCommand();
                    //To compare if the inserted username already exist
                    sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Email=@email";
                    sqlcmd.Parameters.AddWithValue("@email", EmailTB.Text);
                    sqlcmd.Connection = sqlCon;
                    SqlDataReader sdata = sqlcmd.ExecuteReader();
                    if (sdata.HasRows)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            //If user insert alphanumeric value more than 7 characters
            if (UsernameTB.Text.Length > 7 && HasSpecialChars(UsernameTB.Text) == false)
            {
                if (hasSpaceInPassword(PasswordTB.Text) == false)
                {
                    ShowWarning3.Visible = false;
                    ShowWarning1.Visible = false;
                    if (isValidPassword(PasswordTB.Text) == true && PasswordTB.Text.Length > 7)
                    {
                        if (PasswordTB.Text == ConfirmPasswordTB.Text)
                        {
                            if (FirstNameTB.Text.Length == 0)
                            {
                                ShowWarning3.Visible = true;
                                ShowWarning3.Text = "You must enter your first name.";
                                ShowWarning3.ForeColor = System.Drawing.Color.Red;
                            }
                            else if (EmailTB.Text.Length == 0)
                            {
                                ShowWarning3.Visible = true;
                                ShowWarning3.Text = "You must enter your email address.";
                                ShowWarning3.ForeColor = System.Drawing.Color.Red;
                            }
                            else if (hasAtInEmail(EmailTB.Text) == false || hasDotComIneamil(EmailTB.Text) == false || hasAtDotInEmail(EmailTB.Text) == true
                                || hasAtInFirstCharEmail(EmailTB.Text) == true)
                            {
                                ShowWarning3.Visible = true;
                                ShowWarning3.Text = "You must enter valid email address.";
                                ShowWarning3.ForeColor = System.Drawing.Color.Red;
                            }
                            //To check if the sequrity condition is valid or not
                            else if (isSecurityQuestionValid() == false)
                            {
                                ShowWarning3.Visible = true;
                                ShowWarning3.Text = "You must enter all security Questions and Answers";
                                ShowWarning3.ForeColor = System.Drawing.Color.Red;
                                PasswordTB.Text = null;
                                ConfirmPasswordTB.Text = null;
                            }
                            else if (isEmailUnique() == false)
                            {
                                MessageBox.Show("We found a username associated with your email. Please use your username to login. If you forget your username, please use forget username link");
                                PasswordTB.Text = null;
                                ConfirmPasswordTB.Text = null;
                                this.Hide();
                                Login form1 = new Login();
                                form1.Show();
                            }
                            //insert User inforamtion to data base
                            else
                            {
                                ShowWarning3.Visible = false;
                                using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
                                {
                                    sqlCon.Open();
                                    SqlCommand sqlcmd = new SqlCommand();
                                    //To compare if the inserted username already exist
                                    sqlcmd.CommandText = "Insert into [testdb].[dbo].[Users] Values (@firstname,@lastname,@email,@username,@password,@q1,@answer1,@q2,@answer2)";
                                    sqlcmd.Parameters.AddWithValue("@firstname", FirstNameTB.Text);
                                    sqlcmd.Parameters.AddWithValue("@lastname", LastNameTB.Text);
                                    sqlcmd.Parameters.AddWithValue("@email", EmailTB.Text);
                                    sqlcmd.Parameters.AddWithValue("@username", UsernameTB.Text);
                                    //encrypt user password
                                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(PasswordTB.Text);
                                    sqlcmd.Parameters.AddWithValue("@password", passwordHash);
                                    sqlcmd.Parameters.AddWithValue("@q1", Q1TB.Text);
                                    //encrypt user answer1
                                    string answer1Hash = BCrypt.Net.BCrypt.HashPassword(Answer1TB.Text.ToLower());
                                    sqlcmd.Parameters.AddWithValue("@answer1", answer1Hash);
                                    sqlcmd.Parameters.AddWithValue("@q2", Q2TB.Text);
                                    //encrypt user answer2
                                    string answer2Hash = BCrypt.Net.BCrypt.HashPassword(Answer2TB.Text.ToLower());
                                    sqlcmd.Parameters.AddWithValue("@answer2", answer2Hash);
                                    sqlcmd.Connection = sqlCon;
                                    SqlDataReader sdata = sqlcmd.ExecuteReader();
                                    MessageBox.Show("Registration successfull. Please login by using your username and password");
                                    this.Hide();
                                    Login form1 = new Login();
                                    form1.Show();
                                }
                            }
                        }
                        else
                        {
                            ShowWarning3.Visible = true;
                            ShowWarning3.Text = "Both Password did not matched, Please try again.";
                            ShowWarning3.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        ShowWarning3.Visible = true;
                        ShowWarning3.Text = "Invalid password combination!! Please follow passwod Hints.";
                        ShowWarning3.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    ShowWarning3.Visible = true;
                    ShowWarning3.Text = "Password cannot have space";
                    ShowWarning3.ForeColor = System.Drawing.Color.Red;
                }
            }
            //If user forget to enter any username
            else if (UsernameTB.Text.Length == 0)
            {
                ShowWarning2.Visible = false;
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "You must enter a username with minimum 8 characters";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
                PasswordTB.Text = null;
                ConfirmPasswordTB.Text = null;
            }
            else if (UsernameTB.Text.Length > 7 && HasSpecialChars(UsernameTB.Text) == true)
            {
                ShowWarning2.Visible = false;
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "username must be alphanumeric without space";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                ShowWarning2.Visible = false;
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "Username should have minimum 8 characters";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
                PasswordTB.Text = null;
                ConfirmPasswordTB.Text = null;
            }
        }        
        private void EmailTB_Leave(object sender, EventArgs e)
        {

            string email = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            if (Regex.IsMatch(EmailTB.Text, email))
            {
                ShowWarning1.Visible = false;
            }
            else
            {
                ShowWarning1.Visible = true;
                ShowWarning1.Text = "You must enter valid email address.";
                ShowWarning1.ForeColor = System.Drawing.Color.Red;
            }
        }        
    }
}
