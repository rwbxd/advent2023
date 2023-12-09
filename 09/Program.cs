string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\09\\input";
StreamReader reader = new StreamReader(input);

string line;
long p1 = 0;
long p2 = 0;
while ((line = reader.ReadLine()!) != null) {
    List<int> nums = new List<int>();
    foreach (string s in line.Split()) {
        nums.Add(int.Parse(s));
    }
    p1 += nextIntSequence(nums);
    p2 += prevIntSequence(nums);
}

int nextIntSequence(List<int> seq) {
    List<int> diffs = new List<int>();
    for (int i = 0; i < seq.Count - 1; i++) {
        diffs.Add(seq[i+1] - seq[i]);
    }

    if (diffs.All(x => x == 0)) {
        return seq.Last() + diffs[0];
    } else {
        return seq.Last() + nextIntSequence(diffs);
    }
}

int prevIntSequence(List<int> seq) {
    List<int> diffs = new List<int>();
    for (int i = 0; i < seq.Count - 1; i++) {
        diffs.Add(seq[i+1] - seq[i]);
    }

    if (diffs.All(x => x == 0)) {
        return seq[0] - diffs[0];
    } else {
        return seq[0] - prevIntSequence(diffs);
    }
}

Console.WriteLine("Part 1: " + p1);
Console.WriteLine("Part 2: " + p2);