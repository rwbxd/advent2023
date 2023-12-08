// See https://aka.ms/new-console-template for more information

Dictionary<string, string> left = new Dictionary<string, string>();
Dictionary<string, string> right = new Dictionary<string, string>();
HashSet<string> p2as = new HashSet<string>();

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\08\\input";
StreamReader reader = new StreamReader(input);

string moves = reader.ReadLine()!;
reader.ReadLine();

string line;
while ((line = reader.ReadLine()!) != null) {
    string parent = line.Substring(0, 3);
    string l = line.Substring(7, 3);
    string r = line.Substring(12, 3);
    left[parent] = l;
    right[parent] = r;
    if (parent[2] == 'A') p2as.Add(parent);
}

long steps = 0;

long Calculate(string start, bool lastOnly = false) {
    long steps = 0;
    string curstate = start;
    while (!(curstate == "ZZZ" || (lastOnly && curstate[2] == 'Z'))) {
        foreach (char move in moves) {
            steps++;
            if (move == 'R') {
                curstate = right[curstate];
            } else {
                curstate = left[curstate];
            }
            if (curstate == "ZZZ" || (lastOnly && curstate[2] == 'Z')) {
                break;
            }
        }
    }
    return steps;
}

Console.WriteLine("Part 1: " + Calculate("AAA").ToString());

long[] calcs = new long[p2as.Count];
string[] starts = [.. p2as];
for (int i = 0; i < starts.Length; i++) {
    string s = starts[i];
    Console.WriteLine(s + ": " + Calculate(s, true).ToString());
    calcs[i] = Calculate(s, true);
}
Console.WriteLine("Part 2: " + LCM(calcs).ToString());

//https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490
static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}