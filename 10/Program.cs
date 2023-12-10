using Microsoft.VisualBasic;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\10\\input";
StreamReader reader = new StreamReader(input);

string line;
long p1 = 0;
long p2 = 0;
long loopLength = 0;
List<List<char>> grid = new List<List<char>>();
int sRow = 0;
int sCol = 0;
while ((line = reader.ReadLine()!) != null) {
    grid.Add(new List<char>(line.ToCharArray()));
    if (line.Contains("S")) {
        sRow = grid.Count - 1;
        sCol = line.IndexOf('S');
    }
}

int curRow;
int curCol;

bool FindFirstPipe(List<List<char>> grid, out int curRow, out int curCol) {
    if (sRow != 0 && (grid[sRow-1][sCol] is '|' or 'F' or '7')) {
        curRow = sRow - 1; curCol = sCol;
        return true;
    } else if (sRow != (grid.Count - 1) && (grid[sRow+1][sCol] is '|' or 'L' or 'J')) {
        curRow = sRow + 1; curCol = sCol;
        return true;
    } else if (sCol != 0 && (grid[sRow][sCol-1] is '-' or 'L' or 'F')) {
        curRow = sRow; curCol = sCol - 1;
        return true;
    } else {
        curRow = sRow; curCol = sCol + 1;
        return true;
    }
}

FindFirstPipe(grid, out curRow, out curCol);
grid[sRow][sCol] = '*';

int length = 1;
char curVal;

HashSet<char> north = new HashSet<char>(['|', 'L', 'J']);
HashSet<char> south = new HashSet<char>(['|', '7', 'F']);
HashSet<char> east = new HashSet<char>(['-', 'L', 'F']);
HashSet<char> west = new HashSet<char>(['-', '7', 'J']);

bool checkAbove(int i, int j) {
    if (i == 0) return false;
    return south.Contains(grid[i-1][j]);
}
bool checkBelow(int i, int j) {
    if (i == grid.Count - 1) return false;
    return north.Contains(grid[i+1][j]);
}
bool checkLeft(int i, int j) {
    if (j == 0) return false;
    return east.Contains(grid[i][j-1]);
}
bool checkRight(int i, int j) {
    if (j == grid[0].Count - 1) return false;
    return west.Contains(grid[i][j+1]);
}

while (!((curVal = grid[curRow][curCol]) is '.' or '*')) {
    grid[curRow][curCol] = '*'; length++;
    switch (curVal) {
        case 'F':
            if (checkBelow(curRow, curCol)) curRow++;
            else curCol++;
            break;
        case '|':
            if (checkBelow(curRow, curCol)) curRow++;
            else curRow--;
            break;
        case '7':
            if (checkBelow(curRow, curCol)) curRow++;
            else curCol--;
            break;
        case 'J':
            if (checkAbove(curRow, curCol)) curRow--;
            else curCol--;
            break;
        case 'L':
            if (checkAbove(curRow, curCol)) curRow--;
            else curCol++;
            break;
        case '-':
            if (checkRight(curRow, curCol)) curCol++; 
            else curCol--;
            break;
        case '_':
            break;
    }
}

Console.WriteLine("Part 1: " + (length / 2).ToString());

foreach (List<char> row in grid) {
    foreach (char c in row) {
        Console.Write(c);
    }
    Console.WriteLine();
}