using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace TBLab
{
    class node
    {
        public double data;
        public node next;

        public node(double data_, node next_)
        {
            data = data_;
            next = next_;
        }
    }

    class LinkList
    {
        private node head;

        public LinkList()
        {
            head = null;
        }

        public void insertup(double xk)
        {
            node itl, itr;
            itl = this.head;
            itr = this.head;

            while ((itr != null) && (itr.data < xk))
            {
                itl = itr;
                itr = itr.next;
            }

            if (head == null)
            {
                head = new node(xk, null);
            }
            else
            {
                if (itr == head)
                {
                    node dd = new node(xk, head);
                    head = dd;
                }
                else
                {
                    node d = new node(xk, itl.next);
                    itl.next = d;
                }
            }
        }

        public void printDGV(DataGridView dataGridView1)
        {
            node itl = this.head;

            int i = 0;

            while (itl != null)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                dataGridView1.Rows[i].Cells[1].Value = itl.data;

                i++;

                itl = itl.next;
            }
        }

        public double leftgr()
        {
            return this.head.data;
        }

        public double rightgr()
        {
            node itl = this.head;

            double b = itl.data;

            while (itl != null)
            {
                b = itl.data;

                itl = itl.next;
            }

            return b;
        }

        public double teoria(double[] gran, DataGridView dataGridView1, Func<double, double, double> f, int n)
        {
            node itl = this.head;

            int i = 0;
            int num = 0;

            double x = itl.data;
            double a = -1.0 * Math.Sqrt(Math.Sqrt(2.0)) - 0.000000000000001;
            double b = x;
            double q, r = 0.0;

            while (itl != null)
            {
                x = itl.data;

                if ((x < gran[i]) && (itl.next != null))
                {
                    num++;
                    itl = itl.next;
                }
                else
                {
                    if (itl.next == null)
                        itl = itl.next;
                    b = gran[i];

                    q = f(a, b);
                    r += ((num - n * q) * (num - n * q)) / (n * q);

                    dataGridView1.Rows[i].Cells[1].Value = q;

                    a = b;

                    num = 0;

                    i++;
                }
            }

            return r;
        }

        public double gistogram(double[] gran, DataGridView dataGridView1, ZedGraphControl zed, Func<double, double> f, int n)
        {
            GraphPane panel = zed.GraphPane;
            panel.CurveList.Clear();

            PointPairList g_list = new PointPairList();

            node itl = this.head;

            int i = 0;
            int num = 0;

            double d = 0.0;
            double x = itl.data;
            double a = x;
            double b = x;
            double ff, fff;

            while (itl != null)
            {
                x = itl.data;

                if ((x < gran[i]) && (itl.next != null))
                {
                    num++;
                    itl = itl.next;
                }
                else
                {
                    if (itl.next == null)
                        itl = itl.next;
                    b = gran[i];

                    fff = num / (n * (b - a));
                    ff = f((b + a) / 2.0);

                    if (d < Math.Abs(fff - ff))
                    {
                        d = Math.Abs(fff - ff);
                    }

                    g_list.Add(a, 0);
                    g_list.Add(a, fff);
                    g_list.Add(b, fff);
                    g_list.Add(b, 0);

                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (b + a) / 2.0;
                    dataGridView1.Rows[i].Cells[1].Value = ff;
                    dataGridView1.Rows[i].Cells[2].Value = fff;

                    a = b;

                    num = 0;

                    i++;
                }
            }

            LineItem Curve = panel.AddCurve("Гистограмма", g_list, Color.Red, SymbolType.None);

            zed.AxisChange();
            zed.Invalidate();

            return d;
        }

        public double vs(int n)
        {
            node itl = this.head;

            double x = 0.0;

            while (itl != null)
            {
                x += itl.data;

                itl = itl.next;
            }

            x = x / (double)n;

            return x;
        }

        public double vd(int n, double x)
        {
            node itl = this.head;

            double s = 0.0;

            while (itl != null)
            {
                s += (itl.data - x) * (itl.data - x);

                itl = itl.next;
            }

            s = s / (double)n;

            return s;
        }

        public double rv()
        {
            node itl = this.head;

            double x1 = itl.data;
            double xn = x1;

            while (itl != null)
            {
                xn = itl.data;

                itl = itl.next;
            }

            return xn - x1;
        }

        public double vm(int n)
        {
            int k = n / 2;
            int i = 0;

            node itl = this.head;

            while ((itl != null) && (i < k - 1))
            {
                i++;
                itl = itl.next;
            }

            if (n % 2 == 1)
            {
                return itl.next.data;
            }
            else
            {
                return (itl.data + itl.next.data) / 2.0;
            }
        }

        public double artGraph(ZedGraphControl zed, Func<double, double> f, int n, double b1)
        {
            GraphPane panel = zed.GraphPane;

            PointPairList ff_list = new PointPairList();

            node itl = this.head;
            node itt = this.head;

            int i = 0;
            double d = 0.0;
            double fff = 0.0;
            double xx = 0.0;

            while (itl != null)
            {

                double ff = f(itl.data);
                fff = (double)i / (double)n;

                if (d < Math.Abs(fff - ff))
                {
                    d = Math.Abs(fff - ff);
                }

                ff_list.Add(itt.data, fff);
                ff_list.Add(itl.data, fff);
                ff_list.Add(PointPairBase.Missing, PointPairBase.Missing);

                i++;

                itt = itl;
                xx = itl.data;
                itl = itl.next;
            }

            ff_list.Add(xx, 1.0);
            ff_list.Add(b1, 1.0);

            LineItem Curve1 = panel.AddCurve("F^(x)", ff_list, Color.Blue, SymbolType.None);

            zed.AxisChange();
            zed.Invalidate();

            return d;
        }
    }
}
