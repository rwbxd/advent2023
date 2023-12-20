using System.Reflection;

int day = 20;
string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\" + day + "\\input";
Part1();
Part2();

void Part1() {
    long lowPulses = 0; long highPulses = 0;
    Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
    StreamReader reader = new StreamReader(input);
    string line;
    while ((line = reader.ReadLine()!) != null) {
        string[] split1 = line.Split(" -> ");
        string[] receivers = split1[1].Split(", ");
        if (split1[0] == "broadcaster") {
            modules["broadcaster"] = new Broadcast(receivers);
        } else if (split1[0][0] == '%') {
            modules[split1[0][1..]] = new FlipFlop(receivers);
        } else {
            modules[split1[0][1..]] = new Conjunction(receivers);
        }
    }
    Dictionary<string, IModule> sinks = new Dictionary<string, IModule>();
    foreach (var item in modules) {
        foreach (string r in item.Value.Receivers) {
            if (!modules.ContainsKey(r)) {sinks[r] = new Sink();}
            else {modules[r].RegisterInput(item.Key);}
        }
    }
    foreach (var item in sinks) {modules.Add(item.Key, item.Value);}
    
    Queue<Pulse> pulses = new Queue<Pulse>();

    for (int i = 0; i < 1000; i++) {
        pulses.Enqueue(new Pulse("broadcaster", false));
        lowPulses++;
        Console.WriteLine("button -low-> broadcaster");
        while (pulses.Count != 0) {
            Pulse p = pulses.Dequeue();
            string source = p.source;
            IModule sourceModule = modules[source];
            bool inputStrength = p.inputStrength;
            foreach (string receiver in sourceModule.Receivers) {
                if (inputStrength) {
                    highPulses++;
                    Console.WriteLine(string.Format("{0} -high-> {1}", source, receiver));
                } else {
                    lowPulses++;
                    Console.WriteLine(string.Format("{0} -low-> {1}", source, receiver));
                }

                IModule receiverModule = modules[receiver];
                bool newStrength;
                if (receiverModule.Pulse(inputStrength, p.source, out newStrength)) {
                    pulses.Enqueue(new Pulse(receiver, newStrength));
                }
            }
        }
    }

    Console.WriteLine(lowPulses * highPulses);
}

void Part2() {long lowPulses = 0; long highPulses = 0;
    Dictionary<string, IModule> modules = new Dictionary<string, IModule>();
    StreamReader reader = new StreamReader(input);
    string line;
    while ((line = reader.ReadLine()!) != null) {
        string[] split1 = line.Split(" -> ");
        string[] receivers = split1[1].Split(", ");
        if (split1[0] == "broadcaster") {
            modules["broadcaster"] = new Broadcast(receivers);
        } else if (split1[0][0] == '%') {
            modules[split1[0][1..]] = new FlipFlop(receivers);
        } else {
            modules[split1[0][1..]] = new Conjunction(receivers);
        }
    }

    Dictionary<string, IModule> sinks = new Dictionary<string, IModule>();
    HashSet<string> rxSource = new HashSet<string>();
    foreach (var item in modules) {
        foreach (string r in item.Value.Receivers) {
            if (r == "rx") rxSource.Add(item.Key);
            if (!modules.ContainsKey(r)) {sinks[r] = new Sink();}
            else {modules[r].RegisterInput(item.Key);}
        }
    }
    foreach (var item in sinks) {modules.Add(item.Key, item.Value);}

    Dictionary<string, HashSet<string>> rxSourceFull = new Dictionary<string, HashSet<string>>();
    foreach (string s in rxSource) {
        Console.WriteLine("Finding dependencies for " + s);
        HashSet<string> hs = new HashSet<string>();
        Queue<string> q = new Queue<string>();
        q.Enqueue(s);

        while (q.Count != 0) {
            string cur = q.Dequeue();
            if (hs.Contains(cur)) continue;
            foreach (var item in modules) {
                if (item.Value.Receivers.Contains(cur)) {
                    Console.WriteLine(item.Key);
                    q.Enqueue(item.Key);
                    hs.Add(item.Key);
                }
            }
        }
        rxSourceFull[s] = hs;
    }
    
    Dictionary<string, long> lastHigh = new Dictionary<string, long>
    {
        { "ss", 0 },
        { "fz", 0 },
        { "mf", 0 },
        { "fh", 0 }
    };
    List<long> loops = new List<long>();
    Queue<Pulse> pulses = new Queue<Pulse>();

    long iter = 0;
    bool rxActivated = false;
    while (!rxActivated) {
        iter++;
        pulses.Enqueue(new Pulse("broadcaster", false));
        lowPulses++;
        // if (iter % 100_000 == 0) {Console.WriteLine(iter);}
        while (pulses.Count != 0) {
            Pulse p = pulses.Dequeue();
            string source = p.source;
            IModule sourceModule = modules[source];
            bool inputStrength = p.inputStrength;
            foreach (string receiver in sourceModule.Receivers) {
                if (inputStrength) {
                    highPulses++;
                    if (lastHigh.ContainsKey(source)) {
                        Console.WriteLine(string.Format("{0} is high on iter: {1} (prev seen {2} ago)", source, iter, iter - lastHigh[source]));
                        loops.Add(iter - lastHigh[source]);
                        lastHigh.Remove(source);
                        if (lastHigh.Count == 0) {
                            Console.WriteLine(LCM(loops.ToArray()));
                            return;
                        }
                    }
                    // Console.WriteLine(string.Format("{0} -high-> {1}", source, receiver));
                } else {
                    lowPulses++;
                    if (receiver == "rx") { rxActivated = true; }
                    // Console.WriteLine(string.Format("{0} -low-> {1}", source, receiver));
                }

                IModule receiverModule = modules[receiver];
                bool newStrength;
                if (receiverModule.Pulse(inputStrength, p.source, out newStrength)) {
                    pulses.Enqueue(new Pulse(receiver, newStrength));
                }
            }
        }
    }

    Console.WriteLine(iter);
}

//https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490
static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

interface IModule {
    public bool Pulse(bool inputStrength, string source, out bool outputStrength);
    public void RegisterInput(string source);
    public bool isReset();
    char ModuleType { get; }
    string[] Receivers { get; }
}

class FlipFlop(string[] receivers) : IModule {
    public string[] receivers = receivers;
    bool flip = false;

    public bool Pulse(bool inputStrength, string source, out bool outputStrength) {
        if (inputStrength) { // High input
            outputStrength = false; // Doesn't matter
            return false; // No output
        } else { // Low input
            flip = !flip; // Flip the flop
            outputStrength = flip;
            return true; // Yes output
        }
    }

    public void RegisterInput(string source){}

    public string[] Receivers => receivers;

    public char ModuleType => 'f';

    public bool isReset() {return flip = false;}
}

class Conjunction(string[] receivers) : IModule {
    public string[] receivers = receivers;
    Dictionary<string, bool> memory = new Dictionary<string, bool>();
    public bool Pulse(bool inputStrength, string source, out bool outputStrength) {
        memory[source] = inputStrength;
        if (memory.Values.All(x => x == true)) {
            outputStrength = false;
        } else {
            outputStrength = true;
        }
        return true;
    }

    public void RegisterInput(string source) {
        memory[source] = false;
    }

    public string[] Receivers => receivers;

    public char ModuleType => 'c';
    public bool isReset() {return memory.Values.All(x => x == false);}
}

class Broadcast(string[] receivers) : IModule {
    public string[] receivers = receivers;

    public bool Pulse(bool inputStrength, string source, out bool outputStrength) {
        outputStrength = inputStrength;
        return true;
    }
    
    public void RegisterInput(string source){}

    public string[] Receivers => receivers;
    public char ModuleType => 'b';
    public bool isReset() {return true;}
}

class Sink() : IModule {
    public string[] receivers = [];
    public bool Pulse(bool inputStrength, string source, out bool outputStrength) {
        outputStrength = false;
        return false;
    }
    
    public void RegisterInput(string source){}

    public string[] Receivers => receivers;
    public char ModuleType => 's';
    public bool isReset() {return true;}
}

class Pulse(string source, bool inputStrength) {
    public string source = source;
    public bool inputStrength = inputStrength;
}