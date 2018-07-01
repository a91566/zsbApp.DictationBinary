/*
 * 2018年7月1日 11:03:01 郑少宝
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace zsbApp.DictationBinary
{
    public partial class Form1 : Form
    {
        private Timer tClock;

        private Dictionary<int, string> dictUserDictation;

        private System.Diagnostics.Stopwatch watch;

        public Form1()
        {
            InitializeComponent();
            this.Shown += (s, e) => {
                this.setDictation(false);
                this.tClock = new Timer();
                this.tClock.Enabled = true;
                this.tClock.Interval = 1000;
                this.tClock.Tick += (s1, e1) => this.tslClock.Text = System.DateTime.Now.ToString("HH:mm:ss");
                this.txbBinary.KeyPress += (s1, e1) => {
                    if (((int)e1.KeyChar < 48 || (int)e1.KeyChar > 49) && (int)e1.KeyChar != 8)
                        e1.Handled = true;
                };
                this.txbBinary.KeyDown += (s1, e1) => {
                    if (e1.KeyData == Keys.Enter)
                        this.add();
                };
            };
        }

        private void setDictation(bool start)
        {
            this.tslClock.Visible = start;
            this.txbBinary.Enabled = start;
            this.btnDone.Enabled = start;
            this.btnBegin.Enabled = !start;
            this.numericUpDown1.Enabled = !start;
            this.numericUpDown2.Enabled = !start;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            this.setDictation(true);
            this.dictUserDictation = new Dictionary<int, string>();
            this.watch = new System.Diagnostics.Stopwatch();
            this.watch.Start();
        }

        private const string RIGHT = "√";
        private const string WRONG = "×";

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.setDictation(false);
            this.watch.Stop();
            long totaltime = this.watch.ElapsedMilliseconds;
            this.txbLog.Clear();
            if (this.dictUserDictation.Count == 0)
                return;
            int okCount = 0;
            foreach (var item in this.dictUserDictation)
            {
                var actual = Convert.ToString(item.Key, 2);
                if (actual == item.Value)
                {
                    this.txbLog.AppendText($"{item.Key}>{item.Value} {RIGHT}{System.Environment.NewLine}");
                    okCount++;
                }
                else
                {
                    this.txbLog.AppendText($"{item.Key}>{item.Value} {WRONG} {item.Key}的二进制为：{actual}{System.Environment.NewLine}");
                }
            }
            var baifen = (okCount * 100 / this.dictUserDictation.Count).ToString("f2");
            this.txbLog.AppendText($"总耗时：{totaltime / 1000}秒,正确数：{okCount},正确率:{baifen}%{System.Environment.NewLine}");
        }

        private void add()
        {
            if (string.IsNullOrEmpty(this.txbBinary.Text.Trim()))
                return;
            this.dictUserDictation.Add((int)this.numericUpDown1.Value, this.txbBinary.Text.Trim());
            this.txbLog.AppendText($"{this.numericUpDown1.Value}>{this.txbBinary.Text.Trim()}{System.Environment.NewLine}");
            this.txbBinary.Clear();
            this.numericUpDown1.Value++;
            if (this.numericUpDown1.Value >= this.numericUpDown2.Value)
                this.btnDone.PerformClick();
        }
    }
}
