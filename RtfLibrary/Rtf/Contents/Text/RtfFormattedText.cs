using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Specifies font (character) formatting.
    /// </summary>
    [Flags]
    public enum RtfCharacterFormatting
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Subscript = 8,
        Superscript = 16,
        Caps = 32,
        SmallCaps = 64
    }

    
    /// <summary>
    /// Represents text with formatting.
    /// </summary>
    [RtfEnclosingBraces]
    public class RtfFormattedText : RtfTextBase
    {
        private int _fontIndex = -1;
        private float _fontSize = 0F;
        private int _colorIndex = -1;
        private int backgroundColorIndex = -1;
        
        private bool superScript = false;
        private bool subScript = false;


        /// <summary>
        /// Index of an entry in the font table. Default is -1 and is ignored by RtfWriter.
        /// </summary>
        [RtfControlWord("f"), RtfIndex]
        public int FontIndex
        {
            get { return _fontIndex; }
            set { _fontIndex = value; }
        }

        /// <summary>
        /// Gets the font size in half points. Used by RtfWriter.
        /// </summary>
        [RtfControlWord("fs"), RtfInclude(ConditionMember="FontSizeSet")]
        public int HalfPointFontSize
        {
            get { return (int)Math.Round(_fontSize * 2); }
        }

        /// <summary>
        /// Index of an entry in the color table. Default is -1 and is ignored by RtfWriter.
        /// </summary>
        [RtfControlWord("cf"), RtfIndex]
        public int TextColorIndex
        {
            get { return _colorIndex; }
            set { _colorIndex = value; }
        }

        /// <summary>
        /// Index of an entry in the color table. Default is -1 and is ignored by RtfWriter.
        /// </summary>
        [RtfControlWord("cb"), RtfIndex]
        public int BackgroundColorIndex
        {
            get { return backgroundColorIndex; }
            set { backgroundColorIndex = value; }
        }

        [RtfControlWord("b")]
        public bool Bold { get; set; }

        [RtfControlWord("i")]
        public bool Italic { get; set; }

        [RtfControlWord("ul")]
        public bool Underline { get; set; }

        [RtfControlWord("sub")]
        public bool Subscript
        {
            get { return subScript; }
            set 
            {
                if (value)
                {
                    superScript = false;
                }

                subScript = value;
            }
        }
        
        [RtfControlWord("super")]
        public bool Superscript
        {
            get { return superScript; }
            set
            {
                if (value)
                {
                    subScript = false;
                }

                superScript = value;
            }
        }

        [RtfControlWord("caps")]
        public bool Caps { get; set; }

        [RtfControlWord("scaps")]
        public bool SmallCaps { get; set; }
                
        
        /// <summary>
        /// Gets string value of the text.
        /// </summary>
        [RtfTextData]
        public string Text
        {
            get { return sb.ToString(); }
        }
        


        /// <summary>
        /// Font size in points. Default value 0 is ignored by RtfWriter.
        /// </summary>
        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// ConditionMember used by RtfWriter.
        /// </summary>
        public bool FontSizeSet
        {
            get { return _fontSize != 0; }
        }


        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        public RtfFormattedText() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        public RtfFormattedText(string text) : base(text)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        /// <param name="colorIndex">Index of an entry in the color table.</param>
        public RtfFormattedText(string text, int colorIndex) : base(text)
        {
            _colorIndex = colorIndex;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        public RtfFormattedText(RtfCharacterFormatting formatting) : base()
        {
            SetFormatting(formatting);
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        public RtfFormattedText(string text, RtfCharacterFormatting formatting) : base(text)
        {
            SetFormatting(formatting);
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        /// <param name="colorIndex">Index of an entry in the color table.</param>
        public RtfFormattedText(RtfCharacterFormatting formatting, int colorIndex) : base()
        {
            SetFormatting(formatting);
            _colorIndex = colorIndex;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        /// /// <param name="colorIndex">Index of an entry in the color table.</param>
        public RtfFormattedText(string text, RtfCharacterFormatting formatting, int colorIndex) : base(text)
        {
            SetFormatting(formatting);
            _colorIndex = colorIndex;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        /// <param name="fontIndex">Index of an entry in the font table.</param>
        public RtfFormattedText(string text, int fontIndex, float fontSize) : base(text)
        {
            _fontIndex = fontIndex;
            _fontSize = fontSize;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        /// <param name="fontIndex">Index of an entry in the font table.</param>
        public RtfFormattedText(RtfCharacterFormatting formatting, int fontIndex, float fontSize) : base()
        {
            SetFormatting(formatting);
            _fontSize = fontSize;
            _fontIndex = fontIndex;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfFormattedText class.
        /// </summary>
        /// <param name="text">String value to set as text.</param>
        /// <param name="formatting">Character formatting to apply to the text.</param>
        /// <param name="fontIndex">Index of an entry in the font table.</param>
        public RtfFormattedText(string text, RtfCharacterFormatting formatting, int fontIndex, float fontSize) : base(text)
        {
            SetFormatting(formatting);
            _fontSize = fontSize;
            _fontIndex = fontIndex;
        }


        /// <summary>
        /// Applies specified formatting to the text.
        /// </summary>
        /// <param name="formatting">Character formatting to apply.</param>
        public void SetFormatting(RtfCharacterFormatting formatting)
        {
            Bold = (formatting & RtfCharacterFormatting.Bold) == RtfCharacterFormatting.Bold;
            Italic = (formatting & RtfCharacterFormatting.Italic) == RtfCharacterFormatting.Italic;
            Underline = (formatting & RtfCharacterFormatting.Underline) == RtfCharacterFormatting.Underline;
            Subscript = (formatting & RtfCharacterFormatting.Subscript) == RtfCharacterFormatting.Subscript;
            Superscript = (formatting & RtfCharacterFormatting.Superscript) == RtfCharacterFormatting.Superscript;
            Caps = (formatting & RtfCharacterFormatting.Caps) == RtfCharacterFormatting.Caps;
            SmallCaps = (formatting & RtfCharacterFormatting.SmallCaps) == RtfCharacterFormatting.SmallCaps;
        }
    }
}