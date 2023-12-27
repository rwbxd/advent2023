using System.Runtime.CompilerServices;
using System.Security.Cryptography;

int day = 24;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";
Part1();
Part2();

void Part1() {
    StreamReader reader = new StreamReader(input);
    List<Hail> h = new List<Hail>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        h.Add(new Hail(line));
    }

    foreach (Hail piece in h) {
        Console.WriteLine(piece);
    }

    int count = 0;
    long start = 200000000000000;
    long end = 400000000000000;
    // long start = 7;
    // long end = 27;
    for (int h1i = 0; h1i < h.Count; h1i++) {
        var h1 = h[h1i];
        for (int h2i = h1i+1; h2i < h.Count; h2i++) {
            var h2 = h[h2i];

            if (h1.slope == h2.slope) {continue;}

            double intersectX = (h2.b - h1.b) / (h1.slope - h2.slope);
            double intersectY = h1.FindY(intersectX);
            // double h1time = (intersectY - h1.py) / h1.vy;
            // Console.WriteLine(string.Format("{0} + {1} * {2} = {3}", h1.py, (intersectY - h1.py), h1.vy, intersectY));
            // Console.WriteLine(string.Format("{0} + {1} * {2} = {3}", h2.py, (intersectY - h2.py), h2.vy, intersectY));
            // double h2time = (intersectY - h2.py) / h2.vy;
            if (start <= intersectX && intersectX <= end) {
                if (start <= intersectY && intersectY <= end) {
                    if (!pastPoint(intersectX, h1.px, h1.vx) && !pastPoint(intersectX, h2.px, h2.vx)) {
                        Console.WriteLine(string.Format("{0} and {1} cross at ({2}, {3})", h1, h2, intersectX, intersectY));
                        count++;
                    }
                    // Console.WriteLine(h1time + " | " + h2time);
                    // if (h1time == h2time) {
                    //     Console.WriteLine(string.Format("{0} and {1} cross at ({2}, {3})", h1, h2, intersectX, intersectY));
                    //     count++;
                    // }       
                }
            }
        }
    }
    Console.WriteLine("Part 1: " + count);
}

void Part2() {
    StreamReader reader = new StreamReader(input);
    List<Hail> h = new List<Hail>();
    string line;
    while ((line = reader.ReadLine()!) != null) {
        h.Add(new Hail(line));
    }

    int secondTimestep = 2;
    while (true) {
        Console.WriteLine("secondTimestep = " + secondTimestep);
        for (int firstTimestep = 1; firstTimestep < secondTimestep; firstTimestep++) {
            // Console.WriteLine("firstTimestep = " + firstTimestep);
            for (int h1i = 0; h1i < h.Count; h1i++) {
                var h1 = h[h1i].PositionAtTime(firstTimestep); // Find the first hail
                // Console.WriteLine("Testing " + h[h1i] + " with collision time " + firstTimestep);
                // Console.WriteLine("\t1:" + h1i);
                for (int h2i = 0; h2i < h.Count; h2i++) {
                    if (h1i == h2i) {continue;}
                    // Console.WriteLine("\tTesting " + h[h2i] + " with collision time " + secondTimestep);
                    var h2 = h[h2i].PositionAtTime(secondTimestep); // Find the second hail
                    // Console.WriteLine("\t2:" + h2i);
                    HashSet<int> seenTimesteps = [firstTimestep, secondTimestep];
                    Hail expecting = new Hail(h1.px - (h2.px - h1.px) / (secondTimestep - firstTimestep),
                                            h1.py - (h2.py - h1.py) / (secondTimestep - firstTimestep),
                                            h1.pz - (h2.pz - h1.pz) / (secondTimestep - firstTimestep),
                                            (h2.px - h1.px) / (secondTimestep - firstTimestep), (h2.py - h1.py) / (secondTimestep - firstTimestep), (h2.pz - h1.pz) / (secondTimestep - firstTimestep));
                    // Console.WriteLine("\t" + expecting);

                    bool valid = true;
                    int thirdTimestep = secondTimestep+1;
                    for (int h3i = 0; h3i < h.Count; h3i++) { // For each hail
                        if (h3i == h1i || h3i == h2i) {continue;}
                        var h3 = h[h3i];
                        // Console.WriteLine("\t\tX:" + h3);
                        
                        double xint = Intersect(h3.px, h3.vx, expecting.px, expecting.vx);
                        double yint = Intersect(h3.py, h3.vy, expecting.py, expecting.vy);
                        double zint = Intersect(h3.pz, h3.vz, expecting.pz, expecting.vz);

                        List<double> needToEqual = [];
                        if (h3.vx == expecting.vx) {
                            if (h3.px != expecting.px) {
                                valid = false;
                                break;
                            }
                        } else {
                            needToEqual.Add(xint);
                        }

                        if (h3.vy == expecting.vy) {
                            if (h3.py != expecting.py) {
                                valid = false;
                                break;
                            }
                        } else {
                            needToEqual.Add(yint);
                        }

                        if (h3.vz == expecting.vz) {
                            if (h3.pz != expecting.pz) {
                                valid = false;
                                break;
                            }
                        } else {
                            needToEqual.Add(zint);
                        }
                        // Console.WriteLine("\t\t\t" + xint + " " + yint + " " + zint);

                        if (needToEqual.Any(x => x != needToEqual[0])) {
                            // Console.WriteLine("not all were equal");
                            valid = false;
                            break;
                        }

                        if (!IsInteger(needToEqual[0])) {
                            // Console.WriteLine("not integer");
                            valid = false;
                            break;
                        }

                        int newTimestamp = (int) needToEqual[0];
                        // Console.WriteLine(newTimestamp);

                        if (seenTimesteps.Contains(newTimestamp)) {
                            // Console.WriteLine("already seen " + newTimestamp);
                            // foreach (var a in seenTimesteps) {
                                // Console.WriteLine("\t" + a);
                            // }
                            valid = false;
                            break;
                        }

                        seenTimesteps.Add(newTimestamp);
                        // Console.WriteLine("adding " + newTimestamp);
                        // foreach (var a in seenTimesteps) {
                            // Console.WriteLine("\t" + a);
                        // }
                    }
                    if (valid) {
                        Console.WriteLine(h1.px - (expecting.vx * firstTimestep) + h1.py - (expecting.vy * firstTimestep) + h1.pz - (expecting.vz * firstTimestep));
                        return;
                    }
                }
            }
        }
        secondTimestep++;
    }

}

bool pastPoint(double intersectX, double px, double slope) {
    if (slope > 0) {
        return px > intersectX;
    } else {
        return px < intersectX;
    }
}

bool IsInteger(double d) {
    return Math.Abs(d % 1) <= (double.Epsilon * 100);
}

double Intersect(double ap, double av, double bp, double bv) {
    // av(x) + ap == bv(x) + bp
    // (av + bv)(x) == bp - ap
    return (bp - ap) / (av - bv);
}

class Hail{
    public double px; public double py; public double pz;
    public double vx; public double vy; public double vz;
    public double slope;
    public double b;
    public Hail(string s) {
        double[] pos = s.Split('@')[0].Trim().Split(',').Select(double.Parse).ToArray();
        double[] vel = s.Split('@')[1].Trim().Split(',').Select(double.Parse).ToArray();
        px = pos[0]; py = pos[1]; pz = pos[2];
        vx = vel[0]; vy = vel[1]; vz = vel[2];
        slope = vy / vx;
        b = py - (slope * px);
    }
    
    public Hail(double px, double py, double pz) {
        this.px = px; this.py = py; this.pz = pz;
    }
    
    public Hail(double px, double py, double pz, double vx, double vy, double vz) {
        this.px = px; this.py = py; this.pz = pz;
        this.vx = vx; this.vy = vy; this.vz = vz;
    }

    public Hail Copy() {
        return PositionAtTime(0);
    }

    public double FindY(double x) {
        return slope * x + b;
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2} @ {3}, {4}, {5}", px, py, pz, vx, vy, vz);
    }

    public Hail PositionAtTime(int time) {
        return new Hail(px + vx * time, py + vy * time, pz + vz * time, vx, vy, vz);
    }

    public Hail Next() {
        return PositionAtTime(1);
    }
    
    public bool Equals(Hail o) {
        return px == o.px && py == o.py && pz == o.pz;
    }
}