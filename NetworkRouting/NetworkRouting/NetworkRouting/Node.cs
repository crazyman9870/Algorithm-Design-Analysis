using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NetworkRouting
{
    class Node : IComparable<Node>
    {
        public Node prev; //Who is the parent
        public PointF location; //Point Coordinates
        public double lengthFromOrigin; //Total distance to this node
        public int pointNumber; //Keeps track of what point we are dealing with

        public Node(Node prev, PointF location, double lengthFromOrigin, int pointNumber)
        {
            this.prev = prev;
            this.location = location;
            this.lengthFromOrigin = lengthFromOrigin;
            this.pointNumber = pointNumber;
        }

        public int CompareTo(Node obj)
        {
            if (obj == null) { return 1; }
            Node p1 = obj as Node;
            if (p1 != null)
            {
                if (this.lengthFromOrigin != p1.lengthFromOrigin) //if the distances aren't the same, return the compared distances 
                {
                    return this.lengthFromOrigin.CompareTo(p1.lengthFromOrigin);
                }
                else if (this.pointNumber != p1.pointNumber) // else if the pointNumbers aren't the same, compare them
                {
                    return this.pointNumber.CompareTo(p1.pointNumber);
                }
                else if (this.location.X != p1.location.X) //else if the X's aren't the same, compare them 
                {
                    return this.location.X.CompareTo(p1.location.X);
                }
                else if (this.location.Y != p1.location.Y) //else compare Y's 
                {
                    return this.location.Y.CompareTo(p1.location.Y);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new ArgumentException("SpecialPoint not formatted correctly");
            }
        }
    }
}