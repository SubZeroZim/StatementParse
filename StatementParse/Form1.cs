using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatementParse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_savings1_Click(object sender, EventArgs e)
        {
            List<string> emortelleLines = new List<string>();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text Files (.txt)|*.txt";
            openFile.Title = "Select the Emortelle Transaction Reports";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(openFile.FileName))
                    {
                        string line = "";
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            emortelleLines.Add(line);
                        }
                    }
                    GetTransactionLines(GetFixedWidth(emortelleLines), emortelleLines);
                    emortelleLines.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read the file. Original error: " + ex.Message);
                }
            }
        }



        /* Extra Functions */

        

        private List<KeyValuePair<int, int>> GetFixedWidth(List<string> emortelleLines)
        {
            string fixedWidthLine = emortelleLines[7];
            var fixedWidthListLines = fixedWidthLine.Split(' ').Select(g => g.Count()).ToList();
            var fixedWidthList = new List<KeyValuePair<int, int>>();

            int startIndex = 0;

            for (int i = 0; i < fixedWidthListLines.Count(); i++)
            {
                var pair = new KeyValuePair<int, int>(startIndex, fixedWidthListLines[i]);
                fixedWidthList.Add(pair);
                startIndex += fixedWidthListLines[i] + 1;
            }
            return fixedWidthList;
        }

    }
}
