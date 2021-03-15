using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tex0extract {
    public class Program {
        public const string USAGE = @"tex0extract
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: tex0extract.exe archive.pac texture_name output.[png/tex0]";

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
                var child = FindChildrenWithName(node, args[1]).Single();
                child.Export(args[2]);
            }

            return 0;
        }
    }
}
