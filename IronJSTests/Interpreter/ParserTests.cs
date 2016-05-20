using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronJS.Interpreter;

namespace IronJSTests.Interpreter
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.js")]
        public void ParseHelloWorldTest()
        {
            var expectedAST = new ProgramNode(
                new StatementNode[] {
                    new FunctionStatementNode(
                        "hello_world",
                        new string[] { },
                        new StatementNode[] {
                            new FunctionCallStatementNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        new string[] { "console", "log" },
                                        new Position(4, 1)
                                    ),
                                    new ExpressionNode[] {
                                        new TermExpressionNode(
                                            unchecked ((char) -1),
                                            new TermNode(
                                                new StringFactorNode(
                                                    "hello world!",
                                                    new Position(16, 1)
                                                ),
                                                new string[] { },
                                                new FactorNode[] { },
                                                new Position(16, 1)
                                            ),
                                            new Position(16, 1)
                                        )
                                    },
                                    new Position(4, 1)
                                )
                            )
                        },
                        null,
                        new Position(0 ,0)
                    ),
                    new FunctionCallStatementNode(
                        new FunctionCallNode(
                            new PropertyNode(
                                new string[] { "hello_world" },
                                new Position(0, 4)
                            ),
                            new ExpressionNode[] { },
                            new Position(0, 4)
                        )
                    )
                },
                new Position(0, 0)
            );
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var parser = new Parser(new Scanner(text));

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
