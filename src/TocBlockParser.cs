using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

using System;

namespace Leisn.MarkdigToc
{
    public class TocBlockParser : BlockParser, IAttributesParseable
    {
        public TocOptions Options { get; }
        public TryParseAttributesDelegate? TryParseAttributes { get; set; }

        public TocBlockParser(TocOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            OpeningCharacters = new[] { '[' };
        }

        const string TOC = "[toc]";
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
                return BlockState.None;
            var line = processor.Line;
            var column = processor.Column;
            var sourcePosition = line.Start;

            var match = line.MatchLowercase(TOC);
            if (!match)
                return BlockState.None;

            var c = line.CurrentChar;
            for (int i = 0; i < TOC.Length; i++)
                c = processor.NextChar();

            StringSlice trivia = StringSlice.Empty;
            if (processor.TrackTrivia && c.IsSpaceOrTab())
            {
                trivia = new StringSlice(processor.Line.Text, processor.Start, processor.Start);
                processor.NextChar();
            }

            var block = new TocBlock(this)
            {
                Column = column,
                Span = { Start = sourcePosition },
                TriviaAfterAtxHeaderChar = trivia,
                TriviaBefore = processor.UseTrivia(sourcePosition - 1),
                LinesBefore = processor.LinesBefore,
                NewLine = processor.Line.NewLine,
            };
            processor.LinesBefore = null;

            processor.NewBlocks.Push(block);
            if (!processor.TrackTrivia)
            {
                processor.GoToColumn(column + TOC.Length + 1);
            }
            TryParseAttributes?.Invoke(processor, ref processor.Line, block);

            block.Span.End = processor.Line.End;

            if (processor.TrackTrivia)
            {
                var wsa = new StringSlice(processor.Line.Text, processor.Line.End + 1, processor.Line.End);
                block.TriviaAfter = wsa;
                if (wsa.Overlaps(block.TriviaAfterAtxHeaderChar))
                    block.TriviaAfter = StringSlice.Empty;
            }

            return BlockState.Break;
        }

        public override bool Close(BlockProcessor processor, Block block)
        {
            if (!processor.TrackTrivia)
            {
                (block as TocBlock)?.Lines.Trim();
            }
            return true;
        }
    }
}
