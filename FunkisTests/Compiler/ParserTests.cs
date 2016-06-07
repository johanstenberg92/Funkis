using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funkis.Compiler;

namespace FunkisTests.Compiler
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void ParseHelloWorldTest()
        {
            var expectedAST = new ProgramNode(
                new ImportNode[] {
                    new ImportNode(
                        new PropertyNode(
                            "System",
                            new Position(6, 0)
                        ),
                        new Position(0, 0)
                    )
                },
                new DeclarationNode[] {
                    new FuncDeclarationNode(
                        "hello_world",
                        new TypeUnitNode(
                            new Position(23, 0)
                        ),
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        new string[] {
                                            "Console",
                                            "WriteLine"
                                        },
                                        new Position(4, 1)
                                    ),
                                    new TermExpressionNode(
                                        new TermNode(
                                            new StringLiteralNode(
                                                "hello world!",
                                                new Position(23, 1)
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        new Position(0, 0)
                    ),
                    new LetDeclarationNode(
                        new TypeUnitNode(
                            new Position(9, 3)
                        ),
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        "hello_world",
                                        new Position(14, 3)
                                    ),
                                    new TermExpressionNode(
                                        new TermNode(
                                            new UnitLiteralNode(
                                                new Position(26, 3)
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
                        new TypePropertyNode(
                            new PropertyNode(
                                "double",
                                new Position(8, 1)
                            )
                        ),
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
                        new TypePropertyNode(
                            new PropertyNode(
                                "int",
                                new Position(8, 10)
                            )
                        ),
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

        [TestMethod]
        [DeploymentItem(@"TestFiles\add.funk")]
        public void ParseAddTest()
        {
            var expectedAST = new ProgramNode(
                new ImportNode[] {
                    new ImportNode(
                        new PropertyNode(
                            "System",
                            new Position(6, 0)
                        ),
                        new Position(0, 0)
                    )
                },
                new PropertyNode(
                    new string[] {
                        "Functions",
                        "Funk"
                    },
                    new Position(10, 2)
                ),
                new DeclarationNode[] {
                    new FuncDeclarationNode(
                        "add",
                        new TypePropertyNode(
                            new PropertyNode(
                                "int",
                                new Position(32, 4)
                            )
                        ),
                        new IdentifierNode[] {
                            new IdentifierNode(
                                "a",
                                new Position(14, 4)
                            ),
                            new IdentifierNode(
                                "b",
                                new Position(22, 4)
                            )
                        },
                        new TypeNode[] {
                            new TypePropertyNode(
                                new PropertyNode(
                                    "int",
                                    new Position(17, 4)
                                )
                            ),
                            new TypePropertyNode(
                                new PropertyNode(
                                    "int",
                                    new Position(25, 4)
                                )
                            )
                        },
                        new TermExpressionNode(
                            new TermNode(
                                new PropertyNode(
                                    "a",
                                    new Position(38, 4)
                                ),
                                "+",
                                new PropertyNode(
                                    "b",
                                    new Position(42, 4)
                                )
                            )
                        ),
                        new Position(0, 4)
                    ),
                    new LetDeclarationNode(
                        "c",
                        new TypePropertyNode(
                            new PropertyNode(
                                "int",
                                new Position(8, 6)
                            )
                        ),
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        "add",
                                        new Position(14, 6)
                                    ),
                                    new ExpressionNode[] {
                                        new TermExpressionNode(
                                            new TermNode(
                                                new PropertyNode(
                                                    "a",
                                                    new Position(19, 6)
                                                )
                                            )
                                        ),
                                        new TermExpressionNode(
                                            new TermNode(
                                                new PropertyNode(
                                                    "b",
                                                    new Position(22, 6)
                                                )
                                            )
                                        )
                                    }
                                )
                            )
                        ),
                        new Position(0, 6)
                    ),
                    new LetDeclarationNode(
                        new TypeUnitNode(new Position(11, 8)),
                        new TermExpressionNode(
                            new TermNode(
                                new FunctionCallNode(
                                    new PropertyNode(
                                        new string[] {
                                            "Console",
                                            "WriteLine"
                                        },
                                        new Position(14, 8)
                                    ),
                                    new TermExpressionNode(
                                        new TermNode(
                                            new PropertyNode(
                                                "c",
                                                new Position(32, 8)
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        new Position(0, 9)
                    )
                }
            );

            var text = TestFilesHelper.ReadAddTestFile();

            var parser = new Parser(text);

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }

        [TestMethod]
        [DeploymentItem(@"TestFiles\match.funk")]
        public void ParseMatchTest()
        {
            var expectedAST = new ProgramNode(
                new DeclarationNode[] {
                    new LetDeclarationNode(
                        "a",
                        new TypePropertyNode(
                            new PropertyNode(
                                "int",
                                new Position(8, 0)
                            )
                        ),
                        new TermExpressionNode(
                            new TermNode(
                                new IntLiteralNode(
                                    6,
                                    new Position(14, 0)
                                )
                            )
                        ),
                        new Position(0, 0)
                    ),
                    new LetDeclarationNode(
                        "b",
                        new TypePropertyNode(
                            new PropertyNode(
                                "char",
                                new Position(8, 2)
                            )
                        ),
                        new MatchExpressionNode(
                            new TermExpressionNode(
                                new TermNode(
                                    new PropertyNode(
                                        "a",
                                        new Position(21, 2)
                                    )
                                )
                            ),
                            new PatternNode[] {
                                new PatternLiteralNode(
                                    new IntLiteralNode(
                                        5,
                                        new Position(4, 3)
                                    )
                                ),
                                new PatternLiteralNode(
                                    new IntLiteralNode(
                                        8,
                                        new Position(4, 4)
                                    )
                                ),
                                new PatternLiteralNode(
                                    new IntLiteralNode(
                                        6,
                                        new Position(4, 5)
                                    )
                                )
                            },
                            new ExpressionNode[] {
                                new TermExpressionNode(
                                    new TermNode(
                                        new CharLiteralNode(
                                            'a',
                                            new Position(9, 3)
                                        )
                                    )
                                ),
                                new TermExpressionNode(
                                    new TermNode(
                                        new CharLiteralNode(
                                            'b',
                                            new Position(9, 4)
                                        )
                                    )
                                ),
                                new TermExpressionNode(
                                    new TermNode(
                                        new CharLiteralNode(
                                            'c',
                                            new Position(9, 5)
                                        )
                                    )
                                )
                            },
                            new Position(15, 2)
                        ),
                        new Position(0, 2)
                    )
                }
            );

            var text = TestFilesHelper.ReadMatchTestFile();

            var parser = new Parser(text);

            var actualAST = parser.Parse();

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
