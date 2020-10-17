using IntegratedComputerAidedDesignSystem.Infrastructure.Parsers.Interfaces;
using System;

namespace IntegratedComputerAidedDesignSystem.Infrastructure.Parsers
{
    public class Parser
    {
        private const string AllegroHeader = "$PACKAGES";

        private readonly string _text;

        public Parser(string text)
        {
            _text = text;
        }

        private FileFormat GetFormat()
        {
            var fileFormat = FileFormat.Calay;

            if (_text.IndexOf(AllegroHeader, 0, AllegroHeader.Length, StringComparison.Ordinal) > -1)
            {
                fileFormat = FileFormat.Allegro;
            }

            return fileFormat;
        }

        public (Component[] components, Node[] nodes) Parse()
        {
            FileFormat format = GetFormat();

            IFormatParser parser = format switch
            {
                FileFormat.Allegro => new AllegroParser(),
                FileFormat.Calay => new CalayParser(),

                _ => throw new ArgumentException()
            };

            return parser.Parse(_text);
        }
    }
}