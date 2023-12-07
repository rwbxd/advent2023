using System.Runtime.InteropServices;

internal class Program {
    class Hand:IComparable<Hand> {
        int[] Cards;
        public int Bid;
        public int Score;
        public Hand(string cards, int bid, bool jokers = false) {
            Cards = ConvertCardsToPointValues(cards, jokers);
            Bid = bid;
            Score = ScoreHand(Cards, jokers);
        }

        private static int[] ConvertCardsToPointValues(string cards, bool jokers) {
            int[] points = new int[cards.Length];
            for (int i = 0; i < cards.Length; i++) {
                int val;
                char c = cards[i];
                switch (c) {
                    case 'T': val = 10; break;
                    case 'J':
                        if (jokers) val = 0;
                        else val = 11;
                        break;
                    case 'Q': val = 12; break;
                    case 'K': val = 13; break;
                    case 'A': val = 14; break;
                    default: val = int.Parse(c.ToString()); break;
                }
                points[i] = val;
            }
            return points;
        }

        private enum PointValues : int {
            FIVE_OF_A_KIND = 7,
            FOUR_OF_A_KIND = 6,
            FULL_HOUSE = 5,
            THREE_OF_A_KIND = 4,
            TWO_PAIRS = 3,
            ONE_PAIR = 2,
            HIGH_CARD = 1,
        }
        private static int ScoreHand(int[] Cards, bool jokers) {
            if (jokers && Cards.Contains(0)) return ScoreJokersHand(Cards);

            Dictionary<int, int> seen = [];
            foreach (int c in Cards) {
                if (!seen.ContainsKey(c)) seen[c] = 0;
                seen[c] += 1;
            }

            switch (seen.Count) {
                case 1:
                    return (int) PointValues.FIVE_OF_A_KIND;
                case 2:
                    if (seen[Cards[0]] is 1 or 4) {
                        return (int) PointValues.FOUR_OF_A_KIND;
                    } else {
                        return (int) PointValues.FULL_HOUSE;
                    }
                case 3:
                    foreach (int val in seen.Values) {
                        if (val == 3) return (int) PointValues.THREE_OF_A_KIND;
                        else if (val == 2) return (int) PointValues.TWO_PAIRS;
                    }
                    return 0; // This should never hit
                case 4:
                    return (int) PointValues.ONE_PAIR;
                default:
                    return (int) PointValues.HIGH_CARD;
            }
        }

        private static int ScoreJokersHand(int[] Cards) {
            Dictionary<int, int> seen = new Dictionary<int, int>();
            foreach (int c in Cards) {
                if (!seen.ContainsKey(c)) seen[c] = 0;
                seen[c] += 1;
            }

            switch (seen.Count) {
                case 1:
                    return (int) PointValues.FIVE_OF_A_KIND;
                case 2:
                    return (int) PointValues.FIVE_OF_A_KIND;
                case 3:
                    if (seen[0] is 2 || seen.ContainsValue(3)) return (int) PointValues.FOUR_OF_A_KIND;
                    else return (int) PointValues.FULL_HOUSE;
                case 4:
                    return (int) PointValues.THREE_OF_A_KIND;
                default:
                    return (int) PointValues.ONE_PAIR;
            }
        }

        public int CompareTo(Hand? other)
        {
            if (other == null) return -1;
            if (Score != other.Score) return Score.CompareTo(other.Score);

            for (int i = 0; i < Cards.Length; i++) {
                if (Cards[i] != other.Cards[i]) {
                    return Cards[i].CompareTo(other.Cards[i]);
                }
            }

            return 0;
        }

        override public string ToString() {
            return string.Join(",", Cards);
        }
    }

    static readonly string input = "C:\\Users\\Will\\Documents\\code\\advent2023\\07\\input";
    private static void Main() {
        StreamReader reader;

        // Part 1
        reader = new StreamReader(input);
        try {
            List<Hand> hands = new List<Hand>();
            string line;
            while ((line = reader.ReadLine()!) != null) {
                string[] contents = line.Split();
                hands.Add(new Hand(contents[0], int.Parse(contents[1]), false));
            }
            hands.Sort();
            long total = 0;
            for (int i = 0; i < hands.Count; i++) {
                // Console.Write(hands[i]);
                // Console.Write(" ");
                // Console.Write(hands[i].Score);
                // Console.Write(" ");
                // Console.WriteLine(hands[i].Bid);
                total += (i+1) * hands[i].Bid;
            }
            Console.WriteLine("Part 1: " + total.ToString());
        } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
        }

        // Part 2
        reader = new StreamReader(input);
        try {
            List<Hand> hands = new List<Hand>();
            string line;
            while ((line = reader.ReadLine()!) != null) {
                string[] contents = line.Split();
                hands.Add(new Hand(contents[0], int.Parse(contents[1]), true));
            }
            hands.Sort();
            long total = 0;
            for (int i = 0; i < hands.Count; i++) {
                // Console.Write(hands[i]);
                // Console.Write(" ");
                // Console.Write(hands[i].Score);
                // Console.Write(" ");
                // Console.WriteLine(hands[i].Bid);
                total += (i+1) * hands[i].Bid;
            }
            Console.WriteLine("Part 2: " + total.ToString());
        } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
        }
    }


}