using MIATesting.AddForms;
using MIATesting.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MIATesting.Forms.Admin
{
    public partial class TheoryAdmin : Form
    {
        private string id;
        public TheoryAdmin(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            new AddTheory(null).Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            new AddTheory(TheoryDataGrid[0, TheoryDataGrid.SelectedCells[0].RowIndex].Value.ToString()).Show();
        }
        private void loadInfoTheoryFromDB()
        {
            DB db = new DB();

            TheoryDataGrid.Rows.Clear();

            string query = $"select theory.id, theory.header, theory.content, category.name from theory " +
                $"join category on category.id = theory.idCategory ";

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
                    TheoryDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand($"delete from theory where id = {TheoryDataGrid[0, TheoryDataGrid.SelectedCells[0].RowIndex].Value}", db.getConnection());
            db.openConnection();

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Теория удалена");

            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            db.closeConnection();
            loadInfoTheoryFromDB();
        }

        private void TheoryAdmin_Load(object sender, EventArgs e)
        {
            loadInfoTheoryFromDB();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();

            TheoryDataGrid.Rows.Clear();

            string searchString = $"select theory.id, theory.header, theory.content, category.name from theory " +
                $"join category on category.id = theory.idCategory " +
            $"where concat (theory.id, theory.header, theory.content, category.name) like '%" + SearchTextBox.Text + "%'";

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
                    TheoryDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            new AdminPanel(id).Show();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            loadInfoTheoryFromDB();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            for (int j = 0; j < TheoryDataGrid.Columns.Count; j++)
            {
                if (TheoryDataGrid.Columns[j].Visible)
                {
                    worksheet.Cells[1, j] = TheoryDataGrid.Columns[j].HeaderText;
                }
            }
            for (int i = 0; i < TheoryDataGrid.Rows.Count; i++)
            {
                for (int j = 0; j < TheoryDataGrid.Columns.Count; j++)
                {
                    if (TheoryDataGrid.Columns[j].Visible)
                    {
                        worksheet.Cells[i + 2, j] = TheoryDataGrid.Rows[i].Cells[j].Value;
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
