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


int length = 1;
char curVal;

HashSet<char> north = new HashSet<char>(['|', 'L', 'J']);
HashSet<char> south = new HashSet<char>(['|', '7', 'F']);
HashSet<char> east = new HashSet<char>(['-', 'L', 'F']);
HashSet<char> west = new HashSet<char>(['-', '7', 'J']);
HashSet<char> pipeSymbols = new HashSet<char>(['-', '|', 'F', 'J', 'L', '7']);

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

if (checkRight(sRow, sCol) && checkAbove(sRow, sCol)) {
    grid[sRow][sCol] = 'l';
} else if (checkRight(sRow, sCol)) {
    grid[sRow][sCol] = 'f';
} else if (checkAbove(sRow, sCol)) {
    grid[sRow][sCol] = 'j';
} else grid[sRow][sCol] = '}';

while (pipeSymbols.Contains(grid[curRow][curCol])) {
    curVal = grid[curRow][curCol];
    int oldRow = curRow;
    int oldCol = curCol;
    char curSymbol = '*';
    switch (curVal) {
        case 'F':
            if (checkBelow(curRow, curCol)) {curRow++; curSymbol = 'V';}
            else {curCol++; curSymbol = '>';}
            break;
        case '|':
            if (checkBelow(curRow, curCol)) {curRow++; curSymbol = 'V';}
            else {curRow--; curSymbol = '^';}
            break;
        case '7':
            if (checkBelow(curRow, curCol)) {curRow++; curSymbol = 'V';}
            else {curCol--; curSymbol = '<';}
            break;
        case 'J':
            if (checkAbove(curRow, curCol)) {curRow--; curSymbol = '^';}
            else {curCol--; curSymbol = '<';}
            break;
        case 'L':
            if (checkAbove(curRow, curCol)) {curRow--; curSymbol = '^';}
            else {curCol++; curSymbol = '>';}
            break;
        case '-':
            if (checkRight(curRow, curCol)) {curCol++; curSymbol = '>';}
            else {curCol--; curSymbol = '<';}
            break;
        case 'S':
            if (checkRight(curRow, curCol) && checkAbove(curRow, curCol)) curSymbol = 'l';
            else if (checkRight(curRow, curCol)) curSymbol = 'f';
            else if (checkAbove(curRow, curCol)) curSymbol = 'j';
            else curSymbol = '}';
            break;
        case '_':
            break;
    }
    if (curVal is 'F' or 'L' or 'J') curSymbol = (char) (curVal - 'A' + 'a');
    if (curVal is '7') curSymbol = '}';
    grid[oldRow][oldCol] = curSymbol; length++;  
    // Console.WriteLine(grid[curRow][curCol].ToString() + pipeSymbols.Contains(grid[curRow][curCol]).ToString());
}

Console.WriteLine("Part 1: " + (length / 2).ToString());

Dictionary<char, char> cornerPairs = new Dictionary<char, char>();
cornerPairs.Add('f', '7');
cornerPairs.Add('l', 'j');

bool inPipe;
HashSet<char> p2pipes = new HashSet<char>(['V', '^']);
for (int i = 0; i < grid.Count; i++) {
    List<char> row = grid[i];
    inPipe = false;
    for (int j = 0; j < row.Count; j++) {
        char c = row[j];
        Console.Write(c);
        if (p2pipes.Contains(c)) {
            inPipe = !inPipe;
        } else if (c is 'j' or 'l') {
            inPipe = !inPipe;
        } else if (c is 'f' or '}' or '*' or '>' or '<') {
        } else if (inPipe) {
            p2++;
        }
    }
    Console.WriteLine();
}
Console.WriteLine(p2);