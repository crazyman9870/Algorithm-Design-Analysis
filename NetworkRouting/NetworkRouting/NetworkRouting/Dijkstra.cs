using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

//Reference 1 - www.growingwiththeweb.com/2013/02/what-data-structure-net-collections-use.html

namespace NetworkRouting
{
    class Dijkstra
    {
        System.Drawing.Pen blackPen;
        //System.Drawing.Pen pen1;
        //System.Drawing.Pen pen2;
        //System.Drawing.Pen pen3;
        private int startIndex;
        private int endIndex;
        List<PointF> points;
        List<HashSet<int>> adjList;
        Node endNode;


        public Dijkstra(int start, int end, List<PointF> points, List<HashSet<int>> adjList)
        {
            this.startIndex = start;
            this.endIndex = end;
            this.points = points;
            this.adjList = adjList;
            this.endNode = null;
            this.blackPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            //this.pen1 = new System.Drawing.Pen(System.Drawing.Color.LightGreen);
            //this.pen2 = new System.Drawing.Pen(System.Drawing.Color.Red);
            //this.pen3 = new System.Drawing.Pen(System.Drawing.Color.Blue);
        }

        // Max time complextiy O(nlog n) + O(n) + O(4log n) = O(n) + O(4nlog n)
        public void DijkstraAllPath()
        {
            SortedSet<Node> removed = new SortedSet<Node>();
            SortedSet<Node> PQ = new SortedSet<Node>();

            //Add everything to the queue O(n)
            for (int i = 0; i < points.Count; i++)
            {
                if (i != startIndex) PQ.Add(new Node(null, points[i], double.MaxValue, i));
                else PQ.Add(new Node(null, points[i], 0, i)); //O(log n) (Reference 1)
            }

            //PrintTree(PQ);
            Node u;
            HashSet<int> edges;

            while (PQ.Count > 0) //O(n)
            {
                u = DeleteMin(PQ, removed); //O(log n) after ignoring  costants (see function)
                if (u.pointNumber == endIndex) endNode = u;
                edges = adjList[u.pointNumber];
                for (int i = 0; i < edges.Count; i++) //O(3) Constant time, only ever 3 edges no more/less
                {
                    Node v;
                    //Finding node in tree
                    IEnumerable<Node> nodes = PQ.Where(node => node.pointNumber == edges.ElementAt(i)); // Find O(log n) (Reference 1)
                    if (nodes.Count() == 0) nodes = removed.Where(node => node.pointNumber == edges.ElementAt(i)); // Find O(log n) (Reference 1)
                    v = nodes.ElementAt(0);
                    double newDist = DistBetweenTwoNodes(u, v);
                    //This in this if statement represents DecreaseKey.
                    //Complexity of O(2log n) which is O(log n) because constants don't matter
                    if (v.lengthFromOrigin > (u.lengthFromOrigin + newDist))
                    {
                        PQ.Remove(v); //Remove O(log n) (Reference 1)
                        v.prev = u;
                        v.lengthFromOrigin = (u.lengthFromOrigin + newDist);
                        PQ.Add(v); //Add O(log n) (Reference 1)
                    }
                }
            }
        }

        // Max time complextiy O(n) + O(4log n)
        public void DijkstraOnePath()
        {
            SortedSet<Node> removed = new SortedSet<Node>();
            SortedSet<Node> PQ = new SortedSet<Node>();
            PQ.Add(new Node(null, points[startIndex], 0, startIndex));

            Node u;
            HashSet<int> edges;

            while (PQ.Count > 0) //O(n)
            {
                u = DeleteMin(PQ, removed);
                if (u.pointNumber == endIndex)
                {
                    endNode = u;
                    break;
                }
                edges = adjList[u.pointNumber];
                for (int i = 0; i < edges.Count; i++)
                {
                    Node v;
                    //Finding node in tree
                    IEnumerable<Node> nodes = PQ.Where(node => node.pointNumber == edges.ElementAt(i)); // Find O(log n) (Reference 1)
                    if (nodes.Count() == 0) nodes = removed.Where(node => node.pointNumber == edges.ElementAt(i)); // Find O(log n) (Reference 1)
                    //Inside this if statement is where I implented my "insert" function
                    //Complexity of O(log n) as required
                    if (nodes.Count() == 0)
                    {
                        v = new Node(u, points[edges.ElementAt(i)], (u.lengthFromOrigin + DistBetweenTwoPointF(u.location, points[edges.ElementAt(i)])), edges.ElementAt(i));
                        PQ.Add(v); //Add O(log n) (Reference 1)
                    }
                    else //This else represents DecreaseKey. Complexity of O(2log n) which is O(log n) because constants don't matter
                    {
                        v = nodes.ElementAt(0);
                        double newDist = DistBetweenTwoNodes(u, v);
                        if (v.lengthFromOrigin > (u.lengthFromOrigin + newDist))
                        {
                            PQ.Remove(v); //Remove O(log n) (Reference 1)
                            v.prev = u;
                            v.lengthFromOrigin = (u.lengthFromOrigin + newDist);
                            PQ.Add(v); //Add O(log n) (Reference 1)
                        }
                    }
                }
            }
        }


        //Overall complexity O(2 log n)
        private Node DeleteMin(SortedSet<Node> PQ, SortedSet<Node> removed)
        {
            //Grabs the Node with the lowest distance as defined by the comparator
            Node u = PQ.Min; //O(1) Top of the tree
            PQ.Remove(u); //O(log n) (Reference 1)
            removed.Add(u);//O(log n) (Reference 1)
            return u;
        }

        public double DistBetweenTwoNodes(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.location.X - b.location.X, 2) + Math.Pow(a.location.Y - b.location.Y, 2));
        }

        public double DistBetweenTwoPointF(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public PointF GetMidPoint(PointF a, PointF b)
        {
            return (new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2));
        }

        public void DrawPath(Graphics g, Pen pen)
        {
            if (endNode == null) return;
            Node temp = endNode;
            while (temp.pointNumber != startIndex)
            {
                g.DrawLine(pen, temp.location, temp.prev.location);
                g.DrawString("(" + Math.Round(DistBetweenTwoPointF(temp.location, temp.prev.location), 2).ToString() + ")",
                    new Font("Arial", 8), new SolidBrush(Color.Black), GetMidPoint(temp.location, temp.prev.location));
                temp = temp.prev;
            }
        }

        public double GetMaxPath()
        {
            if (endNode == null) return -100;
            return endNode.lengthFromOrigin;
        }

        public void PrintTree(SortedSet<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("Tree at " + i + " = " + list.ElementAt(i).pointNumber);
            }
        }

        //for (int i = 0; i<adjList.Count; i++)
        //{
        //    HashSet<int> list = adjList.ElementAt(i);
        //    for (int j = 0; j<list.Count; j++)
        //    {
        //        Console.WriteLine(list.ElementAt(j));
        //    }
        //    Console.WriteLine();
        //}

    }
}
