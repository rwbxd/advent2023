internal class Program {
    static void Main() {
        long total;
        // Part 1
        total = 0;
        try
        {
            string line;
            using (var reader = new StreamReader("C:\\Users\\Will\\Documents\\code\\advent2023\\04\\input"))
            {
                while ((line = reader.ReadLine()!) != null) {
                    long lineSum = 1;
                    string[] splitNums = line.Split(" | ");
                    // Console.WriteLine(splitNums[1]);
                    string[] cardNums = splitNums[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    string[] winningNums = splitNums[0].Split(": ")[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // Console.WriteLine(splitNums[0].Split(": ")[1]);
                    HashSet<int> winningInts = new HashSet<int>();
                    foreach (string num in winningNums) {
                        // Console.WriteLine(num);
                        winningInts.Add(int.Parse(num));
                    }

                    foreach (string num in cardNums) {
                        int numInt = int.Parse(num);
                        if (winningInts.Contains(numInt)) {
                            lineSum *= 2;
                        }
                    }

                    if (lineSum != 1) {
                        total += lineSum / 2;
                    }
                }
            }
        } catch (Exception e) {
            Console.WriteLine("Exception: " + e.StackTrace);
        }

        Console.WriteLine(total);

        // Part 2
        total = 0;
        try
        {
            Stack<String> cards = new Stack<string>();
            int numCards = 0;
            string line;
            using (var reader = new StreamReader("C:\\Users\\Will\\Documents\\code\\advent2023\\04\\input"))
            {
                while ((line = reader.ReadLine()!) != null) {
                    cards.Push(line);
                }
                numCards = cards.Count;

                Console.WriteLine(cards.Count);

                long[] numWinners = new long[cards.Count];
                int i = cards.Count - 1;
                while (i >= 0) {
                    string card = cards.Pop();
                    Console.WriteLine(i.ToString(), cards.Count);
                    long winners = 0;
                    string[] splitNums = card.Split(" | ");
                    // Console.WriteLine(splitNums[1]);
                    string[] cardNums = splitNums[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    string[] winningNums = splitNums[0].Split(": ")[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // Console.WriteLine(splitNums[0].Split(": ")[1]);
                    HashSet<int> winningInts = new HashSet<int>();
                    foreach (string num in winningNums) {
                        // Console.WriteLine(num);
                        winningInts.Add(int.Parse(num));
                    }

                    foreach (string num in cardNums) {
                        int numInt = int.Parse(num);
                        if (winningInts.Contains(numInt)) {
                            winners += 1;
                        }
                    }

                    long memoWinners = winners;
                    for (int j = 1; j <= winners; j++) {
                        memoWinners += numWinners[i + j];
                    }

                    numWinners[i] = memoWinners;
                    total += memoWinners;

                    i--;
                }
                Console.WriteLine(total + numCards);
            }
        } catch (Exception e) {
            Console.WriteLine("Exception: " + e.StackTrace);
        }

        
    }
}