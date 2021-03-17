using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;

namespace texextract {
    public class Program {
        public const string USAGE = @"texextract
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: texextract.exe archive_file node_name output_file.png";

        public static IEnumerable<ResourceNode> FindChildrenWithName(ResourceNode parent, string name) {
            if (parent.Name == name && (parent is TEX0Node || parent is TPLNode))
                yield return parent;
            foreach (var c in parent.Children) {
                var list = FindChildrenWithName(c, name);
                foreach (var n in list)
                    yield return n;
            }
        }

        public static int Main(string[] args) {
            if (args.Length != 2) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            using (var node = NodeFactory.FromFile(null, args[0])) {
                node.Export(args[1]);
            }

            return 0;
        }
    }
}
