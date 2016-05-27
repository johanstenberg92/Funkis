using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronJS.Interpreter;
using System;

namespace IronJSTests
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
                KeywordToken.Let,
                KeywordToken.Func,
                new IdentifierToken("hello_world"),
                SymbolToken.Assign,
                new IdentifierToken("println"),
                SymbolToken.Parenthesis,
                new StringToken("hello world!"),
                SymbolToken.ClosingParenthesis,
                KeywordToken.Let,
                SymbolToken.Unit,
                SymbolToken.Assign,
                new IdentifierToken("hello_world"),
                SymbolToken.Unit,
                SymbolToken.EOF
            };

            Position[] expectedPositions = {
                new Position(0, 0),
                new Position(4, 0),
                new Position(9, 0),
                new Position(21, 0),
                new Position(4, 1),
                new Position(12, 1),
                new Position(13, 1),
                new Position(27, 1),
                new Position(0, 3),
                new Position(4, 3),
                new Position(7, 3),
                new Position(9, 3),
                new Position(21, 3),
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
        }
    }
}
