namespace FluxInterpreter;

public enum TokenType
{
    LeftParen,      // (
    RightParen,     // )
    LeftBrace,      // {
    RightBrace,     // }
    Comma,          // ,
    Dot,            // .
    Minus,          // -
    Plus,           // +
    Semicolon,      // ;
    Slash,          // /
    Star,           // *

    Neg,            // !
    NegEqual,       // !=
    Equal,          // =
    EqualEqual,     // ==
    Greater,        // >
    GreaterEqual,   // >=
    Less,           // <
    LessEqual,      // <=

    // Literals
    Identifier,     // variable names, function names, etc.
    String,         // "string literals"
    Number,         // 123, 123.45

    // Keywords
    And,            // and
    Class,          // class
    Else,           // else
    False,          // false
    Fun,            // fun
    For,            // for
    If,             // if
    Nil,            // nil
    Or,             // or
    Print,          // print
    Return,         // return
    Super,          // super
    This,           // this
    True,           // true
    Var,            // var
    While,          // while

    Eof             // end of file
}