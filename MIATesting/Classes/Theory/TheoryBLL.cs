using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace MIATesting.Classes.Theory
{
    internal class TheoryBLL
    {
        public bool SaveItem(string header, string content, Image img, string idCategory)
        {
            try
            {
                TheoryFunc objdal = new TheoryFunc();
                return objdal.AddItemsToTable(header, content, img, idCategory);
            }
            catch (Exception e)
            {
                DialogResult result = MessageBox.Show(e.Message.ToString());
                return false;
            }
        }

        public DataTable GetItems()
        {
            try
            {
                TheoryFunc objdal = new TheoryFunc();
                return objdal.ReadItems();
            }
            catch (Exception e)
            {
                DialogResult result = MessageBox.Show(e.Message.ToString());
                return null;
            }
        }
    }
}
