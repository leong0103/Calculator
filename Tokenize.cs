using System.Text.RegularExpressions;

namespace Calculator
{
    public class Tokenizer
    {
        public IEnumerable<FinalToken> Execute(string input)
        {
            var result = new List<Token>();
            input = input.Replace(" ", "");
            foreach(char ch in input)
            {
                var chType = GetTokenType(ch);
                var token = new Token(chType, ch);
                result.Add(token);
            }
            var newResult = GroupToken(result);
            return newResult;
        }
        
        private IEnumerable<FinalToken> GroupToken(List<Token> tokens)
        {
            var result = new List<FinalToken>();
            var buffer = new List<char>();
            for(int i = 0; i < tokens.Count(); i++)
            {
                if(tokens[i].Type == TokenType.Digit || tokens[i].Type == TokenType.Dot)
                {
                    buffer.Add(tokens[i].Value);
                    if(i == tokens.Count() - 1)
                    {
                        result.Add(new FinalToken(TokenType.Digit, String.Join("", buffer)));
                    }
                } else if(tokens[i].Type == TokenType.Operator)
                {
                    result.Add(new FinalToken(TokenType.Digit, String.Join("", buffer)));
                    buffer.Clear();
                    result.Add(new FinalToken(TokenType.Operator, tokens[i].Value.ToString()));
                }
            }

            return result;
        }
        private TokenType GetTokenType(char ch)
        {
            if(IsDigit(ch))
            {
                return TokenType.Digit;
            } else if(IsLetter(ch))
            {
                return TokenType.Letter;
            } else if(IsOperator(ch))
            {
                return TokenType.Operator;
            } else if(IsLeftParenthesis(ch))
            {
                return TokenType.LeftParenthesis;
            } else if(IsRightParenthesis(ch))
            {
                return TokenType.RightParenthesis;
            } else if(IsDot(ch))
            {
                return TokenType.Dot;
            }
            else
            {
                throw new Exception("Wrong Operator.");
            }
        }
        private bool IsDigit(char ch)
        {
            string pattern = @"^\d$";
            var regex = new Regex(pattern);

            var result = regex.Match(ch.ToString());
            return result.Success;
        }

        private bool IsLetter(char ch)
        {
            string pattern = @"[a-z]";
            var regex = new Regex(pattern);

            var result = regex.Match(ch.ToString());
            return result.Success;
        }

        private bool IsOperator(char ch)
        {
            string pattern = @"[\+\-\*\/\^]";
            var regex = new Regex(pattern);

            var result = regex.Match(ch.ToString());
            return result.Success;
        }

        private bool IsLeftParenthesis(char ch)
        {
            return (ch == '(');
        }

        private bool IsRightParenthesis(char ch)
        {
            return (ch == ')');
        }
        
        private bool IsDot(char ch)
        {
            return (ch == '.');
        }
    }
}