using MIATesting.Classes;
using MIATesting.Classes.Theory;
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
    public partial class AddTheory : Form
    {
        Image image;
        private string idTheory;
        public AddTheory(string idTheory)
        {
            InitializeComponent();
            this.idTheory = idTheory;

            loadInfoAboutCategory();

            if (idTheory != null)
            {
                label1.Text = "Изменить теорию";
                AddButton.Text = "Изменить";
                loadInfoTheory();
            }
        }

        private void loadInfoTheory()
        {
            DB db = new DB();
            string queryInfo = $"SELECT * FROM theory WHERE id = '{idTheory}'";
            MySqlCommand mySqlCommand = new MySqlCommand(queryInfo, db.getConnection());

            db.openConnection();

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                HeaderTextBox.Text = reader["header"].ToString();
                ContentTextBox.Text = reader["content"].ToString();
                for (int i = 0; i < CategoryComboBox.Items.Count; i++)
                {
                    if (reader["idCategory"].ToString() != "")
                    {
                        if (Convert.ToInt32((CategoryComboBox.Items[i] as ComboBoxItem).Value) == Convert.ToInt32(reader["idCategory"]))
                        {
                            CategoryComboBox.SelectedIndex = i;
                        }
                    }
                }

            }
            reader.Close();

            db.closeConnection();
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            new AddCategory(null).Show();
        }
        private void loadInfoAboutCategory()
        {
            DB db = new DB();
            string queryInfo = $"SELECT * FROM category ";
            MySqlCommand mySqlCommand = new MySqlCommand(queryInfo, db.getConnection());

            db.openConnection();

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = $" {reader[1]}";
                item.Value = reader[0];
                CategoryComboBox.Items.Add(item);
            }
            reader.Close();
        }

        private void AddTheory_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(opendlg.FileName);
                pictureBox1.Image = image;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (idTheory == null)
            {
                TheoryBLL objbll = new TheoryBLL();

                if (HeaderTextBox.Text.Length == 0 || ContentTextBox.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые данные введены некорректно");
                }
                else
                if (objbll.SaveItem(HeaderTextBox.Text, ContentTextBox.Text, image, (CategoryComboBox.SelectedItem as ComboBoxItem).Value.ToString()))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка!");
                }
            }
            else
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand($"Update theory set header = @header, content = @content, idCategory = @idCategory where id = {idTheory}", db.getConnection());
                command.Parameters.AddWithValue("@header", HeaderTextBox.Text);
                command.Parameters.AddWithValue("@content", ContentTextBox.Text);
                command.Parameters.AddWithValue("@idCategory", (CategoryComboBox.SelectedItem as ComboBoxItem).Value);
                db.openConnection();

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Теория изменена");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                db.closeConnection();
            }
        }

        private void CanceledButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
