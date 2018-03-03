using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatementParse
{
    public partial class Form1 : Form
    {
        string memberName = "";
        string accountNumber = "";
        List<Double> startingBalances = new List<Double>();
        List<KeyValuePair<int, int>> fixedWidthKeyPair = new List<KeyValuePair<int, int>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_savings1_Click(object sender, EventArgs e)
        {
            HistoryTransaction transaction = new HistoryTransaction();
            //List<string> emortelleLines = new List<string>();            
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text Files (.txt)|*.txt";
            openFile.Title = "Select the Emortelle Transaction Reports";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pb_import.Visible = true;
                pb_import.Minimum = 1;
                pb_import.Maximum = 502330;
                pb_import.Value = 1;
                pb_import.Step = 1;

                try
                {
                    using (StreamReader streamReader = new StreamReader(openFile.FileName))
                    {
                        string line = "";

                        if (!File.Exists(Path.GetDirectoryName(openFile.FileName) + "\\savings1.txt"))
                        {
                            // Create a file to write to.
                            StreamWriter sw = File.CreateText(Path.GetDirectoryName(openFile.FileName) + "\\savings1.txt");
                            sw.Close();
                        }

                        while ((line = streamReader.ReadLine()) != null)
                        {

                            transaction = parseLine(line, fixedWidthKeyPair);
                            if (transaction.tran_date != null)
                            {
                                using (StreamWriter sw = File.AppendText(Path.GetDirectoryName(openFile.FileName) + "\\savings1.txt"))
                                {
                                    sw.WriteLine(transaction.account_number + "|" + transaction.name + "|" + transaction.tran_date + "|" + transaction.tran_code + "|" + transaction.account_id
                                        + "|" + transaction.description + "|" + transaction.tran_amount + "|" + transaction.balance);
                                    sw.Close();
                                }
                            }
                            pb_import.PerformStep();
                        }
                        streamReader.Close();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read the file. Original error: " + ex.Message);
                }
            }
        }



        /* Extra Functions */

        

        private List<KeyValuePair<int, int>> GetFixedWidth(string currentLine)
        {
            string fixedWidthLine = currentLine;
            var fixedWidthListLines = fixedWidthLine.Split(' ').Select(g => g.Count()).ToList();
            var fixedWidthList = new List<KeyValuePair<int, int>>();

            int startIndex = 0;

            for (int i = 0; i < fixedWidthListLines.Count(); i++)
            {
                if (i == 3)
                {
                    var pair = new KeyValuePair<int, int>(startIndex-1, fixedWidthListLines[i]+1);
                    fixedWidthList.Add(pair);
                    startIndex += fixedWidthListLines[i] + 1;
                }

                else
                {
                    var pair = new KeyValuePair<int, int>(startIndex, fixedWidthListLines[i]);
                    fixedWidthList.Add(pair);
                    startIndex += fixedWidthListLines[i] + 1;
                }
            }
            return fixedWidthList;
        }

        private HistoryTransaction parseLine(string currentLine, List<KeyValuePair<int, int>> keypair)
        {
            HistoryTransaction transaction = new HistoryTransaction();

            if (Regex.IsMatch(currentLine, @"^Name    :"))
            {
                Regex patternName = new Regex(@"^Name    : (.+)\s{2}");
                Match match = patternName.Match(currentLine);

                if (match.Success)
                {
                    memberName = match.Groups[1].Value.Trim();
                }                
            }

            if (Regex.IsMatch(currentLine, @"Account :"))
            {
                Regex patternAccount = new Regex(@"Account : (\d{8})");
                Match match = patternAccount.Match(currentLine);

                if (match.Success)
                {
                    accountNumber = match.Groups[1].Value;
                }
            }

            if (Regex.IsMatch(currentLine, @"^======"))
            {
                fixedWidthKeyPair = GetFixedWidth(currentLine);
            }

            if (Regex.IsMatch(currentLine, @"^Start"))
            {
                var partsBal = keypair.Select(pair => currentLine.Substring(pair.Key, pair.Value)).ToList();

                for (int i = 4; i < partsBal.Count; i++)
                {
                    startingBalances.Add(Double.Parse(partsBal[i]));
                }
            }

            if (Regex.IsMatch(currentLine, @"^\d{6}"))
            {                
                var parts = keypair.Select(pair => currentLine.Substring(pair.Key, pair.Value)).ToList();

                transaction.account_number = accountNumber;
                transaction.name = memberName;
                transaction.tran_date = parts[0];
                transaction.tran_code = parts[1];
                transaction.description = parts[2];
                //transaction.tran_amount = Double.Parse(parts[3]);

                for (int i = 4; i < parts.Count; i++)
                {
                    if (parts[i].Trim() != "")
                    {
                        switch (i)
                        {
                            case 4:
                                transaction.account_id = "S00";

                                if (startingBalances[0] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[0] = Double.Parse(parts[i]);

                                break;

                            case 5:
                                transaction.account_id = "S01";

                                if (startingBalances[1] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[1] = Double.Parse(parts[i]);

                                break;
                            case 6:
                                transaction.account_id = "D00";

                                if (startingBalances[2] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[2] = Double.Parse(parts[i]);

                                break;

                            case 7:
                                transaction.account_id = "D01";

                                if (startingBalances[3] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[3] = Double.Parse(parts[i]);

                                break;

                            case 8:
                                transaction.account_id = "D02";

                                if (startingBalances[4] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[4] = Double.Parse(parts[i]);

                                break;

                            case 9:
                                transaction.account_id = "D03";

                                if (startingBalances[5] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[5] = Double.Parse(parts[i]);

                                break;

                            case 10:
                                transaction.account_id = "D04";

                                if (startingBalances[6] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[6] = Double.Parse(parts[i]);

                                break;

                            case 11:
                                transaction.account_id = "D05";

                                if (startingBalances[7] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[7] = Double.Parse(parts[i]);

                                break;

                            case 12:
                                transaction.account_id = "D06";

                                if (startingBalances[8] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[8] = Double.Parse(parts[i]);

                                break;

                            case 13:
                                transaction.account_id = "D07";

                                if (startingBalances[9] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[9] = Double.Parse(parts[i]);

                                break;

                            case 14:
                                transaction.account_id = "D08";

                                if (startingBalances[10] > Double.Parse(parts[i]))
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]) * -1;
                                }

                                else
                                {
                                    transaction.tran_amount = Double.Parse(parts[3]);
                                }

                                startingBalances[10] = Double.Parse(parts[i]);

                                break;
                        }

                        transaction.balance = Double.Parse(parts[i]);
                    }
                }
            }

            return transaction;
        }

    }
}
