string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\19\\input";

Part1();
Part2();

void Part1() {
    StreamReader reader = new StreamReader(input);

    Dictionary<string, Workflow> workflows = new();
    string line;
    while ((line = reader.ReadLine()!) != null && line != "") {
        string[] split = line.Split('{');

        workflows.Add(split[0], new Workflow(split[1].Trim('}')));
    }

    long result = 0;
    while ((line = reader.ReadLine()!) != null && line != "") {
        Part curPart = new Part(line);
        string curWorkflow = "in";
        while (!(curWorkflow is "A" or "R")) {
            curWorkflow = workflows[curWorkflow].Work(curPart);
        }
        if (curWorkflow is "A") {
            result += curPart.Sum;
        }
    }
    Console.WriteLine(result);
}

void Part2() {
    StreamReader reader = new StreamReader(input);

    Dictionary<string, Workflow> workflows = new();
    string line;
    while ((line = reader.ReadLine()!) != null && line != "") {
        string[] split = line.Split('{');

        workflows.Add(split[0], new Workflow(split[1].Trim('}')));
    }

    long result = 0;
    Queue<RangeWorkflowPair> queue = new Queue<RangeWorkflowPair>();
    queue.Enqueue(new RangeWorkflowPair(new Range(), "in"));
    while (queue.Count != 0) {
        RangeWorkflowPair rwp = queue.Dequeue();
        Console.WriteLine(rwp.workflow + " " + rwp.r.ToString());
        Range range = rwp.r;
        if (rwp.workflow == "R") {continue;}
        if (rwp.workflow == "A") {
            result += range.GetPossibilities(); 
            Console.WriteLine(range.GetPossibilities());
            continue;
        }
        Workflow w = workflows[rwp.workflow];

        Range newRange;
        foreach (Rule rule in w.rules) {
            range.Split(rule, out newRange);
            queue.Enqueue(new RangeWorkflowPair(newRange, rule.Key));
        }
        queue.Enqueue(new RangeWorkflowPair(range, w.fallback));
    }
    
    Console.WriteLine(result);

}

class Workflow {
    public List<Rule> rules = [];
    public string fallback;
    public Workflow(string x) {
        string[] ruleStrings = x.Split(',');
        foreach (string s in ruleStrings[..^1]) {
            string[] split = s.Split(':');
            if (split[0].Contains('>')) {
                int rating = int.Parse(split[0].Split('>')[1]);
                rules.Add(new Rule(s[0], true, rating, split[1]));
            } else {
                int rating = int.Parse(split[0].Split('<')[1]);
                rules.Add(new Rule(s[0], false, rating, split[1]));
            }
        }
        fallback = ruleStrings[^1];
    }

    public string Work(Part p) {
        string nextWorkflow;
        foreach (Rule rule in rules) {
            if (rule.test(p, out nextWorkflow)) {
                return nextWorkflow;
            }
        }
        return fallback;
    }
}

class Rule {
    public char Letter;
    public int Rating;
    public string Key;
    public bool GT;
    public Rule(char letter, bool greaterThan, int rating, string key) {
        Letter = letter; GT = greaterThan; Rating = rating; Key = key;
    }

    public bool test(Part p, out string nextWorkflow) {
        if ((GT && p.ratings[Letter] > Rating) || (!GT && p.ratings[Letter] < Rating)) {
            nextWorkflow = Key;
            return true;
        } else {
            nextWorkflow = "";
            return false;
        }
    }
}

class Part {
    public readonly Dictionary<char, int> ratings = [];
    public int Sum;
    public Part(string x) {
        x = x.Trim('{', '}');
        foreach (var y in x.Split(',')) {
            string[] z = y.Split('=');
            ratings[z[0][0]] = int.Parse(z[1]);
        }
        foreach (int r in ratings.Values) {Sum += r;}
    }
    public Part(int x, int m, int a, int s) {
        ratings['x'] = x;
        ratings['m'] = m;
        ratings['a'] = a;
        ratings['s'] = s;
        Sum = x + m + a + s;
    }
}

class Range {
    public long xMin;
    public long xMax;
    public long mMin;
    public long mMax;
    public long aMin;
    public long aMax;
    public long sMin;
    public long sMax;

    public Range() {
        xMin = 1; mMin = 1; aMin = 1; sMin = 1;
        xMax = 4000; mMax = 4000; aMax = 4000; sMax = 4000;
    }

    public Range(Range r) {
        this.xMin = r.xMin; this.xMax = r.xMax;
        this.mMin = r.mMin; this.mMax = r.mMax;
        this.aMin = r.aMin; this.aMax = r.aMax;
        this.sMin = r.sMin; this.sMax = r.sMax;
    }

    public override string ToString()
    {
        return String.Format("x: {0}-{1}, m: {2}-{3}, a: {4}-{5}, s: {6}-{7}", xMin, xMax, mMin, mMax, aMin, aMax, sMin, sMax);
    }

    public void Split(Rule rule, out Range newRange) {
        Split(rule.Letter, rule.GT, rule.Rating, out newRange);
    }

    public void Split(char letter, bool gt, int num, out Range newRange) {
        newRange = new Range(this);
        Range lowRange;
        Range highRange;
        int highOffset = 0;
        int lowOffset = 0;

        // New range gets accepted by rule
        if (gt) {
            lowRange = this;
            highRange = newRange;
            highOffset = 1;
        } else {
            lowRange = newRange;
            highRange = this;
            lowOffset = 1;
        }

        switch (letter) {
            case 'x':
                highRange.xMin = num + highOffset;
                lowRange.xMax = num - lowOffset;
                break;
            case 'm':
                highRange.mMin = num + highOffset;
                lowRange.mMax = num - lowOffset;
                break;
            case 'a':
                highRange.aMin = num + highOffset;
                lowRange.aMax = num - lowOffset;
                break;
            case 's':
                highRange.sMin = num + highOffset;
                lowRange.sMax = num - lowOffset;
                break;
        }
    }

    public long GetPossibilities() {
        return (xMax - xMin + 1) * (mMax - mMin + 1) * (aMax - aMin + 1) * (sMax - sMin + 1);
    }
}

class RangeWorkflowPair {
    public Range r;
    public string workflow;
    public RangeWorkflowPair(Range r, string workflow) {
        this.r = r; this.workflow = workflow;
    }
}