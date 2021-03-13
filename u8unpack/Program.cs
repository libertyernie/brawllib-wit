using System;
using System.IO;

namespace u8unpack {
    public class Program {
        public const string USAGE = @"u8unpack
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-u8tools

Built against BrawlLib.dll from BrawlCrate
https://github.com/soopercool101/BrawlCrate

Usage: u8unpack.exe [file.arc]";

        static void Main(string[] args) {
            if (args.Length != 1 || args[0] == "--help" || args[0] == "/?") {
                Console.Error.WriteLine(USAGE);
                return;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File not found: {args[0]}");
                return;
            }

            throw new NotImplementedException();
        }
    }
}
