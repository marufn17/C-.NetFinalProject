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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp form2 = new SignUp();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //login button to get into Home page
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Please enter your username");
            }
            else if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Please enter your password");
            }
            else
            {
                try
                {
                    sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                    sqlCon.Open();
                    SqlDataAdapter sdata = new SqlDataAdapter("Select [Password],[userID],[FirstName],[LastName] from [testdb].[dbo].[Users] where [Username] = '" + textBox1.Text + "'", sqlCon);
                    DataTable dataTable = new DataTable();
                    sdata.Fill(dataTable);                    
                    string answer1 = dataTable.Rows[0][0].ToString();
                    string answer1Hash = BCrypt.Net.BCrypt.HashPassword(textBox2.Text);
                    if (BCrypt.Net.BCrypt.Verify(textBox2.Text, answer1) == true)
                    {                        
                        this.Hide();
                        HomePage home = new HomePage();
                        home.userID = dataTable.Rows[0][1].ToString();
                        home.label5.Text = "Welcome " + dataTable.Rows[0][2].ToString() + " " + dataTable.Rows[0][3].ToString();
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
                    textBox1.Text = null;
                    textBox2.Text = null;
                }
                finally
                {
                    sqlCon.Close();
                }                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Username form3 = new Username();
            form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Password password = new Password();
            password.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
