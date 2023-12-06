// See https://aka.ms/new-console-template for more information
int[] time = [60, 94, 78, 82];
int[] dist = [475, 2138, 1015, 1650];
// int[] time = [7, 15, 30];
// int[] dist = [9, 40, 200];

long p1result = 1;
for (int i = 0; i < time.Length; i++) {
    int curTime = time[i];
    int curDist = dist[i];
    long curRes = FindWinner(curTime, curDist);
    p1result *= CalcPossibilities(curRes, curTime);
}
Console.WriteLine(p1result);

string timestr = "";
string diststr = "";

for (int i = 0; i < time.Length; i++) {
    timestr += time[i].ToString();
    diststr += dist[i].ToString();
}

long p2time = long.Parse(timestr);
long p2dist = long.Parse(diststr);
long p2result = FindWinner(p2time, p2dist);
Console.WriteLine(CalcPossibilities(p2result, p2time));

static long FindWinner(long time, long dist) {
    for (long i = 1; i < time; i++) {
        if (i * (time - i) > dist) {
            return i;
        }
    }
    return 0;
}

static long CalcPossibilities(long res, long time) { return time - res - res + 1; }