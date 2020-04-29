using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PassLock
{
    public partial class Password : Form
    {  
        public Password()
        {
            InitializeComponent();
        }
        bool isValidUsernameAndEmail()
        {
            using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
            {
                sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                //To compare if the inserted username already exist
                sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Email=@email and Username=@username";
                sqlcmd.Parameters.AddWithValue("@email", EmailTB.Text);
                sqlcmd.Parameters.AddWithValue("@username", usernameTB.Text);
                sqlcmd.Connection = sqlCon;
                SqlDataReader sdata = sqlcmd.ExecuteReader();
                if (sdata.HasRows)
                {                    
                    return true;
                }
                else
                {                    
                    return false;
                }
            }
        }
        bool isBothAnswerCorrect()
        {
            using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
            {                
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("Select [Answer1],[Answer2] FROM [testdb].[dbo].[Users]" +
                    " where Username='" + usernameTB.Text + "'", sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                string answer1 = dataTable.Rows[0][0].ToString();
                string answer2 = dataTable.Rows[0][1].ToString();
                string answer1Hash = BCrypt.Net.BCrypt.HashPassword(Answer1TB.Text);
                string answer2Hash = BCrypt.Net.BCrypt.HashPassword(Answer2TB.Text);

                if (BCrypt.Net.BCrypt.Verify(Answer1TB.Text.ToLower(), answer1) == true && BCrypt.Net.BCrypt.Verify(Answer2TB.Text.ToLower(), answer2) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /*
         * will take to the security question section if username and email is valid
         */
        private void FirstNextBtn_Click(object sender, EventArgs e)
        {
            BottomWarning.Visible = false;
            if (isValidUsernameAndEmail() == true)
            {

                using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
                {
                    sqlCon.Open();
                    SqlDataAdapter sdata = new SqlDataAdapter("Select [Q1],[Q2] FROM [testdb].[dbo].[Users]" +
                        " where Username='" + usernameTB.Text + "'", sqlCon);
                    DataTable dataTable = new DataTable();
                    sdata.Fill(dataTable);
                    Q1_label.Text = dataTable.Rows[0][0].ToString();
                    Q2_label.Text = dataTable.Rows[0][1].ToString();
                    panelMid.Left = 62;
                    panelLeft.Left = 454;
                }
            }
            else
            {
                BottomWarning.Visible = true;
            }
        }
        /*
         * will take to the input new password section if user's security answer is correct
         */
        private void SecondNextBtn_Click(object sender, EventArgs e)
        {
            BottomWarning.Visible = false;
            if (Answer2TB.Text.Length == 0 || Answer1TB.Text.Length == 0)
            {
                BottomWarning.Visible = true;
                BottomWarning.Text = "       Answer cannot be empty";

            }
            else
            {
                if (isBothAnswerCorrect() == true)
                {
                    panelRight.Left = 62;
                    panelLeft.Left = 841;
                }
                else
                {
                    BottomWarning.Visible = true;
                    BottomWarning.Text = "        One of your answer is in correct";
                }
            }
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
        private void PasswordTB_Leave(object sender, EventArgs e)
        {
            BottomWarning.Visible = false;
            if (hasSpaceInPassword(PasswordTB.Text) == false)
            {
                BottomWarning.Visible = false;
                if (isValidPassword(PasswordTB.Text) == true)
                {
                    PasswordHint_label.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    PasswordHint_label.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                BottomWarning.Visible = true;
                BottomWarning.Text = "       Space cannot be used in password";
                PasswordHint_label.ForeColor = System.Drawing.Color.Red;
            }
        }
        /*
         * will submit user's new password to the system with condition matched
         */
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            BottomWarning.Visible = false;
            if (hasSpaceInPassword(PasswordTB.Text) == false)
            {
                if (isValidPassword(PasswordTB.Text) == true)
                {
                    PasswordHint_label.ForeColor = System.Drawing.Color.Black;
                    if (PasswordTB.Text == ConfirmPasswordTB.Text)
                    {
                        using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
                        {
                            sqlCon.Open();
                            SqlCommand sqlcmd = new SqlCommand();
                            //To compare if the inserted username already exist
                            sqlcmd.CommandText = "Update [testdb].[dbo].[Users] SET [Password] = @password Where [Username] = @username";
                            sqlcmd.Parameters.AddWithValue("@username", usernameTB.Text);
                            //encrypt user password
                            string passwordHash = BCrypt.Net.BCrypt.HashPassword(ConfirmPasswordTB.Text);
                            sqlcmd.Parameters.AddWithValue("@password", passwordHash);
                            sqlcmd.Connection = sqlCon;
                            SqlDataReader sdata = sqlcmd.ExecuteReader();
                            MessageBox.Show("Password has been changed successfully. Please login by using your username and password");
                            this.Hide();
                            Login form1 = new Login();
                            form1.Show();
                        }
                    }
                    else
                    {
                        BottomWarning.Text = "       Confirm password did not matched";
                        BottomWarning.Visible = true;
                    }
                }
                else
                {
                    PasswordHint_label.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                BottomWarning.Visible = true;
                BottomWarning.Text = "       Space cannot be used in password";
                PasswordHint_label.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void ConfirmPasswordTB_Leave(object sender, EventArgs e)
        {
            BottomWarning.Visible = false;
            if (PasswordTB.Text != ConfirmPasswordTB.Text)
            {
                BottomWarning.Visible = true;
                BottomWarning.Text = "       Confirm password did not matched";
            }
            else
            {
                BottomWarning.Visible = false;
            }
        }
        /*
         * Will take back to Login page
         */
        private void BackToLoginPageBTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }       
    }
}
