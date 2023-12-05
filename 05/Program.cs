internal class Program
{
    static readonly string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\05\\input";
    // static readonly string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\05\\testinput";
    private static void Main(string[] args)
    {
        Console.WriteLine("Part 1: " + Part1().ToString());
        Console.WriteLine("Part 2: " + Part2().ToString());
    }

    private class Conversion {
        private readonly long Start;
        private readonly long End;
        private readonly long Dest;
        
        public Conversion(long[] args) : this(args[0], args[1], args[2]) {}
        public Conversion(long dest, long src, long range) {
            Start = src;
            End = src + range - 1; // inclusive
            Dest = dest;
        }

        public bool Lookup(long x, out long y) {
            if (Start <= x && x <= End) {
                y = x - Start + Dest;
                return true;
            }

            y = x;
            return false;
        }
    }

    private static long Part1() {
        try {
            StreamReader reader = new StreamReader(input);
            string line;
            
            string seeds = reader.ReadLine()!;
            long[] seedNumbers = Array.ConvertAll(seeds.Split(": ")[1].Split(), long.Parse);
            
            List<List<Conversion>> conversions = new List<List<Conversion>>();
            reader.ReadLine(); // blank line
            while ((line = reader.ReadLine()!) != null) {
                // line = map name
                string mapline;
                List<Conversion> curConversion = new List<Conversion>();
                while ((mapline = reader.ReadLine()!) != null && mapline != "") {
                    long[] args = Array.ConvertAll(mapline.Split(), long.Parse);
                    curConversion.Add(new Conversion(args));
                }
                conversions.Add(curConversion);
            }

            long minresult = long.MaxValue;
            foreach (long seed in seedNumbers) {
                long x = seed;
                long y = 0;
                foreach (List<Conversion> conversionList in conversions) {
                    foreach (Conversion c in conversionList) {
                        if (c.Lookup(x, out y)) {
                            break;
                        }
                    }
                    // Console.WriteLine(string.Format("Converting {0} to {1}", x, y));
                    x = y;
                }
                minresult = long.Min(x, minresult);
            }
            
            return minresult;

        } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
        }
        return 0;
    }

    private class SeedGenerator {
        private long Start;
        private long Length;
        public SeedGenerator(long start, long length) {
            Start = start;
            Length = length;
        }

        public IEnumerable<long> GetSeeds() {
            int i = 0;
            while (i < Length) {
                yield return Start + i++;
            }
        }

    }

    private static long Part2() {
        long totalSeeds = 0;
        long curSeed = 0;
        try {
            StreamReader reader = new StreamReader(input);
            string line;
            
            string seeds = reader.ReadLine()!;
            long[] seedNumbers = Array.ConvertAll(seeds.Split(": ")[1].Split(), long.Parse);
            SeedGenerator[] seedGenerators = new SeedGenerator[seedNumbers.Length / 2];
            for (int i = 0; i < seedNumbers.Length; i += 2) {
                seedGenerators[i / 2] = new SeedGenerator(seedNumbers[i], seedNumbers[i + 1]);
                totalSeeds += seedNumbers[i + 1];
            }
            
            List<List<Conversion>> conversions = new List<List<Conversion>>();
            reader.ReadLine(); // blank line
            while ((line = reader.ReadLine()!) != null) {
                // line = map name
                string mapline;
                List<Conversion> curConversion = new List<Conversion>();
                while ((mapline = reader.ReadLine()!) != null && mapline != "") {
                    long[] args = Array.ConvertAll(mapline.Split(), long.Parse);
                    curConversion.Add(new Conversion(args));
                }
                conversions.Add(curConversion);
            }

            DateTime startTime = DateTime.Now;
            DateTime oldTimer = DateTime.Now;
            DateTime curTimer = DateTime.Now;
            double lastValue = 0;
            long minresult = long.MaxValue;
            foreach (SeedGenerator gen in seedGenerators) {
                foreach (long seed in gen.GetSeeds()) {
                    if (curSeed++ % 10_000_000 == 0) {
                        oldTimer = curTimer;
                        curTimer = DateTime.Now;
                        double progression = lastValue - curSeed;
                        lastValue = curSeed;
                        Console.WriteLine("ETA: " + curTimer.Subtract(oldTimer).Multiply((totalSeeds - curSeed) / progression).ToString());
                    }
                    long x = seed;
                    long y = 0;
                    foreach (List<Conversion> conversionList in conversions) {
                        foreach (Conversion c in conversionList) {
                            if (c.Lookup(x, out y)) {
                                break;
                            }
                        }
                        // Console.WriteLine(string.Format("Converting {0} to {1}", x, y));
                        x = y;
                    }
                    minresult = long.Min(x, minresult);
                }
            }
            
            Console.WriteLine("Time elapsed: " + (DateTime.Now - startTime).ToString());
            return minresult;

        } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
        }
        return 0;
    }
}