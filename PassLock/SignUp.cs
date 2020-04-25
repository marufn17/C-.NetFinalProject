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

        private void button2_Click(object sender, EventArgs e)
        {
            //Hide current form
            this.Hide();
            Login form1 = new Login();
            //it will take user to the login page
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //To check inserted username only has alphanumeric value or not
            bool HasSpecialChars(string username)
            {
                return username.Any(ch => !Char.IsLetterOrDigit(ch));
            }
            //If user inset alphanumeric value more than 7 characters
            if (textBox4.Text.Length > 7 && HasSpecialChars(textBox4.Text) == false)
            {
                label11.Visible = false;
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                //To compare if the inserted username already exist
                sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Username=@uName";
                sqlcmd.Parameters.AddWithValue("@uName", textBox4.Text);
                sqlcmd.Connection = sqlCon;
                SqlDataReader sdata = sqlcmd.ExecuteReader();
                if (sdata.HasRows)
                {
                    label10.Visible = true;
                    label10.Text = "Username already exist";
                    label10.ForeColor = System.Drawing.Color.Red;                    
                }
                else
                {
                    label10.Visible = true;
                    label10.Text = "Username available!";
                    label10.ForeColor = System.Drawing.Color.Green;
                }
                sqlCon.Close();
            }
            //If username insert more than 7 characters but username has special charachter
            else if (textBox4.Text.Length > 7 && HasSpecialChars(textBox4.Text) == true)
            {
                label10.Visible = false;
                label11.Visible = true;
                label11.Text = "username must be alphanumeric without space";
                label11.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                label10.Visible = false;
                label11.Visible = true;
                label11.Text = "Username should have minimum 8 characters";
                label11.ForeColor = System.Drawing.Color.Red;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
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
                if (comboBox1.Text.Length == 0 || textBox8.Text.Length == 0 || 
                    comboBox2.Text.Length ==0 || textBox7.Text.Length ==0)
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
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                //To compare if the inserted username already exist
                sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Email=@email";
                sqlcmd.Parameters.AddWithValue("@email", textBox3.Text);
                sqlcmd.Connection = sqlCon;
                SqlDataReader sdata = sqlcmd.ExecuteReader();
                if (sdata.HasRows)
                {
                    sqlCon.Close();
                    return false;
                }
                else
                {
                    sqlCon.Close();
                    return true;
                }

            }
            //If user insert alphanumeric value more than 7 characters
            if (textBox4.Text.Length > 7 && HasSpecialChars(textBox4.Text) == false)
            {
                if (hasSpaceInPassword(textBox5.Text) == false)
                {
                    label12.Visible = false;
                    label11.Visible = false;
                    if (isValidPassword(textBox5.Text) == true && textBox5.Text.Length > 7)
                    {
                        if (textBox5.Text == textBox6.Text)
                        {
                            if (textBox1.Text.Length == 0)
                            {
                                label12.Visible = true;
                                label12.Text = "You must enter your first name.";
                                label12.ForeColor = System.Drawing.Color.Red;
                            }
                            else if (textBox3.Text.Length == 0)
                            {
                                label12.Visible = true;
                                label12.Text = "You must enter your email address.";
                                label12.ForeColor = System.Drawing.Color.Red;
                            }
                            else if (hasAtInEmail(textBox3.Text) == false || hasDotComIneamil(textBox3.Text) == false || hasAtDotInEmail(textBox3.Text) == true
                                || hasAtInFirstCharEmail(textBox3.Text) == true)
                            {
                                label12.Visible = true;
                                label12.Text = "You must enter valid email address.";
                                label12.ForeColor = System.Drawing.Color.Red;
                            }
                            //To check if the sequrity condition is valid or not
                            else if (isSecurityQuestionValid() == false)
                            {
                                label12.Visible = true;
                                label12.Text = "You must enter all security Questions and Answers";
                                label12.ForeColor = System.Drawing.Color.Red;
                                textBox5.Text = null;
                                textBox6.Text = null;
                            }
                            else if (isEmailUnique() == false)
                            {
                                MessageBox.Show("We found a username associated with your email. Please use your username to login. If you forget your username, please use forget username link");
                                textBox5.Text = null;
                                textBox6.Text = null;
                                this.Hide();
                                Login form1 = new Login();
                                form1.Show();
                            }
                            //insert User inforamtion to data base
                            else
                            {
                                label12.Visible = false;
                                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                                sqlCon.Open();
                                SqlCommand sqlcmd = new SqlCommand();
                                //To compare if the inserted username already exist
                                sqlcmd.CommandText = "Insert into [testdb].[dbo].[Users] Values (@firstname,@lastname,@email,@username,@password,@q1,@answer1,@q2,@answer2)";
                                sqlcmd.Parameters.AddWithValue("@firstname", textBox1.Text);
                                sqlcmd.Parameters.AddWithValue("@lastname", textBox2.Text);
                                sqlcmd.Parameters.AddWithValue("@email", textBox3.Text);
                                sqlcmd.Parameters.AddWithValue("@username", textBox4.Text);
                                //encrypt user password
                                string passwordHash = BCrypt.Net.BCrypt.HashPassword(textBox5.Text);
                                sqlcmd.Parameters.AddWithValue("@password", passwordHash);
                                sqlcmd.Parameters.AddWithValue("@q1", comboBox1.Text);
                                //encrypt user answer1
                                string answer1Hash = BCrypt.Net.BCrypt.HashPassword(textBox8.Text.ToLower());
                                sqlcmd.Parameters.AddWithValue("@answer1", answer1Hash);
                                sqlcmd.Parameters.AddWithValue("@q2", comboBox2.Text);
                                //encrypt user answer2
                                string answer2Hash = BCrypt.Net.BCrypt.HashPassword(textBox7.Text.ToLower());
                                sqlcmd.Parameters.AddWithValue("@answer2", answer2Hash);
                                sqlcmd.Connection = sqlCon;
                                SqlDataReader sdata = sqlcmd.ExecuteReader();
                                MessageBox.Show("Registration successfull. Please login by using your username and password");
                                this.Hide();
                                Login form1 = new Login();
                                form1.Show();
                                sqlCon.Close();
                            }
                        }
                        else
                        {
                            label12.Visible = true;
                            label12.Text = "Both Password did not matched, Please try again.";
                            label12.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        label12.Visible = true;
                        label12.Text = "Invalid password combination!! Please follow passwod Hints.";
                        label12.ForeColor = System.Drawing.Color.Red;
                    }

                }
                else
                {
                    label12.Visible = true;
                    label12.Text = "Password cannot have space";
                    label12.ForeColor = System.Drawing.Color.Red;
                }
            }
            //If user forget to enter any username
            else if (textBox4.Text.Length == 0)
            {
                label10.Visible = false;
                label11.Visible = true;
                label11.Text = "You must enter a username with minimum 8 characters";
                label11.ForeColor = System.Drawing.Color.Red;
                textBox5.Text = null;
                textBox6.Text = null;
            }            
            else if (textBox4.Text.Length > 7 && HasSpecialChars(textBox4.Text) == true)
            {
                label10.Visible = false;
                label11.Visible = true;
                label11.Text = "username must be alphanumeric without space";
                label11.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                label10.Visible = false;
                label11.Visible = true;
                label11.Text = "Username should have minimum 8 characters";
                label11.ForeColor = System.Drawing.Color.Red;
                textBox5.Text = null;
                textBox6.Text = null;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            string email = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            if (Regex.IsMatch(textBox3.Text,email))
            {
                label11.Visible = false;
            }
            else
            {
                label11.Visible = true;
                label11.Text = "You must enter valid email address.";
                label11.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
