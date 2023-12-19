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
    for (int x = 1; x <= 4000; x++) {
        Console.WriteLine("x = " + x.ToString());
        for (int m = 1; m <= 4000; m++) { 
            Console.WriteLine("m = " + m.ToString());
            for (int a = 1; a <= 4000; a++) {
                for (int s = 1; s <= 4000; s++) {
                    Part curPart = new Part(x,m,a,s);
                    string curWorkflow = "in";
                    while (!(curWorkflow is "A" or "R")) {
                        curWorkflow = workflows[curWorkflow].Work(curPart);
                    }
                    if (curWorkflow is "A") {
                        result += curPart.Sum;
                    }
                }
            }
        }
    }
    
    Console.WriteLine(result);

}

class Workflow {
    List<Rule> rules = [];
    string fallback;
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
    bool GT;
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