int day = 23;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";
Part1();
Thread thread = new Thread(Part2, 4 * 1024 * 1024);
thread.Start();
thread.Join();

void Part1() {
    StreamReader reader = new StreamReader(input);
    List<List<char>> grid = new List<List<char>>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        grid.Add(line.ToList());
    }

    Console.WriteLine("Part 1: " + FindLongestPath(grid).ToString());
}

void Part2() {
    StreamReader reader = new StreamReader(input);
    List<List<char>> grid = new List<List<char>>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        grid.Add(line.ToList());
    }

    Console.WriteLine("Part 2: " + FindLongestPath2(grid).ToString());
}

long FindLongestPath(List<List<char>> grid, int i = 0, int j = -1, long length = 0) {
    if (j == -1) {j = grid.First().IndexOf('.');}
    int endJ = grid.Last().IndexOf('.'); int endI = grid.Count - 1;
    BoundsChecker bc = new BoundsChecker(grid.Count, grid[0].Count);

    if (i == endI && j == endJ) {
        // Console.WriteLine(length);
        // PrintGrid(grid);
        return length;
    }

    char tmp = grid[i][j];
    grid[i][j] = 'O';

    long max = 0;
    if (bc.check(i+1, j) && !(grid[i+1][j] is '#' or '^' or 'O')) {
        max = Math.Max(max, FindLongestPath(grid, i+1, j, length+1));
    }
    
    if (bc.check(i-1, j) && !(grid[i-1][j] is '#' or 'v' or 'O')) {
        max = Math.Max(max, FindLongestPath(grid, i-1, j, length+1));
    }
    
    if (bc.check(i, j+1) && !(grid[i][j+1] is '#' or '<' or 'O')) {
        max = Math.Max(max, FindLongestPath(grid, i, j+1, length+1));
    }
    
    if (bc.check(i, j-1) && !(grid[i][j-1] is '#' or '>' or 'O')) {
        max = Math.Max(max, FindLongestPath(grid, i, j-1, length+1));
    }

    grid[i][j] = tmp;

    return max;
}

long FindLongestPath2(List<List<char>> grid, int i = 0, int j = -1, long length = 0) {
    if (j == -1) {j = grid.First().IndexOf('.');}
    int endJ = grid.Last().IndexOf('.'); int endI = grid.Count - 1;
    BoundsChecker bc = new BoundsChecker(grid.Count, grid[0].Count);

    if (i == endI && j == endJ) {
        // Console.WriteLine(length);
        // PrintGrid(grid);
        return length;
    }

    grid[i][j] = 'O';

    long max = 0;
    if (bc.check(i+1, j) && !(grid[i+1][j] is '#' or 'O')) {
        max = Math.Max(max, FindLongestPath2(grid, i+1, j, length+1));
    }
    
    if (bc.check(i-1, j) && !(grid[i-1][j] is '#' or 'O')) {
        max = Math.Max(max, FindLongestPath2(grid, i-1, j, length+1));
    }
    
    if (bc.check(i, j+1) && !(grid[i][j+1] is '#' or 'O')) {
        max = Math.Max(max, FindLongestPath2(grid, i, j+1, length+1));
    }
    
    if (bc.check(i, j-1) && !(grid[i][j-1] is '#' or 'O')) {
        max = Math.Max(max, FindLongestPath2(grid, i, j-1, length+1));
    }

    grid[i][j] = '.';

    return max;
}

void PrintGrid(List<List<char>> grid) {
    foreach (var row in grid) {
        foreach (var c in row) {
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

class BoundsChecker(int rows, int cols) {
    int rows = rows; int cols = cols;

    public bool check(int i, int j) {
        return i >= 0 && j >= 0 && i < rows && j < cols;
    }
}