﻿using Guna.UI2.WinForms;
using MIATesting.AddForms;
using MIATesting.Classes;
using MIATesting.Classes.Test;
using MIATesting.UserControls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIATesting.Forms
{
    public partial class Tests : Form
    {
        private string idUser;
        public Tests(string idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            new Menu(idUser).Show();
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            new AddTests().Show();
        }

        private void Tests_Load(object sender, EventArgs e)
        {
            GenerateTest();
        }
        private void GenerateTest()
        {
            TestBLL objbll = new TestBLL();

            DataTable dt = objbll.GetItems(null);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    TestControl[] listItems = new TestControl[dt.Rows.Count];

                    for (int i = 0; i < 1; i++)
                    {
                        int panelNumber = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            Guna2Panel panel = new Guna2Panel
                            {
                                Name = $"Test+{panelNumber}",
                                Size = new Size(480, 190),
                            };
                            listItems[i] = new TestControl(idUser, row["id"].ToString(), row["header"].ToString(), this, row["checkedTheory"].ToString(), checkIsPassedTest(row["id"].ToString()));
                            listItems[i].Dock = DockStyle.Top;

                            if (row["image"] != System.DBNull.Value)
                            {
                                MemoryStream ms = new MemoryStream((byte[])row["image"]);
                                listItems[i].image = new Bitmap(ms);
                            }
                            panel.Controls.Add(listItems[i]);
                            ContentPanel.Controls.Add(panel);
                            panelNumber++;
                        }

                    }
                }
            }
        }
        private bool checkIsPassedTest(string idTest)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM passedTest WHERE idUser = {idUser} AND idTest = {idTest}", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
