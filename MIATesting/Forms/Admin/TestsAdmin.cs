using MIATesting.AddForms;
using MIATesting.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MIATesting.Forms.Admin
{
    public partial class TestsAdmin : Form
    {
        private string id;
        public TestsAdmin(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void loadInfoTest()
        {
            DB db = new DB();

            TestDataGrid.Rows.Clear();

            string query = $"select test.id, test.header, test.dataTest, category.name from test " +
                $"join category on category.id = test.idCategory ";

            db.openConnection();
            using (MySqlCommand mySqlCommand = new MySqlCommand(query, db.getConnection()))
            {
                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                List<string[]> dataDB = new List<string[]>();
                while (reader.Read())
                {
                    dataDB.Add(new string[reader.FieldCount]);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataDB[dataDB.Count - 1][i] = reader[i].ToString();
                    }
                }
                reader.Close();
                foreach (string[] s in dataDB)
                    TestDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            new AdminPanel(id).Show();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            new AddTests().Show();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            loadInfoTest();
        }

        private void TestsAdmin_Load(object sender, EventArgs e)
        {
            loadInfoTest();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand($"delete from test where id = {TestDataGrid[0, TestDataGrid.SelectedCells[0].RowIndex].Value}", db.getConnection());
            db.openConnection();

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Тест удален");

            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            db.closeConnection();
            loadInfoTest();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();

            TestDataGrid.Rows.Clear();

            string searchString = $"select test.id, test.header, test.dataTest, category.name from test " +
                $"join category on category.id = test.idCategory " +
            $"where concat (test.id, test.header, test.dataTest, category.name) like '%" + SearchTextBox.Text + "%'";

            db.openConnection();
            using (MySqlCommand mySqlCommand = new MySqlCommand(searchString, db.getConnection()))
            {
                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                List<string[]> dataDB = new List<string[]>();
                while (reader.Read())
                {
                    dataDB.Add(new string[reader.FieldCount]);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataDB[dataDB.Count - 1][i] = reader[i].ToString();
                    }
                }
                reader.Close();
                foreach (string[] s in dataDB)
                    TestDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            for (int j = 0; j < TestDataGrid.Columns.Count; j++)
            {
                if (TestDataGrid.Columns[j].Visible)
                {
                    worksheet.Cells[1, j] = TestDataGrid.Columns[j].HeaderText;
                }
            }
            for (int i = 0; i < TestDataGrid.Rows.Count; i++)
            {
                for (int j = 0; j < TestDataGrid.Columns.Count; j++)
                {
                    if (TestDataGrid.Columns[j].Visible)
                    {
                        worksheet.Cells[i + 2, j] = TestDataGrid.Rows[i].Cells[j].Value;
                    }
                }
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel File|*.xlsx";
            saveFileDialog1.Title = "Сохранить Excel файл";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                workbook.SaveAs(saveFileDialog1.FileName);
            }
            workbook.Close();
            excelApp.Quit();
        }
    }
}
