using System.Runtime.CompilerServices;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\13\\input";
StreamReader reader = new StreamReader(input);

string line;
long p1 = 0;
long p2 = 0;
while (!reader.EndOfStream) {
    
    List<List<char>> grid = new List<List<char>>();
    while ((line = reader.ReadLine()!) != null && line != "") {
        grid.Add(new List<char>(line.ToCharArray()));
    }

    printGrid(grid);

    bool p1v = false;
    int result;
    if ((result = findVertical(grid)) != -1) {
        p1v = true;
        p1 += result;
    } else {
        result = findHorizontal(grid);
        p1 += 100 * result;
    }
    int p1result = result - 1;

    bool foundValidSmudge = false;
    for (int i = 0; i < grid.Count && !foundValidSmudge; i++) {
        for (int j = 0; j < grid[0].Count; j++) {
            printGrid(grid);
            if (grid[i][j] == '#') {
                grid[i][j] = '.';
            } else {
                grid[i][j] = '#';
            }

            if (p1v) {
                if ((result = findVertical(grid, p1result)) != -1) {
                    p2 += result;
                    foundValidSmudge = true;
                } else if ((result = findHorizontal(grid)) != -1) {
                    p2 += result * 100;
                    foundValidSmudge = true;
                }
            } else {
                if ((result = findVertical(grid)) != -1) {
                    p2 += result;
                    foundValidSmudge = true;
                } else if ((result = findHorizontal(grid, p1result)) != -1) {
                    p2 += result * 100;
                    foundValidSmudge = true;
                }
            }

            if (foundValidSmudge) {
                printGrid(grid);
                break;
            }

            if (grid[i][j] == '#') {
                grid[i][j] = '.';
            } else {
                grid[i][j] = '#';
            }
        }
    }
}

Console.WriteLine(p1);
Console.WriteLine(p2);

void printGrid(List<List<char>> grid) {
    foreach (var l in grid) {
        foreach (var c in l) {
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

bool findReflection(List<List<char>> grid, out int result, int prev = -1) {
    foreach (var l in grid) {
        foreach (var c in l) {
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();

    int v;
    if ((v = findVertical(grid, prev)) != -1) {
        result = v;
        return true;
    }

    int h;
    if ((h = findHorizontal(grid, prev)) != -1) {
        result = h * 100;
        return true;
    }

    result = -1; return false;
}

int findVertical(List<List<char>> grid, int prev = -1) {
    bool foundVertical = false;
    for (int i = 0; i < grid[0].Count - 1; i++) {
        if (i == prev) continue;
        foundVertical = true;
        for (int j = 0; (i-j) >= 0 && (i+1+j) < grid[0].Count; j++) {
            foreach (List<char> row in grid) {
                if (row[i-j] != row[i+1+j]) {
                    foundVertical = false;
                    break;
                }
            }
            if (!foundVertical) break;
        }
        if (foundVertical) {
            Console.WriteLine("Found vertical: " + i.ToString());
            return i + 1;
        }
    }
    return -1;
}

int findHorizontal(List<List<char>> grid, int prev = -1) {
    bool foundHorizontal = false;
    for (int i = 0; i < grid.Count - 1; i++) {
        if (i == prev) continue;
        foundHorizontal = true;
        for (int j = 0; (i-j) >= 0 && (i+1+j) < grid.Count; j++) {
            if (!Enumerable.SequenceEqual(grid[i-j], grid[i+1+j])) {
                foundHorizontal = false; 
                break;
            }
        }
        if (foundHorizontal) {
            Console.WriteLine("Found horizontal: " + i.ToString());
            return (i+1);
        }
    }
    return -1;
}
