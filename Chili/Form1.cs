using Chili.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chili
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Parser parser = new Parser(openFileDialog1.FileName);
                    RenderData RData = parser.Parse();
                    Render R = new Render(RData);
                    R.RenderAll(/*path to PNG file*/);
                    MessageBox.Show("Image.png", "Image successfully creted", MessageBoxButtons.OK);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

        //test










    }
}
