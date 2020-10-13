using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting
{
    public class Ray
    {
        public Point Origin { get; set; }

        public double Angle { get; set; }
    
        public int Magnitude { get; set; }
        public Ray(Point origin, double angle, int magnitude)
        {
            Origin = origin;
            Angle = angle;
            Magnitude = magnitude;
        }
    }

}
