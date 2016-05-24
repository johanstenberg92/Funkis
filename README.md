# Funkis

[![Stories in Ready](https://badge.waffle.io/johanstenberg92/Funkis.svg?label=ready&title=Ready)](http://waffle.io/johanstenberg92/Funkis)

## Introduction
Funkis is a functional and dynamic language running on the CLR using the DLR.

Funkis is inspired by:
* OCaml

This project is, for now, meant to only be a toy/learning project - the "wheel" is re-invented multiple times. 
If looking for inspiration for your own compiler project, please browse the source code and the accompanying tests for inspiration. 
You could also take a look at a "compiler-compiler" such as ANTLR to automagically generate a scanner, lexer and parser for your grammar instead.

### Waffle Throughput Graph

[![Throughput Graph](https://graphs.waffle.io/johanstenberg92/Funkis/throughput.svg)](https://waffle.io/johanstenberg92/Funkis/metrics/throughput)

## Language Specification
Initially only an EBNF-grammar will be supported, later maybe a standard library will be written.

`identifier` is defined implicitly as an identifier.

```
global_declaration =
	"let" identifier = expression
	| declaration

declaration =
    "let" "func" identifier [identifier { "," identifier }] = expression

expression =
    ["+" | "-"] term
	| "|" pattern "->" expression { newline "|" pattern "->" expression }

pattern =
    identifier
	| "[" identifier { "," identifier } "]"
	| "(" identifier { "," identifier } ")"

term = factor { ("*" | "/" | "+" | "-" | "==" | ">=" | "<=" | "!=" | "<" | ">" | "||" | "&&" | "::" |  ) factor }

factor =
    property
	| property expression { "," expression }
	| "(" expression ")"
    | literal
	| "[" expression { "," expression } "]"

literal =
    int
	| float
	| bool
	| char
	| string
	| unit

int = digit { digit }

float = digit "." digit { digit }

bool = ("true" | "false")

char = "'" <TODO> "'"

string = """ <TODO> """

unit = "()"

property = identifier { "." identifier }

identifier = (A-Z | a-z) { (upper_case_letter, lower_case_letter, digit) }

newline = "\n" | "\r\n"

upper_case_letter = ("A" | "B" | "C" | "D" | "E" | "F" | "G" | "H" | "I" | "J" | "K" | "L" | "M" | "N" | "O" | "P" | "Q" | "R" | "S" | "T" | "U" | "V" | "W" | "X" | "Y" | "Z")

lower_case_letter = ("a" | "b" | "c" | "d" | "e" | "f" | "g" | "h" | "i" | "j" | "k" | "l" | "m" | "n" | "o" | "p" | "q" | "r" | "s" | "t" | "u" | "v" | "w" | "x" | "y" | "z")

digit = ("0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9")
```