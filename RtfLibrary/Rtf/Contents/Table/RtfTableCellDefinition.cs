using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Represents cell definition containing information about cell style and width.
    /// </summary>
    public class RtfTableCellDefinition
    {
        private RtfTableCell _cell;
        private RtfTableCellStyle _style;

        private bool
            hasStyle = false,
            widthSet = false,
            firstHorizontallyMergedCell = false,
            horizontallyMergedCell = false,
            firstVerticallyMergedCell = false,
            verticallyMergedCell = false;

        internal int WidthInternal = 9797;

        internal bool FirstHorizontallyMergedCellInternal
        {
            get { return firstHorizontallyMergedCell; }
            set
            {
                if (firstHorizontallyMergedCell = value)
                {
                    horizontallyMergedCell = false;
                    _cell.Clear();
                }
            }
        }

        internal bool HorizontallyMergedCellInternal
        {
            get { return horizontallyMergedCell; }
            set
            {
                if (horizontallyMergedCell = value)
                {
                    firstHorizontallyMergedCell = false;
                    _cell.Clear();
                }
            }
        }

        internal bool FirstVerticallyMergedCellInternal
        {
            get { return firstVerticallyMergedCell; }
            set
            {
                if (firstVerticallyMergedCell = value)
                {
                    verticallyMergedCell = false;
                    _cell.Clear();
                }
            }
        }

        internal bool VerticallyMergedCellInternal
        {
            get { return verticallyMergedCell; }
            set
            {
                if (verticallyMergedCell = value)
                {
                    firstVerticallyMergedCell = false;
                    _cell.Clear();
                }
            }
        }

        internal bool IsWidthSet
        {
            get { return widthSet; }
        }

        internal bool HasStyle
        {
            get { return hasStyle; }
        }

        internal RtfTableCellStyle StyleInternal
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// Gets a Boolean value indicating that the cell is the first cell in a range of table cells to be merged.
        /// </summary>
        [RtfControlWord("clmgf")]
        public bool FirstHorizontallyMergedCell
        {
            get { return firstHorizontallyMergedCell; }
        }

        /// <summary>
        /// Gets a Boolean value indicating that the contents of the table cell are merged with those of the preceding cell.
        /// </summary>
        [RtfControlWord("clmrg")]
        public bool HorizontallyMergedCell
        {
            get { return horizontallyMergedCell; }
        }

        /// <summary>
        /// Gets a Boolean value indicating that the cell is the first cell in a range of table cells to be vertically merged.
        /// </summary>
        [RtfControlWord("clvmgf")]
        public bool FirstVerticallyMergedCell
        {
            get { return firstVerticallyMergedCell; }
        }

        /// <summary>
        /// Gets a Boolean value indicating that the contents of the table cell are vertically merged with those of the preceding cell.
        /// </summary>
        [RtfControlWord("clvmrg")]
        public bool VerticallyMergedCell
        {
            get { return verticallyMergedCell; }
        }

        /// <summary>
        /// Gets or sets the style of the cell.
        /// </summary>
        [RtfInclude]
        public RtfTableCellStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;

                if (hasStyle = _style != null)
                {
                    _cell.Formatting = _style.DefaultParagraphFormatting;
                }
            }
        }

        /// <summary>
        /// Gets the right boundary of the cell in twips.
        /// </summary>
        [RtfControlWord("cellx")]
        public int RightBoundary
        {
            get 
            {
                int boundary = 0;

                for (int i = 0; i <= _cell.ColumnIndexInternal; i++)
                    boundary += _cell.RowInternal.Cells[i].Definition.Width;

                return boundary;
            }
        }

        /// <summary>
        /// Gets or sets the width of the cell in twips.
        /// </summary>
        public int Width
        {
            get { return WidthInternal; }
            set
            {
                widthSet = true;
                WidthInternal = value;
            }
        }

        internal RtfTableCellDefinition(RtfTableCell cell)
        {
            _cell = cell;
        }

        internal RtfTableCellDefinition(RtfTableCell cell, RtfTableCellStyle style)
        {
            _cell = cell;
            _style = style;
        }
    }
}