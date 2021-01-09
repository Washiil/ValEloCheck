using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace ValorantEloTracker
{
    public static class superparser
    {
        public static string regionparse(int i)
        {
            string region = "NA";
            switch (i)
            {
                case 0:
                    region = "NA";
                    break;
                case 1:
                    region = "EU";
                    break;
                case 2:
                    region = "KR";
                    break;
                case 3:
                    region = "AP";
                    break;
            }
            return region;
        }
        public static string ranknameparse(string i)
        {
            string ranknum = i;
            string rankname = "Radiant";
            switch (ranknum)
            {
                case "0":
                    rankname = "Unrated";
                    break;
                case "3":
                    rankname = "Iron 1";
                    break;
                case "4":
                    rankname = "Iron 2";
                    break;
                case "5":
                    rankname = "Iron 3";
                    break;
                case "6":
                    rankname = "Bronze 1";
                    break;
                case "7":
                    rankname = "Bronze 2";
                    break;
                case "8":
                    rankname = "Bronze 3";
                    break;
                case "9":
                    rankname = "Silver 1";
                    break;
                case "10":
                    rankname = "Silver 2";
                    break;
                case "11":
                    rankname = "Silver 3";
                    break;
                case "12":
                    rankname = "Gold 1";
                    break;
                case "13":
                    rankname = "Gold 2";
                    break;
                case "14":
                    rankname = "Gold 3";
                    break;
                case "15":
                    rankname = "Platinum 1";
                    break;
                case "16":
                    rankname = "Platinum 2";
                    break;
                case "17":
                    rankname = "Platinum 3";
                    break;
                case "18":
                    rankname = "Diamond 1";
                    break;
                case "19":
                    rankname = "Diamond 2";
                    break;
                case "20":
                    rankname = "Diamond 3";
                    break;
                case "21":
                    rankname = "Immortal 1";
                    break;
                case "22":
                    rankname = "Immortal 2";
                    break;
                case "23":
                    rankname = "Immortal 3";
                    break;
                case "24":
                    rankname = "Radiant";
                    break;
            }
            Console.WriteLine("Super Rank Parser: " + rankname);
            return rankname;
        }
        public static string mapnameparse(string i)
        {
            string ii = null;
            switch (i)
            {
                case "/Game/Maps/Duality/Duality":
                    ii = "Bind";
                    break;
                case "/Game/Maps/Bonsai/Bonsai":
                    ii = "Split";
                    break;
                case "/Game/Maps/Triad/Triad":
                    ii = "Haven";
                    break;
                case "/Game/Maps/Port/Port":
                    ii = "Ice Box";
                    break;
                case "/Game/Maps/Ascent/Ascent":
                    ii = "Ascent";
                    break;
            }
            return ii;
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime _ToDateTime(this long unixEpochTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(unixEpochTime);

            if (date.Year > 1972)
                return date;

            return epoch.AddSeconds(unixEpochTime);
        }
        public static string colorselectionparse(int i)
        {
            string parsedcolour = "NA";
            switch (i)
            {
                case 0:
                    //Red
                    parsedcolour = "#FFFA0000";
                    break;
                case 1:
                    //Light Red
                    parsedcolour = "#FFFA4454";
                    break;
                case 2:
                    //Blue
                    parsedcolour = "#FF0067FF";
                    break;
                case 3:
                    //Light Blue
                    parsedcolour = "#FF68B0FF";
                    break;
                case 4:
                    //Purple
                    parsedcolour = "#FF9800FF";
                    break;
                case 5:
                    //Light Purple
                    parsedcolour = "#FF9881FF";
                    break;
                case 6:
                    //Orange
                    parsedcolour = "#FFFF4800";
                    break;
                case 7:
                    //Light orange
                    parsedcolour = "#FFFFA840";
                    break;
                case 8:
                    //Yellow
                    parsedcolour = "#FFFFF400";
                    break;
                case 9:
                    //Light Yellow
                    parsedcolour = "#FFFFFBA3";
                    break;
                case 10:
                    //White
                    parsedcolour = "#FFFFFFFF";
                    break;
            }
            return parsedcolour;
        }
    }
}
