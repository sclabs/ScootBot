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
            string result;
            switch (command)
            {
                case "sayhi":
                    result = "hi";
                    break;
                case "rtd":
                    int diceNumber = random.Next(6) + 1;
                    result = diceNumber.ToString();
                    break;
                case "flipcoin":
                    result = "";
                    int coinNumber = random.Next(2);
                    if (coinNumber == 0) { result = "heads"; }
                    else { result = "tails"; }
                    break;
                case "dota":
                    result = "LEEDLE LEEDLE LEEDLE";
                    break;
                case "stardate":
                    DateTime utcNow = DateTime.Now.AddHours(-2);
                    double century = Math.Floor(utcNow.Year / 100.0);
                    double starCentury = century - 19.0; //Hack: In order to keep it consistent with Star Trek
                    double starYear = utcNow.Year - (century * 100.00);
                    double starDay = (utcNow.DayOfYear * 24.0) + utcNow.Hour;
                    double totalHoursInYear = (DateTime.IsLeapYear(utcNow.Year) ? 366 : 365) * 24;
                    double starDate = (starCentury * 10000.0) + (starYear * 100) + (starDay * 100.0 / totalHoursInYear);
                    result = "the current stardate is " + starDate;
                    break;
                case "evetime":
                    result = "the current evetime is " + DateTime.Now.AddHours(-2).ToString("HH:mm:ss"); ;
                    break;
                case "dotabuff":
                    List<string> matchids = new List<string>();
                    AddMatches(matchids, 30545806); // gilgi
                    AddMatches(matchids, 60514096); // boss man papa smurf
                    AddMatches(matchids, 34814716); // slapchop
                    AddMatches(matchids, 63826936); // manlytomb
                    //AddMatches(matchids, 59311372); // holiday
                    AddMatches(matchids, 37784737); // teem capten marten gelganst
                    //AddMatches(matchids, 47374215); // kwint
                    result = "http://dotabuff.com/matches/" + matchids[random.Next(matchids.Count)];
                    break;
                case "dotabuff tritz":
                    List<string> tritz_matchids = new List<string>();
                    AddMatches(tritz_matchids, 63826936); // manlytomb
                    result = "http://dotabuff.com/matches/" + tritz_matchids[random.Next(tritz_matchids.Count)];
                    break;
                case "dotabuff gilgi":
                    List<string> gilgi_matchids = new List<string>();
                    AddMatches(gilgi_matchids, 30545806); // gilgi
                    result = "http://dotabuff.com/matches/" + gilgi_matchids[random.Next(gilgi_matchids.Count)];
                    break;
                case "dotabuff nd":
                    List<string> nd_matchids = new List<string>();
                    AddMatches(nd_matchids, 34814716); // slapchop
                    result = "http://dotabuff.com/matches/" + nd_matchids[random.Next(nd_matchids.Count)];
                    break;
                case "dotabuff papa smurf":
                    List<string> smurf_matchids = new List<string>();
                    AddMatches(smurf_matchids, 60514096); // boss man papa smurf
                    result = "http://dotabuff.com/matches/" + smurf_matchids[random.Next(smurf_matchids.Count)];
                    break;
                case "dotabuff vindicator":
                    List<string> vindicator_matchids = new List<string>();
                    AddMatches(vindicator_matchids, 37784737); // teem capten marten gelganst
                    result = "http://dotabuff.com/matches/" + vindicator_matchids[random.Next(vindicator_matchids.Count)];
                    break;
                case "tritzsay":
                    result = Say("0AlnL_8vlPlmWdEhJTHE1NVl2T19Ed0tWd20wSlN6dGc");
                    break;
                case "ndsay":
                    result = Say("0AlnL_8vlPlmWdGY4NUF5MjNDcTdPamxjYkdNVEJ5X2c");
                    break;
                case "spacesay":
                    result = Quotes.spaceResults[random.Next(Quotes.spaceResults.Length)];
                    break;
                case "jukebox":
                    result = JukeBox();
                    break;
                case "jb":
                    result = JukeBox();
                    break;
                default:
                    if (command.StartsWith("pickone ") && command.Contains(" or "))
                    {
                        string meat = command.Substring(8);
                        string[] pieces = Regex.Split(meat, " or ");
                        result = pieces[random.Next(pieces.Length)];
                    }
                    else if (command.StartsWith("8ball "))
                    {
                        result = Quotes.ballResults[random.Next(Quotes.ballResults.Length)];
                    }
                    else if (command.StartsWith("echo "))
                    {
                        result = command.Split(new Char[] { ' ' }, 2)[1];
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            
            // prevent hax for dayz
            if (result.StartsWith("/leave"))
            {
                return;
            }

            msg.Chat.SendMessage(result);
        }

        private static string JukeBox()
        {
            string url = "https://script.google.com/macros/s/AKfycbydxbcsd0FNJzZBGYczJHyuY7ScKa-dXe36EdvuUt4fdtgiVFk/exec";
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic songlist = response.result;
            dynamic song = songlist[random.Next(songlist.Count)];
            return song.track + " by " + song.artist + " on playlist " + song.playlist + ": " + song.link;
        }

        private static string Say(string id)
        {
            string url = "https://script.google.com/macros/s/AKfycbzbYuok-Ihqm7-QepK73x-GW63GHJG7AHUMvNd7ZBbHFuQRrc8/exec?id=" + id;
            string jsonResponse = GetJSONData(url);
            dynamic response = JsonConvert.DeserializeObject(jsonResponse);
            dynamic quotelist = response.result;
            return quotelist[random.Next(quotelist.Count)];
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
