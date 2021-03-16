using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xmlyttext {
    public class Program {
        public const string USAGE = @"xmlyttextreplace
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: xmlyttext.exe [--read|--replace-line|--replace-text] input_file tag_name < new_text.txt > output_file";

        public static byte[] ParseHexString(string str) {
            byte[] arr = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2) {
                arr[i / 2] = byte.Parse(str.Substring(i, 2), NumberStyles.HexNumber);
            }
            return arr;
        }

        public static string ParseBigEndianNullTerminatedUTF16(byte[] arr) {
            string text = Encoding.BigEndianUnicode.GetString(arr);
            if (!text.EndsWith("\0"))
                Console.Error.WriteLine("Warning: string is not null terminated!");
            else
                text = text.Substring(0, text.Length - 1);
            return text;
        }

        public static byte[] ToBigEndianNullTerminatedUTF16(string text) {
            if (!text.EndsWith("\0"))
                text += '\0';
            return Encoding.BigEndianUnicode.GetBytes(text);
        }

        public static string ToHexString(byte[] arr) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arr) {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static int Main(string[] args) {
            if (args.Length != 3) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            if (!File.Exists(args[1])) {
                Console.Error.WriteLine($"File not found: {args[1]}");
                return 1;
            }

            XElement root = XElement.Load(args[1]);
            var node = root.Descendants("tag")
                .Where(x => x.Attribute(XName.Get("type")).Value == "txt1")
                .Where(x => x.Attribute(XName.Get("name")).Value == args[2])
                .Single()
                .Descendants("text")
                .Single();

            if (args[0] == "--read") {
                string text = ParseBigEndianNullTerminatedUTF16(ParseHexString(node.Value));
                Console.WriteLine(text);
                return 0;
            }

            string newText;

            switch (args[0]) {
                case "--replace-line":
                    newText = Console.ReadLine();
                    break;
                case "--replace-text":
                    using (var stdin = Console.OpenStandardInput())
                    using (var sr = new StreamReader(stdin)) {
                        newText = sr.ReadToEnd();
                    }
                    break;
                default:
                    Console.Error.WriteLine($"Invalid argument: {args[0]}");
                    Console.Error.WriteLine($"Use one of the following: --read, --replace-line, --replace-text");
                    return 1;
            }

            node.Value = ToHexString(ToBigEndianNullTerminatedUTF16(newText));

            root.Save(Console.Out);

            return 0;
        }
    }
}
