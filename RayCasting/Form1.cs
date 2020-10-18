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
        const double Rotations = 0.05;
        Random rand;
        Graphics gfx;
        Bitmap canvas;
        List<Line> Hitboxlines;
        List<Line> CreatedLines;
        public Form1()
        {

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);

            List<Line> temp = new List<Line>()
            {
                   new Line(){X1 = 0, Y1 = 0, X2 = 0, Y2 = pictureBox1.Height},    // left wall
                new Line(){X1 = pictureBox1.Width, Y1 = 0, X2 = pictureBox1.Width, Y2 = pictureBox1.Height}, //right wall
                new Line(){X1 = 0, Y1 = 0, X2 = pictureBox1.Width, Y2 = 0},     //top wall
                new Line(){X1 = 0, Y1 = pictureBox1.Height, X2 = pictureBox1.Width, Y2 = pictureBox1.Height} //bottom wall
            };
            Hitboxlines = new List<Line>();
            rand = new Random();
            CreatedLines = new List<Line>();
            Line line;
            for (int i = 0; i < 5; i++)
            {
                line = new Line();
                line.X1 = rand.Next(0, pictureBox1.Width);
                line.Y1 = rand.Next(0, pictureBox1.Height);

                line.X2 = rand.Next(0, pictureBox1.Width);
                line.Y2 = rand.Next(0, pictureBox1.Height);

                Hitboxlines.Add(line);
            }

            Hitboxlines.AddRange(temp);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateLines();

            Draw();
        }

        private void Draw()
        {
            gfx.Clear(Color.Black);

            foreach (var l in Hitboxlines)
            {
                gfx.DrawLine(Pens.White, new Point((int)l.X1, (int)l.Y1), new Point((int)l.X2, (int)l.Y2));
            }
            foreach (var l in CreatedLines)
            {
                gfx.DrawLine(Pens.LightBlue, new Point((int)l.X1, (int)l.Y1), new Point((int)l.X2, (int)l.Y2));
            }
            pictureBox1.Image = canvas;
        }

        private void UpdateLines()
        {

            Point MousePosition = pictureBox1.PointToClient(Cursor.Position);

            Console.WriteLine($"{MousePosition.X}, {MousePosition.Y}");
            FindIntersection(MousePosition);

        }

        private void FindIntersection(Point origin)
        {
            CreatedLines.Clear();
            for (double an = 0; an <= Math.PI*2; an += Rotations)
            {
                double X = origin.X + (Math.Cos(an) * 10000);
                double Y = origin.Y - (Math.Sin(an) * 10000);
                Line temp = new Line();
                temp.X1 = origin.X;
                temp.Y1 = origin.Y;
                temp.X2 = X;
                temp.Y2 = Y;
                List<(Line, double)> Distances = new List<(Line, double)>();
                foreach (var l in Hitboxlines)
                {
                    if (CheckIntersection(temp, l, out Point endPoint))
                    {
                        Line line = new Line();
                        line.X1 = origin.X;
                        line.Y1 = origin.Y;

                        line.X2 = endPoint.X;
                        line.Y2 = endPoint.Y;
                        Distances.Add((line, GetDistance(origin, endPoint)));    
                    }
                }
                try
                {
                    var first = Distances.OrderBy(x => x.Item2).First();
                    CreatedLines.Add(first.Item1);
                }
                catch
                {

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }



        public static bool CheckIntersection(Line l1, Line l2, out Point end)
        {
            end = default;
            double den = ((l1.X1 - l1.X2) * (l2.Y1 - l2.Y2)) - ((l1.Y1 - l1.Y2) * (l2.X1 - l2.X2));
            double t = ((l1.X1 - l2.X1) * (l2.Y1 - l2.Y2)) - ((l1.Y1 - l2.Y1) * (l2.X1 - l2.X2));
            double u = ((l1.X1 - l1.X2) * (l1.Y1 - l2.Y1)) - ((l1.Y1 - l1.Y2) * (l1.X1 - l2.X1));
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
        private void pictureBox1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }
    }
}
