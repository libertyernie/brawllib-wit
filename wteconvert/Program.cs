using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System;
using System.IO;
using System.Text;

namespace wteconvert {
    public class Program {
        public const string USAGE = @"wteconvert
(C) 2021 libertyernie
https://github.com/libertyernie/brawllib-wit

Built against BrawlLib.dll from BrawlCrate 0.36b
https://github.com/soopercool101/BrawlCrate

Usage: wteconvert.exe file.[wte|tex0] out.[wte|tex0]";

        public unsafe static byte[] ToByteArray(TEX0v1 header) {
            byte[] array = new byte[sizeof(TEX0v1)];
            fixed (byte* ptr = array) {
                TEX0v1* destPtr = (TEX0v1*)ptr;
                *destPtr = header;
            }
            return array;
        }

        public unsafe static TEX0v1 ToTEX0v1(byte[] headerData) {
            fixed (byte* ptr = headerData) {
                TEX0v1* srcPtr = (TEX0v1*)ptr;
                return *srcPtr;
            }
        }

        public static byte[] WTEToTEX0(Stream inputStream) {
            short width, height;

            using (var br = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true)) {
                int tag = br.ReadInt32();
                if (tag != 0x00455457)
                    throw new Exception("This file does not appear to be a valid .wte file.");

                width = br.ReadInt16();
                height = br.ReadInt16();
            }

            inputStream.Seek(0x18, SeekOrigin.Current);

            TEX0v1 header = new TEX0v1(width, height, WiiPixelFormat.RGB5A3, 1);
            byte[] headerData = ToByteArray(header);

            byte[] outputData = new byte[headerData.Length + width * height * 2];
            using (var outputStream = new MemoryStream(outputData))
            using (var headerInputStream = new MemoryStream(headerData, writable: false)) {
                outputStream.Write(headerData, 0, headerData.Length);
                inputStream.CopyTo(outputStream);
            }

            return outputData;
        }

        public unsafe static byte[] TEX0ToWTE(Stream inputStream) {
            byte[] headerData = new byte[TEX0v1.Size];
            inputStream.Read(headerData, 0, headerData.Length);
            TEX0v1 header = ToTEX0v1(headerData);

            short width = header._width;
            short height = header._height;
            byte[] outputData = new byte[0x20 + width * height * 2];
            using (var outputStream = new MemoryStream(outputData)) {
                using (var bw = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true)) {
                    bw.Write(0x00455457);
                    bw.Write(width);
                    bw.Write(height);
                }

                for (int i = 0; i < 0x18; i++)
                    outputStream.WriteByte(0);

                inputStream.CopyTo(outputStream);
            }

            return outputData;
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

            using (TEX0Node node = new TEX0Node()) {
                TEX0v1 header = new TEX0v1(0, 0, WiiPixelFormat.RGB5A3, 1);
                byte[] headerData = ToByteArray(header);
                fixed (byte* headerPtr = &headerData[0]) {
                    node.ReplaceRaw(headerPtr, headerData.Length);
                }

                if (Path.GetExtension(args[0]).ToLowerInvariant() == ".wte") {
                    using (var fs = new FileStream(args[0], FileMode.Open, FileAccess.Read)) {
                        byte[] data = WTEToTEX0(fs);
                        fixed (byte* ptr = &data[0]) {
                            node.ReplaceRaw(ptr, data.Length);
                        }
                    }
                } else {
                    node.Replace(args[0]);
                }

                if (Path.GetExtension(args[1]).ToLowerInvariant() == ".wte") {
                    using (var stream = new UnmanagedMemoryStream((byte*)node.WorkingUncompressed.Address, node.WorkingUncompressed.Length)) {
                        byte[] data = TEX0ToWTE(stream);
                        File.WriteAllBytes(args[1], data);
                    }
                } else {
                    node.Export(args[1]);
                }
            }

            return 0;
        }
    }
}
