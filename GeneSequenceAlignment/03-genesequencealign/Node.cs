using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticsLab
{
    class Node
    {

        public int lowestCost { get; set; }
        public Node previous { get; set; }
        public char aChar { get; set; }
        public char bChar { get; set; }

        public Node(int lowestCost, Node previous, char aChar, char bChar)
        {
            this.lowestCost = lowestCost;
            this.previous = previous;
            this.aChar = aChar;
            this.bChar = bChar;
        }

        
    }
}
