using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nodeextract {
    public class Program {
        public const string USAGE = @"nodeextract
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: tex0extract.exe archive_file node_name output_file.png";

        public static IEnumerable<ResourceNode> FindChildrenWithName(ResourceNode parent, string name) {
            if (parent.Name == name && parent is TEX0Node)
                yield return parent;
            foreach (var c in parent.Children) {
                var list = FindChildrenWithName(c, name);
                foreach (var n in list)
                    yield return n;
            }
        }

        public static int Main(string[] args) {
            if (args.Length != 3) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            using (var node = NodeFactory.FromFile(null, args[0])) {
                var children = FindChildrenWithName(node, args[1]).ToList();
                switch (children.Count) {
                    case 0:
                        Console.Error.WriteLine($"No nodes found with name {args[1]}.");
                        return 1;
                    case 1:
                        children.Single().Export(args[2]);
                        return 0;
                    default:
                        Console.Error.WriteLine($"{children.Count} nodes found with name {args[1]}. Use BrawlCrate to replace the texture manually.");
                        return 1;
                }
            }
        }
    }
}
