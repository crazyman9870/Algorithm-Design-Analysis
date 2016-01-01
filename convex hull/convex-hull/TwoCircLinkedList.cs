using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class CircLinkedLists
    {
        private class Node
        {
            public Node next;
            public Node prev;
            public PointF point;

            public Node(PointF point)
            {
                this.next = null;
                this.prev = null;
                this.point = point;
            }

            public Node(PointF point, Node next, Node prev)
            {
                this.next = next;
                this.prev = prev;
                this.point = point;
            }
        }

        private Node head1;
        private Node head2;
        private Node tail1;
        private Node tail2;
        private Node rightMost;
        private int size1;
        private int size2;

        public CircLinkedLists()
        {
            head1 = null;
            head2 = null;
            rightMost = null;
            size1 = 0;
            size2 = 0;
        }

        /*
        * There should only be lists of size 2/3 that ever call this constructor
        */
        public CircLinkedLists(List<PointF> list)
        {
            if (list.Count == 3)
            {
                double slope1 = calculateSlope(list[0].X, list[0].Y, list[1].X, list[1].Y);
                double slope2 = calculateSlope(list[0].X, list[0].Y, list[2].X, list[2].Y);
                if (slope1 < slope2)
                {
                    insertNodeAtEnd1(list[0]);
                    insertNodeAtEnd1(list[1]);
                    insertNodeAtEnd1(list[2]);
                }
                else
                {
                    insertNodeAtEnd1(list[0]);
                    insertNodeAtEnd1(list[2]);
                    insertNodeAtEnd1(list[1]);
                }
            }
            else
            {
                insertNodeAtEnd1(list[0]);
                insertNodeAtEnd1(list[1]);
            }
        }

        /*
        * Sets up the two sublists so that they can be joined together
        */
        public CircLinkedLists(List<PointF> list1, List<PointF> list2)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                this.insertNodeAtEnd1(list1[i]);
            }
            for (int i = 0; i < list2.Count; i++)
            {
                this.insertNodeAtEnd2(list2[i]);
            }
        }

        public void insertNodeAtEnd1(PointF point)
        {
            if(head1 == null)
            {
                Node newNode = new Node(point, head1, tail1);
                size1++;
                head1 = newNode;
                tail1 = newNode;
            }
            else
            {
                Node newNode = new Node(point, head1, tail1);
                size1++;
                head1.prev = newNode;
                tail1.next = newNode;
                tail1 = newNode;
            }
        }

        public void insertNodeAtEnd2(PointF point)
        {
            if (head2 == null)
            {
                Node newNode = new Node(point, head2, head2);
                size2++;
                head2 = newNode;
                tail2 = newNode;
            }
            else
            {
                Node newNode = new Node(point, head2, tail2);
                size2++;
                head2.prev = newNode;
                tail2.next = newNode;
                tail2 = newNode;
            }
        }

        //Merges the two sublists
        public void mergeLists()
        {
            Node markedTopLeft = null;
            Node markedTopRight = null;
            //printToConsole();
            findRightmostPoint();

            //find the top line setup
            Node previousLeft = null;
            Node previousRight = null;
            Node locationLeft = rightMost;
            Node locationRight = head2;
            double initialSlope = calculateSlope(rightMost.point.X, rightMost.point.Y, head2.point.X, head2.point.Y);
            double newSlope = initialSlope;
            //find the top line
            //Console.WriteLine(initialSlope);

            while (previousLeft != locationLeft || previousRight != locationRight)
            {
                newSlope = initialSlope;
                previousLeft = locationLeft;
                while (initialSlope == newSlope)
                {
                    locationLeft = locationLeft.prev;
                    newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
                    if (newSlope > initialSlope) initialSlope = newSlope;
                    else locationLeft = locationLeft.next;
                }
                newSlope = initialSlope;
                previousRight = locationRight;
                while (initialSlope == newSlope)
                {
                    locationRight = locationRight.next;
                    newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
                    if (newSlope < initialSlope) initialSlope = newSlope;
                    else locationRight = locationRight.prev;
                }
            }
            markedTopLeft = locationLeft;
            markedTopRight = locationRight;

            //find the bottom line setup
            previousLeft = null;
            previousRight = null;
            locationLeft = rightMost;
            locationRight = head2;
            initialSlope = calculateSlope(rightMost.point.X, rightMost.point.Y, head2.point.X, head2.point.Y);
            newSlope = initialSlope;
            //find the bottom line
            while (previousLeft != locationLeft || previousRight != locationRight)
            {
                newSlope = initialSlope;
                previousLeft = locationLeft;
                while (initialSlope == newSlope)
                {
                    locationLeft = locationLeft.next;
                    newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
                    if (newSlope < initialSlope) initialSlope = newSlope;
                    else locationLeft = locationLeft.prev;
                }
                newSlope = initialSlope;
                previousRight = locationRight;
                while (initialSlope == newSlope)
                {
                    locationRight = locationRight.prev;
                    newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
                    if (newSlope > initialSlope) initialSlope = newSlope;
                    else locationRight = locationRight.next;
                }
            }
            markedTopLeft.next = markedTopRight;
            markedTopRight.prev = markedTopLeft;
            locationLeft.prev = locationRight;
            locationRight.next = locationLeft;

            //reset size
            resetSize1();
        }

        private void findRightmostPoint()
        {
            if (head1 == null) return;
            rightMost = head1;
            Node location = head1;
            for(int i = 0; i < size1; i++)
            {
                if (location.point.X > rightMost.point.X) rightMost = location;
                location = location.next;
            }
        }

        //Calculates a slope given x and y coordinates of two points
        private double calculateSlope(double x1, double y1, double x2, double y2)
        {
            return ((y2 - y1) / (x2 - x1));
        }

        private void resetSize1()
        {
            int counter = 1;
            Node location = head1.next;
            while(location != head1)
            {
                counter++;
                location = location.next;
            }
            size1 = counter;
        }

        /*
        * <pre> merge function has been called
        * <post> returns a list of pointf objs to caller
        */
        public List<PointF> toListOfPoints()
        {
            if (head1 == null) return null;
            List<PointF> list = new List<PointF>();
            Node location = head1;
            for(int i = 0; i < size1; i++)
            {
                list.Add(location.point);
                location = location.next;
            }
            return list;
        }

        public PointF getHead1()
        {
            return head1.point;
        }

        public PointF getHead2()
        {
            return head2.point;
        }

        public PointF getTail1()
        {
            return tail1.point;
        }

        public PointF getTail2()
        {
            return tail2.point;
        }

        public PointF getRightMostItem()
        {
            return rightMost.point;
        }

        public int getSize1()
        {
            return size1;
        }

        public int getSize2()
        {
            return size2;
        }

        private void printToConsole()
        {
            Node location = head1;
            Console.WriteLine("Printing list 1 in order");
            for(int i = 0; i < size1; i++)
            {
                Console.WriteLine(location.point.ToString());
                location = location.next;
            }
            location = head2;
            Console.WriteLine("Printing list 2 in order");
            for (int i = 0; i < size2; i++)
            {
                Console.WriteLine(location.point.ToString());
                location = location.next;
            }
        }

        public void print1List(Pen pen, Graphics g)
        {
            Node location = head1;
            for(int i = 0; i < size1; i++)
            {
                Console.WriteLine(location.point.ToString());
                g.DrawLine(pen, location.point, location.next.point);
                location = location.next;
            }
        }

        public void print2Lists(Pen pen, Graphics g)
        {
            Node location = head1;
            for (int i = 0; i < size1; i++)
            {
                Console.WriteLine(location.point.ToString());
                g.DrawLine(pen, location.point, location.next.point);
                location = location.next;
            }
            location = head2;
            for (int i = 0; i < size2; i++)
            {
                Console.WriteLine(location.point.ToString());
                g.DrawLine(pen, location.point, location.next.point);
                location = location.next;
            }
        }
    }
}
