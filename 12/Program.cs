using System.Text.RegularExpressions;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\12\\input";
StreamReader reader = new StreamReader(input);

string line;
long sum = 0;
int iter = 0;

Dictionary<string, Dictionary<int[], int>> memo = new Dictionary<string, Dictionary<int[], int>>();
while ((line = reader.ReadLine()!) != null) {
    Console.WriteLine(iter++);
    string[] segments = line.Split();
    string record = segments[0].Trim('.');
    // record = string.Join('?', Enumerable.Repeat(segments[0], 5));
    int[] nums = segments[1].Split(',').Select(x => int.Parse(x)).ToArray();
    // nums = Enumerable.Repeat(nums, 5).SelectMany(x => x).ToArray();

    string pattern = @"^[\.?]*";
    for (int i = 0; i < nums.Length - 1; i++) pattern += @"[#?]{" + nums[i].ToString() + @"}[\.?]+";
    pattern += @"[#?]{" + nums.Last().ToString() + @"}[\.?]*$";

    // Console.WriteLine("\n" + pattern + "\n");

    Stack<string> s = new Stack<string>([record]);
    string cur;
    var regex = new Regex(Regex.Escape("?"));

    long poss = 0;
    Dictionary<int[], int> memo2;
    while (s.TryPop(out cur)) {
        if (memo.TryGetValue(cur, out memo2)) {
            int stored;
            if (memo2.TryGetValue(nums, out stored)) {
                sum += poss;
            }
        }
        if (Regex.Match(cur, pattern).Success) {
            if (cur.Contains('?')) {
                s.Push(regex.Replace(cur, "#", 1));
                s.Push(regex.Replace(cur, ".", 1));
            } else {
                Console.WriteLine(cur);
                poss++;
            }
        }
    }
    sum += poss;
}
Console.WriteLine(sum);