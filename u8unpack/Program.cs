using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace u8unpack {
    public static class Program {
        public const string USAGE = @"u8unpack
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-u8tools

Built against BrawlLib.dll from BrawlCrate
https://github.com/soopercool101/BrawlCrate

Usage: u8unpack.exe [input.arc]";

        public static IEnumerable<char> InvalidCharacters =>
            Path.GetInvalidFileNameChars()
            .Concat(Path.GetInvalidPathChars())
            .Distinct();

        public static void Export(string output_directory, ResourceNode node) {
            var invalid_file_chars = InvalidCharacters.Intersect(node.Name).ToArray();
            if (invalid_file_chars.Any())
                throw new Exception($"The node name {node.Name} contains invalid character(s): {new string(invalid_file_chars)}");

            string output_path = Path.Combine(output_directory, node.Name);
            Console.WriteLine(output_path);

            if (node is U8FolderNode) {
                foreach (var child in node.Children) {
                    Directory.CreateDirectory(output_path);
                    Export(output_path, child);
                }
            } else {
                if (File.Exists(output_path))
                    throw new Exception($"The file {output_path} already exists.");

                node.Export(output_path);
            }
        }

        public static void Unpack(string output_directory, ResourceNode node) {
            foreach (var c in node.Children) {
                Export(output_directory, c);
            }
        }

        public static void Unpack(string output_directory, string archive_file) {
            using (var node = NodeFactory.FromFile(null, archive_file)) {
                Unpack(output_directory, node);
            }
        }

        public static void Main(string[] args) {
            if (args.Length != 1 || args[0] == "--help" || args[0] == "/?") {
                Console.Error.WriteLine(USAGE);
                return;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File not found: {args[0]}");
                return;
            }

            Unpack(Environment.CurrentDirectory, args[0]);
        }
    }
}
