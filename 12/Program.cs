using System.Text;

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

bool isValidString(string x, string[] nums) {
    // Console.WriteLine(string.Join(',', x.Split('.', StringSplitOptions.RemoveEmptyEntries)));
    if (x.Split(".", StringSplitOptions.RemoveEmptyEntries).Length != nums.Length) { return false; }
    int i = 0;
    // Console.WriteLine("BBB");
    foreach (string s in x.Split('.', StringSplitOptions.RemoveEmptyEntries)) {
        // Console.WriteLine(i.ToString() + " " + s + " " + nums[i]);
        if (s.Length != int.Parse(nums[i])) { return false; }
        i++;
    }
    return true;
}

long findPossibilities(string x, string[] nums, int depth=0, string label="") {
    // Console.WriteLine(string.Join("", Enumerable.Repeat('\t', depth)) + x + " -> " + nums2key(nums));
    x = x.Trim('.');
    if (cache.ContainsKey(x) && cache[x].ContainsKey(nums2key(nums))) return cache[x][nums2key(nums)];

    if (x == "") {
        if (nums.Length == 0) {
            return 1;
        }
        return 0;
    }

    // if there's no #'s needed, return the 1 possibility (all '.'s)
    if (nums.Length == 0) {
        if (x.Contains('#')) { return cacheAndReturn(x, nums, 0); }
        else { return cacheAndReturn(x, nums, 1); }
    }

    // If length < sum(nums), there's no possible combination
    if (x.Length < nums.Select(int.Parse).Sum() + nums.Length - 1) { return cacheAndReturn(x, nums, 0); }

    // If there's no '?', then the string should match
    if (!x.Contains('?')) {
        // Console.WriteLine("AHH");
        isValidString(x, nums);
        if (isValidString(x, nums)) { return cacheAndReturn(x, nums, 1); }
        else { return cacheAndReturn(x, nums, 0); }
    }

    int firstNum = int.Parse(nums[0]);
    if (x[0] == '?') {
        long result = 0;
        
        // Case where ? becomes '.'
        result += findPossibilities(x[1..], nums, depth + 1, "?=.");

        if (x.Length < firstNum) {return cacheAndReturn(x, nums, result);}
        // Case where ? becomes '#'
        bool b = true;
        for (int i = 1; i < firstNum; i++) {
            if (x[i] == '.') { b = false; } // Invalid for there to be a .
        }
        if (b) {
            if (x.Length == firstNum) {
                result += findPossibilities(x[(firstNum)..], nums[1..], depth + 1, "?=#");
            } else {
                if (x[(firstNum)] == '#') {return cacheAndReturn(x, nums, result);}
                // Console.WriteLine("tesT");
                result += findPossibilities(x[(firstNum+1)..], nums[1..], depth + 1, "?=#");
            }
        }
        return cacheAndReturn(x, nums, result);
    } else { // x[0] == '#'
        if (nums.Length == 1) {
            for (int i = 1; i < firstNum; i++) {
                if (x[i] == '.') { return cacheAndReturn(x, nums, 0); } // Invalid for ? to equal #
            }
            for (int i = firstNum; i < x.Length; i++) {
                if (x[i] == '#') { return cacheAndReturn(x, nums, 0); } // Invalid for there to be a # after the intial sequence
            }
            return cacheAndReturn(x, nums, 1);
        }

        if (x.Length < firstNum+1) {return cacheAndReturn(x, nums, 0);}
        for (int i = 1; i < firstNum; i++) {
            if (x[i] == '.') { return cacheAndReturn(x, nums, 0); } // Invalid for ? to equal #
        }

        if (x[(firstNum)] == '#') {return cacheAndReturn(x, nums, 0);}
        return cacheAndReturn(x, nums, findPossibilities(x[(firstNum+1)..], nums[1..], depth + 1, "del #"));
    }
}

long cacheAndReturn(string x, string[] nums, long v) {
    if (!cache.ContainsKey(x)) {cache.Add(x, new Dictionary<string, long>());}
    if (!cache[x].ContainsKey(nums2key(nums))) {cache[x].Add(nums2key(nums), v);}
    // if (v != 0) Console.WriteLine("Caching " + x + " " + string.Join(",", nums) + ": " + v.ToString());
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