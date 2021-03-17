using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace wteconvert {
    public static class Program {
        public const string USAGE = @"wteconvert
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: wteconvert.exe file.[wte|tex0] out.[wte|tex0]";

        public unsafe static TEX0v1 ReadTEX0(this Stream inputStream) {
            byte[] buffer = new byte[sizeof(TEX0v1)];
            inputStream.Read(buffer, 0, buffer.Length);
            fixed (byte* ptr = buffer) {
                return *(TEX0v1*)ptr;
            }
        }

        public unsafe static void WriteTEX0(this Stream outputStream, TEX0v1 header) {
            TEX0v1* headerPtr = &header;
            using (var inputStream = new UnmanagedMemoryStream((byte*)headerPtr, sizeof(TEX0v1))) {
                inputStream.CopyTo(outputStream);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct WTE {
            public BinTag tag;
            public short width;
            public short height;
            private fixed int padding[6];

            public WTE(short width, short height) {
                this.tag = "WTE\0";
                this.width = width;
                this.height = height;
            }
        }

        public unsafe static WTE ReadWTE(this Stream inputStream) {
            byte[] buffer = new byte[sizeof(WTE)];
            inputStream.Read(buffer, 0, buffer.Length);
            fixed (byte* ptr = buffer) {
                return *(WTE*)ptr;
            }
        }

        public unsafe static void WriteWTE(this Stream outputStream, WTE header) {
            WTE* headerPtr = &header;
            using (var inputStream = new UnmanagedMemoryStream((byte*)headerPtr, sizeof(WTE))) {
                inputStream.CopyTo(outputStream);
            }
        }

        public static byte[] WTEToTEX0(Stream inputStream) {
            WTE oldHeader = inputStream.ReadWTE();
            if (oldHeader.tag != "WTE\0")
                throw new Exception("The input file does not appear to be a WTE file.");

            TEX0v1 newHeader = new TEX0v1(oldHeader.width, oldHeader.height, WiiPixelFormat.RGB5A3, 1);

            using (var outputStream = new MemoryStream()) {
                outputStream.WriteTEX0(newHeader);
                inputStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        public static byte[] TEX0ToWTE(Stream inputStream) {
            TEX0v1 oldHeader = inputStream.ReadTEX0();

            if (oldHeader._header._tag != "TEX0")
                throw new Exception("The input file does not appear to be a TEX0 file.");
            if (oldHeader.PixelFormat != WiiPixelFormat.RGB5A3)
                throw new Exception("The format of the TEX0 file is not RGB5A3.");

            WTE newHeader = new WTE(oldHeader._width, oldHeader._height);

            using (var outputStream = new MemoryStream()) {
                outputStream.WriteWTE(newHeader);
                inputStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        public unsafe static void ReplaceRaw(this ResourceNode node, byte[] data) {
            fixed (byte* ptr = data) {
                node.ReplaceRaw(ptr, data.Length);
            }
        }

        public static void ImportFromWTE(this ResourceNode node, string path) {
            using (var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                byte[] data = WTEToTEX0(inputStream);
                node.ReplaceRaw(data);
            }
        }

        public unsafe static void ExportToWTE(this ResourceNode node, string path) {
            using (var stream = new UnmanagedMemoryStream((byte*)node.WorkingUncompressed.Address, node.WorkingUncompressed.Length)) {
                byte[] data = TEX0ToWTE(stream);
                File.WriteAllBytes(path, data);
            }
        }

        public unsafe static int Main(string[] args) {
            if (args.Length != 2) {
                Console.Error.WriteLine(USAGE);
                return 1;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File not found: {args[0]}");
                return 1;
            }

            string tempFile1 = Path.GetTempFileName();
            using (var fs = new FileStream(tempFile1, FileMode.Create, FileAccess.Write)) {
                fs.WriteTEX0(new TEX0v1(0, 0, WiiPixelFormat.RGB5A3, 1));
            }

            using (var node = NodeFactory.FromFile(null, tempFile1)) {
                if (Path.GetExtension(args[0]).ToLowerInvariant() == ".wte") {
                    node.ImportFromWTE(args[0]);
                } else {
                    node.Replace(args[0]);
                }

                if (Path.GetExtension(args[1]).ToLowerInvariant() == ".wte") {
                    node.ExportToWTE(args[1]);
                } else {
                    node.Export(args[1]);
                }
            }

            File.Delete(tempFile1);

            return 0;
        }
    }
}
