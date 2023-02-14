namespace Calculator
{
    public class Token
    {
        public TokenType Type { get; set; }

        public char Value { get; set; }

        public Token(TokenType type, char value)
        {
            Type = type;
            Value = value;
        }
    }
}
