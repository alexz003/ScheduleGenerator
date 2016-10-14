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
    public partial class NewEmployeeDialog : Form
    {
        private Label[] dayLabel;
        private Label[] shiftLabel;

        private CheckBox[] availableCheckBox;
        private CheckBox[] multishiftCheckBox;

        private ComboBox[] shiftAvailable;

        private List<String> lBoxItems = new List<String>();

        public NewEmployeeDialog()
        {
            InitializeComponent();
            InitializeDataComponents();
        }

        public void InitializeDataComponents()
        {
            button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            dayLabel = new Label[Form1.SchedulingDays];
            shiftLabel = new Label[dayLabel.Length];

            availableCheckBox = new CheckBox[dayLabel.Length];
            multishiftCheckBox = new CheckBox[dayLabel.Length];

            shiftAvailable = new ComboBox[dayLabel.Length];
            for (int x = 0; x < dayLabel.Length; x++)
            {
                dayLabel[x] = new Label();
                shiftLabel[x] = new Label();

                availableCheckBox[x] = new CheckBox();
                multishiftCheckBox[x] = new CheckBox();

                shiftAvailable[x] = new ComboBox();

                panel3.Controls.Add(dayLabel[x]);
                panel3.Controls.Add(availableCheckBox[x]);
                panel3.Controls.Add(shiftLabel[x]);
                panel3.Controls.Add(shiftAvailable[x]);
                panel3.Controls.Add(multishiftCheckBox[x]);

                if (x == 0)
                {
                    dayLabel[x].Location = new Point(15, 25);
                    dayLabel[x].Text = "Day " + (x + 1);
                    dayLabel[x].Name = "label " + x;
                    dayLabel[x].AutoSize = true;

                    availableCheckBox[x].Location = new Point(56, 24);
                    availableCheckBox[x].Text = "Available";
                    availableCheckBox[x].Name = "checkbox 1";
                    availableCheckBox[x].AutoSize = true;

                    shiftLabel[x].Location = new Point(131, 25);
                    shiftLabel[x].Text = "Shift";
                    shiftLabel[x].Name = "label";
                    shiftLabel[x].AutoSize = true;

                    shiftAvailable[x].Location = new Point(165, 22);
                    shiftAvailable[x].Items.Add("Any");

                    foreach (String s in Form1.Shifts)
                        shiftAvailable[x].Items.Add(s);

                    shiftAvailable[x].Items.Add("None");
                    shiftAvailable[x].Name = "combobox";
                    shiftAvailable[x].AutoSize = true;

                    multishiftCheckBox[x].Location = new Point(300, 24);
                    multishiftCheckBox[x].Text = "Multiple Shifts";
                    multishiftCheckBox[x].Name = "checkbox 1";
                    multishiftCheckBox[x].AutoSize = true;
                }
                else
                {
                    dayLabel[x].Location = new Point(15, dayLabel[x-1].Location.Y + 26);
                    dayLabel[x].Text = "Day " + (x + 1);
                    dayLabel[x].AutoSize = true;

                    availableCheckBox[x].Location = new Point(56, availableCheckBox[x - 1].Location.Y + 26);
                    availableCheckBox[x].Text = "Available";
                    availableCheckBox[x].AutoSize = true;

                    shiftLabel[x].Location = new Point(131, shiftLabel[x - 1].Location.Y + 26);
                    shiftLabel[x].Text = "Shift";
                    shiftLabel[x].AutoSize = true;

                    shiftAvailable[x].Location = new Point(165, shiftAvailable[x - 1].Location.Y + 26);
                    shiftAvailable[x].Items.Add("Any");

                    foreach (String s in Form1.Shifts)
                        shiftAvailable[x].Items.Add(s);

                    shiftAvailable[x].Items.Add("None");
                    shiftAvailable[x].AutoSize = true;

                    multishiftCheckBox[x].Location = new Point(300, multishiftCheckBox[x-1].Location.Y + 26);
                    multishiftCheckBox[x].Text = "Multiple Shifts";
                    multishiftCheckBox[x].Name = "checkbox 1";
                    multishiftCheckBox[x].AutoSize = true;
                }
            }
            base.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                lBoxItems.Add(textBox3.Text + ",$" + textBox4.Text);
                listBox1.DataSource = null;
                listBox1.DataSource = lBoxItems;
            }
            else
                MessageBox.Show("Nothing to add to selection");
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            String positions = "";
            String wage = "";
            foreach(String st in listBox1.Items) {
                if(positions.Equals(""))
                    positions += st.Substring(0, st.IndexOf(','));
                else
                    positions += "/" + st.Substring(0, st.IndexOf(','));

                if(wage.Equals(""))
                    wage += st.Substring(st.IndexOf('$') + 1);
                else
                    wage += "/" + st.Substring(st.IndexOf('$') + 1);
            }

            bool[] availability = new bool[dayLabel.Length];
            String[] shift = new String[dayLabel.Length];
       
            for(int x = 0; x < dayLabel.Length; x++) {
                availability[x] = availableCheckBox[x].Checked;
                shift[x] = shiftAvailable[x].Text;
            }

            Employee newEmployee = new Employee(textBox1.Text + " " + textBox2.Text, positions, wage, availability, shift);
            Form1.employees.Add(newEmployee);
            this.Dispose();
        }
    }
}
