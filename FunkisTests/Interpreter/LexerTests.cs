using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronJS.Interpreter;
using System;

namespace IronJSTests
{
    [TestClass]
    public class LexerTests
    {
        /*[TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.js")]
        public void TokenizeHelloWorldTest()
        {
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var lexer = new Lexer(new Scanner(text));

            Token[] expectedTokens = {
                KeywordToken.Function,
                new IdentifierToken("hello_world"),
                SymbolToken.Parenthesis,
                SymbolToken.ClosingParenthesis,
                SymbolToken.Bracket,
                new IdentifierToken("console"),
                SymbolToken.Dot,
                new IdentifierToken("log"),
                SymbolToken.Parenthesis,
                new StringToken("hello world!"),
                SymbolToken.ClosingParenthesis,
                SymbolToken.SemiColon,
                SymbolToken.ClosingBracket,
                new IdentifierToken("hello_world"),
                SymbolToken.Parenthesis,
                SymbolToken.ClosingParenthesis,
                SymbolToken.SemiColon,
                SymbolToken.EOF
            };

            Position[] expectedPositions = {
                new Position(0, 0),
                new Position(9, 0),
                new Position(20, 0),
                new Position(21, 0),
                new Position(23, 0),
                new Position(4, 1),
                new Position(11, 1),
                new Position(12, 1),
                new Position(15, 1),
                new Position(16, 1),
                new Position(30, 1),
                new Position(31, 1),
                new Position(0, 2),
                new Position(0, 4),
                new Position(11, 4),
                new Position(12, 4),
                new Position(13, 4),
                new Position(-1, -1)
            };

            var tokenAndPositions = lexer.Tokenize();

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

            Assert.AreEqual(expectedTokens.Length, tokenAndPositions.Length);
            Assert.AreEqual(expectedTokens.Length, expectedPositions.Length);
        }*/
    }
}
