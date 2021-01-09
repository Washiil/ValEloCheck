using GetRankedPoints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using ValorantEloTracker;

namespace Logichandler
{
    public class LogicHandle
    {
        //Authentication Stuff
        public static string AccessToken { get; set; }
        public static string EntitlementToken { get; set; }
        //User stuff
        public static string username { get; set; }
        public static string password { get; set; }
        public static string UserID { get; set; }
        public static string region { get; set; }
        //Games Match stuff
        public static string rankname { get; set; }
        public static List<string> EloAfter = new List<string>();
        public static List<string> EloBefore = new List<string>();
        public static List<string> CompetitiveMovement = new List<string>();
        public static List<int> compranknum = new List<int>();
        public static List<double> elolist = new List<double>();
        public static List<string> maps = new List<string>();
        public static List<DateTime> matchstarts = new List<DateTime>();
        public static string[] CompetiveMovement { get; set; }
        //End Region
        public static string[] logicstartup()
        {
            string[] str = new string[2];
            username = ValorantEloTracker.Properties.Settings1.Default.username;
            password = ValorantEloTracker.Properties.Settings1.Default.password;
            region = superparser.regionparse(ValorantEloTracker.Properties.Settings1.Default.region);
            str[0] = username;
            str[1] = region;
            return str;
        }

        public static bool ValorantLogin()
        {
            try
            {
                CookieContainer cookie = new CookieContainer();
                Authentication.GetAuthorization(cookie);

                var authJson = JsonConvert.DeserializeObject(Authentication.Authenticate(cookie, username, password));
                JToken authObj = JObject.FromObject(authJson);

                if (authObj.ToString().Contains("error"))
                {
                    // error time lmfao
                    MessageBox.Show("Login is Incorrect, please fix login in settings.");
                    return false;
                }
                else
                {
                    string authURL = authObj["response"]["parameters"]["uri"].Value<string>();
                    var access_tokenVar = Regex.Match(authURL, @"access_token=(.+?)&scope=").Groups[1].Value;
                    AccessToken = $"{access_tokenVar}";

                    RestClient client = new RestClient(new Uri("https://entitlements.auth.riotgames.com/api/token/v1"));
                    RestRequest request = new RestRequest(Method.POST);

                    request.AddHeader("Authorization", $"Bearer {AccessToken}");
                    request.AddJsonBody("{}");

                    string response = client.Execute(request).Content;
                    var entitlement_token = JsonConvert.DeserializeObject(response);
                    JToken entitlement_tokenObj = JObject.FromObject(entitlement_token);

                    EntitlementToken = entitlement_tokenObj["entitlements_token"].Value<string>();


                    RestClient userid_client = new RestClient(new Uri("https://auth.riotgames.com/userinfo"));
                    RestRequest userid_request = new RestRequest(Method.POST);

                    userid_request.AddHeader("Authorization", $"Bearer {AccessToken}");
                    userid_request.AddJsonBody("{}");

                    string userid_response = userid_client.Execute(userid_request).Content;
                    dynamic userid = JsonConvert.DeserializeObject(userid_response);
                    JToken useridObj = JObject.FromObject(userid);

                    //Console.WriteLine(userid_response);

                    UserID = useridObj["sub"].Value<string>();
                    return true;
                }
            }
            catch (Exception e)
            {
                var error = e;
                MessageBox.Show("Your Login was invalid, please check your settings.");
                return false;
            }
        }

        static void elolog(IRestResponse rankedresp)
        {
            if (rankedresp.IsSuccessful)
            {
                dynamic RankedJson = JsonConvert.DeserializeObject<JObject>(rankedresp.Content);
                // Debugging IGNORE
                //Console.WriteLine(RankedJson);
                var store = RankedJson["Matches"];
                Console.WriteLine("Stored Data: \n\n" + store + "\n\n");

                for (int i = 0; i <= 19; i++)
                {
                    //Console.WriteLine(i);
                    var game = store[i];
                    int before = game["TierProgressBeforeUpdate"];
                    int after = game["TierProgressAfterUpdate"];
                    string ranknumber = game["TierAfterUpdate"];
                    string gametype = game["CompetitiveMovement"];
                    string map = game["MapID"];
                    long unixtime = game["MatchStartTime"];
                    DateTime convertedtime = superparser._ToDateTime(unixtime);
                    int XD = Convert.ToInt32(ranknumber);
                    double XDD = (XD * 100) - 300 + after;
                    if (gametype == "PROMOTED")
                    {
                        CompetitiveMovement.Add(gametype);
                        EloBefore.Add(before.ToString());
                        EloAfter.Add(after.ToString());
                        compranknum.Add(Convert.ToInt32(ranknumber));
                        elolist.Add(XDD);
                        maps.Add(map);
                        matchstarts.Add(convertedtime);
                    }
                    else if (gametype == "DEMOTED")
                    {
                        CompetitiveMovement.Add(gametype);
                        EloBefore.Add(before.ToString());
                        EloAfter.Add(after.ToString());
                        compranknum.Add(Convert.ToInt32(ranknumber));
                        elolist.Add(XDD);
                        maps.Add(map);
                        matchstarts.Add(convertedtime);
                    }
                    else if (gametype != "MOVEMENT_UNKNOWN")
                    {
                        CompetitiveMovement.Add(gametype);
                        EloBefore.Add(before.ToString());
                        EloAfter.Add(after.ToString());
                        compranknum.Add(Convert.ToInt32(ranknumber));
                        elolist.Add(XDD);
                        maps.Add(map);
                        matchstarts.Add(convertedtime);
                    }
                    else if (gametype == "MOVEMENT_UNKNOWN")
                    {

                    }
                }
                return;
            }
        }

        public static void CheckRankedUpdates()
        {
            try
            {
                RestClient ranked_client = new RestClient(new Uri($"https://pd.{region}.a.pvp.net/mmr/v1/players/{UserID}/competitiveupdates?startIndex=0&endIndex=20"));
                RestRequest ranked_request = new RestRequest(Method.GET);
                RestClient ranked_client2 = new RestClient(new Uri($"https://pd.{region}.a.pvp.net/mmr/v1/players/{UserID}/competitiveupdates?startIndex=20&endIndex=40"));
                RestRequest ranked_request2 = new RestRequest(Method.GET);

                ranked_request.AddHeader("Authorization", $"Bearer {AccessToken}");
                ranked_request.AddHeader("X-Riot-Entitlements-JWT", EntitlementToken);
                ranked_request2.AddHeader("Authorization", $"Bearer {AccessToken}");
                ranked_request2.AddHeader("X-Riot-Entitlements-JWT", EntitlementToken);

                IRestResponse rankedresp = ranked_client.Get(ranked_request);
                IRestResponse rankedresp2 = ranked_client2.Get(ranked_request2);

                Console.WriteLine("\nMost Recent 20 Games\n");
                elolog(rankedresp);
                Console.WriteLine(elolist.Count);
                elolog(rankedresp2);
                Console.WriteLine(elolist.Count);
                rankname = superparser.ranknameparse(Convert.ToString(compranknum[0]));
                //Console.WriteLine((Convert.ToInt32(ranknum) * 100) - 300 + 47);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
