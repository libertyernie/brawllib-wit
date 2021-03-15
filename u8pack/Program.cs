using BrawlLib.SSBB.ResourceNodes;
using System;
using System.IO;

namespace u8pack {
    public class Program {
        public const string USAGE = @"u8unpack
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: u8pack.exe output.arc";

        public static void Pack(string path, U8EntryNode parent) {
            foreach (var file in Directory.EnumerateFiles(path)) {
                Console.WriteLine(file);

                var node = new ARCEntryNode {
                    Name = Path.GetFileName(file)
                };
                parent.AddChild(node);

                byte[] data = File.ReadAllBytes(file);
                using (var ms = new MemoryStream(data)) {
                    node.Initialize(parent, new DataSource(ms));
                }
            }
            foreach (var directory in Directory.EnumerateDirectories(path)) {
                Console.WriteLine(directory);

                var node = new U8FolderNode {
                    Name = Path.GetFileName(directory)
                };
                parent.AddChild(node);
                Pack(directory, node);
            }
        }

        public static U8Node Build(string input_directory) {
            var node = new U8Node();
            Pack(input_directory, node);
            return node;
        }

        public static void Build(string input_directory, string output_file) {
            using (var node = Build(input_directory)) {
                node.Export(output_file);
            }
        }

        public static int Main(string[] args) {
            if (args.Length != 1 || args[0] == "--help" || args[0] == "/?") {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            if (File.Exists(args[0])) {
                Console.Error.WriteLine($"File already exists: {args[0]}");
                return 1;
            }

            Build(Environment.CurrentDirectory, args[0]);
            return 0;
        }
    }
}
