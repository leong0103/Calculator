namespace Calculator
{
    public class FinalToken
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }

        public FinalToken(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
