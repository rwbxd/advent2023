string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\14\\input";
StreamReader reader = new StreamReader(input);
string line;

List<string> grid = new List<string>();
while ((line = reader.ReadLine()!) != null) {
    grid.Add(line);
}

List<int> north = new List<int>();
for (int i = 0; i < grid[0].Length; i++) north.Add(grid.Count);

long p1 = 0;

for (int i = 0; i < grid.Count; i++) {
    string row = grid[i];
    for (int j = 0; j < grid[0].Length; j++) {
        if (row[j] == 'O') {
            p1 += north[j];
            north[j]--;
        } else if (row[j] == '#') {
            north[j] = grid.Count - i - 1;
        }
    }
}
Console.WriteLine(p1);