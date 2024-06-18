using MIATesting.Classes;
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
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace MIATesting.Forms
{
    public partial class PassedTestInfo : Form
    {
        private string header, result, idTest;
        private string textTest;
        private string answers;

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PassedTestInfo_Load(object sender, EventArgs e)
        {
            loadTestInfo();
        }

        private void OutputButton_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();

            // Open source document
            Document sourceDoc = wordApp.Documents.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Шаблон.docx"));

            // Copy content from source document
            sourceDoc.Content.Copy();
            sourceDoc.Close(false);

            // Create a new target document
            Document targetDoc = wordApp.Documents.Add();
            targetDoc.Content.Paste();

            // Fill bookmarks
            FillBookmark(targetDoc, "Название", header);
            FillBookmark(targetDoc, "Результат", result);
            FillBookmark(targetDoc, "Тест", textTest);
            FillBookmark(targetDoc, "Ответы", answers);

            // Show SaveFileDialog to get the save path from the user
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx",
                Title = "Сохранить скопированный документ в"
            };
            saveFileDialog1.ShowDialog();
            string targetPath = saveFileDialog1.FileName;

            // Save the document to the selected path
            if (!string.IsNullOrEmpty(targetPath))
            {
                targetDoc.SaveAs2(targetPath);
            }

            // Close the document and Word application
            targetDoc.Close(false);
            wordApp.Quit();

            // Re-open the saved document to make it visible to the user
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            Document wordDocument = wordApplication.Documents.Open(targetPath);
            wordApplication.Visible = true;
        }

        private void FillBookmark(Document doc, string bookmarkName, string text)
        {
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                Bookmark bookmark = doc.Bookmarks[bookmarkName];
                Range range = bookmark.Range;
                range.Text = text;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public PassedTestInfo(string header, string result, string idTest, string answers)
        {
            InitializeComponent();

            this.result = result;
            this.header = header;
            this.idTest = idTest;

            HeaderLabel.Text = header;
            ResultLabel.Text = result;
            this.answers = answers;
        }
        private void loadTestInfo()
        {
            string pathFile = AppDomain.CurrentDomain.BaseDirectory + "dataTest.txt";

            using (StreamWriter writer = new StreamWriter(pathFile, false))
            {
                writer.Write("");
            }
            DB db = new DB();

            string queryInfo = $"select * from test where test.id = {idTest}";

            MySqlCommand mySqlCommand = new MySqlCommand(queryInfo, db.getConnection());

            db.openConnection();

            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            using (StreamWriter writer = new StreamWriter(pathFile, true))
            {
                while (reader.Read())
                {
                    HeaderLabel.Text = reader["header"].ToString();

                    writer.WriteLine(reader.GetString(reader.GetOrdinal("dataTest")));
                }
            }
            reader.Close();

            db.closeConnection();
            string[] lines = File.ReadAllLines(pathFile);

            foreach (string line in lines)
            {
                if (line.Length != 0)
                {
                    string[] parts = line.Split('|');
                    if (parts[0] == "question")
                    {
                        textTest += $"\n Вопрос {parts[1]} {parts[2]} \n";
                    }
                    else
                    {
                        if (bool.Parse(parts[2]))
                        {
                            textTest += $"{parts[1]} Правильный ответ \n";
                        }
                        if (bool.Parse(parts[2]) == false)
                        {
                            textTest += $"{parts[1]} \n";
                        }
                    }
                }
            }
        }
    }
}
