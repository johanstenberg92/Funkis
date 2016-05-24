# Funkis

[![Stories in Ready](https://badge.waffle.io/johanstenberg92/IronJS.svg?label=ready&title=Ready)](http://waffle.io/johanstenberg92/IronJS)

## Introduction
Funkis is a functional and dynamic language running on the CLR using the DLR.

This project is, for now, meant to only be a toy/learning project - the "wheel" is re-invented multiple times. 
If looking for inspiration for your own compiler project, please browse the source code and the accompanying tests for inspiration. 
You could also take a look at a "compiler-compiler" such as ANTLR to automagically generate a scanner, lexer and parser for your grammar instead.

### Waffle Throughput Graph

[![Throughput Graph](https://graphs.waffle.io/johanstenberg92/IronJS/throughput.svg)](https://waffle.io/johanstenberg92/IronJS/metrics/throughput)

## Language Specification
Initially only an EBNF-grammar will be supported, later maybe a standard library will be written.

### EBNF-grammar
`ident` and `number` are defined implicitly. Only integers are supported.

```
program = statement { statement }

block = ( "{" statement { statement } "}" | statement )

statement =
    "var" ident "=" expression ";"
	| "if" "(" expression ")" block { "else" "if" "(" expression ")" block) } ["else" block]
	| "while" "(" expression ")" block
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
   | """ string """
   | "(" expression ")"

property = ident { "." ident }
```
