using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace TBLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double FR(double x)
        {
            double a = Convert.ToDouble(textBox2.Text);

            if (x < -1.0 * Math.Sqrt(Math.Sqrt(2.0)))
            {
                return 0.0;
            }
            else if (x < 0.0)
            {
                return (-1.0 * x * x * x * x + 2.0) / 4.0;
            }
            else if (x < Math.Acos(1.0 - a / 2.0) / a)
            {
                return 1.0 / 2.0 + 1.0 / a - Math.Cos(a * x) / a;
            }
            else
            {
                return 1.0;
            }
        }

        private double pFR(double x)
        {
            double a = Convert.ToDouble(textBox2.Text);

            if (x < -1.0 * Math.Sqrt(Math.Sqrt(2.0)))
            {
                return 0.0;
            }
            else if (x < 0.0)
            {
                return (-1.0 * x * x * x);
            }
            else if (x < Math.Acos(1.0 - a / 2.0) / a)
            {
                return Math.Sin(a * x);
            }
            else
            {
                return 0.0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(textBox1.Text);
            double a = Convert.ToDouble(textBox2.Text);

            LinkList p = new LinkList();

            Random rnd = new Random();

            for (int i = 0; i < n; i++)
            {
                double u = (double)rnd.Next(10000000) / 10000000.0;
                double nu;

                if (u < 0.5)
                {
                    nu = -1.0 * Math.Sqrt(Math.Sqrt(2.0 - 4.0 * u));

                    p.insertup(nu);
                }
                else
                {
                    nu = Math.Acos(1.0 + a / 2.0 - a * u) / a;

                    p.insertup(nu);
                }
            }

            dataGridView1.Rows.Clear();

            p.printDGV(dataGridView1);

            double mo = (-2.0 / 5.0) * Math.Sqrt(Math.Sqrt(2.0)) - ((1.0 - a / 2.0) / (a * a)) * Math.Acos(1.0 - a / 2.0) + (1.0 / (a * a)) * Math.Sqrt(a - a * a / 4.0);
            double x = p.vs(n);
            double d = Math.Sqrt(2.0) / 3.0 - 1.0 / (a * a) + (1.0 / (a * a * a)) * Math.Acos(1.0 - a / 2.0) * (Math.Sqrt(a - a * a / 4.0) - (1.0 - a / 2.0) * Math.Acos(1.0 - a / 2.0));
            double s = p.vd(n, x);
            double me = p.vm(n);
            double r = p.rv();

            dataGridView2.Rows[0].Cells[0].Value = mo;
            dataGridView2.Rows[0].Cells[1].Value = x;
            dataGridView2.Rows[0].Cells[2].Value = Math.Abs(mo - x);
            dataGridView2.Rows[0].Cells[3].Value = d;
            dataGridView2.Rows[0].Cells[4].Value = s;
            dataGridView2.Rows[0].Cells[5].Value = Math.Abs(d - s);
            dataGridView2.Rows[0].Cells[6].Value = me;
            dataGridView2.Rows[0].Cells[7].Value = r;

            GraphPane panel = zedGraphControl1.GraphPane;
            panel.CurveList.Clear();

            double dd = p.artGraph(zedGraphControl1, FR, n);

            double a0 = p.leftgr();
            double b0 = p.rightgr();

            double h = (b0 - a0) / 1000.0;

            PointPairList f_list = new PointPairList();

            for (int j = 0; j < 1000; j++)
            {
                double xx = a0 + j * h;
                f_list.Add(xx, FR(xx));
            }

            f_list.Add(b0, FR(b0));

            LineItem Curve = panel.AddCurve("F(x)", f_list, Color.Red, SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            dataGridView2.Rows[0].Cells[8].Value = dd;

            int k = Convert.ToInt32(textBox3.Text);

            double[] gran = new double[k];

            for (int i = 0; i < k - 1; i++)
            {
                gran[i] = Convert.ToDouble(dataGridView4.Rows[i].Cells[0].Value);
            }

            double ddd = p.gistogram(gran, dataGridView3, zedGraphControl2, pFR, n);

            dataGridView2.Rows[0].Cells[9].Value = ddd;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();

            if (textBox3.Text != "")
            {
                int k = Convert.ToInt32(textBox3.Text);

                for (int i = 0; i < k - 2; i++)
                {
                    dataGridView4.Rows.Add();
                }
            }
        }
    }
}
