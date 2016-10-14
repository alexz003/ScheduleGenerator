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
    public partial class DailySalesDialog : Form
    {
        private double[] salesInfo;
        private TextBox[] dailyBoxes, positionBoxes;
        private Label[] dailyLabels, positionLabels;


        public DailySalesDialog()
        {
            InitializeComponent();
            InitializeDataComponents();
        }

        public void InitializeDataComponents()
        {
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            salesInfo = new double[Form1.SchedulingDays];

            dailyBoxes = new TextBox[salesInfo.Length];
            positionBoxes = new TextBox[Form1.Positions.Length];

            dailyLabels = new Label[salesInfo.Length];
            positionLabels = new Label[Form1.Positions.Length];

            for (int x = 0; x < salesInfo.Length; x++)
            {
                if (x == 0)
                {
                    dailyLabels[x] = new Label();
                    dailyBoxes[x] = new TextBox();

                    panel1.Controls.Add(dailyLabels[x]);
                    panel1.Controls.Add(dailyBoxes[x]);

                    dailyLabels[x].AutoSize = true;
                    dailyLabels[x].Location = new System.Drawing.Point(3, 6);
                    dailyLabels[x].Name = "label" + x;
                    dailyLabels[x].Text = "Day " + (x + 1);

                    dailyBoxes[x].Size = new Size(72, 20);
                    dailyBoxes[x].Location = new System.Drawing.Point(68, 3);
                    dailyBoxes[x].Name = "textBox" + x;
                    dailyBoxes[x].TabIndex = x;

                }
                else
                {
                    dailyLabels[x] = new Label();
                    dailyBoxes[x] = new TextBox();

                    panel1.Controls.Add(dailyLabels[x]);
                    panel1.Controls.Add(dailyBoxes[x]);

                    dailyLabels[x].AutoSize = true;
                    dailyLabels[x].Location = new System.Drawing.Point(3, dailyLabels[x - 1].Location.Y + 26);
                    dailyLabels[x].Name = "label" + x;
                    dailyLabels[x].Text = "Day " + (x + 1);

                    dailyBoxes[x].Size = new Size(72, 20);
                    dailyBoxes[x].Location = new System.Drawing.Point(68, dailyBoxes[x - 1].Location.Y + 26);
                    dailyBoxes[x].Name = "textBox" + x;
                    dailyBoxes[x].TabIndex = x;
                }
            }

            for (int x = 0; x < Form1.Positions.Length; x++)
            {
                positionLabels[x] = new Label();
                positionBoxes[x] = new TextBox();

                panel2.Controls.Add(positionLabels[x]);
                panel2.Controls.Add(positionBoxes[x]);

                if (x == 0)
                {
                    positionLabels[x].Location = new Point(3, 6);
                    positionLabels[x].AutoSize = true;
                    positionLabels[x].Text = Form1.Positions[x];

                    positionBoxes[x].Location = new Point(68, 3);
                    positionBoxes[x].Size = new Size(72, 20);
                    positionBoxes[x].TabIndex = x;

                }
                else
                {
                    positionLabels[x].Location = new Point(3, positionLabels[x].Location.Y + 26);
                    positionLabels[x].AutoSize = true;
                    positionLabels[x].Text = Form1.Positions[x];

                    positionBoxes[x].Location = new Point(68, positionBoxes[x].Location.Y + 26);
                    positionBoxes[x].Size = new Size(72, 20);
                    positionBoxes[x].TabIndex = x;
                }
            }
                base.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] sales = new double[dailyBoxes.Length];
            for (int x = 0; x < dailyBoxes.Length; x++)
            {
                double value;
                if (dailyBoxes[x].Text != "" && Double.TryParse(dailyBoxes[x].Text, out value) && value != 0)
                    sales[x] = value;
                else
                    MessageBox.Show("One of the boxes was incorrectly filled out.", "Invalid Data");
            }
            
            double[] seRatio = new double[Form1.Positions.Length];

            for (int x = 0; x < Form1.Positions.Length; x++)
            {
                double value;
                if (positionBoxes[x].Text != "" && Double.TryParse(positionBoxes[x].Text, out value) && value != 0)
                    seRatio[x] = value;
                else
                    MessageBox.Show("One of the boxes was incorrectly filled out.", "Invalid Data");
            }

            Form1.seRatioValue = seRatio;
            Form1.PredictedSales = sales;
        }
    }
}
