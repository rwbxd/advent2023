internal class Program
{
    static Dictionary<Tuple<int, int>, List<int>> gears = new Dictionary<Tuple<int, int>, List<int>>();

    private static void Main(string[] args)
    {

        long sum = 0;
        HashSet<string> digits = new HashSet<string>();
        for (int i = 0; i < 10; i++) digits.Add(i.ToString());

        List<string> rows = new List<string>();

        try
        {
            string line;
            using (var reader = new StreamReader("C:\\Users\\Will\\Documents\\code\\advent2023\\03\\input"))
            {
                while ((line = reader.ReadLine()!) != null) {
                    rows.Add(line);
                }
            }
            for (int i = 0; i < rows.Count; i++) {
                string curRow = rows[i];
                bool active = false;
                int curNum = 0;
                int left = 0;
                for (int j = 0; j < curRow.Length; j++) {
                    if (Char.IsDigit(curRow[j])) {
                        if (active) {
                            curNum *= 10;
                            curNum += (int) Char.GetNumericValue(curRow[j]);
                        } else {
                            active = true;
                            curNum = (int) Char.GetNumericValue(curRow[j]);
                            left = j;
                        }
                    } else if (active) {
                        if (FindSymbol(rows, i, left, j-1, curNum)) {
                            sum += curNum;
                        }
                        active = false;
                    }
                }
                if (active) {
                    if (FindSymbol(rows, i, left, curRow.Length - 1, curNum)) {
                        sum += curNum;
                    }
                    active = false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        Console.WriteLine("Sum: " + sum);

        long gearsSum = 0;
        foreach (List<int> item in gears.Values) {
            if (item.Count == 2) {
                gearsSum += item[0] * item[1];
            }
        }
        Console.WriteLine("Gears: " + gearsSum);
    }

    private static bool FindSymbol(List<string> rows, int index, int left, int right, int num) {
        bool returnValue = false;
        if (left == 0) {left = 1;}
        if (right == rows[index].Length - 1) {right = rows[index].Length - 2;}

        if (index != 0) {
            for (int i = left - 1; i <= right + 1; i++) {
                char curChar = rows[index-1][i];
                if (!Char.IsDigit(curChar) && curChar != '.') {
                    returnValue = true;
                    if (curChar == '*') {
                        List<int> l;
                        if (gears.TryGetValue((index - 1, i).ToTuple(), out l)) {
                            if (l != null) l.Add(num);
                        } else {
                            l = [num];
                            gears.Add((index - 1, i).ToTuple(), l);
                        }
                    }
                }
            }
        }

        if (!Char.IsDigit(rows[index][left-1]) && rows[index][left-1] != '.') {
            returnValue = true;
            if (rows[index][left-1] == '*') {
                List<int> l;
                if (gears.TryGetValue((index, left-1).ToTuple(), out l)) {
                    if (l != null) l.Add(num);
                } else {
                    l = [num];
                    gears.Add((index, left-1).ToTuple(), l);
                }
            }
        }
        if (!Char.IsDigit(rows[index][right+1]) && rows[index][right+1] != '.') {
            returnValue = true;
            if (rows[index][right+1] == '*') {
                List<int> l;
                if (gears.TryGetValue((index, right+1).ToTuple(), out l)) {
                    if (l != null) l.Add(num);
                } else {
                    l = [num];
                    gears.Add((index, right+1).ToTuple(), l);
                }
            }
        }

        if (index < rows.Count - 2) {
            for (int i = left - 1; i <= right + 1; i++) {
                char curChar = rows[index+1][i];
                if (!Char.IsDigit(curChar) && curChar != '.') {
                    returnValue = true;
                    if (curChar == '*') {
                        List<int> l;
                        if (gears.TryGetValue((index + 1, i).ToTuple(), out l)) {
                            if (l != null) l.Add(num);
                        } else {
                            l = [num];
                            gears.Add((index + 1, i).ToTuple(), l);
                        }
                    }
                }
            }
        }
        return returnValue;
    }

}

public struct Number {
    Number(int val, int start, int end, int y) {
        Val = val;
        Start = start;
        End = end;
        Y = y;
    }

    int Val { get; }
    int Start { get; }
    int End { get; }
    int Y { get; }
}

