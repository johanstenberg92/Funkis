# IronJS

[![Stories in Ready](https://badge.waffle.io/johanstenberg92/IronJS.svg?label=ready&title=Ready)](http://waffle.io/johanstenberg92/IronJS)

## Introduction
IronJS aims to run Javascript on the CLR using the Dynamic Language Runtime.

This project is, for now, meant to only be a toy/learning project - the "wheel"
is re-invented multiple times. If looking for inspiration for your own compiler project,
please browse the source code and the accompanying tests for inspiration. You could also
take a look at a "compiler-compiler" such as ANTLR to automagically generate a scanner,
lexer and parser for your grammar instead.

### Waffle Throughput Graph

[![Throughput Graph](https://graphs.waffle.io/johanstenberg92/IronJS/throughput.svg)](https://waffle.io/johanstenberg92/IronJS/metrics/throughput)

## Language Features Supported
Initially, only a small subset of javascript will be supported.

### BNF-grammar
`ident` and `number` are defined implicitly. Only integers are supported.

```
program = statement { statement }

statement =
    "var" ident "=" expression ";"
	| "if" "(" expression ")" ( "{" statement { statement } "}" | statement )
	| "while" "(" expression ")" ( "{" statement { statement } "}" | statement )
	| "function" ident "(" [ ident { "," ident } ] } ")" "{" statement { statement } [ "return" expression ";" ] "}"
	| property "=" expression ";"
	| function_call ";"
	| property ("+=" | "-=" | "/=" | "*=") expression ";"

expression = 
    ["+" | "-"] term

function_call = property "(" [ expression { "," expression } ] ")"

term = factor { ("*" | "/" | "+" | "-" | "==" | ">=" | "<=" | "!=" | "<" | ">" | "||" | "&&" ) factor }

factor =
   property
   | function_call
   | number
   | "(" expression ")"

property = ident { "." ident }
```