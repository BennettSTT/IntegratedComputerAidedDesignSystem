using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegratedComputerAidedDesignSystem
{
    public enum FileFormat
    {
        Allegro = 1,
        Calay
    }

    public class Microcircuit
    {
        public string Name { get; set; }

        public List<Output> Outputs { get; } = new List<Output>();
    }

    public class Output
    {
        public string Name { get; set; }

        public Node Node { get; set; }
    }

    public class Node
    {
        public string Name { get; set; }
    }


    public enum Token
    {
        None = 1,

        Node,
        Output,
        Component,

        // other
        NextLine,
        EndOfFile,
    }

    public class SlidingTextWindow
    {
        private string _text;

        private int _basis;
        private int _offset;
        private int _lexemeStart;
        
        public SlidingTextWindow(string text)
        {
            _text = text;
            _basis = 0;
            _offset = 0;
            _lexemeStart = 0;
        }

        public void StartLexemeAnalyze()
        {
            _lexemeStart = _offset;
        }
        
        public int LexemeStartPosition => _basis + _lexemeStart;

        public int LexemeWidth => _offset - _lexemeStart;

        public void AdvanceChar()
        {
            _offset++;
        }
        
        public char PeekChar()
        {
            return _text[_offset];
        }

        public char NextChar()
        {
            var c = PeekChar();
            AdvanceChar();
            return c;
        }

        public string GetText()
        {
            if (LexemeWidth == 0)
            {
                return string.Empty;
            }

            return new string(_text.ToCharArray(), LexemeStartPosition, LexemeWidth);
        }
    }


    public struct TokenInfo
    {
        public Token Token;
        public string Text;

        public TokenInfo(Token token, string text)
        {
            Token = token;
            Text = text;
        }
    }
    
    public class CalayParser
    {
        private SlidingTextWindow _textWindow;
        
        public CalayParser(string text)
        {
            _textWindow = new SlidingTextWindow(text);
        }

        private TokenInfo GetToken()
        {
            _textWindow.StartLexemeAnalyze();
            
            var tokenInfo = default(TokenInfo);
            
            while (true)
            {
                char @char = _textWindow.PeekChar();
                switch (@char)
                {
                    case ';':
                        _textWindow.AdvanceChar();
                        tokenInfo.Text = ";";
                        tokenInfo.Token = Token.NextLine;
                        return tokenInfo;

                    case ' ':
                        if (_textWindow.LexemeWidth == 0)
                        {
                            _textWindow.AdvanceChar();
                            break;
                        }

                        tokenInfo.Text = _textWindow.GetText();
                        tokenInfo.Token = Token.Component;

                        return tokenInfo;

                    case ')':
                        tokenInfo.Text = _textWindow.GetText();
                        tokenInfo.Token = Token.Output;

                        return tokenInfo;

                    case '\n':
                    case '\r':
                        _textWindow.AdvanceChar();
                        break;

                    default:
                        return default;
                }
            }

            return tokenInfo;
        }


        private void ScanComponent(ref TokenInfo tokenInfo)
        {
            
        }
        
        
        public Microcircuit[] Parse(string text)
        {
            GetToken();
            
            
            Dictionary<string, Microcircuit> microcircuitCache = new Dictionary<string, Microcircuit>();

            string[] rows = text.Replace(Environment.NewLine, string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                string[] elements = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length <= 1)
                {
                    throw new Exception();
                }

                var node = new Node { Name = elements[0] };
                for (int i = 1; i < elements.Length; i++)
                {
                    string element = elements[i];

                    var res = element.Split(new[] { '(', '\'', ')' }, StringSplitOptions.RemoveEmptyEntries);

                    if (res.Length != 2)
                    {
                        throw new Exception();
                    }

                    Microcircuit microcircuit;
                    var microcircuitName = res[0];
                    if (!microcircuitCache.TryGetValue(microcircuitName, out microcircuit))
                    {
                        microcircuit = new Microcircuit { Name = microcircuitName };
                        microcircuitCache.Add(microcircuitName, microcircuit);
                    }

                    string outputName = res[1];

                    var output = new Output { Name = outputName, Node = node };

                    microcircuit.Outputs.Add(output);
                }
            }

            return microcircuitCache.Values.ToArray();
        }
    }
}