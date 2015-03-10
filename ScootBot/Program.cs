using Newtonsoft.Json;
using SKYPE4COMLib;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ScootBot
{
    class Program
    {
        private static Skype skype;
        private const string trigger = "!";
        private static Random random = new Random();

        static void Main(string[] args)
        {
            skype = new Skype();
            // Use skype protocol version 7 
            skype.Attach(7, false);
            // Listen 
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(skype_MessageStatus);

            Console.WriteLine("Press any key to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static void skype_MessageStatus(ChatMessage msg, TChatMessageStatus status)
        {
            // Proceed only if the incoming message is a trigger
            if ((status == TChatMessageStatus.cmsSending || status == TChatMessageStatus.cmsReceived) && msg.Body.IndexOf(trigger) == 0)
            {
                // Remove trigger string and make lower case
                string command = msg.Body.Remove(0, trigger.Length).ToLower();

                // Send processed message back to skype chat window
                System.Console.WriteLine(command);
                ProcessCommand(msg, command);
            }
        }

        private static void ProcessCommand(ChatMessage msg, string command)
        {
            // process command
            List<string> result = new List<string>();
            switch (command)
            {
                case "sayhi":
                    result.Add("hi");
                    break;
                case "rtd":
                    int diceNumber = random.Next(6) + 1;
                    result.Add(diceNumber.ToString());
                    break;
                case "flipcoin":
                    string tempResult = "";
                    int coinNumber = random.Next(2);
                    if (coinNumber == 0) { tempResult = "heads"; }
                    else { tempResult = "tails"; }
                    result.Add(tempResult);
                    break;
                case "dota":
                    result.Add("LEEDLE LEEDLE LEEDLE");
                    break;
                case "stardate":
                    DateTime utcNow = DateTime.Now.AddHours(5);
                    //double century = Math.Floor(utcNow.Year / 100.0);
                    //double starCentury = century - 19.0; //Hack: In order to keep it consistent with Star Trek
                    ////double starCentury = century;
                    //double starYear = utcNow.Year - (century * 100.00);
                    //double starDay = (utcNow.DayOfYear * 24.0) + utcNow.Hour;
                    //double totalHoursInYear = (DateTime.IsLeapYear(utcNow.Year) ? 366 : 365) * 24;
                    //double starDate = (starCentury * 10000.0) + (starYear * 100) + (starDay * 100.0 / totalHoursInYear);'
                    string starDate = utcNow.Year.ToString() + '.' + utcNow.DayOfYear.ToString();
                    result.Add("the current stardate is " + starDate);
                    break;
                case "evetime":
                    result.Add("the current evetime is " + DateTime.Now.AddHours(5).ToString("HH:mm:ss"));
                    break;
                case "dotabuff":
                    List<string> matchids = new List<string>();
                    AddMatches(matchids, 30545806); // gilgi
                    AddMatches(matchids, 60514096); // boss man papa smurf
                    AddMatches(matchids, 34814716); // slapchop
                    AddMatches(matchids, 63826936); // manlytomb
                    AddMatches(matchids, 59311372); // holiday
                    AddMatches(matchids, 37784737); // teem capten marten gelganst
                    AddMatches(matchids, 47374215); // kwint
                    //result.Add("http://dotabuff.com/matches/" + matchids[random.Next(matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + matchids[0]);
                    break;
                case "dotabuff tritz":
                    List<string> tritz_matchids = new List<string>();
                    AddMatches(tritz_matchids, 63826936); // manlytomb
                    //result.Add("http://dotabuff.com/matches/" + tritz_matchids[random.Next(tritz_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + tritz_matchids[0]);
                    break;
                case "dotabuff gilgi":
                    List<string> gilgi_matchids = new List<string>();
                    AddMatches(gilgi_matchids, 30545806); // gilgi
                    //result.Add("http://dotabuff.com/matches/" + gilgi_matchids[random.Next(gilgi_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + gilgi_matchids[0]);
                    break;
                case "dotabuff nd":
                    List<string> nd_matchids = new List<string>();
                    AddMatches(nd_matchids, 34814716); // slapchop
                    //result.Add("http://dotabuff.com/matches/" + nd_matchids[random.Next(nd_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + nd_matchids[0]);
                    break;
                case "dotabuff mark":
                    List<string> smurf_matchids = new List<string>();
                    AddMatches(smurf_matchids, 60514096); // boss man papa smurf
                    //result.Add("http://dotabuff.com/matches/" + smurf_matchids[random.Next(smurf_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + smurf_matchids[0]);
                    break;
                case "dotabuff vindi":
                    List<string> vindicator_matchids = new List<string>();
                    AddMatches(vindicator_matchids, 37784737); // teem capten marten gelganst
                    //result.Add("http://dotabuff.com/matches/" + vindicator_matchids[random.Next(vindicator_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + vindicator_matchids[0]);
                    break;
                case "dotabuff kwint":
                    List<string> kwint_matchids = new List<string>();
                    AddMatches(kwint_matchids, 47374215); // kwint
                    //result.Add("http://dotabuff.com/matches/" + kwint_matchids[random.Next(kwint_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + kwint_matchids[0]);
                    break;
                case "dotabuff sehi":
                    List<string> sehi_matchids = new List<string>();
                    AddMatches(sehi_matchids, 59311372); // sehi
                    //result.Add("http://dotabuff.com/matches/" + sehi_matchids[random.Next(sehi_matchids.Count)]);
                    result.Add("http://dotabuff.com/matches/" + sehi_matchids[0]);
                    break;
                case "tritzsay":
                    result.Add(Say("0AlnL_8vlPlmWdEhJTHE1NVl2T19Ed0tWd20wSlN6dGc"));
                    break;
                case "ndsay":
                    result.Add(Say("0AlnL_8vlPlmWdGY4NUF5MjNDcTdPamxjYkdNVEJ5X2c"));
                    break;
                case "gilgisay":
                    result.Add(Say("0AlnL_8vlPlmWdE5uVVBGV2tOaDA5YzdpbHhNQ0dtaWc"));
                    break;
                case "kwintsay":
                    result.Add(Say("0AlnL_8vlPlmWdEJ2SzQ1ZEpPUHUxUGhwdUtBMFIzRXc"));
                    break;
                case "vindisay":
                    result.Add(Say("0AlnL_8vlPlmWdDVTZFphVVJFdkdGbmZhbGVnUjZrb1E"));
                    break;
                case "sehisay":
                    result.Add(Say("0AlnL_8vlPlmWdE9fdFNxUVdobWZGTkQtUlZlZFcycXc"));
                    break;
                case "marksay":
                    result.Add(Say("0AlnL_8vlPlmWdDVMbU5pblpWSktWSm4wbHF3b2lOSFE"));
                    break;
                case "tksay":
                    result.Add(Say("1-Wb6wUqoFDKmdTHBIEeFa4zObk1soQRKodcCnm3SlvE"));
                    break;
                case "lamsay":
                    result.Add(Say("1vMZlG-8QoeO4y-O5YY7leULJ7vUfRMdR35hHA0hv2rY"));
                    break;
                case "spacesay":
                    result.Add(Quotes.spaceResults[random.Next(Quotes.spaceResults.Length)]);
                    break;
                case "swiftsay":
                    result.Add(SwiftSay());
                    break;
                case "jukebox":
                    result.Add(JukeBox());
                    break;
                case "jb":
                    result.Add(JukeBox());
                    break;
                case "aotd":
                    result.Add(Aotd());
                    break;
                case "aotd throwback":
                    result.Add(AotdThrowback());
                    break;
                case "draft":
                    result.Add(Draft("allheroes"));
                    break;
                default:
                    if (command.StartsWith("pickone ") && command.Contains(" or "))
                    {
                        string meat = command.Substring(8);
                        string[] pieces = Regex.Split(meat, " or ");
                        result.Add(pieces[random.Next(pieces.Length)]);
                    }
                    else if (command.StartsWith("8ball "))
                    {
                        result.Add(Quotes.ballResults[random.Next(Quotes.ballResults.Length)]);
                    }
                    else if (command.StartsWith("echo "))
                    {
                        result.Add(command.Split(new Char[] { ' ' }, 2)[1]);
                    }
                    else if (command.StartsWith("jukebox ") || command.StartsWith("jb "))
                    {
                        result.Add(JukeBox(command.Split(new Char[] { ' ' }, 2)[1]));
                    }
                    else if (command.StartsWith("draft "))
                    {
                        result.Add(Draft(command.Split(new Char[] { ' ' }, 2)[1]));
                    }
                    else if (command.StartsWith("wolfram "))
                    {
                        result = Wolfram(command.Split(new Char[] { ' ' }, 2)[1]);
                    }
                    else if (command.StartsWith("weather "))
                    {
                        result = Wolfram("weather " + command.Split(new Char[] { ' ' }, 2)[1]);
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
         
            foreach (string resultMessage in result)
            {
                // prevent hax for dayz
                if (resultMessage.TrimStart().StartsWith("/leave"))
                {
                    return;
                }
                //msg.Chat.SendMessage(resultMessage);
            }
            msg.Chat.SendMessage(String.Join("\n", result));
        }

        private static string SwiftSay()
        {
            string url = "https://script.google.com/macros/s/AKfycbxJhcuk6P6JG0B1YzofwNUoWSQ8yQ-QGvcgcDfloQqF5Vc1JBw/exec";
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic tweetlist = response.result;
            dynamic tweet = tweetlist[random.Next(tweetlist.Count)];
            return tweet;
        }

        private static string AotdThrowback()
        {
            string url = "https://script.google.com/macros/s/AKfycbxZe3OukuZO20ahND9o4mgauaKA7dfFAgjPMFiObc6aYFISO-JQ/exec";
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic aotds = response.result;
            dynamic aotd = aotds[random.Next(aotds.Count - 1)];
            string result = aotd.album + " by " + aotd.artist + " (selected by " + aotd.selectedBy + " on " + aotd.date.ToString("MM/dd/yy") + "): " + aotd.link;
            return Clean(result);
        }

        private static string Aotd()
        {
            string url = "https://script.google.com/macros/s/AKfycbxZe3OukuZO20ahND9o4mgauaKA7dfFAgjPMFiObc6aYFISO-JQ/exec";
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic aotds = response.result;
            dynamic aotd = aotds[aotds.Count-1];
            string result = aotd.album + " by " + aotd.artist + " (selected by " + aotd.selectedBy + " on " + aotd.date.ToString("MM/dd/yy") + "): " + aotd.link;
            return Clean(result);
        }

        private static List<string> Wolfram(string query)
        {
            string url = "https://script.google.com/macros/s/AKfycbyRab1qtHahFG8jYmWRVETiQJ6ERaJRiAzl6mhpSjg268do14E/exec?query=" + query;
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            if ((bool)response.ok)
            {
                dynamic results = response.result;
                List<string> result = new List<string>();
                foreach (string resultMessage in results) {
                    result.Add(Clean(resultMessage));
                }
                return result;
            }
            else
            {
                List<string> result = new List<string>();
                result.Add("something failed");
                return result;
            }
        }

        private static string JukeBox()
        {
            string url = "https://script.google.com/macros/s/AKfycbydxbcsd0FNJzZBGYczJHyuY7ScKa-dXe36EdvuUt4fdtgiVFk/exec";
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic songlist = response.result;
            dynamic song = songlist[random.Next(songlist.Count)];
            string result = song.track + " by " + song.artist + " on playlist " + song.playlist + ": " + song.link;
            return Clean(result);
        }

        private static string Clean(string input)
        {
            while (input.TrimStart().StartsWith("!"))
            {
                input = input.Substring(1);
            }
            return input;
        }

        private static string JukeBox(string playlist)
        {
            string url = "https://script.google.com/macros/s/AKfycby-RHeBmakw97UVnNGIB0UEwBFiJAydIHfvDupwJgA4kaKHVUg/exec?playlist=" + playlist;
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            if ((bool) response.ok)
            {
                dynamic songlist = response.result;
                dynamic song = songlist[random.Next(songlist.Count)];
                string result = song.track + " by " + song.artist + ": " + song.link;
                return Clean(result);
            }
            else
            {
                return "playlist not found or empty";
            }
        }

        private static string Draft(string list)
        {
            if (list.Contains(" ")) {
                string[] pieces = list.Split(new Char[] { ' ' });
                list = String.Join("_", pieces);
            }
            string url = "https://script.google.com/macros/s/AKfycbxda9wYE7V3h_XSWgY4EtadDP6TDcT82gpIVejQgLxMfCXjC6rs/exec?herolist=" + list;
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            if ((bool) response.ok)
            {
                dynamic herolist = response.result;
                if (herolist.Count == 0)
                {
                    return "no heroes match your criteria";
                }
                string result = herolist[random.Next(herolist.Count)];
                return Clean(result);
            }
            else
            {
                return "herolist not found or empty";
            }
        }

        private static string Say(string id)
        {
            string url = "https://script.google.com/macros/s/AKfycbzbYuok-Ihqm7-QepK73x-GW63GHJG7AHUMvNd7ZBbHFuQRrc8/exec?id=" + id;
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic quotelist = response.result;
            string result = quotelist[random.Next(quotelist.Count)];
            return Clean(result);
        }

        private static void AddMatches(List<string> matches, int steamid)
        {
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=0538B63D120712F02F48101C51733DA0&account_id=" + steamid.ToString();
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            for (int i = 0; i < 25; i++)
            {
                string matchid = response.result.matches[i].match_id;
                matches.Add(matchid);
            }
        }

        private static string GetJSONData(string URL)
        {
            System.Net.HttpWebRequest request = null;
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                request.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;

        }
    }
}
