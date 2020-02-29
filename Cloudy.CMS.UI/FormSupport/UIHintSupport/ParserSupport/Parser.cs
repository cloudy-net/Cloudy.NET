using System;
using System.Diagnostics;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    [DebuggerDisplay("{Data.Insert(Position, \"↓\")}")]
    public class Parser : IParser
    {
        string Data { get; }
        int Length { get; }
        int Position { get; set; }

        public Parser(string data)
        {
            Data = data;
            Length = data.Length;
        }

        public void ExpectEnd()
        {
            if (IsEnd())
            {
                return;
            }

            throw new ExpectedEndException(Data, Position);
        }

        public void Expect(params char[] tokens)
        {
            if (Is(tokens))
            {
                return;
            }

            throw new ExpectedTokenException(Data, Position, tokens);
        }

        public void ExpectOrEnd(params char[] tokens)
        {
            if (IsEnd() || Is(tokens))
            {
                return;
            }

            throw new ExpectedTokenOrEndException(Data, Position, tokens);
        }

        public string ReadUntil(params char[] tokens)
        {
            var startIndex = Position;

            ContinueUntil(tokens);
            ReverseWhile(' ');

            if(Position < startIndex)
            {
                Position = startIndex;
            }

            return Data.Substring(startIndex, Position - startIndex);
        }

        public void ContinueUntil(char[] tokens)
        {
            while (!IsEnd() && !Is(tokens))
            {
                Position++;
            }
        }

        public void SkipWhitespace()
        {
            ContinueWhile(' ');
        }

        public void ContinueWhile(params char[] tokens)
        {
            while (!IsEnd() && Is(tokens))
            {
                Position++;
            }
        }

        public void ReverseWhile(params char[] tokens)
        {
            while (!IsStart() && Was(tokens))
            {
                Position--;
            }
        }

        public void Skip() => Position++;
        public bool IsStart() => Position == 0;
        public bool IsEnd() => Position == Length;
        public bool Is(params char[] tokens)
        {
            if (Position > Data.Length - 1)
            {
                throw new UnexpectedEndException(Data, Position);
            }

            return tokens.Contains(Data[Position]);
        }
        public bool Was(params char[] tokens) => tokens.Contains(Data[Position - 1]);
    }
}