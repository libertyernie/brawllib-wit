using System;
using System.IO;

namespace openingextract {
    class Program {
        public const string USAGE = @"openingextract
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: openingextract.exe 000000000.app output.pac";

        public static int Main(string[] args) {
            if (args.Length != 2) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            using (var fs1 = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            using (var fs2 = new FileStream(args[1], FileMode.Create, FileAccess.Write)) {
                fs1.Position = 0x640;
                fs1.CopyTo(fs2);
            }

            return 0;
        }
    }
}
