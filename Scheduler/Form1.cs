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
    public partial class Form1 : Form
    {
        public static List<Employee> employees = new List<Employee>();
        private static String fileLocation = "";
        private static double[] seRatio;
        private static int schedulingDays = 7;
        private static double[] predictedSales;
        private static List<String> positions = new List<string>();
        private SalesInfoDialogue sid = new SalesInfoDialogue();
        private static String[] shifts;

        public Form1()
        {
            InitializeComponent();
            //sid.ShowDialog(this);
            //sid.TopMost = true;

        }

        public static String[] Shifts
        {
            get { return shifts; }
            set { shifts = value; }
        }

        public static double[] PredictedSales
        {
            get { return predictedSales; }
            set { predictedSales = value; }
        }

        public static String FileLocation
        {
            get { return fileLocation; }
            set { fileLocation = value; }
        }
        
        public static String[] Positions
        {
            get
            {
                return positions.ToArray();
            }
        }

        public static double[] seRatioValue
        {
            get { return seRatio; }
            set { seRatio = value; }
        }

        public static int SchedulingDays
        {
            get { return schedulingDays; }
            set { schedulingDays = value; }
        }

        private void openCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sid.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                loadEmployees(fileLocation);
            }
        }

        public void resetEmployees()
        {
            if (MessageBox.Show("Are you sure you wish to reset the employee data?", "Reset Employees?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                employees = new List<Employee>();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
        }

        public void loadEmployees(String filename)
        {
            if (employees.Count != 0)
            {
                resetEmployees();
            }

            String line;
            System.IO.StreamReader sr = new System.IO.StreamReader(filename);

            while ((line = sr.ReadLine()) != null)
            {
                String[] employeeInfo = line.Split(',');
                String[] shift = new String[7];
                bool[] availability = new bool[7];
                for (int x = 3; x < employeeInfo.Length; x++)
                {
                    if (x < shift.Length + 3)
                        availability[x - 3] = Boolean.Parse(employeeInfo[x]);
                    else
                        shift[x - (7 + 3)] = employeeInfo[x];
                }

                if (!positions.Contains(employeeInfo[1]))
                    positions.Add(employeeInfo[1]);

                
                employees.Add(new Employee(employeeInfo[0], employeeInfo[1], employeeInfo[2], availability, shift));
                
            }
            displayEmployees();
            sr.Close();
        }

        public void displayEmployees()
        {
            dataGridView1.Rows.Clear();
            foreach (Employee e in employees)
            {
                List<String> tempString = new List<String>();
                tempString.Add(e.getName());

                String positions = "";
                foreach (String s in e.getPositions())
                {
                    if(positions != "")
                    {
                        positions += "/" + s;
                    } else
                        positions += s;
                }
                tempString.Add(positions);

                for (int x = 0; x < e.getAvailablility().Length; x++ )
                {
                    if (e.getAvailablility()[x])
                        tempString.Add("Yes");
                    else 
                        tempString.Add("No");
                }
                dataGridView1.Rows.Add(tempString.ToArray());
            }
        }

        public void createCustomColumns()
        {
            DateTime today = DateTime.Today;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextTuesday = today.AddDays(daysUntilMonday);

            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("Name", "Name");
            for (int x = 0; x < schedulingDays; x++)
            {
                dataGridView2.Columns.Add(today.AddDays(x).ToShortDateString().ToString(), today.AddDays(x).ToShortDateString().ToString());
            }
            dataGridView2.Refresh();
        }

        public void displaySchedule(Employee[] employee)
        {
            
            foreach (Employee e in employee)
            {
                List<String> tempString = new List<String>();
                tempString.Add(e.getName());
                foreach (String s in e.getWorkSchedule())
                {
                    tempString.Add(s);
                }
                dataGridView2.Rows.Add(tempString.ToArray());
            }
            dataGridView2.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(employees.Count != 0) {
                if (predictedSales == null)
                {
                    DailySalesDialog dsd = new DailySalesDialog();
                    if (dsd.ShowDialog(this) == DialogResult.OK)
                    {
                        SchedulerConfig mainConfig = new SchedulerConfig(seRatio, predictedSales, schedulingDays);
                        if (mainConfig.isPossibleSchedule())
                        {
                            int a = 0;
                            int b = 0;
                            Employee[] employee = mainConfig.createScheduleByWeekVolume();//Employee[] employee = mainConfig.createSchedule2();
                            if (employee != null)
                            {
                                Console.WriteLine("Succeeded " + ++a + " times.");
                                createCustomColumns();
                                employees = new List<Employee>(employee);
                                displaySchedule(employees.ToArray());
                            }
                            else
                                MessageBox.Show("Failed " + ++b + " times.");
                        }
                        else
                            MessageBox.Show("Schedule Not Possible", "Not Possible");
                    }
                }
                else
                {
                    SchedulerConfig mainConfig = new SchedulerConfig(seRatio, predictedSales, schedulingDays);
                    if (mainConfig.isPossibleSchedule())
                    {
                        int a = 0;
                        int b = 0;
                        Employee[] employee = mainConfig.createSchedule2();
                        if (employee != null)
                        {
                            Console.WriteLine("Succeeded " + ++a + " times.");
                            createCustomColumns();
                            employees = new List<Employee>(employee);
                            displaySchedule(employees.ToArray());
                        }
                        else
                            MessageBox.Show("Failed " + ++b + " times.");
                    }
                    else
                        MessageBox.Show("Schedule Not Possible", "Not Possible");
                }

            } else {
                if(MessageBox.Show("No employees loaded. Would you like to load the employee file?", "No Employees Found", 
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                        sid = new SalesInfoDialogue();
                        if (sid.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            loadEmployees(fileLocation);
                        }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewEmployeeDialog ned = new NewEmployeeDialog();
            if (ned.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                displayEmployees();
            }
        }

        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (!System.IO.File.Exists(filename))
                        System.IO.File.Create(filename);
                    else
                    {
                        System.IO.File.Delete(filename);
                        System.IO.File.Create(filename);
                    }
                }
            }
            catch { MessageBox.Show("Could not select file."); }
        }

        private void editSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailySalesDialog d = new DailySalesDialog();
            d.ShowDialog(this);
        }

        private void saveEmployeesToolStripMenuItem_Click(object sender, EventArgs ea)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = "C:\\";
            sfd.RestoreDirectory = true;
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.AddExtension = true;

            if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                String filename = sfd.FileName;
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
                    {

                        foreach (Employee e in employees)
                        {
                            String tempPositions = "";
                            foreach (String s in e.getPositions())
                            {
                                if (tempPositions.Equals(""))
                                    tempPositions += s;
                                else
                                    tempPositions += "/" + s;
                            }

                            String tempString = e.getName() + "," + tempPositions;
                            String tempWage = "";

                            foreach (Double d in e.getWage())
                            {
                                if (tempWage.Equals(""))
                                    tempWage += d;
                                else
                                    tempWage += "/" + d;
                            }

                            tempString += "," + tempWage;

                            for (int x = 0; x < e.getAvailablility().Length; x++)
                                tempString += "," + e.getAvailablility()[x];

                            foreach (String s in e.getShift())
                                tempString += "," + s;

                            sw.WriteLine(tempString);
                        }

                        sw.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("File is already open");
                }
            }
        }
    }
}
