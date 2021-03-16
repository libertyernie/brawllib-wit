using System;
using System.IO;
using System.Text;

namespace openingimet {
    class Program {
        public const string USAGE = @"openingimet
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: openingimet.exe 000000000.app [ja|en|de|fr|es|it|nl|07|08|ko]";

        public static int Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length != 2) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            int lang = int.TryParse(args[1], out int ix) ? ix
                : args[1] == "ja" ? 0
                : args[1] == "en" ? 1
                : args[1] == "de" ? 2
                : args[1] == "fr" ? 3
                : args[1] == "es" ? 4
                : args[1] == "it" ? 5
                : args[1] == "nl" ? 6
                : args[1] == "ko" ? 9
                : -1;
            if (lang == -1) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            using (var fs = new FileStream(args[0], FileMode.Open, FileAccess.Read)) {
                fs.Position = 0x40 + 0x5c + 84 * lang;
                using (var br = new BinaryReader(fs, Encoding.BigEndianUnicode)) {
                    for (int i = 0; i < 42; i++) {
                        char c = br.ReadChar();
                        if (c == 0)
                            break;

                        Console.Write(c);
                    }
                }
            }

            Console.WriteLine();
            return 0;
        }
    }
}
