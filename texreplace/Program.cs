using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace texreplace {
    public class Program {
        public const string USAGE = @"texreplace
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: texreplace.exe file.[tpl|tex0] replacement_file.png";

        public static IEnumerable<ResourceNode> ListTextures(ResourceNode parent) {
            if (parent is TPLTextureNode || parent is TEX0Node)
                yield return parent;
            foreach (var child in parent.Children.SelectMany(ListTextures))
                yield return child;
        }

        public static int Main(string[] args) {
            Application.EnableVisualStyles();

            if (args.Length != 2) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            using (var node = NodeFactory.FromFile(null, args[0])) {
                var texture = ListTextures(node).Single();
                texture.Replace(args[1]);
                texture.Export(args[0]);
            }

            return 0;
        }
    }
}
