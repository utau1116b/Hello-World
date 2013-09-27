namespace Utau.Eramakerview
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.Start = new System.Windows.Forms.Button();
			this.AddTextBox = new System.Windows.Forms.RichTextBox();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.mainPage = new System.Windows.Forms.TabPage();
			this.optionPage = new System.Windows.Forms.TabPage();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.startButton = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.mainPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// Start
			// 
			this.Start.Location = new System.Drawing.Point(84, 157);
			this.Start.Name = "Start";
			this.Start.Size = new System.Drawing.Size(75, 23);
			this.Start.TabIndex = 0;
			this.Start.Text = "Run";
			this.Start.UseVisualStyleBackColor = true;
			this.Start.Click += new System.EventHandler(this.Start_Click);
			// 
			// AddTextBox
			// 
			this.AddTextBox.Location = new System.Drawing.Point(0, 0);
			this.AddTextBox.Name = "AddTextBox";
			this.AddTextBox.Size = new System.Drawing.Size(402, 25);
			this.AddTextBox.TabIndex = 1;
			this.AddTextBox.Text = "";
			this.AddTextBox.TextChanged += new System.EventHandler(this.AddTextBox_TextChanged);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(410, 26);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開くToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(85, 22);
			this.toolStripMenuItem1.Text = "ファイル(&F)";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.mainPage);
			this.tabControl.Controls.Add(this.optionPage);
			this.tabControl.Location = new System.Drawing.Point(0, 27);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(410, 202);
			this.tabControl.TabIndex = 3;
			// 
			// mainPage
			// 
			this.mainPage.BackColor = System.Drawing.Color.Silver;
			this.mainPage.Controls.Add(this.AddTextBox);
			this.mainPage.Location = new System.Drawing.Point(4, 22);
			this.mainPage.Name = "mainPage";
			this.mainPage.Padding = new System.Windows.Forms.Padding(3);
			this.mainPage.Size = new System.Drawing.Size(402, 176);
			this.mainPage.TabIndex = 0;
			this.mainPage.Text = "メイン";
			// 
			// optionPage
			// 
			this.optionPage.BackColor = System.Drawing.Color.Silver;
			this.optionPage.Location = new System.Drawing.Point(4, 22);
			this.optionPage.Name = "optionPage";
			this.optionPage.Padding = new System.Windows.Forms.Padding(3);
			this.optionPage.Size = new System.Drawing.Size(402, 176);
			this.optionPage.TabIndex = 1;
			this.optionPage.Text = "オプション";
			this.optionPage.Click += new System.EventHandler(this.tabPage2_Click);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(0, 264);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(410, 23);
			this.progressBar.TabIndex = 4;
			this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(4, 235);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 5;
			this.startButton.Text = "Run";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// 開くToolStripMenuItem
			// 
			this.開くToolStripMenuItem.Name = "開くToolStripMenuItem";
			this.開くToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.開くToolStripMenuItem.Text = "開く(&O)";
			this.開くToolStripMenuItem.Click += new System.EventHandler(this.開くToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 289);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.Start);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "EramakerView(別人版)";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.mainPage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button Start;
		private System.Windows.Forms.RichTextBox AddTextBox;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage mainPage;
		private System.Windows.Forms.TabPage optionPage;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.ToolStripMenuItem 開くToolStripMenuItem;
    }
}

