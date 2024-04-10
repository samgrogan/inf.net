namespace InfNet.Enums {
    public enum TokenType {
        StartOfFile,
        EndOfFile,
        Literal,                // Text
        NewLine,                // '\n'
        EqualsSymbol,           // '='
        CommaSeparator,         // ','
        LineContinuation,       // '\'
        StartComment,           // ';'
        StartSectionName,       // '['
        EndSectionName,         // ']'
        StartStringToken,       // '%'
        EndStringToken,         // '%'
        StartQuotedString,      // '"'
        EndQuotedString,        // '"'
    }
}
