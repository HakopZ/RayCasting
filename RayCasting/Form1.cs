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
        const double Rotations = 0.01;
        Random rand;
        Graphics gfx;
        Bitmap canvas;
        List<Line2D> Hitboxlines;
        List<Line2D> CreatedLines;
        public Form1()
        {

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);

            List<Line2D> temp = new List<Line2D>()
            {
                   new Line2D(){X1 = 0, Y1 = 0, X2 = 0, Y2 = pictureBox1.Height},    // left wall
                new Line2D(){X1 = pictureBox1.Width, Y1 = 0, X2 = pictureBox1.Width, Y2 = pictureBox1.Height}, //right wall
                new Line2D(){X1 = 0, Y1 = 0, X2 = pictureBox1.Width, Y2 = 0},     //top wall
                new Line2D(){X1 = 0, Y1 = pictureBox1.Height, X2 = pictureBox1.Width, Y2 = pictureBox1.Height} //bottom wall
            };
            Hitboxlines = new List<Line2D>();
            rand = new Random();
            CreatedLines = new List<Line2D>();
            for (int i = 0; i < 5; i++)
            {
                Line2D line = new Line2D
                {
                    X1 = rand.Next(0, pictureBox1.Width),
                    Y1 = rand.Next(0, pictureBox1.Height),

                    X2 = rand.Next(0, pictureBox1.Width),
                    Y2 = rand.Next(0, pictureBox1.Height)
                };

                Hitboxlines.Add(line);
            }

            Hitboxlines.AddRange(temp);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gfx.Clear(Color.Black);
            UpdateLines();

            Draw();
        }

        private void Draw()
        {

            foreach (var l in Hitboxlines)
            {
                gfx.DrawLine(Pens.White, new Point(l.X1, l.Y1), new Point(l.X2, l.Y2));
            }
            pictureBox1.Image = canvas;
        }

        private void UpdateLines()
        {

            Point MousePosition = pictureBox1.PointToClient(Cursor.Position);

            //Console.WriteLine($"{MousePosition.X}, {MousePosition.Y}");
            FindIntersection(MousePosition);

        }

        private void FindIntersection(Point origin)
        {
            for (double an = 0; an <= Math.PI * 2; an += Rotations)
            {
                double X = origin.X + (Math.Cos(an) * 10000);
                double Y = origin.Y - (Math.Sin(an) * 10000);
                Line2D temp = new Line2D
                {
                    X1 = origin.X,
                    Y1 = origin.Y,
                    X2 = (int)X,
                    Y2 = (int)Y
                };
                Line2D Closest = new Line2D { X1 = 0, X2 = int.MaxValue, Y1 = 0, Y2 = int.MaxValue };

                foreach (var l in Hitboxlines)
                {
                    if (CheckIntersection(temp, l, out Point endPoint))
                    {
                        Line2D line = new Line2D
                        {
                            X1 = origin.X,
                            Y1 = origin.Y,

                            X2 = endPoint.X,
                            Y2 = endPoint.Y
                        };
                        double distance = GetDistance(origin, endPoint);
                        double closestDistance = GetDistance(new Point((int)Closest.X1, (int)Closest.Y1), new Point((int)Closest.X2, (int)Closest.Y2));
                        Closest = distance < closestDistance ? line : Closest;
                    }
                }
                if (Closest.X2 != int.MaxValue)
                {
                    PrintLine(Pens.LightBlue, Closest);
                }
            }
        }
        public void PrintLine(Pen color, Line2D line)
        {
            gfx.DrawLine(color, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
        }
        public static bool CheckIntersection(Line2D l1, Line2D l2, out Point end)
        {
            end = default;
            double den = ((l1.X1 - l1.X2) * (l2.Y1 - l2.Y2)) - ((l1.Y1 - l1.Y2) * (l2.X1 - l2.X2));
            double t = ((l1.X1 - l2.X1) * (l2.Y1 - l2.Y2)) - ((l1.Y1 - l2.Y1) * (l2.X1 - l2.X2));
            double u = ((l1.X1 - l1.X2) * (l1.Y1 - l2.Y1)) - ((l1.Y1 - l1.Y2) * (l1.X1 - l2.X1));

            if (den == 0)
            {
                return false;
            }

            t /= den;
            u /= -den;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                end.X = (int)(l1.X1 + (t * (l1.X2 - l1.X1)));
                end.Y = (int)(l1.Y1 + (t * (l1.Y2 - l1.Y1)));
                return true;
            }
            return false;
        }
        public double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
    }
}
