﻿using System;
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
    public partial class Menu : Form
    {
        public static string login;
        private string id;
        public Menu(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TheoryButton_Click(object sender, EventArgs e)
        {
            new Theory(id).Show();
            this.Close();
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            new AdminPanel(id).Show();
        }
    }
}