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

        [TestMethod]
        [DeploymentItem(@"TestFiles\if-else-if-else.js")]
        public void ParseIfElseIfElseTest()
        {
            var expectedAST = new ProgramNode(
                new StatementNode[] {
                    new VarStatementNode(
                        "a",
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    1,
                                    new Position(8, 0)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(8, 0)
                            ),
                            new Position(8, 0)
                        ),
                        new Position(0, 0)
                    ),
                    new VarStatementNode(
                        "b",
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    1,
                                    new Position(8, 1)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(8, 1)
                            ),
                            new Position(8, 1)
                        ),
                        new Position(0, 1)
                    ),
                    new IfStatementNode(
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    1,
                                    new Position(4, 3)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(4, 3)
                            ),
                            new Position(4, 3)
                        ),
                        new StatementNode[] {
                            new VarStatementNode(
                                "c",
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            1,
                                            new Position(12, 4)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(12, 4)
                                    ),
                                    new Position(12, 4)
                                ),
                                new Position(4, 4)
                            )
                        },
                        new Tuple<ExpressionNode, StatementNode[]>[] {
                            new Tuple<ExpressionNode, StatementNode[]>(
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            2,
                                            new Position(11, 5)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(11, 5)
                                    ),
                                    new Position(11, 5)
                                ),
                                new StatementNode[] {
                                    new VarStatementNode(
                                        "d",
                                        new TermExpressionNode(
                                            unchecked ((char) -1),
                                            new TermNode(
                                                new NumberFactorNode(
                                                    1,
                                                    new Position(12, 6)
                                                ),
                                                new string[] { },
                                                new FactorNode[] { },
                                                new Position(12, 6)
                                            ),
                                            new Position(12, 6)
                                        ),
                                        new Position(4, 6)
                                    )
                                }
                            ),
                            new Tuple<ExpressionNode, StatementNode[]>(
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            3,
                                            new Position(11, 6)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(11, 6)
                                    ),
                                    new Position(11, 6)
                                ),
                                new StatementNode[] {
                                    new VarStatementNode(
                                        "e",
                                        new TermExpressionNode(
                                            unchecked ((char) -1),
                                            new TermNode(
                                                new NumberFactorNode(
                                                    1,
                                                    new Position(12, 8)
                                                ),
                                                new string[] { },
                                                new FactorNode[] { },
                                                new Position(12, 8)
                                            ),
                                            new Position(12, 8)
                                        ),
                                        new Position(4, 8)
                                    )
                                }
                            ),
                        },
                        new StatementNode[] {
                            new VarStatementNode(
                                "f",
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            1,
                                            new Position(12, 10)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(12, 10)
                                    ),
                                    new Position(12, 10)
                                ),
                                new Position(4, 10)
                            )
                        },
                        new Position(0, 3)
                    ),
                    new IfStatementNode(
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    1,
                                    new Position(4, 13)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(4, 13)
                            ),
                            new Position(4, 13)
                        ),
                        new StatementNode[] {
                            new OperatorEqualStatementNode(
                                new PropertyNode(
                                    new string[] { "a" },
                                    new Position(7, 13)
                                ),
                                '+',
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            1,
                                            new Position(12, 13)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(12, 13)
                                    ),
                                    new Position(12, 13)
                                ),
                                new Position(7, 13)
                            )
                        },
                        new Tuple<ExpressionNode, StatementNode[]>[] {
                            new Tuple<ExpressionNode, StatementNode[]>(
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            2,
                                            new Position(9, 14)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(9, 14)
                                    ),
                                    new Position(9, 14)
                                ),
                                new StatementNode[] {
                                    new OperatorEqualStatementNode(
                                        new PropertyNode(
                                            new string[] { "a" },
                                            new Position(12, 13)
                                        ),
                                        '+',
                                        new TermExpressionNode(
                                            unchecked ((char) -1),
                                            new TermNode(
                                                new NumberFactorNode(
                                                    2,
                                                    new Position(17, 13)
                                                ),
                                                new string[] { },
                                                new FactorNode[] { },
                                                new Position(17, 13)
                                            ),
                                            new Position(17, 13)
                                        ),
                                        new Position(12, 13)
                                    )
                                }
                            )
                        },
                        new StatementNode[] { },
                        new Position(0, 14)
                    ),
                    new AssignmentStatementNode(
                        new PropertyNode(
                            new string[] { "a" },
                            new Position(0, 16)
                        ),
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    5,
                                    new Position(4, 16)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(4, 16)
                            ),
                            new Position(4, 16)
                        ),
                        new Position(0, 16)
                    )
                },
                new Position(0, 0)
            );
            var text = TestFilesHelper.ReadIfElseIfElseTestFile();

            var parser = new Parser(new Scanner(text));

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
