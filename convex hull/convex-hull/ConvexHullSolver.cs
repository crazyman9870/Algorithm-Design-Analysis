using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;
        System.Drawing.Pen pen1;
        System.Drawing.Pen pen2;
        System.Drawing.Pen pen3;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
            this.pen1 = new System.Drawing.Pen(System.Drawing.Color.LightGreen);
            this.pen2 = new System.Drawing.Pen(System.Drawing.Color.Red);
            this.pen3 = new System.Drawing.Pen(System.Drawing.Color.Blue);
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<System.Drawing.PointF> pointList)
        {
            List<PointF> finalList = recSolve(pointList); //Begin recusive funtion call
            drawList(finalList, pen3); //Draw the final 
        }

        public List<PointF> recSolve(List<PointF> points) //Recursive function DIVIDE AND CONQUER! //O(nlogn)
        {
            
            if (points.Count > 3) //Base case (Keep going until we are small enough)
            {
                List<PointF> leftHalfList = new List<PointF>(); //create list to store left half points
                List<PointF> rightHalfList = new List<PointF>(); //create list to store right half points

                points.Sort((p1, p2) => (p1.X.CompareTo(p2.X))); //Sorts the points by x coordinate

                for (int i = 0; i < points.Count / 2; i++) //Store points in left and right lists O(n/2)
                {
                    leftHalfList.Add(points[i]);
                    rightHalfList.Add(points[i + (points.Count / 2)]);
                }

                if (points.Count % 2 != 0) //handles if there are an odd number of points
                {
                    rightHalfList.Add(points[points.Count - 1]);
                }

                //Recursive calls on left and right halves of the list, comes back with a smaller merged list of points
                List<PointF> newlistleft = recSolve(leftHalfList); //New sublist of size m <= n
                List<PointF> newlistright = recSolve(rightHalfList); //New sublist of size p <= n
                //After being kicked out of the recusion

                CircLinkedLists linked2lists = new CircLinkedLists(newlistleft, newlistright); //Create two circular linked lists of the points O(m * p) (Only in the smallest case are m/p = n)
                linked2lists.mergeLists(); //Merge the current two lists of points using the tangent lines O(logn)
                linked2lists.print2Lists(pen1, g);
                Refresh();
                return linked2lists.toListOfPoints();
            }
            else
            {
                CircLinkedLists linked1lists = new CircLinkedLists(points);
                return linked1lists.toListOfPoints();
            }
        }

        public void printPoints(List<PointF> list)
        {
            Console.WriteLine();
            foreach (PointF p in list)
            {
                Console.WriteLine("X coordinate = " + p.X + "Y coordinate = " + p.Y);
            }
            Console.WriteLine();
        }

        public void drawList(List<PointF> list, Pen pen)
        {
            for(int i = 0; i < list.Count - 1; i++)
            {
                g.DrawLine(pen, list[i], list[i+1]);
            }
            g.DrawLine(pen, list[0], list[list.Count - 1]);
            Refresh();
        }

    }
}
