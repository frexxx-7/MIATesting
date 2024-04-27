using MIATesting.Forms.Admin;
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
    public partial class AdminPanel : Form
    {
        private string id;
        public AdminPanel(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TheoryButton_Click(object sender, EventArgs e)
        {
            new TheoryAdmin(id).Show();
            this.Close();
        }

        private void TestsButton_Click(object sender, EventArgs e)
        {
            new TestsAdmin(id).Show();
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            new CategoryAdmin(id).Show(); 
            this.Close();
        }
    }
}
