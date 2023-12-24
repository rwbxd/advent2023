int day = 22;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";

Part1();
Part2();

void Part1() {
    StreamReader reader = new StreamReader(input);
    List<Brick> bricks = new List<Brick>();
    string line;
    int brickNum = 1;
    while ((line = reader.ReadLine()!) != null) {
        bricks.Add(new Brick(line, brickNum++));
    }

    ShiftBricks(bricks);
    bricks.Sort();

    Dictionary<Brick, int> supportingBricks = new Dictionary<Brick, int>();
    Dictionary<Brick, List<Brick>> supportingList = new Dictionary<Brick, List<Brick>>();
    foreach (Brick b in bricks) {
        supportingBricks[b] = 0;
        supportingList[b] = []; // what is it supporting
    }

    bricks.Reverse();
    for (int z = bricks.First().z2; z >= 1; z--) {
        Console.WriteLine(z.ToString() + ": " + string.Join(",", bricks.Where(x => x.z1 >= z && x.z2 <= z).Select(x => x.ID)));
    }

    foreach (Brick b in bricks) {
        foreach (Brick otherB in bricks) {
            if (b.Equals(otherB)) {continue;}

            if (otherB.z1 != (b.z2 + 1)) {continue;}

            if (b.Overlaps(otherB)) {
                // Console.WriteLine(b.ID + " supports " + otherB.ID);
                supportingBricks[otherB]++;
                supportingList[b].Add(otherB);
            }
        }
    }
    
    int count = 0;
    foreach (Brick b in bricks) {
        bool supporter = false;
        foreach (Brick s in supportingList[b]) {
            if (supportingBricks[s] == 1) {
                supporter = true;
            }
        }
        if (!supporter) {count++;}
    }
    Console.WriteLine("Part 1: " + count);
}

void Part2() {
    StreamReader reader = new StreamReader(input);
    List<Brick> bricks = new List<Brick>();
    string line;
    int brickNum = 1;
    while ((line = reader.ReadLine()!) != null) {
        bricks.Add(new Brick(line, brickNum++));
    }

    ShiftBricks(bricks);
    bricks.Sort();

    Dictionary<Brick, HashSet<Brick>> supportedBy = new Dictionary<Brick, HashSet<Brick>>();
    Dictionary<Brick, HashSet<Brick>> supporting = new Dictionary<Brick, HashSet<Brick>>();
    foreach (Brick b in bricks) {
        supportedBy[b] = []; // how many it supports
        supporting[b] = []; // what supports it
    }

    bricks.Reverse();
    for (int z = bricks.First().z2; z >= 1; z--) {
        Console.WriteLine(z.ToString() + ": " + string.Join(",", bricks.Where(x => x.z1 >= z && x.z2 <= z).Select(x => x.ID)));
    }

    foreach (Brick b in bricks) {
        foreach (Brick otherB in bricks) {
            if (b.Equals(otherB)) {continue;}

            if (otherB.z1 != (b.z2 + 1)) {continue;}

            if (b.Overlaps(otherB)) {
                // Console.WriteLine(b.ID + " supports " + otherB.ID);
                supportedBy[otherB].Add(b);
                supporting[b].Add(otherB);
            }
        }
    }
    
    int count = 0;
    
    foreach (Brick b in bricks) {
        HashSet<Brick> falling = [b];
        Queue<Brick> q = new Queue<Brick>(supporting[b].Where(x => supportedBy[x].Count == 1));
        while (q.Count != 0) {
            Brick s = q.Dequeue();
            if (falling.Contains(s)) {continue;}

            falling.Add(s);
            foreach (var v in supporting[s].Where(x => supportedBy[x].All(y => falling.Contains(y)))) {
                q.Enqueue(v);
            }
        }
        count += falling.Count - 1;
    }
    Console.WriteLine("Part 2: " + count);
}

void ShiftBricks(List<Brick> bricks) {
    bricks.Sort();
    foreach (Brick b in bricks) {
        while (b.z1 > 1) {
            bool canMove = true;
            foreach (Brick otherB in bricks.Where(x => (x.z2 + 1) == b.z1)) {
                // Console.WriteLine("Comparing " + b + " and " + otherB);
                if ((otherB.z2 + 1) < b.z1) {
                    // Console.WriteLine("Continuing...");
                    continue; // ignore this turn
                } 
                if (otherB.z1 >= b.z1) {
                    // Console.WriteLine("We've finished " + b);
                    break; // done this turn
                }

                if (b.Overlaps(otherB)) {
                    // Console.WriteLine("B overlaps otherB");
                    canMove = false;
                    break;
                }
            }
            if (!canMove) {break;}
            else {
                // Console.Write("Decrementing " + b);
                b.z1--;
                b.z2--;
                // Console.WriteLine(" to " + b);
            }
        }
    }
}

class Brick : IComparable<Brick> {
    public int x1; public int y1; public int z1;
    public int x2; public int y2; public int z2;
    public int ID;
    public List<Point> points = [];
    public Brick(string brick, int brickID) {
        int[][] dims = brick.Split("~").Select(b => b.Split(',').Select(int.Parse).ToArray()).ToArray();
        x1 = Math.Min(dims[0][0], dims[1][0]);
        x2 = Math.Max(dims[0][0], dims[1][0]);
        y1 = Math.Min(dims[0][1], dims[1][1]);
        y2 = Math.Max(dims[0][1], dims[1][1]);
        z1 = Math.Min(dims[0][2], dims[1][2]);
        z2 = Math.Max(dims[0][2], dims[1][2]);
        ID = brickID;

        for (int x = x1; x <= x2; x++) {
            for (int y = y1; y <= y2; y++) {
                points.Add(new Point(x, y));
            }
        }
    }

    public override string ToString() {
        return string.Format("{0},{1},{2}~{3},{4},{5}", x1, y1, z1, x2, y2, z2);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        
        Brick other = (Brick) obj;
        return other.x1 == x1 && other.y1 == y1 && other.z1 == z1 && other.x2 == x2 && other.y2 == y2 && other.z2 == z2;
    }

    public int CompareTo(Brick other) {
        if (this.z2 != other.z2) {
            return this.z2.CompareTo(other.z2);
        } else if (this.z1 != other.z1) {
            return this.z1.CompareTo(other.z1);
        } else if (this.y2 != other.y2) {
            return this.y2.CompareTo(other.y2);
        } else if (this.y1 != other.y1) {
            return this.y1.CompareTo(other.y1);
        } else if (this.x2 != other.x2) {
            return this.x2.CompareTo(other.x2);
        } else {
            return this.x1.CompareTo(other.x1);
        }
    }

    public bool Overlaps(Brick other) {
        foreach (var p in points) {
            if (other.points.Contains(p)) {
                return true;
            }
        }
        return false;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}

class Point {
    public int x; public int y;
    
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        Point other = (Point) obj;
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}