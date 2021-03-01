using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Specifies vertical align of the text inside the cell.
    /// </summary>
    [RtfEnumAsControlWord(RtfEnumConversion.UseAttribute)]
    public enum RtfTableCellVerticalAlign
    {
        [RtfControlWord("clvertalt")]
        Top,
        [RtfControlWord("clvertalc")]
        Center,
        [RtfControlWord("clvertalb")]
        Bottom
    }
    
    /// <summary>
    /// Specifies the direction of text flow inside the cell.
    /// </summary>
    [RtfEnumAsControlWord(RtfEnumConversion.UseAttribute)]
    public enum RtfTableCellTextFlow
    {
        [RtfControlWord("cltxlrtb")]
        LeftToRightTopToBottom,
        [RtfControlWord("cltxtbrl")]
        TopToBottomRightToLeft,
        [RtfControlWord("cltxbtlr")]
        BottomToTopLeftToRight,
        [RtfControlWord("cltxlrtbv")]
        LeftToRightTopToBottomVertical,
        [RtfControlWord("cltxtbrlv")]
        TopToBottomRightToLeftVertical
    }

    /// <summary>
    /// Represents table cell style.
    /// </summary>
    public class RtfTableCellStyle
    {
        private RtfTableCellVerticalAlign _verticalAlign = RtfTableCellVerticalAlign.Center;
        private RtfTableCellBorders _borders = new RtfTableCellBorders();
        private RtfTableCellTextFlow _textFlow = RtfTableCellTextFlow.LeftToRightTopToBottom;
        private RtfParagraphFormatting _defaultParagraphFormatting;

        /// <summary>
        /// Gets or sets vertical align of the text inside the cell.
        /// </summary>
        [RtfControlWord]
        public RtfTableCellVerticalAlign VerticalAlign
        {
            get { return _verticalAlign; }
            set { _verticalAlign = value; }
        }

        /// <summary>
        /// Gets borders of the cell.
        /// </summary>
        [RtfInclude]
        public RtfTableCellBorders Borders
        {
            get { return _borders; }
        }

        /// <summary>
        /// Gets or sets the direction of text flow inside the cell
        /// </summary>
        [RtfControlWord]
        public RtfTableCellTextFlow TextFlow
        {
            get { return _textFlow; }
            set { _textFlow = value; }
        }

        /// <summary>
        /// Gets or sets the formatting applied to paragraphs inside the cell by default.
        /// </summary>
        public RtfParagraphFormatting DefaultParagraphFormatting
        {
            get { return _defaultParagraphFormatting; }
            set { _defaultParagraphFormatting = value; }
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfTableCellStyle class.
        /// </summary>
        public RtfTableCellStyle()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfTableCellStyle class.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        public RtfTableCellStyle(RtfBorderSetting borderSetting)
        {
            SetBorders(borderSetting);
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfTableCellStyle class.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="formatting">Formatting applied to paragraphs inside the cell by default.</param>
        public RtfTableCellStyle(RtfBorderSetting borderSetting, RtfParagraphFormatting formatting)
        {
            SetBorders(borderSetting);

            _defaultParagraphFormatting = formatting;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfTableCellStyle class.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="formatting">Formatting applied to paragraphs inside the cell by default.</param>
        /// <param name="align">Vertical align of the text inside the cell.</param>
        public RtfTableCellStyle(RtfBorderSetting borderSetting, RtfParagraphFormatting formatting, RtfTableCellVerticalAlign align)
        {
            SetBorders(borderSetting);

            _defaultParagraphFormatting = formatting;
            _verticalAlign = align;
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfTableCellStyle class.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="formatting">Formatting applied to paragraphs inside the cell by default.</param>
        /// <param name="align">Vertical align of the text inside the cell.</param>
        /// <param name="textFlow">Direction of text flow inside the cell.</param>
        public RtfTableCellStyle(RtfBorderSetting borderSetting, RtfParagraphFormatting formatting, RtfTableCellVerticalAlign align, RtfTableCellTextFlow textFlow)
        {
            SetBorders(borderSetting);

            _defaultParagraphFormatting = formatting;
            _verticalAlign = align;
            _textFlow = textFlow;           
        }

        /// <summary>
        /// Copies all properties of the current cell style to the specified RtfTableCellStyle object.
        /// </summary>
        /// <param name="cellStyle">Cell style object to copy to.</param>
        public void CopyTo(RtfTableCellStyle cellStyle)
        {
            cellStyle._verticalAlign = this._verticalAlign;

            this._borders.Top.CopyTo(cellStyle._borders.Top);
            this._borders.Left.CopyTo(cellStyle._borders.Left);
            this._borders.Bottom.CopyTo(cellStyle._borders.Bottom);
            this._borders.Right.CopyTo(cellStyle._borders.Right);

            cellStyle._textFlow = this._textFlow;
        }

        /// <summary>
        /// Sets border style.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        public void SetBorders(RtfBorderSetting borderSetting)
        {
            SetBorders(borderSetting, .5F, RtfBorderStyle.SingleThicknessBorder, -1);
        }

        /// <summary>
        /// Sets border style.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="width">Width in points.</param>
        public void SetBorders(RtfBorderSetting borderSetting, float width)
        {
            SetBorders(borderSetting, width, RtfBorderStyle.SingleThicknessBorder, -1);
        }

        /// <summary>
        /// Sets border style.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="width">Width in points.</param>
        /// <param name="style">Border style.</param>
        public void SetBorders(RtfBorderSetting borderSetting, float width, RtfBorderStyle style)
        {
            SetBorders(borderSetting, width, style, -1);
        }

        /// <summary>
        /// Sets border style.
        /// </summary>
        /// <param name="borderSetting">Cell border setting.</param>
        /// <param name="width">Width in points.</param>
        /// <param name="style">Border style.</param>
        /// <param name="colorIndex">Index of an entry in the color table.</param>
        public void SetBorders(RtfBorderSetting borderSetting, float width, RtfBorderStyle style, int colorIndex)
        {
            if ((borderSetting & RtfBorderSetting.Top) == RtfBorderSetting.Top)
                Borders.Top.SetProperties(width, style, colorIndex);
            else
                Borders.Top.Width = 0;

            if ((borderSetting & RtfBorderSetting.Left) == RtfBorderSetting.Left)
                Borders.Left.SetProperties(width, style, colorIndex);
            else
                Borders.Left.Width = 0;

            if ((borderSetting & RtfBorderSetting.Bottom) == RtfBorderSetting.Bottom)
                Borders.Bottom.SetProperties(width, style, colorIndex);
            else
                Borders.Bottom.Width = 0;

            if ((borderSetting & RtfBorderSetting.Right) == RtfBorderSetting.Right)
                Borders.Right.SetProperties(width, style, colorIndex);
            else
                Borders.Right.Width = 0;
        }
    }
}