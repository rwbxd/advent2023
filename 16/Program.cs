string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\16\\input";
StreamReader reader = new StreamReader(input);

List<List<char>> grid = new List<List<char>>();
string line;
while ((line = reader.ReadLine()!) != null) {
    grid.Add(line.ToList());
}

long calc(int startI, int startJ, char startD) {
    List<List<char>> energy = new List<List<char>>(grid.Count);
    List<List<HashSet<char>>> seen = new List<List<HashSet<char>>>();
    for (int i = 0; i < grid.Count; i++) {
        List<char> row = new List<char>(grid[0].Count);
        List<HashSet<char>> hsRow = new List<HashSet<char>>();
        for (int j = 0; j < grid[0].Count; j++) {
            row.Add('.');
            hsRow.Add(new HashSet<char>());
        }
        energy.Add(row);
        seen.Add(hsRow);
    }

    Queue<Source> q = new Queue<Source>();
    q.Enqueue(new Source(startI, startJ, startD));


    while (q.Count != 0) {
        Source s = q.Dequeue();
        if (s.I < 0 || s.J < 0 || s.I >= grid.Count || s.J >= grid[0].Count) {
            continue;
        }

        // Console.WriteLine("Hit " + grid[s.I][s.J] + " going " + s.D);

        if (seen[s.I][s.J].Contains(s.D)) { continue; }
        else { seen[s.I][s.J].Add(s.D); }

        energy[s.I][s.J] = '#';

        // foreach (var r in energy) {
        //     foreach (var c in r) {
        //         Console.Write(c);
        //     }
        //     Console.WriteLine();
        // }
        // Console.WriteLine();

        char cur = grid[s.I][s.J];
        switch (cur) {
            case '.':
                switch (s.D) {
                    case 'R':
                        q.Enqueue(new Source(s.I, s.J+1, s.D));
                        break;
                    case 'L':
                        q.Enqueue(new Source(s.I, s.J-1, s.D));
                        break;
                    case 'U':
                        q.Enqueue(new Source(s.I-1, s.J, s.D));
                        break;
                    case 'D':
                        q.Enqueue(new Source(s.I+1, s.J, s.D));
                        break;
                }
                break;
            case '/':
                switch (s.D) {
                    case 'R':
                        q.Enqueue(new Source(s.I-1, s.J, 'U'));
                        break;
                    case 'L':
                        q.Enqueue(new Source(s.I+1, s.J, 'D'));
                        break;
                    case 'D':
                        q.Enqueue(new Source(s.I, s.J-1, 'L'));
                        break;
                    case 'U':
                        q.Enqueue(new Source(s.I, s.J+1, 'R'));
                        break;
                }
                break;
            case '\\':
                switch (s.D) {
                    case 'L':
                        q.Enqueue(new Source(s.I-1, s.J, 'U'));
                        break;
                    case 'R':
                        q.Enqueue(new Source(s.I+1, s.J, 'D'));
                        break;
                    case 'U':
                        q.Enqueue(new Source(s.I, s.J-1, 'L'));
                        break;
                    case 'D':
                        q.Enqueue(new Source(s.I, s.J+1, 'R'));
                        break;
                }
                break;
            case '|':
                switch (s.D) {
                    case 'L':
                    case 'R':
                        q.Enqueue(new Source(s.I-1, s.J, 'U'));
                        q.Enqueue(new Source(s.I+1, s.J, 'D'));
                        break;
                    case 'U':
                        q.Enqueue(new Source(s.I-1, s.J, s.D));
                        break;
                    case 'D':
                        q.Enqueue(new Source(s.I+1, s.J, s.D));
                        break;
                }
                break;
            case '-':
                switch (s.D) {
                    case 'U':
                    case 'D':
                        q.Enqueue(new Source(s.I, s.J-1, 'L'));
                        q.Enqueue(new Source(s.I, s.J+1, 'R'));
                        break;
                    case 'L':
                        q.Enqueue(new Source(s.I, s.J-1, s.D));
                        break;
                    case 'R':
                        q.Enqueue(new Source(s.I, s.J+1, s.D));
                        break;
                }
                break;
        }

        
    }

    // foreach (var r in energy) {
    //     foreach (var c in r) {
    //         Console.Write(c);
    //     }
    //     Console.WriteLine();
    // }
    return energy.Sum(x => x.Count(y => y == '#'));
}

long max = 0;

for (int i = 0; i < grid.Count; i++) {
    max = Math.Max(max, calc(i, 0, 'R'));
    max = Math.Max(max, calc(i, grid[0].Count - 1, 'L'));
}
for (int i = 0; i < grid[0].Count; i++) {
    max = Math.Max(max, calc(0, i, 'D'));
    max = Math.Max(max, calc(grid.Count - 1, i, 'U'));
}

Console.WriteLine(calc(0, 0, 'R'));
Console.WriteLine(max);

class Source {
    public int I;
    public int J;
    public char D; // R,L,U,D
    public Source(int i, int j, char d) {
        I = i; J = j; D = d;
    }
}
