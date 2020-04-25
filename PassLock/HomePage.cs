using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Entity;

namespace PassLock
{
    public partial class HomePage : Form
    {
        //private string ConnectionString = @"Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true;";
        public string userID;
        UserLocker lockerTable = new UserLocker();
        public HomePage()
        {
            InitializeComponent();
        }

        public void clear()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = "";
            lockerTable.AccountID = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            clear();
            DataPopulate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
            {
                MessageBox.Show("Please fill up all the field in Locker");
            }
            else
            {
                lockerTable.UserID = Int32.Parse(userID);
                lockerTable.AccountName = textBox1.Text.Trim();
                lockerTable.Username = textBox2.Text.Trim();
                lockerTable.Password = textBox3.Text.Trim();
                using (DBEntity db = new DBEntity())
                {
                    db.UserLockers.Add(lockerTable);
                    db.SaveChanges();
                }
                clear();
                DataPopulate();
                MessageBox.Show("Record Successfully added");
            }
        }

        void DataPopulate()
        {
            dataGridView1.AutoGenerateColumns = false;
            lockerTable.UserID = Int32.Parse(userID);
            using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
            {
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("SELECT * FROM [testdb].[dbo].[UserLocker] WHERE [UserID] = '"+lockerTable.UserID+"'",sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {                
                lockerTable.AccountID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AccountID"].Value);
                using (DBEntity db = new DBEntity())
                {
                    lockerTable = db.UserLockers.Where(x => x.AccountID == lockerTable.AccountID).FirstOrDefault();
                    textBox1.Text = lockerTable.AccountName;
                    textBox2.Text = lockerTable.Username;
                    textBox3.Text = lockerTable.Password;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
            {
                MessageBox.Show("Please select a record to update");
            }
            else
            {
                lockerTable.UserID = Int32.Parse(userID);
                lockerTable.AccountName = textBox1.Text.Trim();
                lockerTable.Username = textBox2.Text.Trim();
                lockerTable.Password = textBox3.Text.Trim();
                using (DBEntity db = new DBEntity())
                {
                    if (lockerTable.AccountID == 0)
                        db.UserLockers.Add(lockerTable);
                    else
                        db.Entry(lockerTable).State = EntityState.Modified;
                        db.SaveChanges();
                }
                DataPopulate();
                MessageBox.Show("Record Successfully added");
            }
        }
    }
}
