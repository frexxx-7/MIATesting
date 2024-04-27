﻿using MIATesting.AddForms;
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

namespace MIATesting.Forms.Admin
{
    public partial class CategoryAdmin : Form
    {
        private string id;
        public CategoryAdmin(string id)
        {
            InitializeComponent();
            loadInfoCategoryFromDB();
            this.id = id;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            new AddCategory(null).Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            new AddCategory(CategoryDataGrid[0, CategoryDataGrid.SelectedCells[0].RowIndex].Value.ToString()).Show();
        }
        private void loadInfoCategoryFromDB()
        {
            DB db = new DB();

            CategoryDataGrid.Rows.Clear();

            string query = $"select * from category";

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
                    CategoryDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand($"delete from category where id = {CategoryDataGrid[0, CategoryDataGrid.SelectedCells[0].RowIndex].Value}", db.getConnection());
            db.openConnection();

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Категория удаленв");

            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            db.closeConnection();
            loadInfoCategoryFromDB();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            loadInfoCategoryFromDB();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();

            CategoryDataGrid.Rows.Clear();

            string searchString = $"select * from category " +
                $"where concat (name) like '%" + SearchTextBox.Text + "%'";

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
                    CategoryDataGrid.Rows.Add(s);
            }
            db.closeConnection();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
            new AdminPanel(id).Show();
        }
    }
}