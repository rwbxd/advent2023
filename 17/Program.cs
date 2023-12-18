using System.Globalization;
using System.Security.Cryptography;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\17\\input";
StreamReader reader = new StreamReader(input);

List<List<int>> grid = new List<List<int>>();
string line;
while ((line = reader.ReadLine()!) != null) {
    grid.Add(line.ToList().Select(x => (int) char.GetNumericValue(x)).ToList());
}
BoundaryChecker bq = new BoundaryChecker(grid.Count, grid[0].Count);

Console.WriteLine(part1(grid));
Console.WriteLine(part2(grid));

long part1(List<List<int>> grid) {
    List<List<Node>> nodeGrid = new List<List<Node>>(grid.Count);
    for (int i = 0; i < grid.Count; i++) {
        List<Node> nodeRow = new List<Node>(grid[0].Count);
        for (int j = 0; j < grid[0].Count; j++) {
            nodeRow.Add(new Node(i, j));
        }
        nodeGrid.Add(nodeRow);
    }
    Node goal = nodeGrid[grid.Count - 1][grid[0].Count - 1];
    
    // Dictionary<NodeNode, int> cost = new(); // Cost to get there, not including self
    PriorityQueue<NodeNode, int> pq = new();
    NodeNode nn;
    nn = new NodeNode(nodeGrid[0][1], 'R', 1, grid[0][1]);
    pq.Enqueue(nn, grid[0][1] + dist(nn.N, goal));
    // cost[nn] = 0;
    nn = new NodeNode(nodeGrid[1][0], 'D', 1, grid[1][0]);
    pq.Enqueue(nn, grid[1][0] + dist(nn.N, goal));
    // cost[nn] = 0;

    bool foo(int i, int j, int nextCost) {
        return bq.check(i, j);
        // return bq.check(i, j) && (!cost.ContainsKey(nodeGrid[i][j]) || cost[nodeGrid[i][j]] > nextCost);
    }

    bool bar(NodeNode n, char d) {
        if (n.D == d) {
            return n.CD != 3;
        }

        switch (n.D) {
            case 'U': return d != 'D';
            case 'R': return d != 'L';
            case 'L': return d != 'R';
            case 'D': return d != 'U';
        }
        
        return false;
    }

    HashSet<string> seen = new HashSet<string>();
    NodeNode curNN = new NodeNode(nodeGrid[0][0], '?', 0, 0);
    while (pq.Count != 0) {
        
        curNN = pq.Dequeue();
        Node cur = curNN.N;

        if (seen.Contains(curNN.ToString())) {continue;}
        seen.Add(curNN.ToString());

        Console.WriteLine(string.Format("({1}, {2}): {0} | {3}", curNN.Cost, cur.I, cur.J, curNN.D));
        
        if (cur == goal) {
            Console.WriteLine("Found goal");
            break;
        }

        // int nextCost = cost[cur] + grid[cur.I][cur.J];
        int nextCost = curNN.Cost + grid[cur.I][cur.J];
        int ci = cur.I; int cj = cur.J;
        NodeNode newNN;

        if (bar(curNN, 'D') && foo(ci+1, cj, nextCost)) {
            // cost[nodeGrid[ci+1][cj]] = nextCost;
            newNN = createNodeNode(curNN, nodeGrid[ci+1][cj], 'D');
            Console.WriteLine(string.Format("Adding ({0}, {1}) 'D' with cost: {2}", newNN.N.I, newNN.N.J, newNN.Cost));
            pq.Enqueue(newNN, newNN.Cost + dist(newNN.N, goal));
        }

        if (bar(curNN, 'U') && foo(ci-1, cj, nextCost)) {
            // cost[nodeGrid[ci-1][cj]] = nextCost;
            newNN = createNodeNode(curNN, nodeGrid[ci-1][cj], 'U');
            Console.WriteLine(string.Format("Adding ({0}, {1}) 'U' with cost: {2}", newNN.N.I, newNN.N.J, newNN.Cost));
            pq.Enqueue(newNN, newNN.Cost + dist(newNN.N, goal));
        }

        if (bar(curNN, 'R') && foo(ci, cj+1, nextCost)) {
            // cost[nodeGrid[ci][cj+1]] = nextCost;
            newNN = createNodeNode(curNN, nodeGrid[ci][cj+1], 'R');
            Console.WriteLine(string.Format("Adding ({0}, {1}) 'R' with cost: {2}", newNN.N.I, newNN.N.J, newNN.Cost));
            pq.Enqueue(newNN, newNN.Cost + dist(newNN.N, goal));
        }

        if (bar(curNN, 'L') && foo(ci, cj-1, nextCost)) {
            // cost[nodeGrid[ci][cj-1]] = nextCost;
            newNN = createNodeNode(curNN, nodeGrid[ci][cj-1], 'L');
            Console.WriteLine(string.Format("Adding ({0}, {1}) 'L' with cost: {2}", newNN.N.I, newNN.N.J, newNN.Cost));
            pq.Enqueue(newNN, newNN.Cost + dist(newNN.N, goal));
        }
    }

    // foreach (var item in cost) {
    //     Console.WriteLine(string.Format("({0}, {1}): {2}", item.Key.I, item.Key.J, item.Value+grid[item.Key.I][item.Key.J]));
    // }

    Console.WriteLine("\n" + curNN.ToString());

    return curNN.Cost;// + grid[curNN.N.I][curNN.N.J];
}

long part2(List<List<int>> grid) {
    return 0;
}

int dist(Node a, Node b) {
    // return 0;
    return Math.Abs(a.I - b.I) + Math.Abs(a.J - b.J);
}

NodeNode createNodeNode(NodeNode cur, Node next, char dir) {
    // Console.WriteLine("Creating " + dir + " with cost: " + (cur.Cost + grid[next.I][next.J]));
    if (cur.D != dir) {
        return new NodeNode(next, dir, 1, cur.Cost + grid[next.I][next.J]);
    } else {
        return new NodeNode(next, dir, cur.CD + 1, cur.Cost + grid[next.I][next.J]);
    }
}

class Node {
    public int I;
    public int J;

    public Node(int i, int j) {
        I = i; J = j;
    }
}

class NodeNode {
    public Node N;
    public char D;
    public int CD;
    public int Cost;
    public NodeNode(Node n, char d, int charDist, int cost) {
        N = n; D = d; CD = charDist; Cost = cost;
    }
    public override string ToString()
    {
        return string.Format("({0}, {1}): {2} | {3}", N.I, N.J, D, CD);
    }
}

class BoundaryChecker {
    readonly int RC;
    readonly int CC;
    public BoundaryChecker(int rowCount, int colCount) {
        RC = rowCount; CC = colCount;
    }
    public bool check(int i, int j) {
        return i >= 0 && j >= 0 && i < RC && j < CC;
    }
}

