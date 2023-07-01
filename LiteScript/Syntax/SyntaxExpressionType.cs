namespace LiteScript.Syntax;

/// <summary>
/// TODO: Translate those codes into classes
/// </summary>
internal enum SyntaxExpressionType
{
    Invalid,

    Operator,

    ArrayBegin,
    ArraySeparator,
    ArrayEnd,

    ObjectBegin,
    ObjectSeparator,
    ObjectEnd,

    ExpressionBegin, // Parenthesis
    ExpressionEnd,

    CallbackArgBegin,
    CallbackArgSeparator,
    CallbackArgEnd,

    TernaryThen, // ?: operator
    TernaryElse,

    DeclarationSymbol, // var
    DeclarationName,
    DeclarationSeparator,
    DeclarationTypeSpecifier, // :
    DeclarationValueSeparator,

    IfSymbol,
    IfConditionBegin,
    IfConditionEnd,

    WhileSymbol,
    WhileConditionBegin,
    WhileConditionEnd,

    ForSymbol,
    ForExpressionBegin,
    ForExpressionSeparator,
    ForExpressionEnd,

    ForeachSymbol,
    ForeachExpressionBegin,
    ForeachExpressionAssign,
    ForeachExpressionEnd,

    SwitchSymbol,
    SwitchExpressionBegin,
    SwitchExpressionEnd,
    SwitchBlockBegin,
    SwitchBlockEnd,
    SwitchBlockCaseSymbol,
    SwitchBlockCaseSpecifier,
    SwitchStatementSpecifier,

    NewSymbol,
    ReturnSymbol,
    ContinueSymbol,
    BreakSymbol,

    MethodSymbol,
    MethodArgumentsBegin,
    MethodArgumentsSeparator,
    MethodArgumentsTypeSpecifier,
    MethodArgumentsEnd,
    MethodArgumentsReturnTypeSpecifier,
    MethodBlockBegin,
    MethodBlockEnd,
}
