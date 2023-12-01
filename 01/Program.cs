long result = 0;

Dictionary<string, int> digitDict = new Dictionary<string, int>();
digitDict["zero"] = 0;
digitDict["one"] = 1;
digitDict["two"] = 2;
digitDict["three"] = 3;
digitDict["four"] = 4;
digitDict["five"] = 5;
digitDict["six"] = 6;
digitDict["seven"] = 7;
digitDict["eight"] = 8;
digitDict["nine"] = 9;

try {
    using (var reader = new StreamReader("input")) {
        Console.SetIn(reader);
        String line;
        int out1 = 0;
        int out2 = 0;
        while ((line = Console.ReadLine()!) != null) {
            for (int i = 0; i < line.Length; i++) {
                bool canConvert = int.TryParse(line[i].ToString(), out out1);
                if (canConvert) {
                    break;
                }

                bool found = false;
                foreach (var item in digitDict) {
                    if (line.Substring(i).StartsWith(item.Key)) {
                        out1 = item.Value;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            for (int i = line.Length - 1; i >= 0; i--) {
                bool canConvert = int.TryParse(line[i].ToString(), out out2);
                if (canConvert) {
                    break;
                }

                bool found = false;
                foreach (var item in digitDict) {
                    if (line.Substring(i).StartsWith(item.Key)) {
                        out2 = item.Value;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            result += out1 * 10 + out2;
        }
    }
} catch (Exception e) {
    Console.WriteLine("Exception: " + e.Message);
}

Console.WriteLine(result);