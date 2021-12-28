using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;


namespace laba2
{
    public partial class Form1 : Form
    {
        PointPairList list = new PointPairList();
        PointPairList list1;
        List<PointPairList> list2 = new List<PointPairList>();
        LineItem myCurve_1;
        int index = -1;
        public Form1()
        {
            InitializeComponent();
            textBox5.ReadOnly = true;
        }
        private void стартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var g = new Task(start);
            g.Start();
        }
        public void start()
        {

            try
            {
                GraphPane pane = zedGraphControl1.GraphPane;
                pane.CurveList.Clear();
                string x1 = textBox1.Text;
                double x_1 = Convert.ToDouble(x1);
                string x2 = textBox2.Text;
                double x_2 = Convert.ToDouble(x2);
                string e1 = textBox3.Text;
                double e_1 = Convert.ToDouble(e1);
                string func = textBox4.Text;

                if (e_1 <= 0)
                {
                    throw new Exception();
                }
                if (x_1 > x_2)
                {
                    throw new Exception();
                }
                for (double z = x_1; z <= x_2; z += 1)
                {
                    Argument x = new Argument("x = " + z);
                    Expression exp = new Expression(func, x);
                    if (double.IsNaN(exp.calculate()))
                    {
                        throw new Exception();
                    }
                    list.Add(z, exp.calculate());
                    LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.None);
                }
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            catch
            {
                MessageBox.Show("Ашибка");
            }
        }
        private void методToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Task(minf);
            t.Start();
        }
        private void minf()
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            string x1 = textBox1.Text;
            double x_1 = Convert.ToDouble(x1);
            string x2 = textBox2.Text;
            double x_2 = Convert.ToDouble(x2);
            string e1 = textBox3.Text;
            double e_1 = Convert.ToDouble(e1);
            string func = textBox4.Text;

            Argument x;
            Expression exp;
            double xx1 = 0;
            double xx2 = 0;
            double yy1 = 0;
            double yy2 = 0;
            double phi = (Math.Sqrt(5) - 1) / 2;
            double mmm;
            while (true)
            {
                xx1 = x_2 - (x_2 - x_1) * phi;
                xx2 = x_1 + (x_2 - x_1) * phi;
                x = new Argument("x = " + xx1);
                exp = new Expression(func, x);
                yy1 = exp.calculate();
                x = new Argument("x = " + xx2);
                exp = new Expression(func, x);
                yy2 = exp.calculate();

                if (yy1 >= yy2)
                {

                    x_1 = xx1;
                    x = new Argument("x = " + x_1);
                    exp = new Expression(func, x);
                    list1 = new PointPairList();
                    list1.Add(new PointPair(x_1, exp.calculate()));
                    list2.Add(list1);
                    Debug.WriteLine("y1>=y2=>x1=" + x_1);
                }
                else
                {
                    x_2 = xx2;
                    x = new Argument("x = " + x_2);
                    exp = new Expression(func, x);
                    list1 = new PointPairList();
                    list1.Add(new PointPair(x_2, exp.calculate()));
                    list2.Add(list1);
                    Debug.WriteLine("y1<y2=>x2=" + x_2);

                }
                if (Math.Abs(x_2 - x_1) < e_1)
                {
                    Debug.WriteLine("break: " + yy2 + " " + yy1);
                    break;
                }
            }
            mmm = (x_1 + x_2) / 2;
            x = new Argument("x = " + mmm);
            exp = new Expression(func, x);
            double yyy = exp.calculate();
            Action text = () => textBox5.Text = String.Format("{0}, {1}", Math.Round(mmm, textBox3.Text.Length - 2), Math.Round(yyy, textBox3.Text.Length - 2));
            Invoke(text);
            list1 = new PointPairList();
            list1.Add(new PointPair(mmm, yyy));
            list2.Add(list1);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.MaxLength = 10;
        }
        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var b = new Task(back);
            b.Start();
        }
        public void back()
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Remove(myCurve_1);
            if (index > 0)
            {
                --index;
                myCurve_1 = pane.AddCurve("", list2[index], Color.Orange, SymbolType.Circle);
                zedGraphControl1.Invalidate();
                Action text = () => textBox5.Text = String.Format("{0}, {1}", Math.Round(list2[index][0].X, textBox3.Text.Length - 2), Math.Round(list2[index][0].Y, textBox3.Text.Length - 2));
                Invoke(text);
            }
        }
        private void впередToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new Task(forward);
            f.Start();
        }
        public void forward()
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Remove(myCurve_1);
            if (index < list2.Count - 1)
            {
                ++index;
                myCurve_1 = pane.AddCurve("", list2[index], Color.Orange, SymbolType.Circle);
                zedGraphControl1.Invalidate();
                Action text = () => textBox5.Text = String.Format("{0}, {1}", Math.Round(list2[index][0].X, textBox3.Text.Length - 2), Math.Round(list2[index][0].Y, textBox3.Text.Length - 2));
                Invoke(text);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45 && number != 46)
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45 && number != 46)
            {
                e.Handled = true;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.MaxLength = 7;
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45 && number != 46)
            {
                e.Handled = true;
            }
        }
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
