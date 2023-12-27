int day = 25;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";
Part1();
Part2();

void Part1() {
    Dictionary<string, HashSet<string>> connections = new Dictionary<string, HashSet<string>>();
    StreamReader reader = new StreamReader(input);
    string line;
    while ((line = reader.ReadLine()!) != null) {
        string[] split = line.Split(':');
        string source = split[0];
        string[] dests = split[1].Split("", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (!connections.ContainsKey(source)) {connections[source] = [];}
        foreach (var d in dests) {
            if (!connections.ContainsKey(d)) {connections[d] = [];}
            connections[source].Add(d);
            connections[d].Add(source);
        }
    }
}

void Part2() {
    StreamReader reader = new StreamReader(input);
}