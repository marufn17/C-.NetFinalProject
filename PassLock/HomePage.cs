﻿
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
using Excel = Microsoft.Office.Interop.Excel;

namespace PassLock
{
    public partial class HomePage : Form
    {       
        public string userID;
        UserLocker lockerTable = new UserLocker();
        public HomePage()
        {
            InitializeComponent();
        }
        public void clear()
        {
            AccountTB.Text = UsernameTB.Text = PasswordTB.Text = SearchTB.Text = "";
            lockerTable.AccountID = 0;
        } 
        private void HomePage_Load(object sender, EventArgs e)
        {
            clear();
            DataPopulate();
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (AccountTB.Text.Length == 0 || UsernameTB.Text.Length == 0 || PasswordTB.Text.Length == 0)
            {
                MessageBox.Show("Please fill up all the field in Locker");
            }
            else
            {
                lockerTable.UserID = Int32.Parse(userID);
                lockerTable.AccountName = AccountTB.Text.Trim();
                lockerTable.Username = UsernameTB.Text.Trim();
                lockerTable.Password = PasswordTB.Text.Trim();
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
                lockerTable.AccountID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AccountID"].Value);
                using (DBEntity db = new DBEntity())
                {
                    lockerTable = db.UserLockers.Where(x => x.AccountID == lockerTable.AccountID).FirstOrDefault();
                    AccountTB.Text = lockerTable.AccountName;
                    UsernameTB.Text = lockerTable.Username;
                    PasswordTB.Text = lockerTable.Password;
                }            
        }  
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (AccountTB.Text.Length == 0 || UsernameTB.Text.Length == 0 || PasswordTB.Text.Length == 0)
            {
                MessageBox.Show("Please select a record to update");
            }
            else
            {
                lockerTable.UserID = Int32.Parse(userID);
                lockerTable.AccountName = AccountTB.Text.Trim();
                lockerTable.Username = UsernameTB.Text.Trim();
                lockerTable.Password = PasswordTB.Text.Trim();
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
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (AccountTB.Text.Length == 0 || UsernameTB.Text.Length == 0 || PasswordTB.Text.Length == 0)
            {
                MessageBox.Show("Please select a record to delete");
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete this record permanently?", "Cofirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (DBEntity db = new DBEntity())
                    {
                        var record = db.Entry(lockerTable);
                        if (record.State == EntityState.Detached)
                            db.UserLockers.Attach(lockerTable);
                        db.UserLockers.Remove(lockerTable);
                        db.SaveChanges();
                        DataPopulate();
                        clear();
                        MessageBox.Show("Record deleted successfully.");
                    }
                }
            }
        }
        private void ExportBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Export the file ?", "Cofirm Export", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Excel._Application msapp = new Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = msapp.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                //Can be seen the process of writing in excel
                //msapp.Visible = true;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "AccountsDetails";
                //Put Header in excel
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;

                //To put the datagrid data in excel
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count - 1; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                //Pop up save file dailog box
                var saveFile = new SaveFileDialog();
                saveFile.FileName = "ExportFile";
                saveFile.DefaultExt = ".xlsx";
                //File foramt
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFile.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                msapp.Quit();
            }
        }
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            lockerTable.UserID = Int32.Parse(userID);
            using (SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-BDDENA3;Initial Catalog=testdb;Integrated Security=true "))
            {
                sqlCon.Open();
                SqlDataAdapter sdata = new SqlDataAdapter("SELECT * FROM [testdb].[dbo].[UserLocker] WHERE [UserID] = '" + lockerTable.UserID + "' and [AccountName] like '%" + SearchTB.Text + "%'", sqlCon);
                DataTable dataTable = new DataTable();
                sdata.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }
        private void LogoutIcon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to logout?", "Cofirm Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntity db = new DBEntity())
                {
                    this.Hide();
                    Login login = new Login();
                    login.Show();
                    MessageBox.Show("You have successfully Logged out. Thanks for using PassLocker");
                }
            }
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            clear();
            DataPopulate();
        }       
    }
}
