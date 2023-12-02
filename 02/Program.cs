// Part 1


try {
    Dictionary<string, int> max = new Dictionary<string, int>();
    max["red"] = 12;
    max["green"] = 13;
    max["blue"] = 14;
    int ans = 0;
    using (var reader = new StreamReader("C:\\Users\\Will\\Documents\\code\\advent2023\\02\\input")) {
        Console.SetIn(reader);
        string line;
        int gameID = 1;
        while ((line = Console.ReadLine()!) != null) {
            string gamesStr = line.Split(": ")[1];
            string[] games = gamesStr.Split("; ");
            bool valid = true;
            foreach (string game in games) {
                string[] squares = game.Split(", ");
                foreach (string square in squares) {
                    string[] sq = square.Split(" ");
                    string number = sq[0];
                    int num = 0;
                    int.TryParse(number, out num);
                    string color = sq[1];
                    if (max[color] < num) {
                        valid = false;
                        break;
                    }
                }
                if (!valid) break;
            }
            if (valid) ans += gameID;
            gameID++;
        }
    }
    Console.WriteLine(ans);
} catch (Exception e) {
    Console.WriteLine("Exception: " + e.Message);
}




// Part 2
try {
    using (var reader = new StreamReader("C:\\Users\\Will\\Documents\\code\\advent2023\\02\\input")) {
        Console.SetIn(reader);
        string line;
        int part2Answer = 0;
        while ((line = Console.ReadLine()!) != null) {
            string gamesStr = line.Split(": ")[1];
            string[] games = gamesStr.Split("; ");
            Dictionary<string, int> d = new Dictionary<string, int>();
            d["red"] = 0;
            d["green"] = 0;
            d["blue"] = 0;
            foreach (string game in games) {
                string[] squares = game.Split(", ");
                foreach (string square in squares) {
                    string[] sq = square.Split(" ");
                    string number = sq[0];
                    int num = 0;
                    int.TryParse(number, out num);
                    string color = sq[1];
                    if (d[color] < num) d[color] = num;
                }
            }
            part2Answer += d["red"] * d["blue"] * d["green"];
        }
        Console.WriteLine(part2Answer);
    }
} catch (Exception e) {
    Console.WriteLine("Exception: " + e.Message);
}
