using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace TSP
{

    class ProblemAndSolver
    {

        private class TSPSolution
        {
            /// <summary>
            /// we use the representation [cityB,cityA,cityC] 
            /// to mean that cityB is the first city in the solution, cityA is the second, cityC is the third 
            /// and the edge from cityC to cityB is the final edge in the path.  
            /// You are, of course, free to use a different representation if it would be more convenient or efficient 
            /// for your node data structure and search algorithm. 
            /// </summary>
            public ArrayList
                Route;

            public TSPSolution(ArrayList iroute)
            {
                Route = new ArrayList(iroute);
            }


            /// <summary>
            /// Compute the cost of the current route.  
            /// Note: This does not check that the route is complete.
            /// It assumes that the route passes from the last city back to the first city. 
            /// </summary>
            /// <returns></returns>
            public double costOfRoute()
            {
                // go through each edge in the route and add up the cost. 
                int x;
                City here;
                double cost = 0D;

                for (x = 0; x < Route.Count - 1; x++)
                {
                    here = Route[x] as City;
                    cost += here.costToGetTo(Route[x + 1] as City);
                }

                // go from the last city to the first. 
                here = Route[Route.Count - 1] as City;
                cost += here.costToGetTo(Route[0] as City);
                return cost;
            }
        }

        #region Private members 

        /// <summary>
        /// Default number of cities (unused -- to set defaults, change the values in the GUI form)
        /// </summary>
        // (This is no longer used -- to set default values, edit the form directly.  Open Form1.cs,
        // click on the Problem Size text box, go to the Properties window (lower right corner), 
        // and change the "Text" value.)
        private const int DEFAULT_SIZE = 25;

        private const int CITY_ICON_SIZE = 5;

        // For normal and hard modes:
        // hard mode only
        private const double FRACTION_OF_PATHS_TO_REMOVE = 0.20;

        /// <summary>
        /// the cities in the current problem.
        /// </summary>
        private City[] Cities;
        /// <summary>
        /// a route through the current problem, useful as a temporary variable. 
        /// </summary>
        private ArrayList Route;
        /// <summary>
        /// best solution so far. 
        /// </summary>
        private TSPSolution bssf; 

        /// <summary>
        /// how to color various things. 
        /// </summary>
        private Brush cityBrushStartStyle;
        private Brush cityBrushStyle;
        private Pen routePenStyle;


        /// <summary>
        /// keep track of the seed value so that the same sequence of problems can be 
        /// regenerated next time the generator is run. 
        /// </summary>
        private int _seed;
        /// <summary>
        /// number of cities to include in a problem. 
        /// </summary>
        private int _size;

        /// <summary>
        /// Difficulty level
        /// </summary>
        private HardMode.Modes _mode;

        /// <summary>
        /// random number generator. 
        /// </summary>
        private Random rnd;
        #endregion

        #region Public members
        public int Size
        {
            get { return _size; }
        }

        public int Seed
        {
            get { return _seed; }
        }
        #endregion

        #region Constructors
        public ProblemAndSolver()
        {
            this._seed = 1; 
            rnd = new Random(1);
            this._size = DEFAULT_SIZE;

            this.resetData();
        }

        public ProblemAndSolver(int seed)
        {
            this._seed = seed;
            rnd = new Random(seed);
            this._size = DEFAULT_SIZE;

            this.resetData();
        }

        public ProblemAndSolver(int seed, int size)
        {
            this._seed = seed;
            this._size = size;
            rnd = new Random(seed); 
            this.resetData();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Reset the problem instance.
        /// </summary>
        private void resetData()
        {

            Cities = new City[_size];
            Route = new ArrayList(_size);
            bssf = null;

            if (_mode == HardMode.Modes.Easy)
            {
                for (int i = 0; i < _size; i++)
                    Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble());
            }
            else // Medium and hard
            {
                for (int i = 0; i < _size; i++)
                    Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble() * City.MAX_ELEVATION);
            }

            HardMode mm = new HardMode(this._mode, this.rnd, Cities);
            if (_mode == HardMode.Modes.Hard)
            {
                int edgesToRemove = (int)(_size * FRACTION_OF_PATHS_TO_REMOVE);
                mm.removePaths(edgesToRemove);
            }
            City.setModeManager(mm);

            cityBrushStyle = new SolidBrush(Color.Black);
            cityBrushStartStyle = new SolidBrush(Color.Red);
            routePenStyle = new Pen(Color.Blue,1);
            routePenStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        //public void GenerateProblem(int size) // unused
        //{
        //   this.GenerateProblem(size, Modes.Normal);
        //}

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        public void GenerateProblem(int size, HardMode.Modes mode)
        {
            this._size = size;
            this._mode = mode;
            resetData();
        }

        /// <summary>
        /// return a copy of the cities in this problem. 
        /// </summary>
        /// <returns>array of cities</returns>
        public City[] GetCities()
        {
            City[] retCities = new City[Cities.Length];
            Array.Copy(Cities, retCities, Cities.Length);
            return retCities;
        }

        /// <summary>
        /// draw the cities in the problem.  if the bssf member is defined, then
        /// draw that too. 
        /// </summary>
        /// <param name="g">where to draw the stuff</param>
        public void Draw(Graphics g)
        {
            float width  = g.VisibleClipBounds.Width-45F;
            float height = g.VisibleClipBounds.Height-45F;
            Font labelFont = new Font("Arial", 10);

            // Draw lines
            if (bssf != null)
            {
                // make a list of points. 
                Point[] ps = new Point[bssf.Route.Count];
                int index = 0;
                foreach (City c in bssf.Route)
                {
                    if (index < bssf.Route.Count -1)
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[index+1]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    else 
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[0]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    ps[index++] = new Point((int)(c.X * width) + CITY_ICON_SIZE / 2, (int)(c.Y * height) + CITY_ICON_SIZE / 2);
                }

                if (ps.Length > 0)
                {
                    g.DrawLines(routePenStyle, ps);
                    g.FillEllipse(cityBrushStartStyle, (float)Cities[0].X * width - 1, (float)Cities[0].Y * height - 1, CITY_ICON_SIZE + 2, CITY_ICON_SIZE + 2);
                }

                // draw the last line. 
                g.DrawLine(routePenStyle, ps[0], ps[ps.Length - 1]);
            }

            // Draw city dots
            foreach (City c in Cities)
            {
                g.FillEllipse(cityBrushStyle, (float)c.X * width, (float)c.Y * height, CITY_ICON_SIZE, CITY_ICON_SIZE);
            }

        }

        /// <summary>
        ///  return the cost of the best solution so far. 
        /// </summary>
        /// <returns></returns>
        public double costOfBssf ()
        {
            if (bssf != null)
                return (bssf.costOfRoute());
            else
                return -1D; 
        }

        /// <summary>
        ///  solve the problem.  This is the entry point for the solver when the run button is clicked
        /// right now it just picks a simple solution. 
        /// </summary>
        public void solveProblem()
        {

            //start the time
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //Farthest Insertion Algorithm
            ArrayList bssfRoute = new ArrayList();
            int m = 0;
            int r = 0;
            double maxCost = 0;
            //Find initial vertices to add to subgraph
            for (int k = 0; k < Cities.Length; ++k)
            {
                for (int j = k + 1; j < Cities.Length; ++j)
                {
                    double cost = Cities[k].costToGetTo(Cities[j]);
                    if (maxCost < cost)
                    {
                        m = k;
                        r = j;
                        maxCost = cost;
                    }
                }
            }

            bssfRoute.Add(m);
            bssfRoute.Add(r);
            int first = 0;
            int last = 0;
            double minCost;
            //Run until all cities are accounted for
            while (bssfRoute.Count < Cities.Length)
            {
                maxCost = 0;
                m = 0;
                //Find the vertex that is farthest away from any vertex in the sub-graph
                foreach (int inBssf in bssfRoute)
                {
                    for (int k = 0; k < Cities.Length; ++k)
                    {
                        //Only add if we haven't visited and the cost is larger
                        if (!bssfRoute.Contains(k) && maxCost <
                            Cities[k].costToGetTo(Cities[inBssf]))
                        {
                            m = k;
                            maxCost = Cities[k].costToGetTo(Cities[inBssf]);
                        }
                    }
                }
                //Find the arc(i,j) that has the cheapest cost to insert the newest vertex
                minCost = double.PositiveInfinity;
                for (int k = 0; k < bssfRoute.Count; ++k)
                {
                    int kIndex = (int)bssfRoute[k];
                    int j = k + 1;
                    if (j == bssfRoute.Count)
                        j = 0;
                    int jIndex = (int)bssfRoute[j];
                    double cost = findMinCost(kIndex, jIndex, m);
                    if (cost < minCost)
                    {
                        first = kIndex;
                        last = j;
                        minCost = cost;
                    }
                }
                bssfRoute.Insert(last, m);
            }

            //Convert indexes to Route of cities
            int bssfchangecounter = 0;
            Route = new ArrayList();
            foreach (int k in bssfRoute)
                Route.Add(Cities[k]);

            bssf = new TSPSolution(Route);

            double bssfCost = bssf.costOfRoute();
            Boolean hasChanged = true;
            Boolean didChange = false;

            //Check to see if swapping any two vertices produces a better route 
            while (hasChanged)
            {
                hasChanged = false;
                didChange = false;
                ArrayList holder = (ArrayList)Route.Clone();
                for (int i = 0; i < holder.Count; ++i)
                {
                    for (int j = i + 1; j < holder.Count; ++j)
                    {
                        ArrayList temporary = (ArrayList)holder.Clone();
                        City c = (City)temporary[i];
                        temporary[i] = temporary[j];
                        temporary[j] = c;
                        TSPSolution newBssf = new TSPSolution(temporary);
                        if (bssfCost > newBssf.costOfRoute())
                        {
                            hasChanged = true;
                            didChange = true;
                            holder = (ArrayList)temporary.Clone();
                            bssfCost = newBssf.costOfRoute();
                        }
                    }
                }
                if (didChange)
                {
                    Route = (ArrayList)holder.Clone();
                    bssf = new TSPSolution(Route);
                    bssfchangecounter++;
                }
            }
            Console.WriteLine("bssf changed " + bssfchangecounter + " number of times");
            //update the BSSF
            bssf = new TSPSolution(Route);
            bssfCost = bssf.costOfRoute();

            //Prepare to start Branch and Bound
            //create matrix of distances
            double totalCosts = 0;
            double[,] matrix = new double[Cities.Length, Cities.Length];
            for (int i = 0; i < Cities.Length; i++)
            {
                matrix[i, i] = double.PositiveInfinity;
                for (int j = i + 1; j < Cities.Length; j++)
                {
                    double cost = Cities[i].costToGetTo(Cities[j]);
                    matrix[i, j] = cost;
                    matrix[j, i] = cost;
                    totalCosts += cost;
                }
            }

            //get children of initial state
            ArrayList childrenIndexes = new ArrayList();
            for (int i = 1; i < Cities.Length; i++)
                childrenIndexes.Add(i);

            //generate initial state
            State s = new State(0, childrenIndexes, (double[,])matrix.Clone(),
                new ArrayList(), 0, 0, Cities, 0);

            //bound on initial state
            double bound = s.lowerBound;

            //initial state onto queue
            TSP.PriorityQueue<double, State> q =
                new TSP.PriorityQueue<double, State>();
            q.Enqueue(bound - DEFAULT_SIZE * s.depthIntoSolution, s);

            int maxSize = 0;

            int numberofstatescreated = 1;
            //Branch and Bound: 30 second time limit
            while (!q.IsEmpty && timer.Elapsed.Seconds < 30)
            {
                if (q.Count > maxSize)
                    maxSize = q.Count;

                State u = q.Dequeue().Value;

                //lazy pruning
                if (u.lowerBound < bssfCost)
                {
                    ArrayList childStates = u.generateChildrenStates();
                    numberofstatescreated += childStates.Count;
                    foreach (State w in childStates)
                    {
                        if (timer.Elapsed.Seconds > 30)
                            break;
                        //only deal with child state if bound is lower than bssf
                        if (w.lowerBound < bssfCost)
                        {
                            if (w.solutionFound && w.currentCost < bssfCost)
                            {
                                bssfCost = w.currentCost;
                                bssfRoute = w.pathSoFar;
                            }
                            else
                            {
                                double cBound = w.lowerBound;
                                q.Enqueue(w.lowerBound - DEFAULT_SIZE *
                                    w.depthIntoSolution, w);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("States created " + numberofstatescreated);
            Console.WriteLine("Max number of states stored = " + maxSize);
            Console.WriteLine("Numnber of states pruned = " + (numberofstatescreated - maxSize));

            timer.Stop();

            bssf = new TSPSolution(Route);

            //write out solution
            Program.MainForm.tbCostOfTour.Text = " "
                + bssf.costOfRoute();
            TimeSpan ts = timer.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}",
                ts.Minutes, ts.Seconds);
            Program.MainForm.tbElapsedTime.Text = elapsedTime;
            Program.MainForm.Invalidate();

            Console.WriteLine(ToStringSolution());
        }

        public double findMinCost(int i, int j, int r)
        {
            double cost = Cities[r].costToGetTo(Cities[i]) +
                Cities[j].costToGetTo(Cities[r]);
            cost -= Cities[i].costToGetTo(Cities[j]);
            return cost;
        }

        public String ToStringSolution()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('[');

            foreach (City c in Route)
            {
                builder.Append("[" + c.X + "," + c.Y + "],");
            }
            builder.Remove(builder.Length - 1, 1); //Remove extra comma at end
            builder.Append(']');

            return builder.ToString();
        }
        #endregion

        #region State Object

        private class State
        {
            //Definition of state
            public ArrayList pathSoFar;
            public int currentIndex;
            public ArrayList childrenIndexes;
            public double[,] costArray;
            public double currentCost;
            public City[] cities;
            public double lowerBound;
            public int depthIntoSolution;
            public bool solutionFound;

            public State(double b, ArrayList c, double[,] costs, ArrayList path,
                double startCost, int index, City[] cit, int d)
            {
                lowerBound = b;
                childrenIndexes = c;
                costArray = costs;
                pathSoFar = path;
                solutionFound = false;
                currentCost = startCost;
                currentIndex = index;
                cities = cit;
                depthIntoSolution = d;

                //solution found when there are no children to explore 
                if (childrenIndexes.Count == 0)
                    solutionFound = true;

                //generate the lower bound
                this.lowerBound = b + reducedCostMatrix();

                if (solutionFound)
                {
                    currentCost += cit[0].costToGetTo(cit[index]);
                    pathSoFar.Add(index);
                }
            }

            private double reducedCostMatrix()
            {
                double[,] reducedMatrix = costArray;
                double costSoFar = 0;
                double rowsNotChanged = 0;

                //row reduction
                ArrayList rows = (ArrayList)childrenIndexes.Clone();
                rows.Add(currentIndex);
                foreach (int i in rows)
                {
                    double minCost = double.PositiveInfinity;
                    foreach (int j in childrenIndexes)
                        if (reducedMatrix[i, j] < minCost)
                            minCost = reducedMatrix[i, j];

                    if (!double.IsPositiveInfinity(minCost))
                    {
                        foreach (int j in childrenIndexes)
                            reducedMatrix[i, j] -= minCost;
                        costSoFar += minCost;
                    }
                    else
                        rowsNotChanged++;
                }

                //column reduction
                foreach (int i in childrenIndexes)
                {
                    double minCost = double.PositiveInfinity;
                    foreach (int j in rows)
                        if (reducedMatrix[j, i] < minCost)
                            minCost = reducedMatrix[j, i];
                    if (!double.IsPositiveInfinity(minCost))
                        foreach (int j in rows)
                            reducedMatrix[j, i] -= minCost;
                    else
                        rowsNotChanged++;
                }

                if (rowsNotChanged >= reducedMatrix.GetUpperBound(0))
                {
                    solutionFound = true;
                    costSoFar += cities[1].costToGetTo(cities[currentIndex]);
                }

                if (solutionFound)
                    return 0;

                return costSoFar;
            }

            public ArrayList generateChildrenStates()
            {
                ArrayList children = new ArrayList();
                //create child state for all possibilities
                foreach (int i in childrenIndexes)
                {
                    ArrayList newChildren = (ArrayList)childrenIndexes.Clone();
                    newChildren.Remove(i);

                    // update the path based on this child
                    ArrayList newPath = (ArrayList)pathSoFar.Clone();
                    newPath.Add(currentIndex);
                    double cost = cities[currentIndex].costToGetTo(cities[i]);

                    //reduced matrix for new state
                    double[,] newCost = (double[,])costArray.Clone();
                    for (int j = 0; j <= newCost.GetUpperBound(0); j++)
                        newCost[j, currentIndex] = double.PositiveInfinity;

                    children.Add(new State(lowerBound + costArray[currentIndex, i],
                        newChildren, newCost, newPath,
                        currentCost + cost, i, cities, depthIntoSolution + 1));
                }
                return children;
            }
        }
        #endregion
    }
}
