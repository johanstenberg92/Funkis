using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funkis.Compiler;

namespace FunkisTests.Compiler
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void ScanHelloWorldTest()
        {
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var scanner = new Scanner(text);

            char[] characters = {
                'u', 's', 'i', 'n', 'g', ' ', 'S', 'y', 's', 't', 'e', 'm', '\n',
                '\n',
                'l', 'e', 't', ' ', 'f', 'u', 'n', 'c', ' ', 'h',
                'e', 'l', 'l', 'o', '_', 'w', 'o', 'r', 'l', 'd',
                ' ', ':', ' ', '(', ')', ' ', '=', '\n', ' ',
                ' ', ' ', ' ', 'C', 'o', 'n', 's', 'o', 'l', 'e',
                '.', 'W', 'r', 'i', 't', 'e', 'L', 'i', 'n', 'e',
                ' ', '(', '"', 'h', 'e', 'l', 'l', 'o', ' ', 'w',
                'o', 'r', 'l', 'd', '!', '"', ')', '\n', '\n',
                'l', 'e', 't', ' ', '(', ')', ' ', ':', ' ', '(',
                ')', ' ', '=', ' ', 'h', 'e', 'l', 'l', 'o', '_',
                'w', 'o', 'r', 'l', 'd', ' ', '(', ')',
                unchecked ((char) -1)
            };

            int[] columns = {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                0,
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
                27,
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36,
                37, 38,
                0,
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
                -1
            };

            int[] rows = {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                1,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                3, 3, 3, 3, 3,
                4,
                5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
                5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
                -1
            };

            Assert.AreEqual(characters.Length, columns.Length);
            Assert.AreEqual(rows.Length, columns.Length);

            for (var i = 0; i < characters.Length; ++i)
            {
                var character = characters[i];

                var scannerPeek = scanner.Peek();
                Assert.AreEqual(character, scannerPeek);

                var position = scanner.PeekPosition();
                
                int column = columns[i];
                int scannerColumn = position.Column;
                Assert.AreEqual(column, scannerColumn);

                int row = rows[i];
                int scannerRow = position.Row;
                Assert.AreEqual(row, scannerRow);

                var scannerRead = scanner.Read();
                Assert.AreEqual(character, scannerRead);
            }

            Assert.AreEqual(Scanner._eof, scanner.Peek());
            Assert.AreEqual(Scanner._eof, scanner.Read());
        }
    }
}
