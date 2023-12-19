string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\18\\input";

Part1();
Part2();
void Part1() {
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
}

void Part2() {
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
        // dirs.Add(split[0][0]);
        // lens.Add(int.Parse(split[1]));
        string color = split[2].Trim('(', '#', ')');
        dirs.Add(dirDict[color[^1] - '0']);
        lens.Add(Convert.ToInt32(color[..5], 16));
    }

    Console.WriteLine(string.Join(",", lens));

    long area = 0;
    int curWidth = 1;
    int curLength = 1;
    int R = 0; int L = 0; int U = 0; int D = 0;
    Dictionary<int, HashSet<int>> hashmarks = new Dictionary<int, HashSet<int>>{
        { 0, new HashSet<int>(0) }
    };
    Dictionary<int, HashSet<int>> hashcorners = new Dictionary<int, HashSet<int>>{
        { 0, new HashSet<int>(0) }
    };
    
    for (int i = 0; i < dirs.Count; i++) {
        area += lens[i];
        switch (dirs[i]) {
            case 'R':
                // for (int x = 0; x < lens[i]; x++) {hashmarks.Add(string.Format("({0},{1})", curLength, curWidth+x+1));}
                if (!hashmarks.ContainsKey(curLength)) {hashmarks[curLength] = new();}
                if (!hashcorners.ContainsKey(curLength)) {hashcorners[curLength] = new();}
                // hashmarks[curLength].Add(curWidth + 1);
                // hashcorners[curLength].Add(curWidth + 1);
                hashmarks[curLength].Add(curWidth + lens[i]);
                // hashcorners[curLength].Add(curWidth + lens[i]);
                curWidth += lens[i];
                R = Math.Max(R, curWidth);
                break;
            case 'L':
                // for (int x = 0; x < lens[i]; x++) {hashmarks.Add(string.Format("({0},{1})", curLength, curWidth-x-1));}
                if (!hashmarks.ContainsKey(curLength)) {hashmarks[curLength] = new();}
                if (!hashcorners.ContainsKey(curLength)) {hashcorners[curLength] = new();}
                // hashmarks[curLength].Add(curWidth - 1);
                // hashcorners[curLength].Add(curWidth - 1);
                hashmarks[curLength].Add(curWidth - lens[i]);
                // hashcorners[curLength].Add(curWidth - lens[i]);
                curWidth -= lens[i];
                L = Math.Max(L, curWidth * -1);
                break;
            case 'U':
                // for (int x = 0; x < lens[i]; x++) {hashmarks.Add(string.Format("({0},{1})", curLength-x-1, curWidth));}
                if (!hashcorners.ContainsKey(curLength)) {hashcorners[curLength] = new();}
                hashcorners[curLength].Add(curWidth);

                for (int x = 1; x <= lens[i]; x++) {
                    if (!hashmarks.ContainsKey(curLength - x)) {
                        hashmarks[curLength - x] = new();
                    }
                    hashmarks[curLength - x].Add(curWidth);
                }

                if (!hashcorners.ContainsKey(curLength - lens[i])) {hashcorners[curLength - lens[i]] = new();}
                hashcorners[curLength - lens[i]].Add(curWidth);

                curLength -= lens[i];
                U = Math.Max(U, -1 * curLength);
                break;
            case 'D':
                // for (int x = 0; x < lens[i]; x++) {hashmarks.Add(string.Format("({0},{1})", curLength+x+1, curWidth));}
                if (!hashcorners.ContainsKey(curLength)) {hashcorners[curLength] = new();}
                hashcorners[curLength].Add(curWidth);

                for (int x = 1; x <= lens[i]; x++) {
                    if (!hashmarks.ContainsKey(curLength + x)) {
                        hashmarks[curLength + x] = new();
                    }
                    hashmarks[curLength + x].Add(curWidth);
                }

                if (!hashcorners.ContainsKey(curLength + lens[i])) {hashcorners[curLength + lens[i]] = new();}
                hashcorners[curLength + lens[i]].Add(curWidth);

                curLength += lens[i];
                D = Math.Max(D, curLength);
                break;
        }
        length = Math.Max(length, Math.Abs(curLength));
        width = Math.Max(width, Math.Abs(curWidth));
    }
    length = U + D + 1;
    width = R + L + 1;
    int curI = U+1; int curJ = L+1;

    foreach (var i in hashcorners.Keys) {
        foreach (var j in hashcorners[i]) {
            Console.WriteLine(string.Format("{0}, {1}", i, j));
        }
    }

    bool inside;
    List<int> ikeys = hashmarks.Keys.ToList();
    ikeys.Sort();
    Console.WriteLine(area);
    foreach (int i in ikeys) {
        if (i % 1_000_000 == 0) Console.WriteLine("Row " + i.ToString() + "/" + length.ToString());
        inside = false;
        if (!hashmarks.ContainsKey(i)) continue;

        List<int> hsl = hashmarks[i].ToList();
        hsl.Sort();
        int prev = 0;
        bool ignore = false;
        foreach (var j in hsl) { // j = j value of hashmark
            // Console.WriteLine(string.Format("{0} - in: {1}, ig: {2}, prev: {3}", j, inside, ignore, prev));
            if (inside && !ignore) {
                area += j - prev - 1;
            }
            if (isCorner(i,j)) {
                ignore = !ignore;

                if (hasNorth(i,j)) {inside = !inside;}
            } else {
                inside = !inside;
            }
            // Console.WriteLine(string.Format("{4} | {0},{1} - {2} ({3})", i, j, ignore, j - prev - 1, area));
            prev = j;
        }
        // Console.WriteLine(area);
    }

    Console.WriteLine(area);

    bool isCorner(int i, int j) {
        return hashcorners.ContainsKey(i) && hashcorners[i].Contains(j);
    }

    bool hasNorth(int i, int j) {
        return hashmarks.ContainsKey(i-1) && hashmarks[i-1].Contains(j);
    }
}