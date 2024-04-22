using System.Collections.Generic;

namespace InfNet.Enums {
    public enum TokenType {
        StartOfFile,
        EndOfFile,
        NewLine,

        StartComment,           // ';'
        Literal,                // Text
        EqualsSymbol,           // '='
        CommaSeparator,         // ','
        LineContinuation,       // '\'

        StartSectionName,       // '['
        EndSectionName,         // ']'

        StringToken,            // '%'

        QuotedString,           // '"'
    }
}
