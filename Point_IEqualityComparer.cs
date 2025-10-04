using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    /// <summary>
    /// Rewrote the Comparer for 'Point' type since 
    /// the default one was kind of slow for general use.
    /// </summary>
    class Point_IEqualityComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point p1, Point p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y); 
        }

        /// Since X, Y < 2^16 for our purposes, in order to avoid overlap
        /// between cases such as Point(a, b) <-> Point(b, a)
        /// we will place the bits of each point's coordinates
        /// in one half of an int each (so 16 bits PER XY-coordinate)
        public int GetHashCode(Point p)
        {
            return (p.X << 16) ^ p.Y;
        }
    }
}
