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
        /*
         * will take back to Login page
         */
        private void BackToLoginBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login f1 = new Login();
            f1.Show();//Login page shows
        }
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Warning1_label.Visible = false;
                Warning2_label.Visible = false;
                sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true ");
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("Select [Username] from [testdb].[dbo].[Users]" +
                    " where Email='" + EmailTB.Text + "' and [FirstName] ='" + FirstNameTB.Text + "' and [LastName] ='" + LastNameTB.Text + "'", sqlCon);
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
                Warning1_label.Visible = true;
                Warning2_label.Visible = true;
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}
