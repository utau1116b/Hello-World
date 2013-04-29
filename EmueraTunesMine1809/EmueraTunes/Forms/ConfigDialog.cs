using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace MinorShift.Emuera.Forms
{
	[global::System.Reflection.Obfuscation(Exclude=false)]
	internal enum ConfigDialogResult
	{
		Cancel = 0,
		Save = 1,
		SaveReboot = 2,
	}

	internal partial class ConfigDialog : Form
	{
		public ConfigDialog()
		{
			InitializeComponent();
			numericUpDown2.Maximum = 10000;
			numericUpDown3.Maximum = 10000;
			numericUpDown4.Maximum = 1000000;
            numericUpDown4.Minimum = 500;
			numericUpDown1.Maximum = 100;
			numericUpDown9.Maximum = 100;
			numericUpDown6.Maximum = 144;
			numericUpDown7.Maximum = 240;
            numericUpDown8.Minimum = 1;
			numericUpDown8.Maximum = 10;
			numericUpDown5.Maximum = 144;
            numericUpDown5.Minimum = 8;
			numericUpDown10.Maximum = 100000;
            numericUpDown11.Minimum = 20;
            numericUpDown11.Maximum = 80;
			numericUpDownPosX.Maximum = 10000;
			numericUpDownPosY.Maximum = 10000;

		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			SaveConfig();
			Result = ConfigDialogResult.Save;
			this.Close();
		}

		private void buttonReboot_Click(object sender, EventArgs e)
		{
			SaveConfig();
			Result = ConfigDialogResult.SaveReboot;
			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Result = ConfigDialogResult.Cancel;
			this.Close();
		}
		public ConfigDialogResult Result = ConfigDialogResult.Cancel;

		void setCheckBox(CheckBox checkbox, ConfigCode code)
		{
			ConfigItem<bool> item = (ConfigItem<bool>)ConfigData.Instance.GetConfigItem(code);
			checkbox.Checked = item.Value;
			checkbox.Enabled = !item.Fixed;
		}
		void setNumericUpDown(NumericUpDown updown, ConfigCode code)
		{
			ConfigItem<int> item = (ConfigItem<int>)ConfigData.Instance.GetConfigItem(code);
			decimal value = item.Value;
			if (updown.Maximum < value)
				updown.Maximum = value;
			if (updown.Minimum > value)
				updown.Minimum = value;
			updown.Value = value;
			updown.Enabled = !item.Fixed;
		}

		void setColorBox(ColorBox colorBox, ConfigCode code)
		{
			ConfigItem<Color> item = (ConfigItem<Color>)ConfigData.Instance.GetConfigItem(code);
			colorBox.SelectingColor = item.Value;
			colorBox.Enabled = !item.Fixed;
		}
		void setTextBox(TextBox textBox, ConfigCode code)
		{
			ConfigItem<string> item = (ConfigItem<string>)ConfigData.Instance.GetConfigItem(code);
			textBox.Text = item.Value;
			textBox.Enabled = !item.Fixed;
		}

		MainWindow parent = null;
		public void SetConfig(MainWindow mainWindow)
		{
			parent = mainWindow;
			ConfigData config = ConfigData.Instance;
			setCheckBox(checkBox1, ConfigCode.IgnoreCase);
			setCheckBox(checkBox2, ConfigCode.UseRenameFile);
			setCheckBox(checkBox3, ConfigCode.UseMouse);
			setCheckBox(checkBox4, ConfigCode.UseMenu);
			setCheckBox(checkBox5, ConfigCode.UseDebugCommand);
			setCheckBox(checkBox6, ConfigCode.AllowMultipleInstances);
			setCheckBox(checkBox7, ConfigCode.AutoSave);
			setCheckBox(checkBox8, ConfigCode.SizableWindow);
			setCheckBox(checkBox9, ConfigCode.UseImageBuffer);
			setCheckBox(checkBox10, ConfigCode.UseReplaceFile);
			setCheckBox(checkBox11, ConfigCode.IgnoreUncalledFunction);
			//setCheckBox(checkBox12, ConfigCode.ReduceFormattedStringOnLoad);
			setCheckBox(checkBox13, ConfigCode.DisplayReport);
			setCheckBox(checkBox14, ConfigCode.ButtonWrap);
			setCheckBox(checkBox15, ConfigCode.SearchSubdirectory);
			setCheckBox(checkBox16, ConfigCode.SortWithFilename);
			setCheckBox(checkBox17, ConfigCode.SetWindowPos);
			setCheckBox(checkBox18, ConfigCode.UseKeyMacro);
			setCheckBox(checkBox20, ConfigCode.AllowFunctionOverloading);
			setCheckBox(checkBox19, ConfigCode.WarnFunctionOverloading);
            setCheckBox(checkBox21, ConfigCode.WindowMaximixed);
            setCheckBox(checkBox22, ConfigCode.WarnNormalFunctionOverloading);
			setCheckBox(checkBox23, ConfigCode.WarnBackCompatibility);
			setCheckBox(checkBoxCompatiErrorLine, ConfigCode.CompatiErrorLine);
			setCheckBox(checkBoxCompatiCALLNAME, ConfigCode.CompatiCALLNAME);
			setCheckBox(checkBox24, ConfigCode.UseSaveFolder);
			setCheckBox(checkBox27, ConfigCode.SystemSaveInUTF8);
			setCheckBox(checkBoxCompatiRAND, ConfigCode.CompatiRAND);
			setCheckBox(checkBoxCompatiLinefeedAs1739, ConfigCode.CompatiLinefeedAs1739);
			setCheckBox(checkBoxFuncNoIgnoreCase, ConfigCode.CompatiFunctionNoignoreCase);
			setCheckBox(checkBoxSystemFullSpace, ConfigCode.SystemAllowFullSpace);
			setCheckBox(checkBox12, ConfigCode.CompatiFuncArgOptional);
			setCheckBox(checkBox25, ConfigCode.CompatiFuncArgAutoConvert);
			setCheckBox(checkBox26, ConfigCode.SystemSaveInBinary);
			setNumericUpDown(numericUpDown2, ConfigCode.WindowX);
			setNumericUpDown(numericUpDown3, ConfigCode.WindowY);
			setNumericUpDown(numericUpDown4, ConfigCode.MaxLog);
			setNumericUpDown(numericUpDown1, ConfigCode.PrintCPerLine);
			setNumericUpDown(numericUpDown9, ConfigCode.PrintCLength);
			setNumericUpDown(numericUpDown6, ConfigCode.LineHeight);
			setNumericUpDown(numericUpDown7, ConfigCode.FPS);
			setNumericUpDown(numericUpDown8, ConfigCode.ScrollHeight);
			setNumericUpDown(numericUpDown5, ConfigCode.FontSize);
			setNumericUpDown(numericUpDown10, ConfigCode.InfiniteLoopAlertTime);
            setNumericUpDown(numericUpDown11, ConfigCode.SaveDataNos);

			setNumericUpDown(numericUpDownPosX, ConfigCode.WindowPosX);
			setNumericUpDown(numericUpDownPosY, ConfigCode.WindowPosY);

			setColorBox(colorBoxFG, ConfigCode.ForeColor);
			setColorBox(colorBoxBG, ConfigCode.BackColor);
			setColorBox(colorBoxSelecting, ConfigCode.FocusColor);
			setColorBox(colorBoxBacklog, ConfigCode.LogColor);

			ConfigItem<TextDrawingMode> itemTDM = (ConfigItem<TextDrawingMode>)ConfigData.Instance.GetConfigItem(ConfigCode.TextDrawingMode);
			switch (itemTDM.Value)
			{
				case TextDrawingMode.WINAPI:
					comboBoxTextDrawingMode.SelectedIndex = 0; break;
				case TextDrawingMode.TEXTRENDERER:
					comboBoxTextDrawingMode.SelectedIndex = 1; break;
				case TextDrawingMode.GRAPHICS:
					comboBoxTextDrawingMode.SelectedIndex = 2; break;
			}
			comboBoxTextDrawingMode.Enabled = !itemTDM.Fixed;

			ConfigItem<string> itemStr = (ConfigItem<string>)ConfigData.Instance.GetConfigItem(ConfigCode.FontName);
			string fontname = itemStr.Value;
			int nameIndex = comboBox2.Items.IndexOf(fontname);
			if (nameIndex >= 0)
				comboBox2.SelectedIndex = nameIndex;
			else
			{
				comboBox2.Text = fontname;
				//nameIndex = comboBox2.Items.IndexOf("ＭＳ ゴシック");
				//if (nameIndex >= 0)
				//    comboBox2.SelectedIndex = nameIndex;
			}
			comboBox2.Enabled = !itemStr.Fixed;


			ConfigItem<ReduceArgumentOnLoadFlag> itemRA = (ConfigItem<ReduceArgumentOnLoadFlag>)ConfigData.Instance.GetConfigItem(ConfigCode.ReduceArgumentOnLoad);
			switch (itemRA.Value)
			{
				case ReduceArgumentOnLoadFlag.NO:
					comboBoxReduceArgumentOnLoad.SelectedIndex = 0; break;
				case ReduceArgumentOnLoadFlag.ONCE:
					comboBoxReduceArgumentOnLoad.SelectedIndex = 1; break;
				case ReduceArgumentOnLoadFlag.YES:
					comboBoxReduceArgumentOnLoad.SelectedIndex = 2; break;
			}
			comboBoxReduceArgumentOnLoad.Enabled = !itemRA.Fixed;


			ConfigItem<int> itemInt = (ConfigItem<int>)ConfigData.Instance.GetConfigItem(ConfigCode.DisplayWarningLevel);
			if (itemInt.Value <= 0)
				comboBox5.SelectedIndex = 0;
			else if (itemInt.Value >= 3)
				comboBox5.SelectedIndex = 3;
			else
				comboBox5.SelectedIndex = itemInt.Value;
			comboBox5.Enabled = !itemInt.Fixed;


			ConfigItem<DisplayWarningFlag> itemDWF = (ConfigItem<DisplayWarningFlag>)ConfigData.Instance.GetConfigItem(ConfigCode.FunctionNotFoundWarning);
			switch (itemDWF.Value)
			{
				case DisplayWarningFlag.IGNORE:
					comboBox3.SelectedIndex = 0; break;
				case DisplayWarningFlag.LATER:
					comboBox3.SelectedIndex = 1; break;
				case DisplayWarningFlag.ONCE:
					comboBox3.SelectedIndex = 2; break;
				case DisplayWarningFlag.DISPLAY:
					comboBox3.SelectedIndex = 3; break;
			}
			comboBox3.Enabled = !itemDWF.Fixed;

			itemDWF = (ConfigItem<DisplayWarningFlag>)ConfigData.Instance.GetConfigItem(ConfigCode.FunctionNotCalledWarning);
			switch (itemDWF.Value)
			{
				case DisplayWarningFlag.IGNORE:
					comboBox4.SelectedIndex = 0; break;
				case DisplayWarningFlag.LATER:
					comboBox4.SelectedIndex = 1; break;
				case DisplayWarningFlag.ONCE:
					comboBox4.SelectedIndex = 2; break;
				case DisplayWarningFlag.DISPLAY:
					comboBox4.SelectedIndex = 3; break;
			}
			comboBox4.Enabled = !itemDWF.Fixed;

            ConfigItem<UseLanguage> itemLang = (ConfigItem<UseLanguage>)ConfigData.Instance.GetConfigItem(ConfigCode.useLanguage);
            switch (itemLang.Value)
            {
                case UseLanguage.JAPANESE:
                    comboBox1.SelectedIndex = 0; break;
                case UseLanguage.KOREAN:
                    comboBox1.SelectedIndex = 1; break;
                case UseLanguage.CHINESE_HANS:
                    comboBox1.SelectedIndex = 2; break;
                case UseLanguage.CHINESE_HANT:
                    comboBox1.SelectedIndex = 3; break;
            }

            textBox1.Text = Config.TextEditor;
            textBox2.Text = Config.EditorArg;
		}

		private void SaveConfig()
		{
			ConfigData config = ConfigData.Instance.Copy();
			config.GetConfigItem(ConfigCode.IgnoreCase).SetValue<bool>(checkBox1.Checked);
			config.GetConfigItem(ConfigCode.UseRenameFile).SetValue<bool>(checkBox2.Checked);
			config.GetConfigItem(ConfigCode.UseMouse).SetValue<bool>(checkBox3.Checked);
			config.GetConfigItem(ConfigCode.UseMenu).SetValue<bool>(checkBox4.Checked);
			config.GetConfigItem(ConfigCode.UseDebugCommand).SetValue<bool>(checkBox5.Checked);
			config.GetConfigItem(ConfigCode.AllowMultipleInstances).SetValue<bool>(checkBox6.Checked);
			config.GetConfigItem(ConfigCode.AutoSave).SetValue<bool>(checkBox7.Checked);
			config.GetConfigItem(ConfigCode.SizableWindow).SetValue<bool>(checkBox8.Checked);
			config.GetConfigItem(ConfigCode.UseImageBuffer).SetValue<bool>(checkBox9.Checked);
			config.GetConfigItem(ConfigCode.UseReplaceFile).SetValue<bool>(checkBox10.Checked);
			config.GetConfigItem(ConfigCode.IgnoreUncalledFunction).SetValue<bool>(checkBox11.Checked);
			//config.GetConfigItem(ConfigCode.ReduceFormattedStringOnLoad).SetValue<bool>(checkBox12.Checked);
			config.GetConfigItem(ConfigCode.DisplayReport).SetValue<bool>(checkBox13.Checked);
			config.GetConfigItem(ConfigCode.ButtonWrap).SetValue<bool>(checkBox14.Checked);
			config.GetConfigItem(ConfigCode.SearchSubdirectory).SetValue<bool>(checkBox15.Checked);
			config.GetConfigItem(ConfigCode.SortWithFilename).SetValue<bool>(checkBox16.Checked);
			config.GetConfigItem(ConfigCode.SetWindowPos).SetValue<bool>(checkBox17.Checked);
			config.GetConfigItem(ConfigCode.UseKeyMacro).SetValue<bool>(checkBox18.Checked);
			config.GetConfigItem(ConfigCode.AllowFunctionOverloading).SetValue<bool>(checkBox20.Checked);
			config.GetConfigItem(ConfigCode.WarnFunctionOverloading).SetValue<bool>(checkBox19.Checked);
            config.GetConfigItem(ConfigCode.WindowMaximixed).SetValue<bool>(checkBox21.Checked);
            config.GetConfigItem(ConfigCode.WarnNormalFunctionOverloading).SetValue<bool>(checkBox22.Checked);
			config.GetConfigItem(ConfigCode.WarnBackCompatibility).SetValue<bool>(checkBox23.Checked);
			config.GetConfigItem(ConfigCode.CompatiErrorLine).SetValue<bool>(checkBoxCompatiErrorLine.Checked);
			config.GetConfigItem(ConfigCode.CompatiCALLNAME).SetValue<bool>(checkBoxCompatiCALLNAME.Checked);
			config.GetConfigItem(ConfigCode.UseSaveFolder).SetValue<bool>(checkBox24.Checked);
			config.GetConfigItem(ConfigCode.CompatiRAND).SetValue<bool>(checkBoxCompatiRAND.Checked);
			config.GetConfigItem(ConfigCode.CompatiLinefeedAs1739).SetValue<bool>(checkBoxCompatiLinefeedAs1739.Checked);
			config.GetConfigItem(ConfigCode.SystemSaveInUTF8).SetValue<bool>(checkBox27.Checked);

			config.GetConfigItem(ConfigCode.CompatiFuncArgOptional).SetValue<bool>(checkBox12.Checked);
			config.GetConfigItem(ConfigCode.CompatiFuncArgAutoConvert).SetValue<bool>(checkBox25.Checked);
			config.GetConfigItem(ConfigCode.SystemSaveInBinary).SetValue<bool>(checkBox26.Checked);

			config.GetConfigItem(ConfigCode.CompatiFunctionNoignoreCase).SetValue<bool>(checkBoxFuncNoIgnoreCase.Checked);
			config.GetConfigItem(ConfigCode.SystemAllowFullSpace).SetValue<bool>(checkBoxSystemFullSpace.Checked);


			config.GetConfigItem(ConfigCode.WindowX).SetValue<int>((int)numericUpDown2.Value);
			config.GetConfigItem(ConfigCode.WindowY).SetValue<int>((int)numericUpDown3.Value);
			config.GetConfigItem(ConfigCode.MaxLog).SetValue<int>((int)numericUpDown4.Value);
			config.GetConfigItem(ConfigCode.PrintCPerLine).SetValue<int>((int)numericUpDown1.Value);
			config.GetConfigItem(ConfigCode.PrintCLength).SetValue<int>((int)numericUpDown9.Value);
			config.GetConfigItem(ConfigCode.LineHeight).SetValue<int>((int)numericUpDown6.Value);
			config.GetConfigItem(ConfigCode.FPS).SetValue<int>((int)numericUpDown7.Value);
			config.GetConfigItem(ConfigCode.ScrollHeight).SetValue<int>((int)numericUpDown8.Value);
			config.GetConfigItem(ConfigCode.InfiniteLoopAlertTime).SetValue<int>((int)numericUpDown10.Value);
            config.GetConfigItem(ConfigCode.SaveDataNos).SetValue<int>((int)numericUpDown11.Value);

			config.GetConfigItem(ConfigCode.WindowPosX).SetValue<int>((int)numericUpDownPosX.Value);
			config.GetConfigItem(ConfigCode.WindowPosY).SetValue<int>((int)numericUpDownPosY.Value);

			config.GetConfigItem(ConfigCode.FontSize).SetValue<int>((int)numericUpDown5.Value);
			int nameIndex = comboBox2.SelectedIndex;
			if (nameIndex >= 0)
				config.GetConfigItem(ConfigCode.FontName).SetValue<string>((string)comboBox2.SelectedItem);
			else
				config.GetConfigItem(ConfigCode.FontName).SetValue<string>(comboBox2.Text);



			config.GetConfigItem(ConfigCode.ForeColor).SetValue<Color>(colorBoxFG.SelectingColor);
			config.GetConfigItem(ConfigCode.BackColor).SetValue<Color>(colorBoxBG.SelectingColor);
			config.GetConfigItem(ConfigCode.FocusColor).SetValue<Color>(colorBoxSelecting.SelectingColor);
			config.GetConfigItem(ConfigCode.LogColor).SetValue<Color>(colorBoxBacklog.SelectingColor);

			switch (comboBoxTextDrawingMode.SelectedIndex)
			{
				case 0:
					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue<TextDrawingMode>(TextDrawingMode.WINAPI); break;
				case 1:
					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue<TextDrawingMode>(TextDrawingMode.TEXTRENDERER); break;
				case 2:
					config.GetConfigItem(ConfigCode.TextDrawingMode).SetValue<TextDrawingMode>(TextDrawingMode.GRAPHICS); break;
			}

			switch (comboBoxReduceArgumentOnLoad.SelectedIndex)
			{
				case 0:
					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue<ReduceArgumentOnLoadFlag>(ReduceArgumentOnLoadFlag.NO); break;
				case 1:
					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue<ReduceArgumentOnLoadFlag>(ReduceArgumentOnLoadFlag.ONCE); break;
				case 2:
					config.GetConfigItem(ConfigCode.ReduceArgumentOnLoad).SetValue<ReduceArgumentOnLoadFlag>(ReduceArgumentOnLoadFlag.YES); break;
			}
			config.GetConfigItem(ConfigCode.DisplayWarningLevel).SetValue<int>(comboBox5.SelectedIndex);


			switch (comboBox3.SelectedIndex)
			{
				case 0:
					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.IGNORE); break;
				case 1:
					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.LATER); break;
				case 2:
					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.ONCE); break;
				case 3:
					config.GetConfigItem(ConfigCode.FunctionNotFoundWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.DISPLAY); break;
			}
			switch (comboBox4.SelectedIndex)
			{
				case 0:
					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.IGNORE); break;
				case 1:
					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.LATER); break;
				case 2:
					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.ONCE); break;
				case 3:
					config.GetConfigItem(ConfigCode.FunctionNotCalledWarning).SetValue<DisplayWarningFlag>(DisplayWarningFlag.DISPLAY); break;
			}
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    config.GetConfigItem(ConfigCode.useLanguage).SetValue<UseLanguage>(UseLanguage.JAPANESE); break;
                case 1:
                    config.GetConfigItem(ConfigCode.useLanguage).SetValue<UseLanguage>(UseLanguage.KOREAN); break;
                case 2:
                    config.GetConfigItem(ConfigCode.useLanguage).SetValue<UseLanguage>(UseLanguage.CHINESE_HANS); break;
                case 3:
                    config.GetConfigItem(ConfigCode.useLanguage).SetValue<UseLanguage>(UseLanguage.CHINESE_HANT); break;
            }

            config.GetConfigItem(ConfigCode.TextEditor).SetValue<string>(textBox1.Text);
            config.GetConfigItem(ConfigCode.EditorArgument).SetValue<string>(textBox2.Text);

			config.SaveConfig();
		}


		private void comboBoxReduceArgumentOnLoad_SelectedIndexChanged(object sender, EventArgs e)
		{
			//いちいち切り替えるのが面倒なのでまとめて却下
			/*if (comboBoxReduceArgumentOnLoad.SelectedIndex == 0)
			{
				comboBox3.Enabled = false;
				comboBox4.Enabled = false;
				comboBox5.Enabled = false;
				checkBox12.Enabled = false;
				checkBox11.Enabled = false;
			}
			else
			{
				comboBox3.Enabled = true;
				comboBox4.Enabled = true;
				comboBox5.Enabled = true;
				checkBox12.Enabled = true;
				checkBox11.Enabled = true;
			}*/


		}


		private void button1_Click(object sender, EventArgs e)
		{
			if (parent == null)
				return;
			if (numericUpDown2.Enabled)
				numericUpDown2.Value = parent.MainPicBox.Width;
			if (numericUpDown3.Enabled)
				numericUpDown3.Value = parent.MainPicBox.Height + (int)Config.LineHeight;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (parent == null)
				return;
			if (numericUpDownPosX.Enabled)
			{
				if (numericUpDownPosX.Maximum < parent.Location.X)
					numericUpDownPosX.Maximum = parent.Location.X;
				if (numericUpDownPosX.Minimum > parent.Location.X)
					numericUpDownPosX.Minimum = parent.Location.X;
				numericUpDownPosX.Value = parent.Location.X;
			}
			if (numericUpDownPosY.Enabled)
			{
				if (numericUpDownPosY.Maximum < parent.Location.Y)
					numericUpDownPosY.Maximum = parent.Location.Y;
				if (numericUpDownPosY.Minimum > parent.Location.Y)
					numericUpDownPosY.Minimum = parent.Location.Y;
				numericUpDownPosY.Value = parent.Location.Y;
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (!comboBox2.Enabled)
				return;
			InstalledFontCollection ifc = new InstalledFontCollection();
			foreach (FontFamily ff in ifc.Families)
			{
				if (!ff.IsStyleAvailable(FontStyle.Regular))
					continue;
				if (!ff.IsStyleAvailable(FontStyle.Bold))
					continue;
				if (!ff.IsStyleAvailable(FontStyle.Italic))
					continue;
				if (!ff.IsStyleAvailable(FontStyle.Strikeout))
					continue;
				if (!ff.IsStyleAvailable(FontStyle.Underline))
					continue;
				comboBox2.Items.Add(ff.Name);
			}
			string fontname = comboBox2.Text;
			if (!string.IsNullOrEmpty(fontname))
			{
				int nameIndex = comboBox2.Items.IndexOf(fontname);
				if (nameIndex >= 0)
					comboBox2.SelectedIndex = nameIndex;
			}
		}

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"c:\Program Files";
            openFileDialog1.FileName = "";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
		private void button7_Click(object sender, EventArgs e)
		{//eramaker仕様
			if (checkBoxCompatiErrorLine.Enabled)
				checkBoxCompatiErrorLine.Checked = true;
			if (checkBoxCompatiCALLNAME.Enabled)
				checkBoxCompatiCALLNAME.Checked = true;
			if (checkBoxCompatiRAND.Enabled)
				checkBoxCompatiRAND.Checked = true;
			if (checkBoxFuncNoIgnoreCase.Enabled)
				checkBoxFuncNoIgnoreCase.Checked = true;
			if (checkBoxCompatiLinefeedAs1739.Enabled)
				checkBoxCompatiLinefeedAs1739.Checked = false;
			if (checkBox12.Enabled)
				checkBox12.Checked = false;
			if (checkBox25.Enabled)
				checkBox25.Checked = false;
			
		}

		private void button8_Click(object sender, EventArgs e)
		{//最新Emuera仕様 - 全部false
			if (checkBoxCompatiErrorLine.Enabled)
				checkBoxCompatiErrorLine.Checked = false;
			if (checkBoxCompatiCALLNAME.Enabled)
				checkBoxCompatiCALLNAME.Checked = false;
			if (checkBoxCompatiRAND.Enabled)
				checkBoxCompatiRAND.Checked = false;
			if (checkBoxFuncNoIgnoreCase.Enabled)
				checkBoxFuncNoIgnoreCase.Checked = false;
			if (checkBoxCompatiLinefeedAs1739.Enabled)
				checkBoxCompatiLinefeedAs1739.Checked = false;

			if (checkBox12.Enabled)
				checkBox12.Checked = false;
			if (checkBox25.Enabled)
				checkBox25.Checked = false;
		}
	}
}