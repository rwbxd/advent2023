using System.Runtime.InteropServices.JavaScript;

string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\15\\input";
StreamReader reader = new StreamReader(input);

string inputLine = "";
string line;
while ((line = reader.ReadLine()!) != null) {
    inputLine += line;
}
List<string> seq = inputLine.Split(",").ToList();

int p1 = 0;
foreach (string s in seq) {
    p1 += HASH(s);
}
Console.WriteLine(p1);

Dictionary<int, List<string>> hm = new Dictionary<int, List<string>>();
for (int i = 0; i < 256; i++) {hm.Add(i, new List<string>());}
foreach (string s in seq) {
    string label = s.Split('=')[0].Split('-')[0];
    int hashcode = HASH(label);
    if (s.Last() == '-') {
        for (int i = 0; i < hm[hashcode].Count; i++) {
            if (hm[hashcode][i].StartsWith(label)) {
                hm[hashcode].RemoveAt(i);
                break;
            } 
        }
    } else {
        string focal = s.Split('=')[1];
        bool found = false;
        for (int i = 0; i < hm[hashcode].Count; i++) {
            if (hm[hashcode][i].StartsWith(label)) {
                found = true;
                hm[hashcode][i] = label + " " + focal;
                break;
            } 
        }
        if (!found) {
            hm[hashcode].Add(label + " " + focal);
        }
    }
}
long p2 = 0;
for (int i = 0; i < 256; i++) {
    List<string> l = hm[i];
    
    for (int j = 0; j < l.Count; j++) {
        Console.WriteLine(l[j]);
        p2 += ((i + 1) * (j + 1) * (int.Parse(l[j].Split()[1])));
    }
}
Console.WriteLine(p2);

int HASH(string s, int result = 0) {
    foreach (char c in s) {
        int code = c;
        result += code;
        result *= 17;
        result %= 256;
    }
    return result;
}
// Console.WriteLine(HASH("HASH"));