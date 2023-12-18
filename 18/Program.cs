string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\18\\input";
StreamReader reader = new StreamReader(input);

int width = 1;
int length = 1;

string line;
List<char> dirs = new();
List<int> lens = new();
List<string> colors = new();
Dictionary<int, char> dirDict = new()
{
    { 0, 'R' },
    { 1, 'D' },
    { 2, 'L' },
    { 3, 'U' }
};
while ((line = reader.ReadLine()!) != null) {
    string[] split = line.Split();
    dirs.Add(split[0][0]);
    lens.Add(int.Parse(split[1]));
    // string color = split[2].Trim('(', '#', ')');
    // dirs.Add(dirDict[color[^1] - '0']);
    // lens.Add(Convert.ToInt32(color[..5], 16));
}

Console.WriteLine(string.Join(",", lens));

int curWidth = 1;
int curLength = 1;
int R = 0; int L = 0; int U = 0; int D = 0;
for (int i = 0; i < dirs.Count; i++) {
    switch (dirs[i]) {
        case 'R':
            curWidth += lens[i];
            R = Math.Max(R, curWidth);
            break;
        case 'L':
            curWidth -= lens[i];
            L = Math.Max(L, curWidth * -1);
            break;
        case 'U':
            curLength -= lens[i];
            U = Math.Max(U, -1 * curLength);
            break;
        case 'D':
            curLength += lens[i];
            D = Math.Max(D, curLength);
            break;
    }
    length = Math.Max(length, Math.Abs(curLength));
    width = Math.Max(width, Math.Abs(curWidth));
}
length = U + D + 1;
width = R + L + 1;
Console.WriteLine(string.Join(',', new int[]{length, width}));
char[,] grid = new char[length, width];
for (int i = 0; i < length; i++) {for (int j = 0; j < width; j++) {grid[i,j] = '.';}}
int curI = U+1; int curJ = L+1;
Console.WriteLine(string.Join(',', new int[]{R,L,U,D}));
Console.WriteLine(string.Join(',', new int[]{length, width}));
for (int it = 0; it < dirs.Count; it++) {
    Console.WriteLine(curI.ToString() + " " + curJ.ToString() + " " + dirs[it] + " " + lens[it]);
    switch (dirs[it]) {
        case 'R':
            for (int _ = 0; _ < lens[it]; _++) {grid[curI, curJ++] = '#';}
            break;
        case 'L':
            for (int _ = 0; _ < lens[it]; _++) {grid[curI, curJ--] = '#';}
            break;
        case 'U':
            for (int _ = 0; _ < lens[it]; _++) {grid[curI--, curJ] = '#';}
            break;
        case 'D':
            for (int _ = 0; _ < lens[it]; _++) {grid[curI++, curJ] = '#';}
            break;
    }
}

int area = 0;
bool inside;
for (int i = 0; i < grid.GetLength(0); i++) {
    inside = false;
    for (int j = 0; j < grid.GetLength(1); j++) {
        if (grid[i,j] == '#') {
            area++;
            if ((j == 0 || grid[i,j-1] != '#') && (j + 1 == grid.GetLength(1) || grid[i,j+1] != '#')) {
                inside = !inside;
            } else {
                if (j == 0 || grid[i,j-1] != '#') {
                    if (i != 0 && grid[i-1, j] == '#') {
                        inside = !inside;
                    }
                } else if (j + 1 == grid.GetLength(1) || grid[i,j+1] != '#') {
                    if (i != 0 && grid[i-1, j] == '#') {
                        inside = !inside;
                    }
                }
            }
        } else {
            if (inside) {
                grid[i,j] = ' ';
                area++;
            }
        }
        Console.Write(grid[i,j]);
    }
    Console.WriteLine();
}

Console.WriteLine(area);