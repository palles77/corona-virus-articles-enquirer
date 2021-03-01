using CovidLib;

namespace CovidEnquirer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PhraseToSearchLabel = new System.Windows.Forms.Label();
            this.PhraseToSearchTextBox = new System.Windows.Forms.TextBox();
            this.PhraseToSearchButton = new System.Windows.Forms.Button();
            this.SearchResultsListBox = new System.Windows.Forms.ListBox();
            this.SearchResultsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SearchForArticleInGoogleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveArticleAsWordDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenArticleInWordPadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TranslateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchResultsLabel = new System.Windows.Forms.Label();
            this.ArticleRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.GoogleButton = new System.Windows.Forms.Button();
            this.SaveArticleButton = new System.Windows.Forms.Button();
            this.OpenArticleButton = new System.Windows.Forms.Button();
            this.OptionalMessageLabel = new System.Windows.Forms.Label();
            this.ResultsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.TranslateButton = new System.Windows.Forms.Button();
            this.LanguagesComboBox = new System.Windows.Forms.ComboBox();
            this.TitleSearchTextBox = new DelayTypingTextBox();
            this.SearchResultsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsSplitContainer)).BeginInit();
            this.ResultsSplitContainer.Panel1.SuspendLayout();
            this.ResultsSplitContainer.Panel2.SuspendLayout();
            this.ResultsSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhraseToSearchLabel
            // 
            this.PhraseToSearchLabel.AutoSize = true;
            this.PhraseToSearchLabel.Location = new System.Drawing.Point(4, 12);
            this.PhraseToSearchLabel.Name = "PhraseToSearchLabel";
            this.PhraseToSearchLabel.Size = new System.Drawing.Size(43, 13);
            this.PhraseToSearchLabel.TabIndex = 0;
            this.PhraseToSearchLabel.Text = "Phrase:";
            // 
            // PhraseToSearchTextBox
            // 
            this.PhraseToSearchTextBox.Location = new System.Drawing.Point(68, 9);
            this.PhraseToSearchTextBox.Name = "PhraseToSearchTextBox";
            this.PhraseToSearchTextBox.Size = new System.Drawing.Size(651, 20);
            this.PhraseToSearchTextBox.TabIndex = 0;
            // 
            // PhraseToSearchButton
            // 
            this.PhraseToSearchButton.Location = new System.Drawing.Point(725, 9);
            this.PhraseToSearchButton.Name = "PhraseToSearchButton";
            this.PhraseToSearchButton.Size = new System.Drawing.Size(75, 23);
            this.PhraseToSearchButton.TabIndex = 1;
            this.PhraseToSearchButton.Text = "Search";
            this.PhraseToSearchButton.UseVisualStyleBackColor = true;
            this.PhraseToSearchButton.Click += new System.EventHandler(this.PhraseToSearchButtonClick);
            // 
            // SearchResultsListBox
            // 
            this.SearchResultsListBox.ContextMenuStrip = this.SearchResultsContextMenu;
            this.SearchResultsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchResultsListBox.FormattingEnabled = true;
            this.SearchResultsListBox.Location = new System.Drawing.Point(0, 0);
            this.SearchResultsListBox.Name = "SearchResultsListBox";
            this.SearchResultsListBox.Size = new System.Drawing.Size(263, 397);
            this.SearchResultsListBox.TabIndex = 0;
            this.SearchResultsListBox.SelectedIndexChanged += new System.EventHandler(this.SearchResultsListBoxSelectedIndexChanged);
            // 
            // SearchResultsContextMenu
            // 
            this.SearchResultsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchForArticleInGoogleToolStripMenuItem,
            this.SaveArticleAsWordDocumentToolStripMenuItem,
            this.OpenArticleInWordPadToolStripMenuItem,
            this.TranslateToolStripMenuItem});
            this.SearchResultsContextMenu.Name = "SearchResultsContextMenu";
            this.SearchResultsContextMenu.Size = new System.Drawing.Size(290, 92);
            // 
            // searchForArticleInGoogleToolStripMenuItem
            // 
            this.SearchForArticleInGoogleToolStripMenuItem.Name = "searchForArticleInGoogleToolStripMenuItem";
            this.SearchForArticleInGoogleToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.SearchForArticleInGoogleToolStripMenuItem.Text = "Search  For Selected Title In Google";
            this.SearchForArticleInGoogleToolStripMenuItem.Click += new System.EventHandler(this.SearchForArticleInGoogleToolStripMenuItemClick);
            // 
            // saveArticleAsWordDocumentToolStripMenuItem
            // 
            this.SaveArticleAsWordDocumentToolStripMenuItem.Name = "saveArticleAsWordDocumentToolStripMenuItem";
            this.SaveArticleAsWordDocumentToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.SaveArticleAsWordDocumentToolStripMenuItem.Text = "Save Selected Article As Word Document";
            this.SaveArticleAsWordDocumentToolStripMenuItem.Click += new System.EventHandler(this.SaveArticleAsWordDocumentToolStripMenuItemClick);
            // 
            // openArticleInWordPadToolStripMenuItem
            // 
            this.OpenArticleInWordPadToolStripMenuItem.Name = "openArticleInWordPadToolStripMenuItem";
            this.OpenArticleInWordPadToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.OpenArticleInWordPadToolStripMenuItem.Text = "Open Selected Article In Word Pad";
            this.OpenArticleInWordPadToolStripMenuItem.Click += new System.EventHandler(this.OpenArticleInWordPadToolStripMenuItemClick);
            // 
            // translateToolStripMenuItem
            // 
            this.TranslateToolStripMenuItem.Name = "translateToolStripMenuItem";
            this.TranslateToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.TranslateToolStripMenuItem.Text = "Translate";
            this.TranslateToolStripMenuItem.Click += new System.EventHandler(this.TranslateToolStripMenuItemClick);
            // 
            // label1
            // 
            this.SearchResultsLabel.AutoSize = true;
            this.SearchResultsLabel.Location = new System.Drawing.Point(4, 101);
            this.SearchResultsLabel.Name = "label1";
            this.SearchResultsLabel.Size = new System.Drawing.Size(104, 13);
            this.SearchResultsLabel.TabIndex = 4;
            this.SearchResultsLabel.Text = "Filter Search Results";
            // 
            // ArticleRichTextBox
            // 
            this.ArticleRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArticleRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.ArticleRichTextBox.Name = "ArticleRichTextBox";
            this.ArticleRichTextBox.Size = new System.Drawing.Size(526, 397);
            this.ArticleRichTextBox.TabIndex = 0;
            this.ArticleRichTextBox.Text = "";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(7, 62);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(793, 23);
            this.ProgressBar.TabIndex = 7;
            // 
            // GoogleButton
            // 
            this.GoogleButton.Location = new System.Drawing.Point(274, 96);
            this.GoogleButton.Name = "GoogleButton";
            this.GoogleButton.Size = new System.Drawing.Size(129, 23);
            this.GoogleButton.TabIndex = 2;
            this.GoogleButton.Text = "Google Selected Title";
            this.GoogleButton.UseVisualStyleBackColor = true;
            this.GoogleButton.Click += new System.EventHandler(this.GoogleButtonClick);
            // 
            // SaveArticleButton
            // 
            this.SaveArticleButton.Location = new System.Drawing.Point(409, 96);
            this.SaveArticleButton.Name = "SaveArticleButton";
            this.SaveArticleButton.Size = new System.Drawing.Size(173, 23);
            this.SaveArticleButton.TabIndex = 4;
            this.SaveArticleButton.Text = "Save Original Article As Word";
            this.SaveArticleButton.UseVisualStyleBackColor = true;
            this.SaveArticleButton.Click += new System.EventHandler(this.SaveArticleButtonClick);
            // 
            // OpenArticleButton
            // 
            this.OpenArticleButton.Location = new System.Drawing.Point(588, 125);
            this.OpenArticleButton.Name = "OpenArticleButton";
            this.OpenArticleButton.Size = new System.Drawing.Size(212, 23);
            this.OpenArticleButton.TabIndex = 6;
            this.OpenArticleButton.Text = "Open Translated Article In WordPad";
            this.OpenArticleButton.UseVisualStyleBackColor = true;
            this.OpenArticleButton.Click += new System.EventHandler(this.OpenArticleButtonClick);
            // 
            // OptionalMessageLabel
            // 
            this.OptionalMessageLabel.AutoSize = true;
            this.OptionalMessageLabel.Location = new System.Drawing.Point(4, 42);
            this.OptionalMessageLabel.Name = "OptionalMessageLabel";
            this.OptionalMessageLabel.Size = new System.Drawing.Size(101, 13);
            this.OptionalMessageLabel.TabIndex = 11;
            this.OptionalMessageLabel.Text = "<optional message>";
            // 
            // ResultsSplitContainer
            // 
            this.ResultsSplitContainer.Location = new System.Drawing.Point(7, 152);
            this.ResultsSplitContainer.Name = "ResultsSplitContainer";
            // 
            // ResultsSplitContainer.Panel1
            // 
            this.ResultsSplitContainer.Panel1.Controls.Add(this.SearchResultsListBox);
            // 
            // ResultsSplitContainer.Panel2
            // 
            this.ResultsSplitContainer.Panel2.Controls.Add(this.ArticleRichTextBox);
            this.ResultsSplitContainer.Size = new System.Drawing.Size(793, 397);
            this.ResultsSplitContainer.SplitterDistance = 263;
            this.ResultsSplitContainer.TabIndex = 12;
            // 
            // TranslateButton
            // 
            this.TranslateButton.Location = new System.Drawing.Point(409, 125);
            this.TranslateButton.Name = "TranslateButton";
            this.TranslateButton.Size = new System.Drawing.Size(173, 23);
            this.TranslateButton.TabIndex = 5;
            this.TranslateButton.Text = "Translate Using Google";
            this.TranslateButton.UseVisualStyleBackColor = true;
            this.TranslateButton.Click += new System.EventHandler(this.TranslateButtonClick);
            // 
            // LanguagesComboBox
            // 
            this.LanguagesComboBox.FormattingEnabled = true;
            this.LanguagesComboBox.Location = new System.Drawing.Point(274, 125);
            this.LanguagesComboBox.Name = "LanguagesComboBox";
            this.LanguagesComboBox.Size = new System.Drawing.Size(129, 21);
            this.LanguagesComboBox.TabIndex = 3;
            // 
            // SearchResultsTextBox
            // 
            this.TitleSearchTextBox.Location = new System.Drawing.Point(7, 125);
            this.TitleSearchTextBox.Name = "SearchResultsTextBox";
            this.TitleSearchTextBox.Size = new System.Drawing.Size(263, 20);
            this.TitleSearchTextBox.TabIndex = 1;
            this.TitleSearchTextBox.DelayedTextChanged += new System.EventHandler(this.TitleSearchTextBoxTextChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.PhraseToSearchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 561);
            this.Controls.Add(this.TitleSearchTextBox);
            this.Controls.Add(this.LanguagesComboBox);
            this.Controls.Add(this.TranslateButton);
            this.Controls.Add(this.SearchResultsLabel);
            this.Controls.Add(this.GoogleButton);
            this.Controls.Add(this.ResultsSplitContainer);
            this.Controls.Add(this.OpenArticleButton);
            this.Controls.Add(this.OptionalMessageLabel);
            this.Controls.Add(this.SaveArticleButton);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.PhraseToSearchButton);
            this.Controls.Add(this.PhraseToSearchTextBox);
            this.Controls.Add(this.PhraseToSearchLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Article Searcher";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.SearchResultsContextMenu.ResumeLayout(false);
            this.ResultsSplitContainer.Panel1.ResumeLayout(false);
            this.ResultsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsSplitContainer)).EndInit();
            this.ResultsSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PhraseToSearchLabel;
        private System.Windows.Forms.TextBox PhraseToSearchTextBox;
        private System.Windows.Forms.Button PhraseToSearchButton;
        private System.Windows.Forms.ListBox SearchResultsListBox;
        private System.Windows.Forms.Label SearchResultsLabel;
        private System.Windows.Forms.RichTextBox ArticleRichTextBox;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ContextMenuStrip SearchResultsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem SearchForArticleInGoogleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveArticleAsWordDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenArticleInWordPadToolStripMenuItem;
        private System.Windows.Forms.Button GoogleButton;
        private System.Windows.Forms.Button SaveArticleButton;
        private System.Windows.Forms.Button OpenArticleButton;
        private System.Windows.Forms.Label OptionalMessageLabel;
        private System.Windows.Forms.SplitContainer ResultsSplitContainer;
        private System.Windows.Forms.Button TranslateButton;
        private System.Windows.Forms.ComboBox LanguagesComboBox;
        private System.Windows.Forms.ToolStripMenuItem TranslateToolStripMenuItem;
        private DelayTypingTextBox TitleSearchTextBox;
    }
}
