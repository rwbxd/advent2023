using System.Text.RegularExpressions;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\12\\input";
StreamReader reader = new StreamReader(input);

string line;
long sum = 0;
int iter = 0;

Dictionary<string, Dictionary<string, long>> cache = new Dictionary<string, Dictionary<string, long>>();
while ((line = reader.ReadLine()!) != null) {
    Console.WriteLine(iter++);
    string[] segments = line.Split();
    string record = segments[0].Trim('.');
    // record = string.Join('?', Enumerable.Repeat(segments[0], 5));
    string[] nums = segments[1].Split(',');
    // nums = Enumerable.Repeat(nums, 5);
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

long findPossibilities(string x, string[] nums, int depth=0, string label="") {
    x = x.Trim('.');
    Console.WriteLine(string.Join(" | ", [x, nums2key(nums), depth.ToString(), label]));
    if (cache.ContainsKey(x) && cache[x].ContainsKey(nums2key(nums))) {
        return cache[x][nums2key(nums)];
    }
    // generateRegexString is our "is this string possible" string - if impossible, return 0
    if (!Regex.Match(x, generateRegexString(nums)).Success) { return 0; }
    // if there's no #'s needed, return the 1 possibility (all '.'s)
    if (nums.Length == 0) {return 1;}
    if (!x.Contains('?')) {
        return Regex.Match(x.Trim('.'), generateRegexString(nums)).Success ? 1 : 0;
    } else {
        if (x[0] == '?') {
            long result = 0;
            result += findPossibilities(x[1..], nums, depth + 1, "?=.") + findPossibilities(x.Remove(0, 1).Insert(0, "#"), nums, depth + 1, "?=#");
            if (!cache.ContainsKey(x)) {
                cache.Add(x, new Dictionary<string, long>());    
            }
            Console.WriteLine("Adding cache[" + x + "] with (" + nums2key(nums) + ", " + result.ToString() + ")");
            if (!cache[x].ContainsKey(nums2key(nums))) {cache[x].Add(nums2key(nums), result);}
            return result;
        } else {
            nums[0] = (int.Parse(nums[0]) - 1).ToString();
            if (nums[0] == "0") {
                nums = nums[1..];
                if (x[1] == '#') {return 0;}
                return findPossibilities(x[2..], nums, depth + 1, "del # and nums[0]");
            }
            long result = findPossibilities(x[1..], nums, depth + 1, "del #");
            return result;
        }
    }
}

string generateRegexString(string[] nums) {
    if (nums.Length == 0) {
        return @"^[\.?]*$";
    }

    string pattern = @"^[\.?]*";
    for (int i = 0; i < nums.Length - 1; i++) pattern += @"[#?]{" + nums[i] + @"}[\.?]+";
    pattern += @"[#?]{" + nums.Last() + @"}[\.?]*$";
    return pattern;
}
Console.WriteLine(sum);