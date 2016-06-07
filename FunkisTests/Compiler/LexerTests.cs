using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funkis.Compiler;

namespace FunkisTests.Compiler
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void LexHelloWorldTest()
        {
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var lexer = new Lexer(new Scanner(text));

            Token[] expectedTokens = {
                KeywordToken.Using,
                new IdentifierToken("System"),
                KeywordToken.Let,
                KeywordToken.Func,
                new IdentifierToken("hello_world"),
                SymbolToken.Colon,
                SymbolToken.Unit,
                SymbolToken.Assign,
                new IdentifierToken("Console"),
                SymbolToken.Dot,
                new IdentifierToken("WriteLine"),
                SymbolToken.Parenthesis,
                new StringToken("hello world!"),
                SymbolToken.ClosingParenthesis,
                KeywordToken.Let,
                SymbolToken.Unit,
                SymbolToken.Colon,
                SymbolToken.Unit,
                SymbolToken.Assign,
                new IdentifierToken("hello_world"),
                SymbolToken.Unit,
                SymbolToken.EOF
            };

            Position[] expectedPositions = {
                new Position(0, 0),
                new Position(6, 0),
                new Position(0, 2),
                new Position(4, 2),
                new Position(9, 2),
                new Position(21, 2),
                new Position(23, 2),
                new Position(26, 2),
                new Position(4, 3),
                new Position(11, 3),
                new Position(12, 3),
                new Position(22, 3),
                new Position(23, 3),
                new Position(37, 3),
                new Position(0, 5),
                new Position(4, 5),
                new Position(7, 5),
                new Position(9, 5),
                new Position(12, 5),
                new Position(14, 5),
                new Position(26, 5),
                new Position(-1, -1)
            };

            var tokenAndPositions = lexer.Tokenize();

            Assert.AreEqual(expectedTokens.Length, tokenAndPositions.Length);
            Assert.AreEqual(expectedTokens.Length, expectedPositions.Length);

            for (var i = 0; i < expectedTokens.Length; ++i)
            {
                var tokenAndPosition = tokenAndPositions[i];

                var expectedToken = expectedTokens[i];
                var actualToken = tokenAndPosition.Item1;

                Assert.AreEqual(expectedToken, actualToken);

                var expectedPosition = expectedPositions[i];
                var actualPosition = tokenAndPosition.Item2;

                Assert.AreEqual(expectedPosition, actualPosition);
            }
        }
    }
}
