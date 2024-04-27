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

namespace MIATesting.AddForms
{
    public partial class AddCategory : Form
    {
        private string idCategory;
        public AddCategory(string idCategory)
        {
            InitializeComponent();
            this.idCategory = idCategory;

            if (idCategory != null)
            {
                label1.Text = "Изменить категорию";
                AddButton.Text = "Изменить";
                loadInfoCategory();
            }
        }
        private void loadInfoCategory()
        {
            DB db = new DB();
            string queryInfo = $"SELECT * FROM category WHERE id = '{idCategory}'";
            MySqlCommand mySqlCommand = new MySqlCommand(queryInfo, db.getConnection());

            db.openConnection();

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                NameTextBox.Text = reader["name"].ToString();
            }
            reader.Close();

            db.closeConnection();
        }
        private void CanceledButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            if (idCategory == null)
            {
                MySqlCommand command = new MySqlCommand($"INSERT into category (name) values(@name)", db.getConnection());
                command.Parameters.AddWithValue("@name", NameTextBox.Text);
                db.openConnection();
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Категория добавлена");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                db.closeConnection();
            }
            else
            {
                MySqlCommand command = new MySqlCommand($"Update category set name = @name where id = {idCategory}", db.getConnection());
                command.Parameters.AddWithValue("@name", NameTextBox.Text);
                db.openConnection();

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Категория изменена");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                db.closeConnection();
            }
        }
    }
}
