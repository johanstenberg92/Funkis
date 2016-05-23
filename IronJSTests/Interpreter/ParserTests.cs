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
                                    0,
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
                        new ExpressionNode[] {
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
                            )
                        },
                        new StatementNode[][] {
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
                            },
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
                        new ExpressionNode[] {
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
                        },
                        new StatementNode[][] {
                            new StatementNode[] {
                                new OperatorEqualStatementNode(
                                    new PropertyNode(
                                        new string[] { "a" },
                                        new Position(12, 14)
                                    ),
                                    '+',
                                    new TermExpressionNode(
                                        unchecked ((char) -1),
                                        new TermNode(
                                            new NumberFactorNode(
                                                2,
                                                new Position(17, 14)
                                            ),
                                            new string[] { },
                                            new FactorNode[] { },
                                            new Position(17, 14)
                                        ),
                                        new Position(17, 14)
                                    ),
                                    new Position(12, 14)
                                )
                            }
                        },
                        new StatementNode[] { },
                        new Position(0, 13)
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

        [TestMethod]
        [DeploymentItem(@"TestFiles\while.js")]
        public void ParseWhileTest()
        {
            var expectedAST = new ProgramNode(
                new StatementNode[] {
                    new VarStatementNode(
                        "a",
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new NumberFactorNode(
                                    5,
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
                    new WhileStatementNode(
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new PropertyFactorNode(
                                    new PropertyNode(
                                        new string[] { "a" },
                                        new Position(7, 2)
                                    ),
                                    new Position(7, 2)
                                ),
                                new string[] { "!=" },
                                new FactorNode[] {
                                    new NumberFactorNode(
                                        6,
                                        new Position(12, 2)
                                    )
                                },
                                new Position(7, 2)
                            ),
                            new Position(7, 2)
                        ),
                        new StatementNode[] {
                            new FunctionCallStatementNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        new string[] { "who", "is", "john", "galt" },
                                        new Position(4, 3)
                                    ),
                                    new ExpressionNode[] { },
                                    new Position(4, 3)
                                )
                            )
                        },
                        new Position(0, 2)
                    ),
                    new WhileStatementNode(
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new PropertyFactorNode(
                                    new PropertyNode(
                                        new string[] { "a" },
                                        new Position(7, 6)
                                    ),
                                    new Position(7, 6)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(7, 6)
                            ),
                            new Position(7, 6)
                        ),
                        new StatementNode[] {
                            new OperatorEqualStatementNode(
                                new PropertyNode(
                                    new string[] { "a" },
                                    new Position(10, 6)
                                ),
                                '-',
                                new TermExpressionNode(
                                    unchecked ((char) -1),
                                    new TermNode(
                                        new NumberFactorNode(
                                            1,
                                            new Position(15, 6)
                                        ),
                                        new string[] { },
                                        new FactorNode[] { },
                                        new Position(15, 6)
                                    ),
                                    new Position(15, 6)
                                ),
                                new Position(10, 6)
                            )
                        },
                        new Position(0, 6)
                    )
                },
                new Position(0, 0)
            );

            var text = TestFilesHelper.ReadWhileTestFile();

            var parser = new Parser(new Scanner(text));

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }

        [TestMethod]
        [DeploymentItem(@"TestFiles\nested-expressions.js")]
        public void ParseNestedExpressionsTest()
        {
            var expectedAST = new ProgramNode(
                new StatementNode[] {
                    new VarStatementNode(
                        "a",
                        new TermExpressionNode(
                            unchecked ((char) -1),
                            new TermNode(
                                new ExpressionFactorNode(
                                    new TermExpressionNode(
                                        unchecked ((char) -1),
                                        new TermNode(
                                            new NumberFactorNode(
                                                5,
                                                new Position(9, 0)
                                            ),
                                            new string[] { "&&" },
                                            new FactorNode[] {
                                                new NumberFactorNode(
                                                    4,
                                                    new Position(14, 0)
                                                )
                                            },
                                            new Position(9, 0)
                                        ),
                                        new Position(9, 0)
                                    ),
                                    new Position(8, 0)
                                ),
                                new string[] { "||" },
                                new FactorNode[] {
                                    new NumberFactorNode(
                                        6,
                                        new Position(20, 0)
                                    )
                                },
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
                                new ExpressionFactorNode(
                                    new TermExpressionNode(
                                        unchecked ((char) -1),
                                        new TermNode(
                                            new NumberFactorNode(
                                                6,
                                                new Position(9, 2)
                                            ),
                                            new string[] { "!=" },
                                            new FactorNode[] {
                                                new ExpressionFactorNode(
                                                    new TermExpressionNode(
                                                        unchecked ((char) -1),
                                                        new TermNode(
                                                            new NumberFactorNode(
                                                                8,
                                                                new Position(15, 2)
                                                            ),
                                                            new string[] { "!=" },
                                                            new FactorNode[] {
                                                                new NumberFactorNode(
                                                                    7,
                                                                    new Position(20, 2)
                                                                )
                                                            },
                                                            new Position(15, 2)
                                                        ),
                                                        new Position(15, 2)
                                                    ),
                                                    new Position(14, 2)
                                                )
                                            },
                                            new Position(9, 2)
                                        ),
                                        new Position(9, 2)
                                    ),
                                    new Position(8, 2)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(8, 2)
                            ),
                            new Position(8, 2)
                        ),
                        new Position(0, 2)
                    )
                },
                new Position(0, 0)
            );

            var text = TestFilesHelper.ReadNestedExpressionsTestFile();

            var parser = new Parser(new Scanner(text));

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }

        [TestMethod]
        [DeploymentItem(@"TestFiles\comments.js")]
        public void ParseCommentsTest()
        {
            var expectedAST = new ProgramNode(
                new StatementNode[] {
                    new VarStatementNode(
                        "a",
                        new TermExpressionNode(
                            '-',
                            new TermNode(
                                new NumberFactorNode(
                                    5,
                                    new Position(33, 1)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(33, 1)
                            ),
                            new Position(32, 1)
                        ),
                        new Position(0, 1)
                    ),
                    new VarStatementNode(
                        "b",
                        new TermExpressionNode(
                            '+',
                            new TermNode(
                                new NumberFactorNode(
                                    6,
                                    new Position(9, 10)
                                ),
                                new string[] { },
                                new FactorNode[] { },
                                new Position(9, 10)
                            ),
                            new Position(8, 10)
                        ),
                        new Position(0, 10)
                    )
                },
                new Position(0, 1)
            );

            var text = TestFilesHelper.ReadCommentsTestFile();

            var parser = new Parser(new Scanner(text));

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
