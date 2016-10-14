using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler
{
    public partial class SalesInfoDialogue : Form
    {

        private List<String> lBoxItems = new List<string>();

        public SalesInfoDialogue()
        {
            InitializeComponent();
            InitializeDataComponents();
            button2.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void InitializeDataComponents()
        {
            this.DesktopLocation = new Point(base.Location.X, base.Location.Y);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    String filename = null;
                    SaveFileDialog sfd = new SaveFileDialog();

                    sfd.InitialDirectory = "C:\\";
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FilterIndex = 2;
                    sfd.RestoreDirectory = true;

                    if ((filename = sfd.FileName) != null)
                    {
                        textBox1.Text = filename;
                        if (!System.IO.File.Exists(filename))
                            System.IO.File.Create(filename);
                        else 
                        {
                            System.IO.File.Delete(filename);
                            System.IO.File.Create(filename);
                        }
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                }
                catch { MessageBox.Show("Could not select file."); }
            }
            else
            {
                String filename = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((filename = openFileDialog1.FileName) != null)
                        {
                            textBox1.Text = filename;
                        }
                        else
                        {
                            textBox1.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
                else
                {
                    textBox1.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            if (System.IO.File.Exists(textBox1.Text))
            {
                int num;
                if (int.TryParse(domainUpDown1.Text, out num) && num != 0)
                {
                    Form1.SchedulingDays = num;
                    Form1.FileLocation = textBox1.Text;
                    Form1.Shifts = new String[lBoxItems.Count];

                    for (int x = 0; x < lBoxItems.Count; x++)
                        Form1.Shifts[x] = lBoxItems.ToArray()[x];
                }
                else
                    MessageBox.Show("Not a valid work days", "Invalid Work Days");
            }
            else
            {
                MessageBox.Show("Not a valid file.", "Invalid File");
            }
            
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                lBoxItems.Add(textBox2.Text);
                listBox1.DataSource = null;
                listBox1.DataSource = lBoxItems;
            }
            else
                MessageBox.Show("Nothing to add to selection");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;

            try
            {
                // Remove the item in the List.
                lBoxItems.RemoveAt(selectedIndex);
            }
            catch
            {
            }

            listBox1.DataSource = null;
            listBox1.DataSource = lBoxItems;
        }
    }
}
