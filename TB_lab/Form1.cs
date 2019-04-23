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

        private double gamma2(int r)
        {
            double f = 1.0;
            double ff = (double)r / 2.0;

            if (r % 2 == 0)
            {
                for (int i = 1; i < r / 2 - 1; i++)
                    f *= i;

                return f;
            }
            else
            {
                while (ff != 0.5)
                {
                    f *= ff - 1.0;
                    ff--;
                }

                f *= Math.Sqrt(Math.PI);

                return f;
            }
        }

        private double hi2(double x, int r)
        {
            if (x <= 0.0)
                return 0.0;
            else
            {
                return Math.Pow(2.0, -1 * (double)r / 2.0) * Math.Pow(x, (double)r / 2.0 - 1) * Math.Exp(-1 * x / 2.0) / gamma2(r);
            }
        }

        private double inthi(double a, double b, int n, int r)
        {
            double sum = 0.0;

            for (int i = 1; i <= n; i++)
            {
                sum += (hi2(a + (b - a) * (i - 1) / (double)n, r) + hi2(a + (b - a) * i / (double)n, r)) * (b - a) / ((double)n * 2.0);
            }

            return sum;
        }

        private double intpFR(double z1, double z2)
        {
            double a = Convert.ToDouble(textBox2.Text);
            double kor = -1.0 * Math.Sqrt(Math.Sqrt(2.0));
            double acos = Math.Acos(1.0 - a / 2.0) / a;

            if (z2 <= kor)
            {
                return 0.0;
            }
            else if (z2 <= 0.0)
            {
                if (z1 <= kor)
                {
                    return -0.25 * (z2 * z2 * z2 * z2 - 2.0);
                }
                else
                {
                    return -0.25 * (z2 * z2 * z2 * z2 - z1 * z1 * z1 * z1);
                }
            }
            else if (z2 <= acos)
            {
                if (z1 <= 0.0)
                {
                    return ((0.25 * z1 * z1 * z1 * z1) + ((-1.0 / a) * (Math.Cos(a * z2) - 1.0)));
                }
                else
                {
                    return ((-1.0 / a) * (Math.Cos(a * z2) - Math.Cos(a * z1)));
                }
            }
            else if (z1 <= acos)
            {
                return ((-1.0 / a) * ((1.0 - 0.5 * a) - Math.Cos(a * z1)));
            }
            else
            {
                return 0.0;
            }
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

            if (!checkBox1.Checked)
            {
                for (int i = 0; i < k - 1; i++)
                {
                    gran[i] = Convert.ToDouble(dataGridView4.Rows[i].Cells[0].Value);
                }

                gran[k - 1] = b0;
            }
            else
            {
                h = (b0 - a0) / (double)k;

                for (int j = 0; j < k - 1; j++)
                {
                    gran[j] = a0 + (j + 1) * h;

                    dataGridView4.Rows[j].Cells[0].Value = gran[j];
                }

                gran[k - 1] = b0;
            }

            dataGridView3.Rows.Clear();

            double ddd = p.gistogram(gran, dataGridView3, zedGraphControl2, pFR, n);

            dataGridView2.Rows[0].Cells[9].Value = ddd;

            double r0 = p.teoria(gran, dataGridView4, intpFR, n);
            double f_ = 1 - inthi(0.0, r0, n, k - 1);
            double alpha = Convert.ToDouble(textBox4.Text);

            if (f_ < alpha)
            {
                label5.Text = "Решение: отвергаем";
            }
            else
            {
                label5.Text = "Решение: принимаем";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();

            if (textBox3.Text != "")
            {
                int k = Convert.ToInt32(textBox3.Text);

                for (int i = 0; i < k - 1; i++)
                {
                    dataGridView4.Rows.Add();
                }
            }
        }
    }
}
