# Rhexia (Rhex-I-A)

Rhexia or Rhex Intelligence Artificielle, is a languange written by Rhexis.

Raw Code: `45 - 5 * (46 / (foo * bar) * (x + y))`

Parsed Code:
```
Program { 
    Kind = Program,
    Body = [
        BinaryExpression {
            Kind = BinaryExpression,
            Left = NumericLiteral { 
                Kind = NumericLiteral,
                Value = 45
            },
            Right = BinaryExpression {
                Kind = BinaryExpression,
                Left = NumericLiteral {
                    Kind = NumericLiteral,
                    Value = 5
                },
                Right = BinaryExpression {
                    Kind = BinaryExpression,
                    Left = BinaryExpression {
                        Kind = BinaryExpression,
                        Left = NumericLiteral {
                            Kind = NumericLiteral,
                            Value = 46
                        },
                        Right = BinaryExpression {
                            Kind = BinaryExpression,
                            Left = Identifier {
                                Kind = Identifier,
                                Symbol = foo
                            },
                            Right = Identifier {
                                Kind = Identifier,
                                Symbol = bar
                            },
                            Operator = *
                        },
                        Operator = /
                    },
                    Right = BinaryExpression {
                        Kind = BinaryExpression,
                        Left = Identifier {
                            Kind = Identifier,
                            Symbol = x
                        },
                        Right = Identifier {
                            Kind = Identifier,
                            Symbol = y
                        },
                        Operator = +
                    },
                    Operator = *
                },
                Operator = *
            },
            Operator = -
        }
    ]
}
```