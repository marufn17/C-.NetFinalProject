using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PassLock
{
    public partial class Password : Form
    {
        public static SqlConnection sqlCon;

        public Password()
        {
            InitializeComponent();
        }



        bool isValidUsernameAndEmail()
        {
            sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
            sqlCon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            //To compare if the inserted username already exist
            sqlcmd.CommandText = "Select * from [testdb].[dbo].[Users] where Email=@email and Username=@username";
            sqlcmd.Parameters.AddWithValue("@email", textBox2.Text);
            sqlcmd.Parameters.AddWithValue("@username", textBox1.Text);
            sqlcmd.Connection = sqlCon;
            SqlDataReader sdata = sqlcmd.ExecuteReader();
            if (sdata.HasRows)
            {
                sqlCon.Close();
                return true;
            }
            else
            {
                sqlCon.Close();
                return false;
            }
        }

        bool isBothAnswerCorrect()
        {
            sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
            sqlCon.Open();
            SqlDataAdapter sdata = new SqlDataAdapter("Select [Answer1],[Answer2] FROM [testdb].[dbo].[Users]" +
                " where Username='" + textBox1.Text + "'", sqlCon);
            DataTable dataTable = new DataTable();
            sdata.Fill(dataTable);
            string answer1 = dataTable.Rows[0][0].ToString();
            string answer2 = dataTable.Rows[0][1].ToString();
            string answer1Hash = BCrypt.Net.BCrypt.HashPassword(textBox4.Text);
            string answer2Hash = BCrypt.Net.BCrypt.HashPassword(textBox3.Text);

            if (BCrypt.Net.BCrypt.Verify(textBox4.Text.ToLower(), answer1) == true && BCrypt.Net.BCrypt.Verify(textBox3.Text.ToLower(), answer2) == true)
            {
                sqlCon.Close();
                return true;
            }
            else
            {
                sqlCon.Close();
                return false;
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            label7.Visible = false;
            if (isValidUsernameAndEmail() == true)
            {
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("Select [Q1],[Q2] FROM [testdb].[dbo].[Users]" +
                    " where Username='" + textBox1.Text + "'", sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                label4.Text = dataTable.Rows[0][0].ToString();
                label3.Text = dataTable.Rows[0][1].ToString();
                panelMid.Left = 48;
                panelLeft.Left = 387;
                sqlCon.Close();
            }
            else
            {
                label7.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label7.Visible = false;
            if (textBox3.Text.Length == 0 || textBox4.Text.Length == 0)
            {
                label7.Visible = true;
                label7.Text = "       Answer cannot be empty";
                
            }
            else
            {
                if (isBothAnswerCorrect() == true)
                {
                    panelRight.Left = 48;
                    panelLeft.Left = 722;
                }
                else
                {
                    label7.Visible = true;
                    label7.Text = "        One of your answer is in correct";
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

        private void textBox6_Leave(object sender, EventArgs e)
        {
            label7.Visible = false;
            if (hasSpaceInPassword(textBox6.Text)==false)
            {
                label7.Visible = false;
                if (isValidPassword(textBox6.Text) == true)
                {
                    label8.ForeColor = System.Drawing.Color.Black;
                }
                else 
                {
                    label8.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                label7.Visible = true;
                label7.Text = "       Space cannot be used in password";
                label8.ForeColor = System.Drawing.Color.Red;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            label7.Visible = false;
            if (hasSpaceInPassword(textBox6.Text) == false)
            {                
                if (isValidPassword(textBox6.Text) == true)
                {
                    label8.ForeColor = System.Drawing.Color.Black;
                    if (textBox6.Text == textBox5.Text)
                    {
                        sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                        sqlCon.Open();
                        SqlCommand sqlcmd = new SqlCommand();
                        //To compare if the inserted username already exist
                        sqlcmd.CommandText = "Update [testdb].[dbo].[Users] SET [Password] = @password Where [Username] = @username";                        
                        sqlcmd.Parameters.AddWithValue("@username", textBox1.Text);
                        //encrypt user password
                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(textBox5.Text);
                        sqlcmd.Parameters.AddWithValue("@password", passwordHash);
                        sqlcmd.Connection = sqlCon;
                        SqlDataReader sdata = sqlcmd.ExecuteReader();
                        MessageBox.Show("Password has been changed successfully. Please login by using your username and password");
                        this.Hide();
                        Login form1 = new Login();
                        form1.Show();
                        sqlCon.Close();
                    }
                    else
                    {                        
                        label7.Text = "       Confirm password did not matched";
                        label7.Visible = true;
                    }
                }
                else
                {
                    label8.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                label7.Visible = true;
                label7.Text = "       Space cannot be used in password";
                label8.ForeColor = System.Drawing.Color.Red;
            }

        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            label7.Visible = false;
            if (textBox6.Text != textBox5.Text)
            {
                label7.Visible = true;
                label7.Text = "       Confirm password did not matched";
            }
            else
            {
                label7.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }
    }
}
