using System;
using System.IO;
using System.Text;

namespace bannercfgfromtxt {
    public class Program {
        public const string USAGE = @"u8unpack
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: bannercfgfromtxt.exe path/to/banner.cfg.txt";

        public static int Main(string[] args) {
            if (args.Length != 1 || args[0] == "--help" || args[0] == "/?") {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            string[] lines = File.ReadAllLines(args[0]);

            using (var fs = new FileStream(args[0], FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs, Encoding.BigEndianUnicode)) {
                foreach (string line in lines) {
                    int index = line.IndexOf(":");
                    if (index > 0) {
                        string key = line.Substring(0, index);
                        if (File.Exists($"{key}.txt")) {
                            string newValue = File.ReadAllText($"{key}.txt").Trim();
                            sw.WriteLine($"{key}:{newValue}");
                            continue;
                        }
                    }

                    sw.WriteLine(line);
                }
            }

            return 0;
        }
    }
}
