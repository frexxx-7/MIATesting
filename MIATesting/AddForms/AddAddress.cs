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
    public partial class AddAddress : Form
    {
        private string idAddress;
        public AddAddress(string idAddress)
        {
            InitializeComponent();
            this.idAddress = idAddress;

            if (idAddress!=null)
            {
                label5.Text = "Изменить адрес";
                AddButton.Text = "Сохранить";
                loadInfoAddress();
            }
        }

        private void loadInfoAddress()
        {
            DB db = new DB();
            string queryInfo = $"SELECT * FROM address WHERE id = '{idAddress}'";
            MySqlCommand mySqlCommand = new MySqlCommand(queryInfo, db.getConnection());

            db.openConnection();

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                HouseTextBox.Text = reader["house"].ToString();
                StreetTextBox.Text = reader["street"].ToString();
                CityTextBox.Text = reader["city"].ToString();
                CountryTextBox.Text = reader["country"].ToString();
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
            if (idAddress==null)
            {
                MySqlCommand command = new MySqlCommand($"INSERT into address (house, street, city, country) values(@house, @street, @city, @country)", db.getConnection());
                command.Parameters.AddWithValue("@house", HouseTextBox.Text);
                command.Parameters.AddWithValue("@street", StreetTextBox.Text);
                command.Parameters.AddWithValue("@city", CityTextBox.Text);
                command.Parameters.AddWithValue("@country", CountryTextBox.Text);
                db.openConnection();

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Адрес добавлен");
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                db.closeConnection();
            }
            else
            {
                MySqlCommand command = new MySqlCommand($"Update address set house = @house, street = @street, city = @city, country = @country where id = {idAddress}", db.getConnection());
                command.Parameters.AddWithValue("@house", HouseTextBox.Text);
                command.Parameters.AddWithValue("@street", StreetTextBox.Text);
                command.Parameters.AddWithValue("@city", CityTextBox.Text);
                command.Parameters.AddWithValue("@country", CountryTextBox.Text);
                db.openConnection();

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Адрес изменен");
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
