namespace LiteScript.Syntax;

public enum SyntaxOperator
{
    // Priority levels
    PostIncr, PostDecr, Call, Get, Member,                        // P1
    PreIncr, PreDecr, UnaryPlus, UnaryMinus, Not, BitNot, New,    // P2
    Mul, Div, Mod,                                                // P3
    Add, Sub,                                                     // P4
    LShift, RShift,                                               // P5
    Less, LessEqu, Great, GreatEqu,                               // P6
    Equ, Dif,                                                     // P7
    BitAnd,                                                       // P8
    BitXor,                                                       // P9
    BitOr,                                                        // P10
    And,                                                          // P11
    Or,                                                           // P12
    Assign, AddAssign, SubAssign, MulAssign, DivAssign,           // P13
}
