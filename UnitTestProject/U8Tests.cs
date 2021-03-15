using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestProject {
    [TestClass]
    public class U8Tests {
        [TestMethod]
        public void PackAndUnpack() {
            var tempDir1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempDir2 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDir1);
            File.WriteAllText(Path.Combine(tempDir1, "aaaaaaaa.txt"), "Parent file 1");

            Directory.CreateDirectory(Path.Combine(tempDir1, "sample1"));
            File.WriteAllText(Path.Combine(tempDir1, "sample1", "demo.txt"), "File in subdirectory");

            File.WriteAllText(Path.Combine(tempDir1, "zzzzzzzz.txt"), "Parent file 2");

            var tempFile = Path.GetTempFileName();
            u8pack.Program.Build(tempDir1, tempFile);
            u8unpack.Program.Unpack(tempDir2, tempFile);
            File.Delete(tempFile);

            Assert.AreEqual("Parent file 1", File.ReadAllText(Path.Combine(tempDir1, "aaaaaaaa.txt")));
            Assert.AreEqual("Parent file 2", File.ReadAllText(Path.Combine(tempDir1, "zzzzzzzz.txt")));
            Assert.AreEqual("File in subdirectory", File.ReadAllText(Path.Combine(tempDir1, "sample1", "demo.txt")));

            Directory.Delete(tempDir1, true);
            Directory.Delete(tempDir2, true);
        }
    }
}
