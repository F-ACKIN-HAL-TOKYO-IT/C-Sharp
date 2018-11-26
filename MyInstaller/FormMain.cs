using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MyInstaller
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            MyInitializeComponent();
        }
        
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            foreach (object obj in this.splitContainer1.Panel1.Controls)
            {
                Panel panel = (Panel)obj;
                panel.Dock = DockStyle.Fill;
                panel.Enabled = false;
            }
            base.Width = 540;
            base.Height = 300;
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.stage = FormMain.STAGE.PAGE1;
            this.ControlPanel();
        }
        
        private void ControlPanel()
        {
            switch (this.stage)
            {
                case FormMain.STAGE.PAGE1:
                    this.buttonPrev.Enabled = false;
                    this.buttonNext.Enabled = true;
                    this.panel1.Enabled = true;
                    this.panel2.Enabled = false;
                    this.panel1.BringToFront();
                    this.buttonNext.Focus();
                    return;
                case FormMain.STAGE.PAGE2:
                    this.buttonPrev.Enabled = true;
                    this.buttonNext.Enabled = this.checkBoxAcceptLicense.Checked;
                    this.panel1.Enabled = false;
                    this.panel2.Enabled = true;
                    this.panel3.Enabled = false;
                    this.panel2.BringToFront();
                    this.checkBoxAcceptLicense.Focus();
                    return;
                case FormMain.STAGE.PAGE3:
                    this.buttonNext.Enabled = (this.textBoxUserName.TextLength != 0);
                    this.panel2.Enabled = false;
                    this.panel3.Enabled = true;
                    this.panel4.Enabled = false;
                    this.panel3.BringToFront();
                    this.textBoxUserName.Focus();
                    return;
                case FormMain.STAGE.PAGE4:
                    this.buttonNext.Enabled = true;
                    this.panel3.Enabled = false;
                    this.panel4.Enabled = true;
                    this.panel5.Enabled = false;
                    this.panel4.BringToFront();
                    this.buttonNext.Focus();
                    return;
                case FormMain.STAGE.PAGE5:
                    this.buttonNext.Text = "次へ(&N) ＞";
                    this.buttonNext.Enabled = (this.textBoxInstallPath.TextLength != 0);
                    this.panel4.Enabled = false;
                    this.panel5.Enabled = true;
                    this.panel6.Enabled = false;
                    this.panel5.BringToFront();
                    this.textBoxInstallPath.Focus();
                    this.textBoxInstallPath.SelectAll();
                    return;
                case FormMain.STAGE.PAGE6:
                    {
                        if (!Directory.Exists(this.textBoxInstallPath.Text))
                        {
                            if (DialogResult.Yes == MessageBox.Show(this, "指定されたディレクトリが存在しません。自動で作成してもよろしいですか？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                            {
                                try
                                {
                                    Directory.CreateDirectory(this.textBoxInstallPath.Text);
                                    goto IL_24B;
                                }
                                catch
                                {
                                    MessageBox.Show(this, "ディレクトリ作成に失敗しました。ディレクトリパスを見直してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    this.buttonPrev_Click(null, null);
                                    break;
                                }
                            }
                            this.buttonPrev_Click(null, null);
                            return;
                        }
                        IL_24B:
                        this.labelUserName.Text = this.textBoxUserName.Text;
                        string text = "しない";
                        if (this.checkBoxSendErrorReport.Checked)
                        {
                            text = "する";
                        }
                        this.labelSendErrorReport.Text = text;
                        this.labelInstallPath.Text = this.textBoxInstallPath.Text;
                        this.buttonNext.Text = "インストール";
                        this.panel5.Enabled = false;
                        this.panel6.Enabled = true;
                        this.panel7.Enabled = false;
                        this.panel6.BringToFront();
                        this.buttonNext.Focus();
                        return;
                    }
                case FormMain.STAGE.PAGE7:
                    this.buttonPrev.Enabled = false;
                    this.buttonNext.Enabled = false;
                    this.panel6.Enabled = false;
                    this.panel7.Enabled = true;
                    this.panel8.Enabled = false;
                    this.panel7.BringToFront();
                    this.Refresh();
                    while (this.progressBar1.Value != this.progressBar1.Maximum)
                    {
                        this.progressBar1.PerformStep();
                        Thread.Sleep(500);
                    }
                    this.buttonNext_Click(null, null);
                    return;
                case FormMain.STAGE.PAGE8:
                    this.buttonPrev.Enabled = false;
                    this.buttonCancel.Enabled = false;
                    this.buttonNext.Text = "完了";
                    this.buttonNext.Enabled = true;
                    this.panel7.Enabled = false;
                    this.panel8.Enabled = true;
                    this.panel8.BringToFront();
                    this.buttonNext.Focus();
                    return;
                case FormMain.STAGE.PAGE9:
                    File.Create(this.textBoxInstallPath.Text + Path.DirectorySeparatorChar.ToString() + "install.txt");
                    base.Close();
                    break;
                default:
                    return;
            }
        }
        
        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.stage++;
            this.ControlPanel();
        }
        
        private void buttonPrev_Click(object sender, EventArgs e)
        {
            this.stage--;
            this.ControlPanel();
        }
        
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "インストールをキャンセルしてもよろしいですか？", Application.ProductName, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                base.Close();
            }
        }
        
        private void checkBoxAcceptLicense_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonNext.Enabled = this.checkBoxAcceptLicense.Checked;
        }
        
        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            this.buttonNext.Enabled = (this.textBoxUserName.TextLength != 0);
        }
        
        private void buttonInstallPathBrowse_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
            {
                this.textBoxInstallPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
        
        private void textBoxInstallPath_TextChanged(object sender, EventArgs e)
        {
            this.buttonNext.Enabled = (this.textBoxInstallPath.TextLength != 0);
        }

        private void MyInitializeComponent()
        {
            this.splitContainer1 = new SplitContainer();
            this.panel8 = new Panel();
            this.label18 = new Label();
            this.panel7 = new Panel();
            this.progressBar1 = new ProgressBar();
            this.label15 = new Label();
            this.panel6 = new Panel();
            this.labelInstallPath = new Label();
            this.labelSendErrorReport = new Label();
            this.labelUserName = new Label();
            this.label8 = new Label();
            this.label6 = new Label();
            this.label13 = new Label();
            this.label11 = new Label();
            this.panel5 = new Panel();
            this.label3 = new Label();
            this.buttonInstallPathBrowse = new Button();
            this.textBoxInstallPath = new TextBox();
            this.label9 = new Label();
            this.panel4 = new Panel();
            this.checkBoxSendErrorReport = new CheckBox();
            this.label7 = new Label();
            this.panel3 = new Panel();
            this.label1 = new Label();
            this.textBoxUserName = new TextBox();
            this.label5 = new Label();
            this.panel2 = new Panel();
            this.checkBoxAcceptLicense = new CheckBox();
            this.label14 = new Label();
            this.label4 = new Label();
            this.panel1 = new Panel();
            this.label2 = new Label();
            this.buttonCancel = new Button();
            this.buttonNext = new Button();
            this.buttonPrev = new Button();
            this.folderBrowserDialog1 = new FolderBrowserDialog();
            this.label16 = new Label();
            this.splitContainerBase = new SplitContainer();
            ((ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)this.splitContainerBase).BeginInit();
            this.splitContainerBase.Panel1.SuspendLayout();
            this.splitContainerBase.Panel2.SuspendLayout();
            this.splitContainerBase.SuspendLayout();
            base.SuspendLayout();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.panel8);
            this.splitContainer1.Panel1.Controls.Add(this.panel7);
            this.splitContainer1.Panel1.Controls.Add(this.panel6);
            this.splitContainer1.Panel1.Controls.Add(this.panel5);
            this.splitContainer1.Panel1.Controls.Add(this.panel4);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNext);
            this.splitContainer1.Panel2.Controls.Add(this.buttonPrev);
            this.splitContainer1.Size = new Size(845, 700);
            this.splitContainer1.SplitterDistance = 644;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            this.panel8.Controls.Add(this.label18);
            this.panel8.Location = new Point(12, 581);
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(518, 51);
            this.panel8.TabIndex = 7;
            this.label18.AutoSize = true;
            this.label18.Location = new Point(3, 18);
            this.label18.Name = "label18";
            this.label18.Size = new Size(171, 12);
            this.label18.TabIndex = 2;
            this.label18.Text = "インストールが正常に完了しました。";
            this.panel7.Controls.Add(this.progressBar1);
            this.panel7.Controls.Add(this.label15);
            this.panel7.Location = new Point(12, 507);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(518, 68);
            this.panel7.TabIndex = 6;
            this.progressBar1.Location = new Point(22, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(470, 16);
            this.progressBar1.TabIndex = 3;
            this.label15.AutoSize = true;
            this.label15.Location = new Point(6, 17);
            this.label15.Name = "label15";
            this.label15.Size = new Size(72, 12);
            this.label15.TabIndex = 2;
            this.label15.Text = "インストール中";
            this.panel6.Controls.Add(this.labelInstallPath);
            this.panel6.Controls.Add(this.labelSendErrorReport);
            this.panel6.Controls.Add(this.labelUserName);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Location = new Point(11, 384);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(519, 117);
            this.panel6.TabIndex = 5;
            this.labelInstallPath.AutoSize = true;
            this.labelInstallPath.Location = new Point(147, 88);
            this.labelInstallPath.Name = "labelInstallPath";
            this.labelInstallPath.Size = new Size(169, 12);
            this.labelInstallPath.TabIndex = 7;
            this.labelInstallPath.Text = "[指定されたインストールディレクトリ]";
            this.labelSendErrorReport.AutoSize = true;
            this.labelSendErrorReport.Location = new Point(147, 64);
            this.labelSendErrorReport.Name = "labelSendErrorReport";
            this.labelSendErrorReport.Size = new Size(73, 12);
            this.labelSendErrorReport.TabIndex = 6;
            this.labelSendErrorReport.Text = "[する／しない]";
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new Point(86, 39);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new Size(113, 12);
            this.labelUserName.TabIndex = 5;
            this.labelUserName.Text = "[入力された利用者名]";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(21, 88);
            this.label8.Name = "label8";
            this.label8.Size = new Size(115, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "インストールディレクトリ：";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(21, 64);
            this.label6.Name = "label6";
            this.label6.Size = new Size(120, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "エラー情報の自動送信：";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(7, 13);
            this.label13.Name = "label13";
            this.label13.Size = new Size(383, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "以下の内容でインストールします。よろしければ、インストールをクリックしてください。";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(21, 39);
            this.label11.Name = "label11";
            this.label11.Size = new Size(59, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "利用者名：";
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.buttonInstallPathBrowse);
            this.panel5.Controls.Add(this.textBoxInstallPath);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new Point(11, 305);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(519, 68);
            this.panel5.TabIndex = 4;
            this.label3.Location = new Point(7, 9);
            this.label3.Name = "label3";
            this.label3.Size = new Size(252, 27);
            this.label3.TabIndex = 6;
            this.label3.Text = "インストール先のディレクトリを指定してください。";
            this.buttonInstallPathBrowse.Location = new Point(392, 34);
            this.buttonInstallPathBrowse.Name = "buttonInstallPathBrowse";
            this.buttonInstallPathBrowse.Size = new Size(75, 23);
            this.buttonInstallPathBrowse.TabIndex = 1;
            this.buttonInstallPathBrowse.Text = "参照";
            this.buttonInstallPathBrowse.UseVisualStyleBackColor = true;
            this.buttonInstallPathBrowse.Click += this.buttonInstallPathBrowse_Click;
            this.textBoxInstallPath.Location = new Point(136, 36);
            this.textBoxInstallPath.Name = "textBoxInstallPath";
            this.textBoxInstallPath.Size = new Size(245, 19);
            this.textBoxInstallPath.TabIndex = 0;
            this.textBoxInstallPath.TextChanged += this.textBoxInstallPath_TextChanged;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(15, 39);
            this.label9.Name = "label9";
            this.label9.Size = new Size(115, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "インストールディレクトリ：";
            this.panel4.Controls.Add(this.checkBoxSendErrorReport);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Location = new Point(11, 231);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(518, 68);
            this.panel4.TabIndex = 3;
            this.checkBoxSendErrorReport.AutoSize = true;
            this.checkBoxSendErrorReport.Checked = true;
            this.checkBoxSendErrorReport.CheckState = CheckState.Checked;
            this.checkBoxSendErrorReport.Location = new Point(17, 44);
            this.checkBoxSendErrorReport.Name = "checkBoxSendErrorReport";
            this.checkBoxSendErrorReport.Size = new Size(258, 16);
            this.checkBoxSendErrorReport.TabIndex = 0;
            this.checkBoxSendErrorReport.Text = "問題が生じたとき、エラー情報を自動で送信する。";
            this.checkBoxSendErrorReport.UseVisualStyleBackColor = true;
            this.label7.Location = new Point(7, 14);
            this.label7.Name = "label7";
            this.label7.Size = new Size(252, 27);
            this.label7.TabIndex = 3;
            this.label7.Text = "アプリケーション品質向上に協力してください。";
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.textBoxUserName);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new Point(12, 159);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(517, 66);
            this.panel3.TabIndex = 2;
            this.label1.Location = new Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(157, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "利用者名を入力してください。";
            this.textBoxUserName.Location = new Point(85, 38);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new Size(206, 19);
            this.textBoxUserName.TabIndex = 0;
            this.textBoxUserName.TextChanged += this.textBoxUserName_TextChanged;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(20, 41);
            this.label5.Name = "label5";
            this.label5.Size = new Size(59, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "利用者名：";
            this.panel2.Controls.Add(this.checkBoxAcceptLicense);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new Point(11, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(517, 95);
            this.panel2.TabIndex = 1;
            this.checkBoxAcceptLicense.AutoSize = true;
            this.checkBoxAcceptLicense.Location = new Point(16, 78);
            this.checkBoxAcceptLicense.Name = "checkBoxAcceptLicense";
            this.checkBoxAcceptLicense.Size = new Size(84, 16);
            this.checkBoxAcceptLicense.TabIndex = 4;
            this.checkBoxAcceptLicense.Text = "同意します。";
            this.checkBoxAcceptLicense.UseVisualStyleBackColor = true;
            this.checkBoxAcceptLicense.CheckedChanged += this.checkBoxAcceptLicense_CheckedChanged;
            this.label14.Location = new Point(21, 38);
            this.label14.Name = "label14";
            this.label14.Size = new Size(157, 18);
            this.label14.TabIndex = 0;
            this.label14.Text = "利用許諾契約書・・・";
            this.label4.Location = new Point(6, 12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(487, 26);
            this.label4.TabIndex = 2;
            this.label4.Text = "次の使用許諾契約書をお読みください。続行するには、利用許諾契約書に同意する必要があります。";
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new Point(11, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(518, 39);
            this.panel1.TabIndex = 0;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(218, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "インストールするには次へをクリックしてください。";
            this.buttonCancel.Location = new Point(429, 23);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += this.buttonCancel_Click;
            this.buttonNext.Location = new Point(317, 23);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "次へ(&N) ＞";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += this.buttonNext_Click;
            this.buttonPrev.Location = new Point(228, 22);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new Size(75, 23);
            this.buttonPrev.TabIndex = 0;
            this.buttonPrev.Text = "＜ 戻る(&B)";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += this.buttonPrev_Click;
            this.folderBrowserDialog1.Description = "インストール先のディレクトリを指定してください。";
            this.label16.AutoSize = true;
            this.label16.Location = new Point(3, 12);
            this.label16.Name = "label16";
            this.label16.Size = new Size(85, 12);
            this.label16.TabIndex = 2;
            this.label16.Text = "Myインストーラー";
            this.splitContainerBase.Dock = DockStyle.Fill;
            this.splitContainerBase.FixedPanel = FixedPanel.Panel1;
            this.splitContainerBase.IsSplitterFixed = true;
            this.splitContainerBase.Location = new Point(0, 0);
            this.splitContainerBase.Name = "splitContainerBase";
            this.splitContainerBase.Orientation = Orientation.Horizontal;
            this.splitContainerBase.Panel1.Controls.Add(this.label16);
            this.splitContainerBase.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainerBase.Size = new Size(845, 733);
            this.splitContainerBase.SplitterDistance = 32;
            this.splitContainerBase.SplitterWidth = 1;
            this.splitContainerBase.TabIndex = 3;
            this.splitContainerBase.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(845, 733);
            base.Controls.Add(this.splitContainerBase);
            base.MaximizeBox = false;
            base.Name = "FormMain";
            this.Text = "MyInstaller";
            base.Load += this.FormMain_Load;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainerBase.Panel1.ResumeLayout(false);
            this.splitContainerBase.Panel1.PerformLayout();
            this.splitContainerBase.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.splitContainerBase).EndInit();
            this.splitContainerBase.ResumeLayout(false);
            base.ResumeLayout(false);
        }
        
        private FormMain.STAGE stage;
        private SplitContainer splitContainer1;
        private Panel panel8;
        private Label label18;
        private Panel panel7;
        private ProgressBar progressBar1;
        private Label label15;
        private Panel panel6;
        private Label label13;
        private Label label11;
        private Panel panel5;
        private Button buttonInstallPathBrowse;
        private TextBox textBoxInstallPath;
        private Label label9;
        private Panel panel4;
        private CheckBox checkBoxSendErrorReport;
        private Label label7;
        private Panel panel3;
        private TextBox textBoxUserName;
        private Label label5;
        private Panel panel2;
        private CheckBox checkBoxAcceptLicense;
        private Label label14;
        private Label label4;
        private Panel panel1;
        private Label label2;
        private Button buttonCancel;
        private Button buttonNext;
        private Button buttonPrev;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label16;
        private Label label3;
        private Label label1;
        private SplitContainer splitContainerBase;
        private Label labelInstallPath;
        private Label labelSendErrorReport;
        private Label labelUserName;
        private Label label8;
        private Label label6;
        private enum STAGE
        {
            PAGE1,
            PAGE2,
            PAGE3,
            PAGE4,
            PAGE5,
            PAGE6,
            PAGE7,
            PAGE8,
            PAGE9
        }
    }
}
