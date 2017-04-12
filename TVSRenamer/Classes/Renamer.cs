﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TVSRenamer {
    class Renamer {
        public static void RenameBatch(List<string> locations, string finalLoc , Show show) {
            show.aliases.AddRange(API.GetAliases(show));
            locations.Insert(0, finalLoc);
            if (!show.name.Equals(Path.GetDirectoryName(finalLoc), StringComparison.InvariantCultureIgnoreCase)) {
                finalLoc += "\\" + show.name;
            }
            List<string> files = ScanEpisodes(locations, show);
            RenameFiles(files, finalLoc,show);
        }
        public static string GetName(string showName, int season, int episode, string epName) {
            if (season < 10) {
                if (episode < 10) {
                    return showName + " - S0" + season + "E0" + episode + " - " + epName;
                } else if (episode >= 10) {
                    return showName + " - S0" + season + "E" + episode + " - " + epName;
                }
            } else if (season >= 10) {
                if (episode < 10) {
                    return showName + " - S" + season + "E0" + episode + " - " + epName;
                } else if (episode >= 10) {
                    return showName + " - S" + season + "E" + episode + " - " + epName;
                }

            }
            return null;
        }

        public static string GetValidName(string path, string name, string extension, string original) {
            int filenumber = 1;
            string final;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Match m = new Regex("s[0-5][0-9]", RegexOptions.IgnoreCase).Match(name);
            int s = Int32.Parse(m.Value.Remove(0, 1));
            foreach (char c in invalid) {
                name = name.Replace(c.ToString(), "");
            }
            if (s < 10) {
                path += "\\Season 0" + s;
                Directory.CreateDirectory(path);
            } else if (s >= 10) {
                path += "\\Season " + s;
                Directory.CreateDirectory(path);
            }
            final = path + "\\" + name + extension;
            if (original != final) {
                while (File.Exists(final)) {
                    final = path + "\\" + name + "_" + filenumber + extension;
                    filenumber++;
                }
            }
            return final;
        }
        public static List<string> ScanEpisodes(List<string> locations, Show s) {
            List<string> showFiles = new List<string>();
            List<string> files = new List<string>();
            foreach (string location in locations) {
                if (Directory.Exists(location)) { 
                    files.AddRange(Directory.GetFiles(location, "*.*", System.IO.SearchOption.AllDirectories));
                }
            }
            foreach (string file in files) {
                foreach (string alias in s.aliases) {
                    if (Path.GetFileName(file).IndexOf(alias, StringComparison.OrdinalIgnoreCase) >= 0 && !showFiles.Contains(file)) {
                        showFiles.Add(file);
                    }
                }
            }
            return FilterExtensions(showFiles);
        }
        public static Tuple<int, int> GetInfo(string file) {
            Match season = new Regex("[s][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            Match episode = new Regex("[e][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            Match special = new Regex("[0-5][0-9][x][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            if (season.Success && episode.Success) {
                int s = Int32.Parse(season.Value.Remove(0, 1));
                int e = Int32.Parse(episode.Value.Remove(0, 1));
                return new Tuple<int, int>(s, e);
            } else if (special.Success) {
                int s = Int32.Parse(special.Value.Substring(0, 2));
                int e = Int32.Parse(special.Value.Substring(3, 2));
                return new Tuple<int, int>(s, e);
            }
            return null;
        }
        public static List<string> FilterExtensions(List<string> files) {
            string[] fileExtension = new string[9] { ".mkv", ".srt", ".m4v", ".avi", ".mp4", ".mov", ".sub", ".wmv", ".flv" };
            List<string> filtered = new List<string>();
            foreach (string file in files) {
                if (fileExtension.Any(file.Contains)) {
                    filtered.Add(file);
                }
            }
            return filtered;
        }
        public static void RenameFiles(List<string> files, string path, Show show) {
            List<Episode> EPNames = API.RequestEpisodes(show);
            foreach (string file in files) {
                Tuple<int, int> t = GetInfo(file);
                int season = t.Item1;
                int episode = t.Item2;
                var selectedEP = EPNames.FirstOrDefault(o => o.season == season && o.episode == episode);
                int index = EPNames.FindIndex(o => o.season == season && o.episode == episode);
                if (selectedEP == null) {
                    MessageBox.Show("This TV Show doesnt have episode " + episode + " in season " + season + ".\nFile " + file + " won't be renamed", "Error");
                } else {
                    string output = GetValidName(path, GetName(show.name, selectedEP.season, selectedEP.episode, selectedEP.name), Path.GetExtension(file), file);
                    if (file != output) {
                        File.Move(file, output);                      
                    }
                }
            }
        }
    }
}
