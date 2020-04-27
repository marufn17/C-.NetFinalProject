using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PassLock
{
    public partial class Username : Form
    {
        public static SqlConnection sqlCon;
        public Username()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label5.Visible = false;
                label6.Visible = false;
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("Select [Username] from [testdb].[dbo].[Users]" +
                    " where Email='" + textBox3.Text + "' and [FirstName] ='"+ textBox1.Text + "' and [LastName] ='"+ textBox2.Text +"'", sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                string username = dataTable.Rows[0][0].ToString();
                MessageBox.Show("Your username is " + username + ". Please use your username to login.");
                this.Hide();
                PassLock.Login form1 = new PassLock.Login();
                form1.Show();
            }
            catch (System.IndexOutOfRangeException sioex)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
            finally 
            {
                sqlCon.Close();
            }            
        }
        /*
         * will take back to Login page
         */
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login f1 = new Login();
            f1.Show();//Login page shows
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                label5.Visible = false;
                label6.Visible = false;
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("Select [Username] from [testdb].[dbo].[Users]" +
                    " where Email='" + textBox3.Text + "' and [FirstName] ='" + textBox1.Text + "' and [LastName] ='" + textBox2.Text + "'", sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                string username = dataTable.Rows[0][0].ToString();
                MessageBox.Show("Your username is " + username + ". Please use your username to login.");
                this.Hide();
                PassLock.Login form1 = new PassLock.Login();
                form1.Show();
            }
            catch (System.IndexOutOfRangeException sioex)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}
