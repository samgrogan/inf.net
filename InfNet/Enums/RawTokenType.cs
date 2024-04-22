namespace InfNet.Enums {
    internal enum RawTokenType {
        StartComment,           // ';'
        Literal,                // Text
        EqualsSymbol,           // '='
        CommaSeparator,         // ','

        StartSectionName,       // '['
        EndSectionName,         // ']'

        StringToken,            // '%'

        QuotedString,           // '"'
    }
}
