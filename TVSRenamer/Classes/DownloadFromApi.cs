using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TVSRenamer {
     public class API {
        public static List<TVShow> getShows(string name) {
            List<TVShow> list = new List<TVShow>();
            name = name.Replace(" ", "+");
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "TVS-Renamer");
            string responseFromServer = null;
            try {
                responseFromServer = wc.DownloadString("http://api.tvmaze.com/search/shows?q=" + name);
                
            } catch (Exception) {
                return list;
            }
            JArray test = JArray.Parse(responseFromServer);
            foreach (JToken jt in (JToken)test) { 
                JToken value = jt["show"];
                list.Add(value.ToObject<TVShow>());
               
            }
            list = list.OrderBy(x => x.weight).ToList();
            list.Reverse();
            return list;
        }
        private static string getToken() {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/login");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try {
                using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                    string data = "{\"apikey\": \"0E73922C4887576A\",\"username\": \"Kaharonus\",\"userkey\": \"28E2687478CA3B16\"}";
                    streamWriter.Write(data);
                }
            } catch (WebException) { MessageBox.Show("Connection error"); return null; }
            string text;
            try {
                var response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    text = sr.ReadToEnd();
                    text = text.Remove(text.IndexOf("\"token\""), "\"token\"".Length);
                    text = text.Split('\"', '\"')[1];
                    return text;
                }
            } catch (Exception e) {
                MessageBox.Show("Connection error!\n"+ e.Message); return null;
            }
        }
        public static List<string> GetAliases(TVShow s) {
            List<string> aliases = new List<string>();
            /*Regex reg = new Regex(@"\([0-9]{4}\)");
            Regex reg2 = new Regex(@"\.");
            aliases.Add(s.name);
            string temp = s.name;
            Match m = reg2.Match(temp);
            while (m.Success) {
                temp = temp.Remove(m.Index, 1);
                aliases.Add(s.name.Remove(m.Index, 1));
                m = reg2.Match(temp);
            }
            Match snMatch = reg.Match(s.name);
            if (snMatch.Success) {
                aliases.Add(reg.Replace(s.name, ""));
            }
            foreach (string alias in getAliasToken(s.tvdbID)) {
                aliases.Add(alias);
                Match regMatch = reg.Match(alias);
                if (regMatch.Success) {
                    aliases.Add(reg.Replace(alias, ""));
                }
            }
            for (int i = 0; i < aliases.Count(); i++) {
                if (aliases[i].Contains(" ")) {
                    aliases.Add(aliases[i].Replace(" ", "."));
                }
            }*/
            return aliases;
        }
        private static JToken getAliasToken(int id) {
            HttpWebRequest request = getRequest("https://api.thetvdb.com/series/" + id);
            try {
                var response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    JObject jo = JObject.Parse(sr.ReadToEnd());
                    return jo["data"]["aliases"];
                }
            } catch (WebException) {
                return null;
            }
        }
        private static HttpWebRequest getRequest(string link) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("Accept-Language", "en");
            request.Headers.Add("Authorization", "Bearer " + API.getToken());
            return request;
        }
        public static List<Episode> RequestEpisodes(TVShow s) {
            List<Episode> list = new List<Episode>();
            WebRequest wr = WebRequest.Create("http://api.tvmaze.com/shows/"+ s.id+"/episodes");
            HttpWebResponse response = null;
            try {
                response = (HttpWebResponse)wr.GetResponse();
            } catch (Exception) {
                return list;
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JArray jo = JArray.Parse(responseFromServer);
            foreach (JToken jt in jo) {
                Episode e = new Episode();
                e.season = Int32.Parse(jt["season"].ToString());
                e.episode = Int32.Parse(jt["number"].ToString());
                e.name = jt["name"].ToString();
                list.Add(e);
            }
            return list;
        }
    }
    public class TVShow {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        public List<string> genres { get; set; }
        public string status { get; set; }
        public int? runtime { get; set; }
        public string premiered { get; set; }
        public string officialSite { get; set; }
        public Schedule schedule { get; set; }
        public Rating rating { get; set; }
        public int? weight { get; set; }
        public Network network { get; set; }
        public object webChannel { get; set; }
        public Externals externals { get; set; }
        public Image image { get; set; }
        public string summary { get; set; }
        public int? updated { get; set; }
        public class Rating {
            public double? average { get; set; }
        }
        public class Country {
            public string name { get; set; }
            public string code { get; set; }
            public string timezone { get; set; }
        }
        public class Network {
            public int? id { get; set; }
            public string name { get; set; }
            public Country country { get; set; }
        }
        public class Externals {
            public int? tvrage { get; set; }
            public int? thetvdb { get; set; }
            public string imdb { get; set; }
        }
        public class Image {
            public string medium { get; set; }
            public string original { get; set; }
        }
        public class Schedule {
            public string time { get; set; }
            public List<object> days { get; set; }
        }

    }
    public class Episode {
        public string name;
        public int season;
        public int episode;
    }
}



    

