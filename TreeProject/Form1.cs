using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TreeProject
{
    public partial class Form1 : Form
    {
        static BinTreeNode<string> t2 = null;
        static Boolean z = false;
        static Boolean z2 = false;
        static int[] xlocs = new int[0];
        static int[] xlocs2 = new int[0];
        static int smallest = 100000000;
        static int numNodes = 0;
        int index = 0;
        static int count = 1;
        static int RootX = -1;
        static int RootY = -1;
        static string s = "";
        static int mona = 0;
        static Panel panel1 = new Panel();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point p = new Point(10, 65);
            panel1.Location = p;
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Width = this.Width - 40;
            panel1.Height = this.Height - 150;
            panel1.BackColor = Color.LightGreen;
            this.Controls.Add(panel1);
        }

        //-----------------------------------------------------------------------------------------------------

        void draw(BinTreeNode<string> t, int y)  //מקבלת עץ בינארי של מחרוזות ומספר שלם (שהוא אפס) ומציירת את חוליות העץ בהתאמה
        {
            if (t.GetLeft() != null)
                draw(t.GetLeft(), y + 100);
            System.Drawing.Graphics g = panel1.CreateGraphics();
            int i = 0;
            for (; i < xlocs.Length; i++)
            {
                if (xlocs[i] != 0)
                {

                    t.setPlaceInXlocs(index);
                    index++;

                    System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(xlocs[i], y, 50, 50);
                    g.DrawEllipse(System.Drawing.Pens.Black, rectangle);
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    g.DrawString(t.GetInfo(), font, brush, xlocs[i] + 20, y + 20);

                    if (t.getNum() == 1)
                    {
                        RootX = i;
                        RootY = y;
                    }

                    xlocs[i] = 0;
                    break;
                }
            }

            if (t.GetRight() != null)
                draw(t.GetRight(), y + 100);
        }

        //-----------------------------------------------------------------------------------------------------

        int nodesNum(BinTreeNode<string> t) // מקבלת עץ של מחרוזות ומחזירה כמה חוליות הוא  מכיל
        {
            if (t == null)
                return 0;
            return nodesNum(t.GetLeft()) + nodesNum(t.GetRight()) + 1;
        }

        //----------------------------------------------------------------------------------------------------- 

        void drawLines(BinTreeNode<string> t, int i, int y) //מקבלת עץ של מחרוזות ושני מספרים שלמים המייצגים את מיקום שורש העץ על הפאנל ומציירת את הקווים שבין כל חולייה וחולייה
        {
            System.Drawing.Graphics g = panel1.CreateGraphics();
            if (t == null)
                return;

            if (t.GetLeft() != null && t.GetRight() != null)
            {
                g.DrawLine(Pens.Black, xlocs2[i] + 25, y + 50, (xlocs2[i] + 25) - ((t.getPlaceInXlocs() - t.GetLeft().getPlaceInXlocs()) * (panel1.Width / numNodes)), y + 100);
                g.DrawLine(Pens.Black, xlocs2[i] + 25, y + 50, (xlocs2[i] + 25) + ((t.GetRight().getPlaceInXlocs() - t.getPlaceInXlocs()) * (panel1.Width / numNodes)), y + 100);
            }

            if (t.GetLeft() != null && t.GetRight() == null)
            {
                g.DrawLine(Pens.Black, xlocs2[i] + 25, y + 50, (xlocs2[i] + 25) - ((t.getPlaceInXlocs() - t.GetLeft().getPlaceInXlocs()) * (panel1.Width / numNodes)), y + 100);
            }

            if (t.GetLeft() == null && t.GetRight() != null)
            {
                g.DrawLine(Pens.Black, xlocs2[i] + 25, y + 50, (xlocs2[i] + 25) + ((t.GetRight().getPlaceInXlocs() - t.getPlaceInXlocs()) * (panel1.Width / numNodes)), y + 100);
            }

            if (t.GetLeft() != null)
                drawLines(t.GetLeft(), i - (t.getPlaceInXlocs() - t.GetLeft().getPlaceInXlocs()), y + 100);
            if (t.GetRight() != null)
                drawLines(t.GetRight(), i + (t.GetRight().getPlaceInXlocs() - t.getPlaceInXlocs()), y + 100);
        }

        //-----------------------------------------------------------------------------------------------------

        void arrange(BinTreeNode<string> t, string[] s) //מקבלת עץ של מחרוזות ומערך ריק של מחרוזות ומכניסה את ערכי חוליות העץ לתוך המערך בסדר תוכי, כך שיהיה מקטן לגדול
        {
            if (t.GetLeft() != null)
            arrange(t.GetLeft(), s);
            s[mona] =t.GetInfo() + "|";
            mona++;
            if (t.GetRight() != null)
            arrange(t.GetRight(), s);
        }

        static BinTreeNode<string> BuildTreeOfParser(String str) // מקבלת מחרוזת ומחזירה עץ של מחרוזות
        {
            BinTreeNode<string> tr = null;
            int countBrackets = 0;
            for (int i = str.Length - 1; i > 0; i--)
            {
                if (str[i] == ')')
                    countBrackets++;
                if (str[i] == '(')
                    countBrackets--;
                if (countBrackets == 0 && (str[i] == '+' || str[i] == '-') && tr == null)
                {
                    tr = new BinTreeNode<string>(str[i].ToString());
                    tr.SetRight(BuildTreeOfParser(str.Substring(i + 1, str.Length - i - 1)));
                    tr.SetLeft(BuildTreeOfParser(str.Substring(0, i)));
                }
            }
            countBrackets = 0;
            for (int i = str.Length - 1; i > 0; i--)
            {
                if (str[i] == ')')
                    countBrackets++;
                if (str[i] == '(')
                    countBrackets--;
                if (countBrackets == 0 && (str[i] == '*' || str[i] == '/') && tr == null)
                {
                    tr = new BinTreeNode<string>(str[i].ToString());
                    tr.SetRight(BuildTreeOfParser(str.Substring(i + 1, str.Length - i - 1)));
                    tr.SetLeft(BuildTreeOfParser(str.Substring(0, i)));
                }
            }
            countBrackets = 0;
            for (int i = str.Length - 1; i > 0; i--)
            {
                if (str[i] == ')')
                    countBrackets++;
                if (str[i] == '(')
                    countBrackets--;
                if (countBrackets == 0 && str[i] == '^' && tr == null)
                {
                    tr = new BinTreeNode<string>(str[i].ToString());
                    tr.SetRight(BuildTreeOfParser(str.Substring(i + 1, str.Length - i - 1)));
                    tr.SetLeft(BuildTreeOfParser(str.Substring(0, i)));
                }
            }
            countBrackets = 0;
            for (int i = str.Length - 1; i > 0; i--)
            {
                if (str[i] == ')')
                    countBrackets++;
                if (str[i] == '(')
                    countBrackets--;
                if (countBrackets == 0 && tr == null)
                {
                    if (str[i] == 'n' && str[i - 1] == 'i' && str[i - 2] == 's')
                    {
                        tr = new BinTreeNode<string>("sin");
                        tr.SetRight(BuildTreeOfParser(str.Substring(i + 1, str.Length - i - 1)));
                        tr.SetLeft(BuildTreeOfParser(str.Substring(0, i)));
                    }
                    if (str[i] == 's' && str[i - 1] == 'o' && str[i - 2] == 'c')
                    {
                        tr = new BinTreeNode<string>("cos");
                        tr.SetRight(BuildTreeOfParser(str.Substring(i + 1, str.Length - i - 1)));
                        tr.SetLeft(BuildTreeOfParser(str.Substring(0, i)));
                    }
                }
            }
            countBrackets = 0;
            for (int i = str.Length - 1; i > 0; i--)
            {
                if (str[i] == ')')
                    countBrackets++;
                if (str[i] == '(')
                    countBrackets--;
                if (countBrackets == 0 && (str[i] == 'g' && str[i - 1] == 'o' && str[i - 2] == 'l') && tr == null)
                {
                    tr = new BinTreeNode<string>("log");
                    int u = i + 2;
                    string s1 = "", s2 = "";
                    while (str[u] != ',')
                    {
                        s1 = s1 + str[u];
                        u++;
                    }
                    u++;
                    while (str[u] != ')')
                    {
                        s2 = s2 + str[u];
                        u++;
                    }
                    tr.SetRight(BuildTreeOfParser(s1));
                    tr.SetLeft(BuildTreeOfParser(s2));
                }
            }
            if (str[0] == '(' && str[str.Length - 1] == ')')
                tr = BuildTreeOfParser(str.Substring(1, str.Length - 2));
            if (tr == null)
                tr = new BinTreeNode<string>(str);
            return tr;
        }

        //-----------------------------------------------------------------------------------------------------

        static BinTreeNode<string> BuildTreeOfSearch(String str, BinTreeNode<string> tr)   // מקבלת מחרוזת ועץ של מחרוזות ובונה את העץ על פי המחרוזת       // tr = global t2
        {
            if (tr == null)
            {
                t2 = new BinTreeNode<string>(str);
                return tr;
            }
            else
                if (int.Parse(str) < int.Parse(tr.GetInfo()))
                {
                    if (tr.GetLeft() == null)
                        tr.SetLeft(new BinTreeNode<string>(str));
                    else
                        return BuildTreeOfSearch(str, tr.GetLeft());
                }

                else
                    if ((int.Parse(str) > int.Parse(tr.GetInfo())) || (int.Parse(str) == int.Parse(tr.GetInfo())))
                    {
                        if (tr.GetRight() == null)
                            tr.SetRight(new BinTreeNode<string>(str));
                        else
                            return BuildTreeOfSearch(str, tr.GetRight());
                    }

            return tr;
        }

        //-----------------------------------------------------------------------------------------------------

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            System.Drawing.Graphics g = panel1.CreateGraphics();
            if (!z2)
            {
                if (!z)
                    z = true;
                else
                    z = false;
                if (z)
                {
                    BinTreeNode<string> t = BuildTreeOfParser(textBox1.Text);
                    int a = 0;
                    for (int i = 0; i < numNodes; i++)
                    {
                        g.DrawLine(Pens.Black, a, 0, a, panel1.Height);
                        a = a + panel1.Width / numNodes;
                    }
                }
                if (!z)
                {
                    BinTreeNode<string> t = BuildTreeOfParser(textBox1.Text);
                    int a = panel1.Width / numNodes;
                    for (int i = 0; i < numNodes - 1; i++)
                    {
                        g.DrawLine(Pens.LightGreen, a, 0, a, panel1.Height);
                        a = a + panel1.Width / numNodes;
                    }
                }
            }

            else
            {

                if (!z)
                    z = true;
                else
                    z = false;
                if (z)
                {
                    int a = 0;
                    for (int i = 0; i < numNodes; i++)
                    {
                        g.DrawLine(Pens.Black, a, 0, a, panel1.Height);
                        a = a + panel1.Width / numNodes;
                    }
                }
                if (!z)
                {
                    int a = panel1.Width / numNodes;
                    for (int i = 0; i < numNodes - 1; i++)
                    {
                        g.DrawLine(Pens.LightGreen, a, 0, a, panel1.Height);
                        a = a + panel1.Width / numNodes;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            BinTreeNode<string> t = BuildTreeOfParser(textBox1.Text);
            numNodes = nodesNum(t);
            Array.Resize(ref xlocs, numNodes);
            Array.Resize(ref xlocs2, numNodes);
            for (int i = 0; i < xlocs.Length; i++)
            {
                xlocs[i] = ((panel1.Width / numNodes / 2) - 25) + ((i) * (panel1.Width / numNodes));
                xlocs2[i] = ((panel1.Width / numNodes / 2) - 25) + ((i) * (panel1.Width / numNodes));
            }

            draw(t, 0);
            drawLines(t, RootX, RootY);
        }

        //-----------------------------------------------------------------------------------------------------

        private void button3_Click_1(object sender, EventArgs e)
        {
            System.Drawing.Graphics g = panel1.CreateGraphics();
            z2 = true;
            BuildTreeOfSearch(textBox2.Text, t2);
            //---------------------------------------
            mona = 0;
            label4.Text = "";
            label3.Text = label3.Text + textBox2.Text + "|";
            string[] s = new string[nodesNum(t2)];
            arrange(t2, s);
            for (int i = 0; i < s.Length; i++)
                label4.Text = label4.Text + s[i];
            //----------------------------------------
            for (int i = 0; i < panel1.Width; i++)
                g.DrawLine(Pens.LightGreen, i, 0, i, panel1.Height);
            numNodes = nodesNum(t2);
            Array.Resize(ref xlocs, numNodes);
            Array.Resize(ref xlocs2, numNodes);
            for (int i = 0; i < xlocs.Length; i++)
            {
                xlocs[i] = ((panel1.Width / numNodes / 2) - 25) + ((i) * (panel1.Width / numNodes));
                xlocs2[i] = ((panel1.Width / numNodes / 2) - 25) + ((i) * (panel1.Width / numNodes));
            }

            draw(t2, 0);
            drawLines(t2, RootX, RootY);
            textBox2.Text = "";
        }

        //-----------------------------------------------------------------------------------------------------

        class BinTreeNode<T>
        {
            private BinTreeNode<T> left;
            private T info;
            private BinTreeNode<T> right;
            private int num;
            private int placeInXlocs = -1;

            public BinTreeNode(T x)
            {
                this.left = null;
                this.info = x;
                this.right = null;
                this.num = count;
                count++;
            }
            public BinTreeNode(BinTreeNode<T> left, T x, BinTreeNode<T> right)
            {
                this.left = left;
                this.info = x;
                this.right = right;
                this.num = count;
                count++;
            }
            public T GetInfo()
            {
                return this.info;
            }
            public void SetInfo(T x)
            {
                this.info = x;
            }
            public BinTreeNode<T> GetLeft()
            {
                return this.left;
            }
            public BinTreeNode<T> GetRight()
            {
                return this.right;
            }
            public void SetLeft(BinTreeNode<T> tree)
            {
                this.left = tree;
            }
            public void SetRight(BinTreeNode<T> tree)
            {
                this.right = tree;
            }
            public override string ToString()
            {
                return this.info.ToString();
            }
            public int getNum()
            {
                return this.num;
            }
            public int getPlaceInXlocs()
            {
                return this.placeInXlocs;
            }
            public void setPlaceInXlocs(int x)
            {
                this.placeInXlocs = x;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        private void button2_Click(object sender, EventArgs e)
        {
            System.Drawing.Graphics g = panel1.CreateGraphics();
            t2 = null;
            z = false;
            xlocs = new int[0];
            xlocs2 = new int[0];
            smallest = 100000000;
            numNodes = 0;
            index = 0;
            count = 1;
            RootX = -1;
            RootY = -1;
            checkBox1.Checked = false;
            textBox1.Text = "";
            mona = 0;
            label3.Text = "";
            label4.Text = "";
            for (int i = 0; i < panel1.Width; i++)
                g.DrawLine(Pens.LightGreen, i, 0, i, panel1.Height);
        }

        //-----------------------------------------------------------------------------------------------------
        
    }
}

