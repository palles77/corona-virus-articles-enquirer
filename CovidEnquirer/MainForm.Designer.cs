﻿namespace CovidEnquirer
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
            this.searchForArticleInGoogleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveArticleAsWordDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openArticleInWordPadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ArticleRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.GoogleButton = new System.Windows.Forms.Button();
            this.SaveArticleButton = new System.Windows.Forms.Button();
            this.OpenArticleButton = new System.Windows.Forms.Button();
            this.SearchResultsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhraseToSearchLabel
            // 
            this.PhraseToSearchLabel.AutoSize = true;
            this.PhraseToSearchLabel.Location = new System.Drawing.Point(12, 9);
            this.PhraseToSearchLabel.Name = "PhraseToSearchLabel";
            this.PhraseToSearchLabel.Size = new System.Drawing.Size(43, 13);
            this.PhraseToSearchLabel.TabIndex = 0;
            this.PhraseToSearchLabel.Text = "Phrase:";
            // 
            // PhraseToSearchTextBox
            // 
            this.PhraseToSearchTextBox.Location = new System.Drawing.Point(68, 9);
            this.PhraseToSearchTextBox.Name = "PhraseToSearchTextBox";
            this.PhraseToSearchTextBox.Size = new System.Drawing.Size(631, 20);
            this.PhraseToSearchTextBox.TabIndex = 0;
            // 
            // PhraseToSearchButton
            // 
            this.PhraseToSearchButton.Location = new System.Drawing.Point(705, 7);
            this.PhraseToSearchButton.Name = "PhraseToSearchButton";
            this.PhraseToSearchButton.Size = new System.Drawing.Size(75, 23);
            this.PhraseToSearchButton.TabIndex = 1;
            this.PhraseToSearchButton.Text = "Search";
            this.PhraseToSearchButton.UseVisualStyleBackColor = true;
            this.PhraseToSearchButton.Click += new System.EventHandler(this.PhraseToSearchButton_Click);
            // 
            // SearchResultsListBox
            // 
            this.SearchResultsListBox.ContextMenuStrip = this.SearchResultsContextMenu;
            this.SearchResultsListBox.FormattingEnabled = true;
            this.SearchResultsListBox.Location = new System.Drawing.Point(7, 110);
            this.SearchResultsListBox.Name = "SearchResultsListBox";
            this.SearchResultsListBox.Size = new System.Drawing.Size(329, 368);
            this.SearchResultsListBox.TabIndex = 2;
            this.SearchResultsListBox.SelectedIndexChanged += new System.EventHandler(this.SearchResultsListBox_SelectedIndexChanged);
            // 
            // SearchResultsContextMenu
            // 
            this.SearchResultsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchForArticleInGoogleToolStripMenuItem,
            this.saveArticleAsWordDocumentToolStripMenuItem,
            this.openArticleInWordPadToolStripMenuItem});
            this.SearchResultsContextMenu.Name = "SearchResultsContextMenu";
            this.SearchResultsContextMenu.Size = new System.Drawing.Size(243, 70);
            // 
            // searchForArticleInGoogleToolStripMenuItem
            // 
            this.searchForArticleInGoogleToolStripMenuItem.Name = "searchForArticleInGoogleToolStripMenuItem";
            this.searchForArticleInGoogleToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.searchForArticleInGoogleToolStripMenuItem.Text = "Search For Article In Google";
            this.searchForArticleInGoogleToolStripMenuItem.Click += new System.EventHandler(this.searchForArticleInGoogleToolStripMenuItem_Click);
            // 
            // saveArticleAsWordDocumentToolStripMenuItem
            // 
            this.saveArticleAsWordDocumentToolStripMenuItem.Name = "saveArticleAsWordDocumentToolStripMenuItem";
            this.saveArticleAsWordDocumentToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.saveArticleAsWordDocumentToolStripMenuItem.Text = "Save Article As Word Document";
            this.saveArticleAsWordDocumentToolStripMenuItem.Click += new System.EventHandler(this.saveArticleAsWordDocumentToolStripMenuItem_Click);
            // 
            // openArticleInWordPadToolStripMenuItem
            // 
            this.openArticleInWordPadToolStripMenuItem.Name = "openArticleInWordPadToolStripMenuItem";
            this.openArticleInWordPadToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.openArticleInWordPadToolStripMenuItem.Text = "Open Article In Word Pad";
            this.openArticleInWordPadToolStripMenuItem.Click += new System.EventHandler(this.openArticleInWordPadToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Search Results:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(343, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Article:";
            // 
            // ArticleRichTextBox
            // 
            this.ArticleRichTextBox.Location = new System.Drawing.Point(346, 110);
            this.ArticleRichTextBox.Name = "ArticleRichTextBox";
            this.ArticleRichTextBox.Size = new System.Drawing.Size(434, 368);
            this.ArticleRichTextBox.TabIndex = 3;
            this.ArticleRichTextBox.Text = "";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(15, 48);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(765, 23);
            this.ProgressBar.TabIndex = 7;
            // 
            // GoogleButton
            // 
            this.GoogleButton.Location = new System.Drawing.Point(417, 84);
            this.GoogleButton.Name = "GoogleButton";
            this.GoogleButton.Size = new System.Drawing.Size(75, 23);
            this.GoogleButton.TabIndex = 8;
            this.GoogleButton.Text = "Google";
            this.GoogleButton.UseVisualStyleBackColor = true;
            this.GoogleButton.Click += new System.EventHandler(this.GoogleButton_Click);
            // 
            // SaveArticleButton
            // 
            this.SaveArticleButton.Location = new System.Drawing.Point(498, 84);
            this.SaveArticleButton.Name = "SaveArticleButton";
            this.SaveArticleButton.Size = new System.Drawing.Size(75, 23);
            this.SaveArticleButton.TabIndex = 9;
            this.SaveArticleButton.Text = "Save Article";
            this.SaveArticleButton.UseVisualStyleBackColor = true;
            this.SaveArticleButton.Click += new System.EventHandler(this.SaveArticleButton_Click);
            // 
            // OpenArticleButton
            // 
            this.OpenArticleButton.Location = new System.Drawing.Point(579, 84);
            this.OpenArticleButton.Name = "OpenArticleButton";
            this.OpenArticleButton.Size = new System.Drawing.Size(75, 23);
            this.OpenArticleButton.TabIndex = 10;
            this.OpenArticleButton.Text = "Open Article";
            this.OpenArticleButton.UseVisualStyleBackColor = true;
            this.OpenArticleButton.Click += new System.EventHandler(this.OpenArticleButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.PhraseToSearchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 491);
            this.Controls.Add(this.OpenArticleButton);
            this.Controls.Add(this.SaveArticleButton);
            this.Controls.Add(this.GoogleButton);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.ArticleRichTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchResultsListBox);
            this.Controls.Add(this.PhraseToSearchButton);
            this.Controls.Add(this.PhraseToSearchTextBox);
            this.Controls.Add(this.PhraseToSearchLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Article Searcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.SearchResultsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PhraseToSearchLabel;
        private System.Windows.Forms.TextBox PhraseToSearchTextBox;
        private System.Windows.Forms.Button PhraseToSearchButton;
        private System.Windows.Forms.ListBox SearchResultsListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox ArticleRichTextBox;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ContextMenuStrip SearchResultsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem searchForArticleInGoogleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveArticleAsWordDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openArticleInWordPadToolStripMenuItem;
        private System.Windows.Forms.Button GoogleButton;
        private System.Windows.Forms.Button SaveArticleButton;
        private System.Windows.Forms.Button OpenArticleButton;
    }
}