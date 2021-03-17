using System;
using System.Drawing;
using System.IO;

namespace imgoverlay {
    public static class Program {
        public const string USAGE = @"imgoverlay
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Usage: imgoverlay.exe larger.png smaller.png x y width height output.png";

        public static int Main(string[] args) {
            if (args.Length != 7) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File not found: {args[0]}");
                return 1;
            }

            if (!File.Exists(args[1])) {
                Console.Error.WriteLine($"File not found: {args[1]}");
                return 1;
            }

            if (!float.TryParse(args[2], out float x)) {
                Console.Error.WriteLine("Could not parse x");
                return 1;
            }

            if (!float.TryParse(args[3], out float y)) {
                Console.Error.WriteLine("Could not parse y");
                return 1;
            }

            if (!float.TryParse(args[4], out float width)) {
                Console.Error.WriteLine("Could not parse width");
                return 1;
            }

            if (!float.TryParse(args[5], out float height)) {
                Console.Error.WriteLine("Could not parse height");
                return 1;
            }

            using (var large = Image.FromFile(args[0]))
            using (var small = Image.FromFile(args[1]))
            using (var canvas = new Bitmap(large))
            using (var g = Graphics.FromImage(canvas)) {
                g.DrawImage(small, x, y, width, height);
                canvas.Save(args[6]);
            }

            return 0;
        }
    }
}
