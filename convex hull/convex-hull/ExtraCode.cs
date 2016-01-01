using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2_convex_hull
{
    class ExtraCode
    {
    }

    //public void circularLinkedListTestFunction()
    //{
    //    CircularLinkedList cll = new CircularLinkedList();

    //    Console.WriteLine(cll.getSize());

    //    cll.insertNodeAtEnd(new PointF((float)1.0, (float)1.0));
    //    Console.WriteLine(cll.getSize());
    //    Console.WriteLine(cll.getFirstItem().ToString());
    //    Console.WriteLine(cll.getLastItem().ToString());
    //    Console.WriteLine(cll.getRightMostItem().ToString());
    //    Console.WriteLine(cll.getPointAtIndex(0).ToString());
    //    Console.WriteLine(cll.getPointAtIndex(1).ToString());
    //    Console.WriteLine(cll.getPointAtIndex(-1).ToString());


    //    Console.WriteLine("---------------------------------------------------");
    //    cll.insertNodeAtEnd(new PointF((float)5.0, (float)1.0));
    //    cll.insertNodeAtEnd(new PointF((float)1.0, (float)5.0));
    //    cll.insertNodeAtEnd(new PointF((float)5.0, (float)5.0));
    //    Console.WriteLine(cll.getSize());
    //    Console.WriteLine(cll.getFirstItem().ToString());
    //    Console.WriteLine(cll.getLastItem().ToString());
    //    Console.WriteLine(cll.getRightMostItem().ToString());
    //    Console.WriteLine(cll.getPointAtIndex(0).ToString());
    //    Console.WriteLine(cll.getPointAtIndex(2).ToString());
    //    Console.WriteLine(cll.getPointAtIndex(3).ToString());

    //    PointF search = new PointF((float)1.0, (float)5.0);
    //    int index = cll.findPoint(search);
    //    Console.WriteLine("Index = " + index);
    //}



    //public void mergeLists()
    //{
    //    findRightmostPoint();

    //    //find the top line setup
    //    Node locationLeft = rightMost;
    //    Node locationRight = head2;
    //    double initialSlope = calculateSlope(rightMost.point.X, rightMost.point.Y, head2.point.X, head2.point.Y);
    //    double newSlope = initialSlope;
    //    //find the top line
    //    Console.WriteLine(initialSlope);

    //    //for (int i = 0; i < 3; i++)
    //    //{
    //    while (initialSlope == newSlope)
    //    {
    //        locationLeft = locationLeft.prev;
    //        newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
    //        if (newSlope < initialSlope) initialSlope = newSlope;
    //        else locationLeft = locationLeft.next;
    //    }
    //    while (initialSlope == newSlope)
    //    {
    //        locationRight = locationRight.next;
    //        newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
    //        if (newSlope > initialSlope) initialSlope = newSlope;
    //        else locationRight = locationRight.prev;
    //    }
    //    //}
    //    locationLeft.next = locationRight;
    //    locationRight.prev = locationLeft;

    //    //find the bottom line setup
    //    locationLeft = rightMost;
    //    locationRight = head2;
    //    initialSlope = calculateSlope(rightMost.point.X, rightMost.point.Y, head2.point.X, head2.point.Y);
    //    newSlope = initialSlope;
    //    //find the bottom line
    //    //for (int i = 0; i < 3; i++)
    //    //{
    //    while (initialSlope == newSlope)
    //    {
    //        locationLeft = locationLeft.prev;
    //        newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
    //        if (newSlope > initialSlope) initialSlope = newSlope;
    //        else locationLeft = locationLeft.next;
    //    }
    //    while (initialSlope == newSlope)
    //    {
    //        locationRight = locationRight.next;
    //        newSlope = calculateSlope(locationLeft.point.X, locationLeft.point.Y, locationRight.point.X, locationRight.point.Y);
    //        if (newSlope < initialSlope) initialSlope = newSlope;
    //        else locationRight = locationRight.prev;
    //    }
    //    //}
    //    locationLeft.prev = locationRight;
    //    locationRight.next = locationLeft;

    //    //reset size
    //    resetSize1();
    //}
}
