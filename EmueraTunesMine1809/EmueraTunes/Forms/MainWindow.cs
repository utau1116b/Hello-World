using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MinorShift._Library;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.Forms;
using MinorShift.Emuera.GameSound.SoundX;

namespace MinorShift.Emuera
{
	internal sealed partial class MainWindow : Form
	{

		public MainWindow()
		{
			InitializeComponent();
			SavePicBox.picBox = pictureBox1;
			pictureBox1.DoubleClick += new EventHandler(pictureBox1_DoubleClick);
			if (Config.SetWindowPos)
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = new Point(Config.WindowPosX, Config.WindowPosY);
			}
			if (Program.DebugMode)
				デバッグToolStripMenuItem.Visible = true;

			((EraPictureBox)mainPicBox).SetStyle();
			this.ResizeEnd += new EventHandler(MainWindow_ResizeEnd);
			this.Resize += new EventHandler(MainWindow_Resize);
			lastState = WindowState;
			int winX = Program.ClientX;
			Program.ClientX = 0;
			if (winX == 0)
				winX = Config.WindowX;
			winY = Program.ClientY;
			Program.ClientY = 0;
			if (winY == 0)
				winY = Config.WindowY;
			if (Config.SizableWindow)
				this.FormBorderStyle = FormBorderStyle.Sizable;
			else
				this.FormBorderStyle = FormBorderStyle.Fixed3D;
			setWindowSize(Config.WindowX, winY);

			int minimamY = 100;
			if (minimamY > this.Height)
				minimamY = this.Height;
			int maximamY = 2560;
			if (maximamY < this.Height)
				maximamY = this.Height;
			this.MinimumSize = new Size(this.Width, minimamY);
			this.MaximumSize = new Size(this.Width, maximamY);
			richTextBox1.ForeColor = Config.ForeColor;
			richTextBox1.BackColor = Config.BackColor;
			mainPicBox.BackColor = Config.BackColor;
			this.BackColor = Config.BackColor;

			richTextBox1.Font = Config.Font;
			richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			this.MaximizeBox = Config.SizableWindow;
			if (Config.SizableWindow && Config.WindowMaximixed)
				this.WindowState = FormWindowState.Maximized;

			if (Program.state != FormWindowState.Normal)
			{
				WindowState = Program.state;
				Program.state = FormWindowState.Normal;
			}

			folderSelectDialog.SelectedPath = Program.ErbDir;
			folderSelectDialog.ShowNewFolderButton = false;

			openFileDialog.InitialDirectory = Program.ErbDir;
			openFileDialog.Filter = "ERBファイル (*.erb)|*.erb";
			openFileDialog.FileName = "";
			openFileDialog.Multiselect = true;
			openFileDialog.RestoreDirectory = true;

			timer.Enabled = true;
			console = new EmueraConsole(this);
			macroMenuItems[0] = マクロ01ToolStripMenuItem;
			macroMenuItems[1] = マクロ02ToolStripMenuItem;
			macroMenuItems[2] = マクロ03ToolStripMenuItem;
			macroMenuItems[3] = マクロ04ToolStripMenuItem;
			macroMenuItems[4] = マクロ05ToolStripMenuItem;
			macroMenuItems[5] = マクロ06ToolStripMenuItem;
			macroMenuItems[6] = マクロ07ToolStripMenuItem;
			macroMenuItems[7] = マクロ08ToolStripMenuItem;
			macroMenuItems[8] = マクロ09ToolStripMenuItem;
			macroMenuItems[9] = マクロ10ToolStripMenuItem;
			macroMenuItems[10] = マクロ11ToolStripMenuItem;
			macroMenuItems[11] = マクロ12ToolStripMenuItem;
			foreach (ToolStripMenuItem item in macroMenuItems)
				item.Click += new EventHandler(マクロToolStripMenuItem_Click);
		}
		private ToolStripMenuItem[] macroMenuItems = new ToolStripMenuItem[KeyMacro.MaxMacro];
		public PictureBox MainPicBox { get { return mainPicBox; } }
		public VScrollBar ScrollBar { get { return vScrollBar; } }
		public RichTextBox TextBox { get { return richTextBox1; } }
		private EmueraConsole console = null;

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & Keys.KeyCode) == Keys.B && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control)
			{
				if (WindowState != FormWindowState.Minimized)
				{
					WindowState = FormWindowState.Minimized;
					return true;
				}
			}
			else if (((keyData & Keys.KeyCode) == Keys.C && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control) || (keyData & Keys.KeyCode) == Keys.Insert && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control)
			{
				if (this.richTextBox1.SelectedText == "")
				{
					ClipBoardDialog dialog = new ClipBoardDialog();
					dialog.StartPosition = FormStartPosition.CenterParent;
					dialog.Setup(this, console);
					dialog.ShowDialog();
					return true;
				}
			}
			else if (((keyData & Keys.KeyCode) == Keys.V && ((keyData & Keys.Modifiers) & Keys.Control) == Keys.Control) || (keyData & Keys.KeyCode) == Keys.Insert && ((keyData & Keys.Modifiers) & Keys.Shift) == Keys.Shift)
			{
				if (Clipboard.GetDataObject() == null || !Clipboard.ContainsText())
					return true;
				else
				{
					if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
						richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
					return true;
				}
			}
			//else if (((int)keyData == (int)Keys.Control + (int)Keys.D) && Program.DebugMode)
			//{
			//    console.OpenDebugDialog();
			//    return true;
			//}
			//else if (((int)keyData == (int)Keys.Control + (int)Keys.R) && Program.DebugMode)
			//{
			//    if ((console.DebugDialog != null) && (console.DebugDialog.Created))
			//        console.DebugDialog.UpdateData();
			//}
			else if (Config.UseKeyMacro)
			{
				if ((int)keyData == (int)Keys.F1 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F1, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F1)
				{
					this.richTextBox1.Text = KeyMacro.MacroF1;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F2 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F2, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F2)
				{
					this.richTextBox1.Text = KeyMacro.MacroF2;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F3 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F3, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F3)
				{
					this.richTextBox1.Text = KeyMacro.MacroF3;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F4 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F4, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F4)
				{
					this.richTextBox1.Text = KeyMacro.MacroF4;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F5 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F5, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F5)
				{
					this.richTextBox1.Text = KeyMacro.MacroF5;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F6 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F6, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F6)
				{
					this.richTextBox1.Text = KeyMacro.MacroF6;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F7 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F7, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F7)
				{
					this.richTextBox1.Text = KeyMacro.MacroF7;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F8 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F8, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F8)
				{
					this.richTextBox1.Text = KeyMacro.MacroF8;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F9 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F9, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F9)
				{
					this.richTextBox1.Text = KeyMacro.MacroF9;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F10 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F10, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F10)
				{
					this.richTextBox1.Text = KeyMacro.MacroF10;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F11 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F11, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F11)
				{
					this.richTextBox1.Text = KeyMacro.MacroF11;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
				else if ((int)keyData == (int)Keys.F12 + (int)Keys.Shift)
				{
					if (this.richTextBox1.Text != "")
						KeyMacro.SetMacro(Keys.F12, this.richTextBox1.Text);
					return true;
				}
				else if ((int)keyData == (int)Keys.F12)
				{
					this.richTextBox1.Text = KeyMacro.MacroF12;
					this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
					return true;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const int SC_MOVE = 0xf010;
			const int SC_MAXIMIZE = 0xf030;

			// WM_SYSCOMMAND (SC_MOVE) を無視することでフォームを移動できないようにする
			if (m.Msg == WM_SYSCOMMAND)
			{
				int wparam = m.WParam.ToInt32() & 0xfff0;
				switch (wparam)
				{
					case SC_MOVE:
						if (WindowState == FormWindowState.Maximized)
							return;
						break;
					case SC_MAXIMIZE:
						if (Screen.AllScreens.Length == 1)
						{
							this.MaximizedBounds = new Rectangle(this.Left, 0, Config.WindowX, Screen.PrimaryScreen.WorkingArea.Height);
						}
						else
						{
							for (int i = 0; i < Screen.AllScreens.Length; i++)
							{
								if (this.Left >= Screen.AllScreens[i].Bounds.Left && this.Left < Screen.AllScreens[i].Bounds.Right)
								{
									this.MaximizedBounds = new Rectangle(this.Left - Screen.AllScreens[i].Bounds.Left, Screen.AllScreens[i].Bounds.Top, Config.WindowX, Screen.AllScreens[i].WorkingArea.Height);
									break;
								}
							}
						}
						base.WndProc(ref m);
						return;
				}
			}
			base.WndProc(ref m);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (!this.Created)
				return;
			timer.Enabled = false;
			console.Initialize();
		}
		int winY;

		void MainWindow_ResizeEnd(object sender, EventArgs e)
		{
			windowResized();
		}

		FormWindowState lastState;
		//最大化、最小化はResizeEndでは捕まえられない。
		void MainWindow_Resize(object sender, EventArgs e)
		{
			if (WindowState == lastState)
				return;
			if (WindowState == FormWindowState.Minimized)
				return;
			lastState = WindowState;
			windowResized();
		}

		private void windowResized()
		{
			int height = this.ClientSize.Height;

			if (Config.UseMenu)
			{
				height -= menuStrip.Height;
			}
			if (winY == height)
				return;
			winY = height;
			setWindowSize(Config.WindowX, winY);
		}

		private void setWindowSize(int windowX, int windowY)
		{
			this.SuspendLayout();
			Size menuSize = new Size(0, 0);
			if (Config.UseMenu)
			{
				menuStrip.Enabled = true;
				menuStrip.Visible = true;
				menuStrip.Size = new Size(windowX + vScrollBar.Width, menuStrip.Height);
				menuSize = menuStrip.Size;
			}
			else
			{
				menuStrip.Enabled = false;
				menuStrip.Visible = false;
			}
			this.ClientSize = new Size(windowX + vScrollBar.Width, windowY + menuSize.Height);
			vScrollBar.Size = new Size(vScrollBar.Width, windowY);
			vScrollBar.Location = new Point(this.ClientSize.Width - vScrollBar.Width, menuSize.Height);//new Point(windowX, menuSize.Height);
			mainPicBox.Size = new Size(windowX, windowY - (int)Config.LineHeight);
			mainPicBox.Location = new Point(0, menuSize.Height);
			mainPicBox.BackColor = (console == null) ? Config.BackColor : console.bgColor;
			this.BackColor = mainPicBox.BackColor;
			if (mainPicBox.Image != null)
				mainPicBox.Image.Dispose();
			if (Config.UseImageBuffer)
				mainPicBox.Image = new Bitmap(mainPicBox.Size.Width, mainPicBox.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			else
				mainPicBox.Image = null;
			richTextBox1.Size = new Size(windowX, (int)Config.LineHeight);
			richTextBox1.Location = new Point(0, menuSize.Height + mainPicBox.Height);
			this.ResumeLayout();
			if (console != null)
			{
				console.MainPicBoxSizeChanged();
				//ImageBufferを使わない方式ではOnPaintで描画する
				if (Config.UseImageBuffer)
					console.RefreshStrings(true);
			}
		}


		private void mainPicBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (!Config.UseMouse)
				return;
			if (console == null)
				return;
			console.MoveMouse(e.Location);
		}
		bool changeTextbyMouse = false;
		private void mainPicBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (!Config.UseMouse)
				return;
			if (console == null)
				return;
			if (console.IsWaitingAnyKey && !console.IsError)
			{
				if ((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
				{
					if (vScrollBar.Value != vScrollBar.Maximum)
					{
						vScrollBar.Value = vScrollBar.Maximum;
						console.RefreshStrings(true);
						//WAIT系の時は戻るだけにしておく
						return;
					}
					if (e.Button == MouseButtons.Right)
						PressEnterKey(true, null);
					else
						PressEnterKey(false, null);
					return;
				}

			}
			if (console.SelectingButton == null)//選択中でないなら無視
			{
				if (console.IsError)
				{
					PressEnterKey(false, null);
					return;
				}
				if (vScrollBar.Value != vScrollBar.Maximum)
				{//履歴表示中
					if ((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
					{
						vScrollBar.Value = vScrollBar.Maximum;
						console.RefreshStrings(true);
					}
				}
				return;
			}
			//左が押されたなら選択。
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				string str = console.SelectedString;
				if (str == null)
					return;
				if (vScrollBar.Value != vScrollBar.Maximum)
				{
					vScrollBar.Value = vScrollBar.Maximum;
					console.RefreshStrings(true);
				}
				changeTextbyMouse = console.IsWaintingOnePhrase;
				richTextBox1.Text = str;
				ScriptPosition pos = console.SelectedPos;
				//右が押しっぱなしならスキップ追加。
				if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
					PressEnterKey(true, pos);
				else
					PressEnterKey(false, pos);
				return;
			}
		}

		private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			//上端でも下端でもないなら描画を控えめに。
			if (console == null)
				return;
			console.RefreshStrings((vScrollBar.Value == vScrollBar.Maximum) || (vScrollBar.Value == vScrollBar.Minimum));
		}

		public void PressEnterKey(bool mesSkip, ScriptPosition ErrPos)
		{
			if (console == null)
				return;
			if (console.inProcess)
			{
				richTextBox1.Text = "";
				return;
			}
			string str = richTextBox1.Text;
			if (console.IsWaintingOnePhrase)
			{
				str = str.Remove(0, last_inputed.Length);
				last_inputed = "";
			}
			updateInputs(str);
			console.PressEnterKey(mesSkip, str, ErrPos);
		}

		string[] prevInputs = new string[100];
		int selectedInputs = 100;
		int lastSelected = 100;
		void updateInputs(string cur)
		{
			if (string.IsNullOrEmpty(cur))
				return;
			if (selectedInputs == prevInputs.Length || cur != prevInputs[prevInputs.Length - 1])
			{
				for (int i = 0; i < prevInputs.Length - 1; i++)
				{
					prevInputs[i] = prevInputs[i + 1];
				}
				prevInputs[prevInputs.Length - 1] = cur;
				//1729a eramakerと同じ処理系に変更 1730a 再修正
				if (selectedInputs > 0 && selectedInputs != prevInputs.Length && cur == prevInputs[selectedInputs - 1])
					lastSelected = --selectedInputs;
				else
					lastSelected = 100;
			}
			else
			{
				lastSelected = selectedInputs;
			}
			richTextBox1.Text = "";
			selectedInputs = prevInputs.Length;
		}

		void movePrev(int move)
		{
			if (move == 0)
				return;
			//if((selectedInputs != prevInputs.Length) &&(prevInputs[selectedInputs] != richTextBox1.Text))
			//	selectedInputs =  prevInputs.Length;
			int next;
			if (lastSelected != prevInputs.Length && selectedInputs == prevInputs.Length)
			{
				if (move == -1)
					move = 0;
				next = lastSelected + move;
				lastSelected = prevInputs.Length;
			}
			else
				next = selectedInputs + move;
			if ((next < 0) || (next > prevInputs.Length))
				return;
			if (next == prevInputs.Length)
			{
				selectedInputs = next;
				richTextBox1.Text = "";
				return;
			}
			if (string.IsNullOrEmpty(prevInputs[next]))
				if (++next == prevInputs.Length)
					return;

			selectedInputs = next;
			richTextBox1.Text = prevInputs[next];
			richTextBox1.SelectionStart = 0;
			richTextBox1.SelectionLength = richTextBox1.Text.Length;
			return;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("ゲームを終了します", "終了", MessageBoxButtons.OKCancel);
			if (result != DialogResult.OK)
				return;
			this.Close();

		}

		private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("ゲームを再起動します", "再起動", MessageBoxButtons.OKCancel);
			if (result != DialogResult.OK)
				return;
			this.Reboot();
		}

		//private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    openFileDialog.InitialDirectory = StaticConfig.SavDir;
		//    DialogResult result = openFileDialog.ShowDialog();
		//    string filepath = openFileDialog.FileName;
		//    if (!File.Exists(filepath))
		//    {
		//        MessageBox.Show("ファイルがありません", "File Not Found");
		//        return;
		//    }
		//}

		public void Reboot()
		{
			console.forceStopTimer();
			Program.Reboot = true;
			Program.state = WindowState;
			if (WindowState != FormWindowState.Normal)
				WindowState = FormWindowState.Normal;
			Program.ClientX = mainPicBox.Width;
			Program.ClientY = mainPicBox.Height + (int)Config.Font.Size;
			this.Close();
		}

		public void GotoTitle()
		{
			if (console == null)
				return;
			console.GotoTitle();
		}

		public void ReloadErb()
		{
			if (console == null)
				return;
			console.ReloadErb();
		}

		private void mainPicBox_MouseLeave(object sender, EventArgs e)
		{
			if (console == null)
				return;
			if (Config.UseMouse)
				console.LeaveMouse();
		}


		private void コンフィグCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowConfigDialog();
		}

		public void ShowConfigDialog()
		{
			ConfigDialog dialog = new ConfigDialog();
			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.SetConfig(this);
			dialog.ShowDialog();
			if (dialog.Result == ConfigDialogResult.SaveReboot)
			{
				console.forceStopTimer();
				Program.Reboot = true;
				Program.state = WindowState;
				if (WindowState != FormWindowState.Normal)
					WindowState = FormWindowState.Normal;
				Program.ClientX = mainPicBox.Width;
				Program.ClientY = mainPicBox.Height + (int)Config.Font.Size;
				this.Close();
			}
		}

		private void タイトルへ戻るTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (console.IsInitializing)
			{
				MessageBox.Show("起動中には使用できません");
				return;
			}
			if (console.notToTitle)
			{
				if (console.byError)
					MessageBox.Show("コード解析でエラーが発見されたため、タイトルへは飛べません");
				else
					MessageBox.Show("解析モードのためタイトルへは飛べません");
				return;
			}
			DialogResult result = MessageBox.Show("タイトル画面へ戻ります", "タイトル画面に戻る", MessageBoxButtons.OKCancel);
			if (result != DialogResult.OK)
				return;
			this.GotoTitle();
		}

		private void コードを読み直すcToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (console.IsInitializing)
			{
				MessageBox.Show("起動中には使用できません");
				return;
			}
			DialogResult result = MessageBox.Show("ERBファイルを読み直します", "ERBファイル読み直し", MessageBoxButtons.OKCancel);
			if (result != DialogResult.OK)
				return;
			this.ReloadErb();

		}

		private void mainPicBox_Paint(object sender, PaintEventArgs e)
		{
			if (console == null)
				return;
			console.rePaint(e.Graphics);
			//if (!StaticConfig.UseImageBuffer)
			//{
			//    e.Graphics.Clear(StaticConfig.BackColor);
			//    console.RefreshStrings(true, e.Graphics);
			//}
		}

		private void ログを保存するSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (console == null)
				return;
			saveFileDialog.InitialDirectory = Program.ExeDir;
			DateTime time = DateTime.Now;
			string fname = time.ToString("yyyyMMdd-HHmmss");
			fname += ".log";
			saveFileDialog.FileName = fname;
			DialogResult result = saveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				console.outputLog(Path.GetFileName(saveFileDialog.FileName));
			}
		}

		private void ログをクリップボードにコピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				ClipBoardDialog dialog = new ClipBoardDialog();
				dialog.Setup(this, console);
				dialog.ShowDialog();
			}
			catch (Exception)
			{
				MessageBox.Show("予期せぬエラーが発生したためクリップボードを開けません");
				return;
			}
		}

		private void ファイルを読み直すFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (console == null)
				return;
			if (console.IsInitializing)
			{
				MessageBox.Show("起動中には使用できません");
				return;
			}
			DialogResult result = openFileDialog.ShowDialog();
			List<string> filepath = new List<string>();
			if (result == DialogResult.OK)
			{
				foreach (string fname in openFileDialog.FileNames)
				{
					if (!File.Exists(fname))
					{
						MessageBox.Show("ファイルがありません", "File Not Found");
						return;
					}
					if (Path.GetExtension(fname).ToUpper() != ".ERB")
					{
						MessageBox.Show("ERBファイル以外は読み込めません", "ファイル形式エラー");
						return;
					}
					if (fname.StartsWith(Program.ErbDir, StringComparison.OrdinalIgnoreCase))
						filepath.Add(Program.ErbDir + fname.Substring(Program.ErbDir.Length));
					else
						filepath.Add(fname);
				}
				console.ReloadPartialErb(filepath);
			}
		}

		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Config.UseKeyMacro)
				KeyMacro.SaveMacro();
			//ほっとしても勝手に閉じるが、その場合はDebugDialogのClosingイベントが発生しない
			if ((Program.DebugMode) && (console.DebugDialog != null) && (console.DebugDialog.Created))
				console.DebugDialog.Close();
		}

		private void フォルダを読み直すFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (console == null)
				return;
			if (console.IsInitializing)
			{
				MessageBox.Show("起動中には使用できません");
				return;
			}
			List<KeyValuePair<string, string>> filepath = new List<KeyValuePair<string, string>>();
			if (folderSelectDialog.ShowDialog() == DialogResult.OK)
			{
				console.ReloadFolder(folderSelectDialog.SelectedPath);
			}
		}

		void richTextBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//if (!Config.UseMouse)
			//	return;
			if (!vScrollBar.Enabled)
				return;
			if (console == null)
				return;
			//e.Deltaには大きな値が入っているので符号のみ採用する
			int move = -Math.Sign(e.Delta) * vScrollBar.SmallChange * Config.ScrollHeight;
			//スクロールが必要ないならリターンする
			if ((vScrollBar.Value == vScrollBar.Maximum && move > 0) || (vScrollBar.Value == vScrollBar.Minimum && move < 0))
				return;
			int value = vScrollBar.Value + move;
			if (value >= vScrollBar.Maximum)
				vScrollBar.Value = vScrollBar.Maximum;
			else if (value <= vScrollBar.Minimum)
				vScrollBar.Value = vScrollBar.Minimum;
			else
				vScrollBar.Value = value;
			//上端でも下端でもないなら描画を控えめに。
			console.RefreshStrings((vScrollBar.Value == vScrollBar.Maximum) || (vScrollBar.Value == vScrollBar.Minimum));
			//ボタンとの関係をチェック
			if (Config.UseMouse)
				console.MoveMouse(mainPicBox.PointToClient(Cursor.Position));
		}

		private bool textBox_flag = true;
		private string last_inputed = "";

		public void update_lastinput()
		{
			last_inputed = richTextBox1.Text;
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			if (console == null)
				return;
			if (!textBox_flag)
				return;
			if (!console.IsWaintingOnePhrase && !console.IsWaitAnyKeyAndGo)
				return;
			if (string.IsNullOrEmpty(richTextBox1.Text))
				return;
			if (changeTextbyMouse)
			{
				changeTextbyMouse = false;
				//if (richTextBox1.Text.Length > 1)
				//    richTextBox1.Text = richTextBox1.Text.Remove(1);
				return;
			}
			textBox_flag = false;
			if (console.IsWaitAnyKeyAndGo)
			{
				richTextBox1.Clear();
				last_inputed = "";
			}
			//if (richTextBox1.Text.Length > 1)
			//    richTextBox1.Text = richTextBox1.Text.Remove(1);
			PressEnterKey(false, null);
			textBox_flag = true;
		}

		private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (console == null)
				return;
			if ((int)e.KeyData == (int)Keys.PageUp || (int)e.KeyData == (int)Keys.PageDown)
			{
				e.SuppressKeyPress = true;
				int move = 10;
				if ((int)e.KeyData == (int)Keys.PageUp)
					move *= -1;
				//スクロールが必要ないならリターンする
				if ((vScrollBar.Value == vScrollBar.Maximum && move > 0) || (vScrollBar.Value == vScrollBar.Minimum && move < 0))
					return;
				int value = vScrollBar.Value + move;
				if (value >= vScrollBar.Maximum)
					vScrollBar.Value = vScrollBar.Maximum;
				else if (value <= vScrollBar.Minimum)
					vScrollBar.Value = vScrollBar.Minimum;
				else
					vScrollBar.Value = value;
				//上端でも下端でもないなら描画を控えめに。
				console.RefreshStrings((vScrollBar.Value == vScrollBar.Maximum) || (vScrollBar.Value == vScrollBar.Minimum));
				return;
			}
			else if (vScrollBar.Value != vScrollBar.Maximum)
			{
				vScrollBar.Value = vScrollBar.Maximum;
				console.RefreshStrings(true);
			}
			if (e.KeyCode == Keys.Return)
			{
				e.SuppressKeyPress = true;
				if (!console.IsRunning)
					PressEnterKey(false, null);
				return;
			}
			if (e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				if (!console.IsRunning)
					PressEnterKey(true, null);
				return;
			}
			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Home || e.KeyCode == Keys.Back)
			{
				if ((richTextBox1.SelectionStart == 0 && richTextBox1.SelectedText.Length == 0) || richTextBox1.Text.Length == 0)
				{
					e.SuppressKeyPress = true;
					return;
				}
			}
			if (e.KeyCode == Keys.Right || e.KeyCode == Keys.End)
			{
				if (richTextBox1.SelectionStart == richTextBox1.Text.Length || richTextBox1.Text.Length == 0)
				{
					e.SuppressKeyPress = true;
					return;
				}
			}
			if (e.KeyCode == Keys.Up)
			{
				e.SuppressKeyPress = true;
				if (console.IsRunning)
					return;
				movePrev(-1);
				return;
			}
			if (e.KeyCode == Keys.Down)
			{
				e.SuppressKeyPress = true;
				if (console.IsRunning)
					return;
				movePrev(1);
				return;
			}
			if (e.KeyCode == Keys.Insert)
			{
				e.SuppressKeyPress = true;
				return;
			}
		}

		private void デバッグウインドウを開くToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.DebugMode)
				return;
			console.OpenDebugDialog();
		}

		private void デバッグ情報の更新ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.DebugMode)
				return;
			if ((console.DebugDialog != null) && (console.DebugDialog.Created))
				console.DebugDialog.UpdateData();
		}

		private void AutoVerbMenu_Opened(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning))
			{
				切り取り.Enabled = false;
				コピー.Enabled = false;
				貼り付け.Enabled = false;
				実行.Enabled = false;
				削除.Enabled = false;
				マクロToolStripMenuItem.Enabled = false;
				for (int i = 0; i < macroMenuItems.Length; i++)
					macroMenuItems[i].Enabled = false;
				return;
			}
			実行.Enabled = true;
			if (Config.UseKeyMacro)
			{
				マクロToolStripMenuItem.Enabled = true;
				for (int i = 0; i < macroMenuItems.Length; i++)
					macroMenuItems[i].Enabled = KeyMacro.GetMacro(macroMenuItems[i].ShortcutKeys).Length > 0;
				//マクロ01ToolStripMenuItem.Enabled = KeyMacro.MacroF1.Length > 0;
				//マクロ02ToolStripMenuItem.Enabled = KeyMacro.MacroF2.Length > 0;
				//マクロ03ToolStripMenuItem.Enabled = KeyMacro.MacroF3.Length > 0;
				//マクロ04ToolStripMenuItem.Enabled = KeyMacro.MacroF4.Length > 0;
				//マクロ05ToolStripMenuItem.Enabled = KeyMacro.MacroF5.Length > 0;
				//マクロ06ToolStripMenuItem.Enabled = KeyMacro.MacroF6.Length > 0;
				//マクロ07ToolStripMenuItem.Enabled = KeyMacro.MacroF7.Length > 0;
				//マクロ08ToolStripMenuItem.Enabled = KeyMacro.MacroF8.Length > 0;
				//マクロ09ToolStripMenuItem.Enabled = KeyMacro.MacroF9.Length > 0;
				//マクロ10ToolStripMenuItem.Enabled = KeyMacro.MacroF10.Length > 0;
				//マクロ11ToolStripMenuItem.Enabled = KeyMacro.MacroF11.Length > 0;
				//マクロ12ToolStripMenuItem.Enabled = KeyMacro.MacroF12.Length > 0;
			}
			else
			{
				マクロToolStripMenuItem.Enabled = false;
				for (int i = 0; i < macroMenuItems.Length; i++)
					macroMenuItems[i].Enabled = false;
			}
			if (richTextBox1.SelectedText.Length > 0)
			{
				切り取り.Enabled = true;
				コピー.Enabled = true;
				削除.Enabled = true;
			}
			else
			{
				切り取り.Enabled = false;
				コピー.Enabled = false;
				削除.Enabled = false;
			}
			if (Clipboard.ContainsText())
				貼り付け.Enabled = true;
			else
				貼り付け.Enabled = false;

		}

		private void 切り取り_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning) || !切り取り.Enabled)
				return;
			if (richTextBox1.SelectedText.Length > 0)
				richTextBox1.Cut();
		}

		private void コピー_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning) || !コピー.Enabled)
				return;
			else if (richTextBox1.SelectedText.Length > 0)
				richTextBox1.Copy();
		}

		private void 貼り付け_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning) || !貼り付け.Enabled)
				return;
			if (Clipboard.GetDataObject() != null && Clipboard.ContainsText())
			{
				if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
					//Clipboard.SetText(Clipboard.GetText(TextDataFormat.UnicodeText));
					richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
				//richTextBox1.Paste();
				//if (richTextBox1.SelectedText.Length > 0)
				//    richTextBox1.SelectedText = "";
				//richTextBox1.AppendText(Clipboard.GetText());
			}
		}

		private void 削除_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning) || !削除.Enabled)
				return;
			if (richTextBox1.SelectedText.Length > 0)
				richTextBox1.SelectedText = "";
		}

		private void 実行_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning) || !実行.Enabled)
				return;
			PressEnterKey(false, null);
		}


		private void マクロToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((console == null) || (console.IsRunning))
				return;
			if (!Config.UseKeyMacro)
				return;
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			string macro = KeyMacro.GetMacro(item.ShortcutKeys);
			if (macro.Length > 0)
			{
				richTextBox1.Text = macro;
				this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
			}
		}

		private void mainPicBox_Click(object sender, EventArgs e)
		{

		}

		private static MinorShift.Emuera.Picture.CreateRegion createRegion = new Picture.CreateRegion();
		//private static Bitmap bmp2;

		private static Emuera.Picture.PaintBoxImage pbi = new Picture.PaintBoxImage();
		private static PictureBox PicImage = SavePicBox.picBox;
		//private static int click_Times = 0;
		//private static bool scaleH;
		//private static bool scaleT;

		public void ImageOpen(string fileName)
		{

			if (pictureBox1.Image != null) { pictureBox1.Image.Dispose(); }
			pictureBox1 = SavePicBox.picBox;

			SaveControl.c = pictureBox1.Parent;
			pictureBox1.Parent = mainPicBox;

			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;

			System.Drawing.Size tSize = mainPicBox.ClientSize;
			pictureBox1.Size = tSize;

			pictureBox1.BackColor = Color.Transparent;

			Bitmap bmp = new Bitmap(fileName);

			//比率合せ
			/*
			float aspectPicBox = tSize.Height / tSize.Width;
			float aspectBmp = bmp.Height / bmp.Width;
			//縦に合わせる
			if (aspectBmp >= aspectPicBox)
			{
                
				nSize.Height = tSize.Height / bmp.Height;
				nSize.Width = nSize.Height / (int)(1 / aspectBmp) / bmp.Width;
				nw = nSize.Width;
				nh = nSize.Height;
					//(int)(tSize.Width * aspectBmp);
			}
			else
			{
				//scaleT = true;
				//nSize.Width = tSize.Width;
				//nSize.Height = tSize.Width * aspectBmp;
			}
			Bitmap bmp2 = new Bitmap(bmp, bmp.Width * 2, bmp.Height * 2);
            
			*/
			//比率合せ終了

			//位置合わせ開始
			pictureBox1.Size = bmp.Size;
			pictureBox1.Left = tSize.Width / 2 - bmp.Width / 2;
			//位置合わせ終了

			//表示前にPictureBoxのリージョン準備
			pictureBox1.Region = createRegion.CreateRegionFromBitmap(bmp);

			bmp.MakeTransparent();
			pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
			pictureBox1.Image = bmp;
			pictureBox1.InitialImage = bmp;

		}

		private void pictureBox1_DoubleClick(object sender, EventArgs e)
		{
			if (pictureBox1.Image != null)
			{
				pictureBox1.Parent = SaveControl.c;
				pictureBox1.Image.Dispose();
				pictureBox1.Image = null;
				pictureBox1.Region.Dispose();
				mainPicBox.BringToFront();
				pictureBox1.SendToBack();
				pictureBox1.Parent = this;

			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			if (pictureBox1.Image != null)
			{


			}
		}

		public void ImageClose()
		{
			if (pictureBox1.Image != null)
			{
				pictureBox1.Parent = SaveControl.c;
				pictureBox1.Image.Dispose();
				pictureBox1.Image = null;
				pictureBox1.Region.Dispose();
				mainPicBox.BringToFront();
				pictureBox1.SendToBack();
				pictureBox1.Parent = this;

			}

		}

		public class SaveControl
		{
			public static Control c = null;
		}

		public class SavePicBox
		{
			public static PictureBox picBox = null;
		}

		private void ボリュームToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowVolumeDialog();
		}
		public void ShowVolumeDialog()
		{
			VolumeDialog dialog = new VolumeDialog();
			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.ShowDialog();
		}

	}
}