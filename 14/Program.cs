using Grid = System.Collections.Generic.List<char[]>;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\14\\input";
StreamReader reader = new StreamReader(input);
string line;

Grid grid = new Grid();
while ((line = reader.ReadLine()!) != null) {
    grid.Add(line.ToCharArray());
}

Console.WriteLine("Part 1: " + calcNorthWeight(shiftNorth(grid)).ToString());
Console.WriteLine("Part 2: " + calcPart2(grid).ToString());

long calcNorthWeight(Grid g) {
    long result = 0;

    for (int i = 0; i < g.Count; i++) {
        for (int j = 0; j < g.Count; j++) {
            if (g[i][j] == 'O') {
                result += (g.Count - i);
            }
        }
    }
    return result;
}

long calcPart2(Grid g) {
    HashSet<string> seen = new HashSet<string>();
    Grid sc = g;
    int iterations = 0;
    while (!seen.Contains(g2s(sc))) {
        seen.Add(g2s(sc));
        sc = spinCycle(sc);
        // printGrid(sc);
        iterations++;
    }

    int remainingIterations = 1_000_000_000 - iterations;

    string cyclePattern = g2s(sc);
    int cycleLength = 1;
    while (g2s(sc = spinCycle(sc)) != cyclePattern) {
        cycleLength++;
    }

    for (int i = 0; i < (remainingIterations % cycleLength); i++) {
        sc = spinCycle(sc);
    }
    
    Console.WriteLine(remainingIterations.ToString() + " " + cycleLength.ToString());
    return calcNorthWeight(sc);
}

string g2s(Grid g) {
    string res = "";
    foreach (var row in g) {
        foreach (var c in row) {
            res += c;
        }
    }
    return res;
}

void printGrid(Grid g) {
    foreach (var row in g) {
        foreach (var c in row) {
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

Grid spinCycle(Grid g) {
    return shiftEast(shiftSouth(shiftWest(shiftNorth(g))));
}


Grid shiftNorth(Grid g) {
    List<int> north = new List<int>();
    for (int i = 0; i < g[0].Length; i++) north.Add(0);

    for (int i = 0; i < g.Count; i++) {
        for (int j = 0; j < g[0].Length; j++) {
            if (g[i][j] == 'O') {
                g[i][j] = '.';
                g[north[j]++][j] = 'O';
            } else if (g[i][j] == '#') {
                north[j] = i+1;
            }
        }
    }

    return g;
}

Grid shiftWest(Grid g) {
    List<int> west = new List<int>();
    for (int i = 0; i < g.Count; i++) west.Add(0);

    for (int i = 0; i < g.Count; i++) {
        for (int j = 0; j < g[0].Length; j++) {
            if (g[i][j] == 'O') {
                g[i][j] = '.';
                g[i][west[i]++] = 'O';
            } else if (g[i][j] == '#') {
                west[i] = j+1;
            }
        }
    }

    return g;
}

Grid shiftEast(Grid g) {
    List<int> east = new List<int>();
    for (int i = 0; i < g.Count; i++) east.Add((g[0].Length - 1));

    for (int i = 0; i < g.Count; i++) {
        for (int j = g[0].Length - 1; j >= 0; j--) {
            if (g[i][j] == 'O') {
                g[i][j] = '.';
                g[i][east[i]--] = 'O';
            } else if (g[i][j] == '#') {
                east[i] = j-1;
            }
        }
    }

    return g;
}

Grid shiftSouth(Grid g) {
    List<int> south = new List<int>();
    for (int i = 0; i < g[0].Length; i++) south.Add((g.Count - 1));

    for (int i = g.Count - 1; i >= 0; i--) {
        for (int j = 0; j < g[0].Length; j++) {
            if (g[i][j] == 'O') {
                g[i][j] = '.';
                g[south[j]--][j] = 'O';
            } else if (g[i][j] == '#') {
                south[j] = i-1;
            }
        }
    }

    return g;
}