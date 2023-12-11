string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\11\\input";
StreamReader reader = new StreamReader(input);

string line;
List<List<char>> galaxy = new List<List<char>>();
while ((line = reader.ReadLine()!) != null) {
    galaxy.Add(new List<char>(line.ToCharArray()));
}

// for (int i = 0; i < galaxy.Count; i++) {
//     for (int j = 0; j < galaxy[0].Count; j++) {
//         Console.Write(galaxy[i][j]);
//     }
//     Console.WriteLine();
// }

List<int> rows = new List<int>();
for (int i = 0; i < galaxy.Count; i++) {
    if (galaxy[i].All(x => x == '.')) {
        rows.Add(i);
    }
}

int nFound = 1;
List<int> cols = new List<int>();
for (int i = 0; i < galaxy[0].Count; i++) {
    bool has = false;
    foreach (List<char> g in galaxy) {
        if (g[i] != '.') {
            has = true;
            g[i] = nFound++.ToString()[0];
        }
    }
    if (!has) cols.Add(i);
}

cols.Reverse();
// foreach (int c in cols) {
//     for (int i = 0; i < galaxy.Count; i++) {
//         galaxy[i].Insert(c, '.');
//     }
// }

List<char> emptyRow = new List<char>(galaxy[0].Count);
for (int i = 0; i < galaxy[0].Count; i++) emptyRow.Add('.');
rows.Reverse();
// foreach (int r in rows) {
//     galaxy.Insert(r, emptyRow);
// }

List<Pair> pairs = new List<Pair>();
for (int i = 0; i < galaxy.Count; i++) {
    for (int j = 0; j < galaxy[0].Count; j++) {
        // Console.Write(galaxy[i][j]);
        if (galaxy[i][j] != '.') {
            pairs.Add(new Pair(galaxy[i][j], i, j));
        }
    }
    // Console.WriteLine();
}

// for (int i = 0; i < galaxy.Count; i++) {
//     for (int j = 0; j < galaxy[0].Count; j++) {
//         Console.Write(galaxy[i][j]);
//     }
//     Console.WriteLine();
// }

Console.WriteLine(string.Join(",", cols));
Console.WriteLine(string.Join(",", rows));
long CalculateExpandedDistance(List<Pair> pairs, int expansionFactor, List<int> rows, List<int> cols) {
    long sum = 0;
    for (int i = 0; i < pairs.Count; i++) {
        for (int j = i+1; j < pairs.Count; j++) {
            Pair p = pairs[i];
            Pair q = pairs[j];

            int expandedX = cols.Count(item => item < Math.Max(p.C, q.C) && item > Math.Min(p.C, q.C));
            int expandedY = rows.Count(item => item < Math.Max(p.R, q.R) && item > Math.Min(p.R, q.R));
            // Console.WriteLine(string.Format("{0} ({4}, {5}) {1} ({6}, {7}) X: {2}, Y: {3}", p.ID, q.ID, expandedX, expandedY, p.C, p.R, q.C, q.R));

            sum += Math.Abs(p.C - q.C) + expandedX * (expansionFactor - 1) + Math.Abs(p.R - q.R) + expandedY * (expansionFactor - 1);
        }
    }
    return sum;
}

Console.WriteLine(CalculateExpandedDistance(pairs, 2, rows, cols));
Console.WriteLine(CalculateExpandedDistance(pairs, 10, rows, cols));
Console.WriteLine(CalculateExpandedDistance(pairs, 1_000_000, rows, cols));

class Pair {
    public int R;
    public int C;
    public char ID;
    public Pair(char id, int r, int c) {
        ID = id; R = r; C = c;
    }
}