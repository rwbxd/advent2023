int day = 21;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";
Part1();
Part2();

void Part1() {
    StreamReader reader = new StreamReader(input);
    int i = -1;
    int SI = 0; int SJ = 0;
    List<List<char>> grid = new List<List<char>>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        i++;
        grid.Add(line.ToList());
        if (line.Contains('S')) {
            SI = i;
            SJ = line.IndexOf('S');
        }
    }
    Point marker = new Point(-1, -1);
    Queue<Point> points = new Queue<Point>();
    points.Enqueue(new Point(SI, SJ));
    BoundsChecker bc = new BoundsChecker(grid.Count, grid[0].Count);
    HashSet<Point> hs = new HashSet<Point>();
    for (i = 0; i < 64; i++) {
        // Console.WriteLine(i);
        // Console.WriteLine(points.Count);
        points.Enqueue(marker);
        HashSet<Point> seen = new HashSet<Point>();
        Point p;
        while (!(p = points.Dequeue()).Equals(marker)) {
            foreach (var adj in p.GetAdjacent()) {
                if (!seen.Contains(adj) && bc.Check(adj.i, adj.j) && grid[adj.i][adj.j] != '#') {
                    points.Enqueue(adj);
                    seen.Add(adj);
                }
            }
        }
    }
    Console.WriteLine(points.Count);
}

void Part2() {
    StreamReader reader = new StreamReader(input);
    int i = -1;
    int SI = 0; int SJ = 0;
    List<List<char>> grid = new List<List<char>>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        i++;
        grid.Add(line.ToList());
        if (line.Contains('S')) {
            SI = i;
            SJ = line.IndexOf('S');
        }
    }
    Point marker = new Point(-1, -1);
    Queue<Point> points = new Queue<Point>();
    points.Enqueue(new Point(SI, SJ));
    BoundsChecker bc = new BoundsChecker(grid.Count, grid[0].Count);
    HashSet<Point> hs = new HashSet<Point>();
    for (i = 0; i < 26501365; i++) {
        if (i % 10000 == 0) {Console.WriteLine(i);}
        // Console.WriteLine(points.Count);
        points.Enqueue(marker);
        HashSet<Point> seen = new HashSet<Point>();
        Point p;
        while (!(p = points.Dequeue()).Equals(marker)) {
            foreach (var adj in p.GetAdjacent()) {
                Point adjwrap = bc.Wraparound(adj);
                if (!seen.Contains(adjwrap) && grid[adjwrap.i][adjwrap.j] != '#') {
                    points.Enqueue(adjwrap);
                    seen.Add(adjwrap);
                }
            }
        }
    }
    Console.WriteLine(points.Count);
}

class Point(int i, int j) {
    public int i = i; public int j = j;

    public override bool Equals(object? obj)
    {
        return Equals(obj as Point);
    }

    public override int GetHashCode()
    {
        return string.Format("{0},{1}", i, j).GetHashCode();
    }

    public bool Equals(Point obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        // TODO: write your implementation of Equals() here
        return i == obj.i && j == obj.j;
    }

    public List<Point> GetAdjacent() {
        return [new Point(i-1, j), new Point(i+1, j), new Point(i, j-1), new Point(i, j+1)];
    }
}

class BoundsChecker(int numRows, int numCols) {
    int numRows = numRows; int numCols = numCols;

    public bool Check(int i, int j) {
        return i >= 0 && j >= 0 && i < numRows && j < numCols;
    }

    public Point Wraparound(Point p) {
        if (Check(p.i, p.j)) return p;
        int newi; int newj;

        if (p.i == -1) {
            newi = numRows - 1;
        } else if (p.i == numRows) {
            newi = 0;
        } else {
            newi = p.i;
        }

        if (p.j == -1) {
            newj = numCols - 1;
        } else if (p.j == numCols) {
            newj = 0;
        } else {
            newj = p.j;
        }

        return new Point(newi, newj);
    }
}