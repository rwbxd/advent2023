using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\12\\input";
StreamReader reader = new StreamReader(input);

string line;
long sum = 0;
int iter = 0;

Dictionary<string, Dictionary<string, long>> cache = new Dictionary<string, Dictionary<string, long>>();
DateTime startTime = DateTime.Now;
while ((line = reader.ReadLine()!) != null) {
    Console.WriteLine(iter++.ToString() + " | " + TimeSpan.FromSeconds((DateTime.Now - startTime).TotalSeconds).ToString(@"hh\:mm\:ss\:fff"));
    string[] segments = line.Split();
    string record = segments[0].Trim('.');
    record = string.Join('?', Enumerable.Repeat(segments[0], 5));
    string[] nums = segments[1].Split(',');
    nums = Enumerable.Repeat(nums, 5).SelectMany(x => x).ToArray();
    long poss = findPossibilities(record, nums);
    Console.WriteLine("Possibilities: " + poss);
    sum += poss;
    // string pattern = generateRegexString(nums);

    // // Console.WriteLine("\n" + pattern + "\n");

    // Stack<string> s = new Stack<string>([record]);
    // string cur;
    // var regex = new Regex(Regex.Escape("?"));

    // long poss = 0;
    // Dictionary<string, long> memo2;
    // while (s.TryPop(out cur)) {
    //     if (cache.TryGetValue(cur, out memo2)) {
    //         long stored;
    //         if (memo2.TryGetValue(nums2key(nums), out stored)) {
    //             sum += poss;
    //         }
    //     }
    //     if (Regex.Match(cur, pattern).Success) {
    //         if (cur.Contains('?')) {
    //             s.Push(regex.Replace(cur, "#", 1));
    //             s.Push(regex.Replace(cur, ".", 1));
    //         } else {
    //             Console.WriteLine(cur);
    //             poss++;
    //         }
    //     }
    // }
    // sum += poss;
}

string nums2key(string[] nums) {
    return string.Join(',', nums);
}

string depthTabs(int depth) {
    return string.Join("", Enumerable.Repeat("\t", depth));
}

long findPossibilities(string x, string[] nums, int depth=0, string label="") {
    Console.WriteLine(x);
    // Console.WriteLine(depthTabs(depth) + x + " | [" + string.Join(",", nums) + "]");
    x = x.Trim('.');
    // Console.WriteLine(string.Join(" | ", [x, nums2key(nums), depth.ToString(), label]));
    if (cache.ContainsKey(x) && cache[x].ContainsKey(nums2key(nums))) {
        // Console.WriteLine(depthTabs(depth) + "Returning cached result: " + cache[x][nums2key(nums)].ToString());
        return cache[x][nums2key(nums)];
    }

    // generateRegexString is our "is this string possible" string - if impossible, return 0
    // if (!Regex.Match(x, generateRegexString(nums)).Success) {
    //     // Console.WriteLine(depthTabs(depth + 1) + "Invalid string.");
    //     return 0;
    // }

    // if there's no #'s needed, return the 1 possibility (all '.'s)
    // also, if there's no possible variations (and we know it's valid per above), return the 1 possibility
    if (nums.Length == 0 || !x.Contains('?')) {
        // Console.WriteLine(depthTabs(depth + 1) + "Valid string.");
        // return 1;

        if (!Regex.Match(x, generateRegexString(nums)).Success) {
        // Console.WriteLine(depthTabs(depth + 1) + "Invalid string.");
            return 0;
        }
        return 1;
    }

    if (x[0] == '?') {
        long result = 0;
        result += findPossibilities(x.Remove(0, 1).Insert(0, "."), nums, depth + 1, "?=.") + findPossibilities(x.Remove(0, 1).Insert(0, "#"), nums, depth + 1, "?=#");
        // Console.WriteLine(depthTabs(depth + 1) + "Adding cache[" + x + "] with (" + nums2key(nums) + ", " + result.ToString() + ")");
        // Console.WriteLine(depthTabs(depth + 1) + "Found: " + result.ToString());
        return cacheAndReturn(x, nums, result);
    } else { // x[0] == '#'
        if (!Regex.Match(x, generateRegexString(nums)).Success) {
        // Console.WriteLine(depthTabs(depth + 1) + "Invalid string.");
            return 0;
        }
        if (nums.Length == 1) { return 1; }
        if (!Regex.Match(x, generateRegexString(nums)).Success) {
        // Console.WriteLine(depthTabs(depth + 1) + "Invalid string.");
            return 0;
        }
        long result = findPossibilities(x[(int.Parse(nums[0]) + 1)..], nums[1..], depth + 1, "del #");
        // Console.WriteLine(depthTabs(depth + 1) + "Adding cache[" + x + "] with (" + nums2key(nums) + ", " + result.ToString() + ")");
        // Console.WriteLine(depthTabs(depth + 1) + "Found: " + result.ToString());
        return cacheAndReturn(x, nums, result);
    }
}

long cacheAndReturn(string x, string[] nums, long v) {
    if (!cache.ContainsKey(x)) {cache.Add(x, new Dictionary<string, long>());}
    if (!cache[x].ContainsKey(nums2key(nums))) {cache[x].Add(nums2key(nums), v);}
    return v;
}

string generateRegexString(string[] nums) {
    if (nums.Length == 0) {
        return @"^[\.?]*$";
    }

    StringBuilder pattern = new StringBuilder(@"^[\.?]*");
    for (int i = 0; i < nums.Length - 1; i++) pattern.Append(@"[#?]{" + nums[i] + @"}[\.?]+");
    pattern.Append(@"[#?]{" + nums.Last() + @"}[\.?]*$");
    return pattern.ToString();
}
Console.WriteLine(sum);