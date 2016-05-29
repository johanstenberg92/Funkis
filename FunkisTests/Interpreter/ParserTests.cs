using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronJS.Interpreter;

namespace IronJSTests.Interpreter
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void ParseHelloWorldTest()
        {
            var expectedAST = new ProgramNode(
                new DeclarationNode[] {
                    new FuncDeclarationNode(
                        "hello_world",
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        "println",
                                        new Position(4, 1)
                                    ),
                                    new TermExpressionNode(
                                        new TermNode(
                                            new StringLiteralNode(
                                                "hello world!",
                                                new Position(13, 1)
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        new Position(0, 0)
                    ),
                    new LetDeclarationNode(
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        "hello_world",
                                        new Position(9, 3)
                                    ),
                                    new TermExpressionNode(
                                        new TermNode(
                                            new UnitLiteralNode(
                                                new Position(21, 3)
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        new Position(0, 3)
                    )
                }
            );

            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var parser = new Parser(text);

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }

        [TestMethod]
        [DeploymentItem(@"TestFiles\comments.funk")]
        public void ParseCommentsTest()
        {
            var expectedAST = new ProgramNode(
                new DeclarationNode[] {
                    new LetDeclarationNode(
                        "a",
                        new TermExpressionNode(
                            '-',
                            new TermNode(
                                new IntLiteralNode(
                                    5,
                                    new Position(33, 1)
                                )
                            ),
                            new Position(32, 1)
                        ),
                        new Position(0, 1)
                    ),
                    new LetDeclarationNode(
                        "b",
                        new TermExpressionNode(
                            '+',
                            new TermNode(
                                new IntLiteralNode(
                                    5,
                                    new Position(9, 10)
                                )
                            ),
                            new Position(8, 10)
                        ),
                        new Position(0, 1)
                    )
                }
            );

            var text = TestFilesHelper.ReadCommentsTestFile();

            var parser = new Parser(text);

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
