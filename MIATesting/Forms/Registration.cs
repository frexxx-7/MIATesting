﻿using MIATesting.Classes;
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

namespace MIATesting.Forms
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            new Autorization().Show();
            this.Close();
        }

        private void RegistrationButton_Click(object sender, EventArgs e)
        {

            if (LoginTextBox.Text == "Admin" || LoginTextBox.Text == "admin")
            {
                MessageBox.Show("Логин 'Admin' недоступен");
            }
            else
            if (PasswordTextBox.Text != RepeatPasswordTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают");
            }
            else
            {
                DB db = new DB();

                MySqlCommand command = new MySqlCommand("INSERT INTO users (login, password) " +
                    "VALUES (@login, @password)", db.getConnection());

                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = LoginTextBox.Text;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PasswordTextBox.Text;

                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Аккаунт создан!");
                    Autorization auth = new Autorization();
                    auth.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка создания аккаунта");
                }

                db.closeConnection();
            }
        }
    }
}
