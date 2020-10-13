using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shapes;
using Point = System.Drawing.Point;

namespace RayCasting
{
    public partial class Form1 : Form
    {
        const double Rotations = 0.1;
        Random rand;
        Graphics gfx;
        Bitmap canvas;
        List<Line> lines;
        public Form1()
        {
           
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);

            lines = new List<Line>();
            rand = new Random();

            Line line;
            for (int i = 0; i < 5; i++)
            {
                line = new Line();
                line.X1 = rand.Next(0, pictureBox1.Width);
                line.Y1 = rand.Next(0, pictureBox1.Height);
                
                line.X2 = rand.Next(0, pictureBox1.Width);
                line.Y2 = rand.Next(0, pictureBox1.Height);

                lines.Add(line);
            }
            Line LeftWall = new Line();
            LeftWall.X1 = 0;
            LeftWall.Y1 = 0;
            LeftWall.X2 = 0;
            LeftWall.Y2 = pictureBox1.Height;

            Line RightWall = new Line();
            LeftWall.X1 = pictureBox1.Width;
            LeftWall.Y1 = 0;
            LeftWall.X2 = pictureBox1.Width;
            LeftWall.Y2 = pictureBox1.Height;

            Line TopWall = new Line();
            LeftWall.X1 = 0;
            LeftWall.Y1 = 0;
            LeftWall.X2 = pictureBox1.Width;
            LeftWall.Y2 = 0;

            Line BottomWall = new Line();
            LeftWall.X1 = 0;
            LeftWall.Y1 = pictureBox1.Height;
            LeftWall.X2 = pictureBox1.Width;
            LeftWall.Y2 = pictureBox1.Height;

            lines.Add(LeftWall);
            lines.Add(RightWall);
            lines.Add(TopWall);
            lines.Add(BottomWall);
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateLines();

            Draw();
        }

        private void Draw()
        {
            gfx.Clear(Color.Black);

            foreach (var l in lines)
            {
                gfx.DrawLine(Pens.White, new Point((int)l.X1, (int)l.Y1), new Point((int)l.X2, (int)l.Y2));
            }
            pictureBox1.Image = canvas;
        }
        private void UpdateLines()
        {
            Point MousePosition = Cursor.Position;
            if(!FindIntersection(MousePosition))
            {
                throw new Exception("Hello??");
            }
        }
        
        private bool FindIntersection(Point origin)
        {
            for (double an = 0; an <= Math.PI * 2; an += Rotations)
            {
                double X = Math.Cos(an) * 10000;
                double Y = Math.Sin(an) * 10000;
                Line temp = new Line();
                temp.X1 = origin.X;
                temp.Y1 = origin.Y;
                temp.X2 = X;
                temp.Y2 = Y;
                foreach (var l in lines)
                {
                    if(CheckIntersection(temp, l, out Point endPoint))
                    {
                        Line line = new Line();
                        line.X1 = origin.X;
                        line.Y1 = origin.Y;

                        line.X2 = endPoint.X;
                        line.Y2 = endPoint.Y;
                        lines.Add(line);
                        return true;
                    }
                }
            }

            return false;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        public static bool CheckIntersection(Line l1, Line l2, out Point end)
        {
            end = default;
            double den = (l1.X1 - l1.X2) * (l2.Y1 - l2.Y2) - (l1.Y1 - l1.Y2) * (l2.X1 - l2.X2);
            double t = (l1.X1 - l2.X1) * (l2.Y1 - l2.Y2) - (l1.Y1 - l2.Y1) * (l2.X1 - l2.X2);
            double u = (l1.X1 - l1.X2) * (l1.Y1 - l2.Y1) - (l1.Y1 - l1.Y2) * (l1.X1 - l2.X1);
            t /= den;
            u /= den;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                end.X = (int)(l1.X1 * t * (l1.X2 - l1.X1));
                end.Y = (int)(l1.Y1 * t * (l1.Y2 - l1.Y1));
                return true;
            }
            //int x = x1 * t * (x2 - x1)
            // int y = y1 * t * (y2 - y1)
            return false;
        }
    }
}
