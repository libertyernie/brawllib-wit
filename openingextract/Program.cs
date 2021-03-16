using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Compression;
using System;
using System.IO;
using System.Linq;

namespace openingextract {
    class Program {
        public const string USAGE = @"openingextract
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: openingextract.exe 000000000.app [banner|icon|sound] output.arc";

        public unsafe static int Main(string[] args) {
            if (args.Length != 3) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            if (!new[] { "banner", "icon", "sound" }.Contains(args[1])) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            string parent_archive = Path.GetTempFileName();

            try {
                using (var fs1 = new FileStream(args[0], FileMode.Open, FileAccess.Read))
                using (var fs2 = new FileStream(parent_archive, FileMode.Create, FileAccess.Write)) {
                    fs1.Position = 0x640;
                    fs1.CopyTo(fs2);
                }

                using (var parent_node = NodeFactory.FromFile(null, parent_archive)) {
                    var child_node = parent_node.FindChild($"meta/{args[1]}.bin", false);

                    CompressionHeader* header = (CompressionHeader*)(child_node.WorkingSource.Address + 0x24);
                    byte[] buffer = new byte[checked((int)header->ExpandedSize)];
                    fixed (byte* ptr = buffer) {
                        LZ77.Expand(header, ptr, buffer.Length);
                    }

                    File.WriteAllBytes(args[2], buffer);
                }

                return 0;
            } finally {
                if (File.Exists(parent_archive))
                    File.Delete(parent_archive);
            }
        }
    }
}
