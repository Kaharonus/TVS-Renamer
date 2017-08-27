using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TVSRenamer {
  /*  class Renamer {
        public static void RenameBatch(List<string> locations, string finalLoc , TVShow show) {
            show.aliases.AddRange(API.GetAliases(show));
            locations.Insert(0, finalLoc);
            if (!show.name.Equals(Path.GetFileName(finalLoc), StringComparison.InvariantCultureIgnoreCase)) {
                finalLoc += "\\" + show.name;
            }
            List<string> files = ScanEpisodes(locations, show);
            List<Episode> EPNames = API.RequestEpisodes(show);
            foreach (string file in files) {
                Tuple<int, int> t = GetInfo(file);
                if (t != null) { 
                    int season = t.Item1;
                    int episode = t.Item2;
                    Episode selectedEP = EPNames.FirstOrDefault(o => o.season == season && o.episode == episode);
                    int index = EPNames.FindIndex(o => o.season == season && o.episode == episode);
                    if (selectedEP == null) {
                    } else {
                        RenameFiles(file, finalLoc, show, selectedEP, index);
                    }
                }
            }
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

        public static string GetValidName(string path, string name, string extension, string original,int s) {
            int filenumber = 1;
            string final;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
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
            file = Path.GetFileName(file);
            Match season = new Regex("[s][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            Match episode = new Regex("[e][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            Match special = new Regex("[0-5][0-9][x][0-5][0-9]", RegexOptions.IgnoreCase).Match(file);
            if (season.Success && episode.Success && (episode.Index-season.Index) < 5) {
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
        public static long GetDirectorySize(string parentDirectory) {
            return new DirectoryInfo(parentDirectory).GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
        }
        public static void RenameFiles(string file, string path, Show show,Episode epi,int index) {          
            string output = GetValidName(path, GetName(show.name, epi.season, epi.episode, epi.name), Path.GetExtension(file), file,epi.season);
            if (file != output) {
                bool moved = false;
                do {
                    try {
                        File.Move(file, output);
                        DeleteFiles(file);
                        moved = true;
                     } catch (Exception e) {
                        DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("File " + file + " couldn't be renamed.\nClose apps that might be using it.\n\nTry again?", "Error", MessageBoxButtons.YesNo);
                        System.Windows.Forms.MessageBox.Show(e.Message);
                        if (dialogResult == DialogResult.No) {
                            moved = true;
                        }
                    }
                } while (moved != true);
            }                            
        }
        public static void DeleteFiles(string file) {
            string dirname = Path.GetDirectoryName(file);
            List<string> files = FilterExtensions(Directory.GetFiles(dirname, "*", SearchOption.AllDirectories).ToList());
            long size = GetDirectorySize(dirname);          
            if (Properties.Settings.Default.Delete && size < Properties.Settings.Default.maxSize*1000000) {
                if (files.Count == 0) { 
                    Directory.Delete(dirname,true);
                }
                //MessageBox.Show("Deleted");
            }
        }


    }*/
}
