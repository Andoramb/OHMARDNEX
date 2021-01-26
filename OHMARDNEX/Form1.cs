using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO.Ports;
using System.Linq;
using System.Text;

using OpenHardwareMonitor.Hardware;
namespace Arduino_PC_Monitor
{
    public partial class Form1 : Form
    {
        // static string data;
        readonly Computer c = new Computer()
        {
            GPUEnabled = true,
            CPUEnabled = true,
            RAMEnabled = true,
            HDDEnabled = true,
            MainboardEnabled = true,
            FanControllerEnabled = true
        };

        private BackgroundWorker backgroundWorker1;
        private ComboBox cbPort;
        private Label label1;
        private Label label2;
        private Button btnConnect;
        private Timer timer1;
        private IContainer components;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private NotifyIcon notifyIcon1;
        private SerialPort selectedPort = new SerialPort();
        private TextBox tbInterval;
        private ComboBox cbBaudRate;
        private Label label3;
        private CheckBox cbCPUTemp;
        private CheckBox cbCPULoad;
        private CheckBox cbCPUPower;
        private Button btnSendCmd;
        private TextBox tbRawCmd;
        private CheckBox cbGPURam;
        private CheckBox cbGPULoad;
        private CheckBox cbGPUTemp;
        private CheckBox cbHDD2;
        private CheckBox cbHDD1;
        private CheckBox cbRAMUsage; 
        private Label label4;
        private Label label5;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem restoreToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.BalloonTipText = "OHMArdNex has been minimized to system tray";
                notifyIcon1.ShowBalloonTip(1000);
                this.ShowInTaskbar = false;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCPUTemp = new System.Windows.Forms.CheckBox();
            this.cbCPULoad = new System.Windows.Forms.CheckBox();
            this.cbCPUPower = new System.Windows.Forms.CheckBox();
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.tbRawCmd = new System.Windows.Forms.TextBox();
            this.cbGPURam = new System.Windows.Forms.CheckBox();
            this.cbGPULoad = new System.Windows.Forms.CheckBox();
            this.cbGPUTemp = new System.Windows.Forms.CheckBox();
            this.cbHDD2 = new System.Windows.Forms.CheckBox();
            this.cbHDD1 = new System.Windows.Forms.CheckBox();
            this.cbRAMUsage = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbPort
            // 
            this.cbPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(13, 43);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(121, 30);
            this.cbPort.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(287, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Update in ms";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(422, 45);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(115, 30);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 304);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(561, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "Created by Andor Ambarus";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipTitle = "OHMArdNex";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "OHMArdNex";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 52);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tbInterval
            // 
            this.tbInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInterval.Location = new System.Drawing.Point(287, 45);
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(100, 27);
            this.tbInterval.TabIndex = 8;
            this.tbInterval.Text = "2000";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.cbBaudRate.Location = new System.Drawing.Point(150, 43);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(121, 30);
            this.cbBaudRate.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(150, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 22);
            this.label3.TabIndex = 1;
            this.label3.Text = "Baud Rate";
            // 
            // cbCPUTemp
            // 
            this.cbCPUTemp.AutoSize = true;
            this.cbCPUTemp.Checked = true;
            this.cbCPUTemp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCPUTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCPUTemp.Location = new System.Drawing.Point(12, 115);
            this.cbCPUTemp.Name = "cbCPUTemp";
            this.cbCPUTemp.Size = new System.Drawing.Size(118, 26);
            this.cbCPUTemp.TabIndex = 9;
            this.cbCPUTemp.Text = "CPU Temp";
            this.cbCPUTemp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbCPUTemp.UseVisualStyleBackColor = true;
            // 
            // cbCPULoad
            // 
            this.cbCPULoad.AutoSize = true;
            this.cbCPULoad.Checked = true;
            this.cbCPULoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCPULoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCPULoad.Location = new System.Drawing.Point(12, 144);
            this.cbCPULoad.Name = "cbCPULoad";
            this.cbCPULoad.Size = new System.Drawing.Size(112, 26);
            this.cbCPULoad.TabIndex = 9;
            this.cbCPULoad.Text = "CPU Load";
            this.cbCPULoad.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbCPULoad.UseVisualStyleBackColor = true;
            // 
            // cbCPUPower
            // 
            this.cbCPUPower.AutoSize = true;
            this.cbCPUPower.Checked = true;
            this.cbCPUPower.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCPUPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCPUPower.Location = new System.Drawing.Point(12, 173);
            this.cbCPUPower.Name = "cbCPUPower";
            this.cbCPUPower.Size = new System.Drawing.Size(123, 26);
            this.cbCPUPower.TabIndex = 9;
            this.cbCPUPower.Text = "CPU Power";
            this.cbCPUPower.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbCPUPower.UseVisualStyleBackColor = true;
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendCmd.Location = new System.Drawing.Point(450, 221);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(87, 29);
            this.btnSendCmd.TabIndex = 4;
            this.btnSendCmd.Text = "Send";
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // tbRawCmd
            // 
            this.tbRawCmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRawCmd.Location = new System.Drawing.Point(214, 222);
            this.tbRawCmd.Name = "tbRawCmd";
            this.tbRawCmd.Size = new System.Drawing.Size(230, 27);
            this.tbRawCmd.TabIndex = 10;
            // 
            // cbGPURam
            // 
            this.cbGPURam.AutoSize = true;
            this.cbGPURam.Checked = true;
            this.cbGPURam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGPURam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGPURam.Location = new System.Drawing.Point(157, 173);
            this.cbGPURam.Name = "cbGPURam";
            this.cbGPURam.Size = new System.Drawing.Size(112, 26);
            this.cbGPURam.TabIndex = 12;
            this.cbGPURam.Text = "GPU RAM";
            this.cbGPURam.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbGPURam.UseVisualStyleBackColor = true;
            // 
            // cbGPULoad
            // 
            this.cbGPULoad.AutoSize = true;
            this.cbGPULoad.Checked = true;
            this.cbGPULoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGPULoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGPULoad.Location = new System.Drawing.Point(157, 144);
            this.cbGPULoad.Name = "cbGPULoad";
            this.cbGPULoad.Size = new System.Drawing.Size(113, 26);
            this.cbGPULoad.TabIndex = 13;
            this.cbGPULoad.Text = "GPU Load";
            this.cbGPULoad.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbGPULoad.UseVisualStyleBackColor = true;
            // 
            // cbGPUTemp
            // 
            this.cbGPUTemp.AutoSize = true;
            this.cbGPUTemp.Checked = true;
            this.cbGPUTemp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGPUTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGPUTemp.Location = new System.Drawing.Point(157, 115);
            this.cbGPUTemp.Name = "cbGPUTemp";
            this.cbGPUTemp.Size = new System.Drawing.Size(119, 26);
            this.cbGPUTemp.TabIndex = 14;
            this.cbGPUTemp.Text = "GPU Temp";
            this.cbGPUTemp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbGPUTemp.UseVisualStyleBackColor = true;
            // 
            // cbHDD2
            // 
            this.cbHDD2.AutoSize = true;
            this.cbHDD2.Checked = true;
            this.cbHDD2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHDD2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHDD2.Location = new System.Drawing.Point(303, 173);
            this.cbHDD2.Name = "cbHDD2";
            this.cbHDD2.Size = new System.Drawing.Size(78, 26);
            this.cbHDD2.TabIndex = 15;
            this.cbHDD2.Text = "HDD2";
            this.cbHDD2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbHDD2.UseVisualStyleBackColor = true;
            // 
            // cbHDD1
            // 
            this.cbHDD1.AutoSize = true;
            this.cbHDD1.Checked = true;
            this.cbHDD1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHDD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHDD1.Location = new System.Drawing.Point(303, 144);
            this.cbHDD1.Name = "cbHDD1";
            this.cbHDD1.Size = new System.Drawing.Size(78, 26);
            this.cbHDD1.TabIndex = 16;
            this.cbHDD1.Text = "HDD1";
            this.cbHDD1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbHDD1.UseVisualStyleBackColor = true;
            // 
            // cbRAMUsage
            // 
            this.cbRAMUsage.AutoSize = true;
            this.cbRAMUsage.Checked = true;
            this.cbRAMUsage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRAMUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRAMUsage.Location = new System.Drawing.Point(303, 115);
            this.cbRAMUsage.Name = "cbRAMUsage";
            this.cbRAMUsage.Size = new System.Drawing.Size(125, 26);
            this.cbRAMUsage.TabIndex = 17;
            this.cbRAMUsage.Text = "RAM Usage";
            this.cbRAMUsage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbRAMUsage.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 22);
            this.label4.TabIndex = 11;
            this.label4.Text = "Send:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 22);
            this.label5.TabIndex = 18;
            this.label5.Text = "Send custom command";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(391, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Created by Andor Ambarus";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(561, 326);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbHDD2);
            this.Controls.Add(this.cbHDD1);
            this.Controls.Add(this.cbRAMUsage);
            this.Controls.Add(this.cbGPURam);
            this.Controls.Add(this.cbGPULoad);
            this.Controls.Add(this.cbGPUTemp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbRawCmd);
            this.Controls.Add(this.cbCPUPower);
            this.Controls.Add(this.cbCPULoad);
            this.Controls.Add(this.cbCPUTemp);
            this.Controls.Add(this.tbInterval);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSendCmd);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBaudRate);
            this.Controls.Add(this.cbPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "OHMArdNex";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Init()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    cbPort.Items.Add(port);
                }
                cbPort.SelectedIndex = 0;
                cbBaudRate.SelectedIndex = 1;
                notifyIcon1.Visible = true;
                selectedPort.Parity = Parity.None;
                selectedPort.StopBits = StopBits.One;
                selectedPort.DataBits = 8;
                selectedPort.Handshake = Handshake.None;
                selectedPort.RtsEnable = false;
                selectedPort.BaudRate = Convert.ToInt32(cbBaudRate.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            c.Open();
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = true;
            this.statusStrip1.Text = "Created by Andor Ambarus - 2021";
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!selectedPort.IsOpen)
                {
                    selectedPort.PortName = cbPort.Text;
                    selectedPort.Open();
                    Ini();
                    timer1.Interval = Convert.ToInt32(tbInterval.Text);
                    timer1.Enabled = true;
                    toolStripStatusLabel1.Text = "Sending data...";
                    btnConnect.Text = "Disconnect";
                }
                else
                {
                    timer1.Enabled = false;
                    toolStripStatusLabel1.Text = "Connect to Arduino...";
                    selectedPort.Close();
                    btnConnect.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                toolStripStatusLabel1.Text = "COM Port not responding...";
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Status();
            }
            catch (Exception ex)
            {
                timer1.Stop();

                if (!selectedPort.IsOpen)
                    selectedPort.Close();
                statusStrip1.Text = "Disconnected";

                MessageBox.Show(ex.Message);
                toolStripStatusLabel1.Text = "unhadled exception...  :) ";
            }
        }


        void SendRaw(string s)
        {

            byte[] messageBytes = Encoding.ASCII.GetBytes(s);
            byte[] endBytes = { 0xFF, 0xFF, 0xFF };
            int length = messageBytes.Length + endBytes.Length;
            byte[] bytesToSend = new byte[length];

            messageBytes.CopyTo(bytesToSend, 0);
            endBytes.CopyTo(bytesToSend, messageBytes.Length);

            selectedPort.Write(bytesToSend, 0, bytesToSend.Length);
        }
        
        void Send(string s)
        {

            byte[] messageBytes = Encoding.ASCII.GetBytes(s);
            byte[] endBytes = { 0x22, 0xFF, 0xFF, 0xFF };
            int length = messageBytes.Length + endBytes.Length;
            byte[] bytesToSend = new byte[length];

            messageBytes.CopyTo(bytesToSend, 0);
            endBytes.CopyTo(bytesToSend, messageBytes.Length);

            selectedPort.Write(bytesToSend, 0, bytesToSend.Length);
        }
        
        void Sendtemp(string s)  // Separate functino called for the degree symbol
        {

            byte[] messageBytes = Encoding.ASCII.GetBytes(s);
            byte[] endBytes = {0xB0, 0x43, 0x22, 0xFF, 0xFF, 0xFF };  //Ends the sent string with °C"FFFFFF
            int length = messageBytes.Length + endBytes.Length;
            byte[] bytesToSend = new byte[length];

            messageBytes.CopyTo(bytesToSend, 0);
            endBytes.CopyTo(bytesToSend, messageBytes.Length);

            selectedPort.Write(bytesToSend, 0, bytesToSend.Length);
        }
        
        private string GetResponse()
        {
            //byte[] bytesToSend = new byte[9];

            //selectedPort.R(bytesToSend, 0, bytesToSend.Length);           
            //return (Encoding.UTF8.GetString(bytesToSend));
            //lbResponse.Text = selectedPort.ReadLine().ToString();
            return (selectedPort.ReadLine());

        }//not used atm
       
        private void btnSendCmd_Click(object sender, EventArgs e)
        {
            if (!selectedPort.IsOpen)
            {
                selectedPort.PortName = cbPort.Text;
                selectedPort.Open();
            }
            //SendRaw("info.raminfo.txt=\"" + (sensor.Value.GetValueOrDefault()).ToString() + "\"");
            SendRaw(tbRawCmd.Text.ToString());
            //GetResponse();

        }

        private void Ini()
        {
            foreach (var hardware in c.Hardware)
            {
                hardware.Update();
              
                if (hardware.HardwareType.ToString().Contains("RAM") && cbRAMUsage.Checked)
                {
                    Send("info.raminfo.txt=\"" + hardware.Name.ToString());
                }
                if (hardware.HardwareType.ToString().Contains("CPU") && cbCPULoad.Checked)
                {
                    Send("info.cpuinfo.txt=\"" + hardware.Name.ToString());
                }
                if (hardware.HardwareType.ToString().Contains("Gpu") && cbGPULoad.Checked)
                {
                    Send("info.gpuinfo.txt=\"" + hardware.Name.ToString());
                }
                if (hardware.HardwareType.ToString().Contains("HDD"))
                {
                    if (hardware.Identifier.ToString().Contains("0") && cbHDD1.Checked)
                    {
                        Send("info.hdd1info.txt=\"" + hardware.Name.ToString());
                        Send("status2.hdd1txt.txt=\"" + hardware.Name.ToString());
                    }
                    else if (hardware.Identifier.ToString().Contains("1") && cbHDD2.Checked)
                    {
                        Send("info.hdd2info.txt=\"" + hardware.Name.ToString());
                        Send("status2.hdd2txt.txt=\"" + hardware.Name.ToString());
                    }
                }
                if (hardware.HardwareType.ToString().Contains("Mainboard"))
                {
                    Send("info.mbinfo.txt=\"" + hardware.Name.ToString());
                }

            }
        }

        private void Status()
        {
            foreach (var hardware in c.Hardware.Where(c => c.HardwareType == HardwareType.CPU))
            {
                foreach (var sensor in hardware.Sensors)
                {
                    hardware.Update();
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Package") && cbCPUTemp.Checked)
                    {
                        Send("status1.cputemp.val=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                        Sendtemp("status1.cputemptxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());  //quotes closed in the Sendtemp definition

                        if (sensor.Value.GetValueOrDefault() > 80)
                        {
                            Send("sleep=0");
                        }

                        // System.Diagnostics.Debug.WriteLine("cputemp.val: " + sensor.Value.GetValueOrDefault());
                    }

                    if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU Total") && cbCPULoad.Checked)
                    {
                        Send("status1.cpuload.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                        Send("status1.cpuloadtxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                        // System.Diagnostics.Debug.WriteLine("cpuload.pic: " + sensor.Value.GetValueOrDefault());
                    }

                    if (sensor.SensorType == SensorType.Power && sensor.Name.Contains("CPU Package") && cbCPUPower.Checked)
                    {
                        Send("status2.cpupowertxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + " W");

                    }
                }
            }

            foreach (var hardware in c.Hardware)
            {
                hardware.Update();
                if (hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    foreach (var sensor in hardware.Sensors)
                    {

                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU Core") && cbGPUTemp.Checked)
                        {
                            Send("status1.gputemp.val=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Sendtemp("status1.gputemptxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString()); //quotes closed in the Sendtemp definition

                            if (sensor.Value.GetValueOrDefault() > 80)
                                Send("sleep=0");

                            // System.Diagnostics.Debug.WriteLine("gputemp.val: " + sensor.Value.GetValueOrDefault());
                        }

                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core") && cbGPULoad.Checked)
                        {
                            Send("status1.gpuload.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Send("status1.gpuloadtxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                            // System.Diagnostics.Debug.WriteLine("gpuload.val: " + sensor.Value.GetValueOrDefault());
                        }

                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Memory") && cbGPURam.Checked)
                        {
                            Send("status1.gpuram.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Send("status1.gpuramtxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");

                            // System.Diagnostics.Debug.WriteLine("gputemp.val: " + sensor.Value.GetValueOrDefault());
                        }
                        if (sensor.SensorType == SensorType.Fan && cbGPUTemp.Checked)
                        {
                            Send("status2.gpufantxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + " RPM");
                        }
                        if (sensor.SensorType == SensorType.Control && sensor.Name.Contains("Fan") && cbGPUTemp.Checked)
                        {
                            Send("status2.gpufan.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                        }
                    }
                }

                if (hardware.HardwareType == HardwareType.GpuAti)
                {

                    foreach (var sensor in hardware.Sensors)
                    {
                        hardware.Update();
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU Core") && cbGPUTemp.Checked)
                        {
                            Send("status1.gputemp.val=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Sendtemp("status1.gputemptxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString()); //quotes closed in the Sendtemp definition

                            if (sensor.Value.GetValueOrDefault() > 80)
                                Send("sleep=0");

                            // System.Diagnostics.Debug.WriteLine("gputemp.val: " + sensor.Value.GetValueOrDefault());
                        }

                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core") && cbGPULoad.Checked)
                        {
                            Send("status1.gpuload.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Send("status1.gpuloadtxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                            // System.Diagnostics.Debug.WriteLine("gpuload.val: " + sensor.Value.GetValueOrDefault());
                        }

                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Memory") && cbGPURam.Checked)
                        {
                            Send("status2.gpupower.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Send("status2.gpupowertxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + " W");
                            // System.Diagnostics.Debug.WriteLine("gpupower.pic: " + sensor.Value.GetValueOrDefault());

                        }
                        if (sensor.SensorType == SensorType.Fan && cbGPUTemp.Checked)
                        {
                            Send("status2.gpufantxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + " RPM");
                        }
                        if (sensor.SensorType == SensorType.Control && sensor.Name.Contains("Fan") && cbGPUTemp.Checked)
                        {
                            Send("status2.gpufan.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                        }
                    }
                }

                if (hardware.HardwareType == HardwareType.RAM)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        hardware.Update();
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Memory") && cbRAMUsage.Checked)
                        {
                            Send("status1.ramload.pic=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                            Send("status1.ramloadtxt.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                            // System.Diagnostics.Debug.WriteLine("ramload.pic: " + sensor.Value.GetValueOrDefault());

                        }
                    }
                }

                if (hardware.HardwareType == HardwareType.HDD)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        hardware.Update();
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Used Space"))
                        {
                            if (sensor.Hardware.Identifier.ToString().Contains("0") && cbHDD1.Checked)
                            {
                                Send("status2.hdd1.val=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                                Send("status2.hdd1size.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                            }

                            if (sensor.Hardware.Identifier.ToString().Contains("1") && cbHDD2.Checked)
                            {
                                Send("status2.hdd2.val=" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString());
                                Send("status2.hdd2size.txt=\"" + Convert.ToInt32(sensor.Value.GetValueOrDefault()).ToString() + "%");
                            }
                        }
                    }
                }
            }

            Send("screensaver.time.txt=\"" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
            Send("screensaver.date.txt=\"" + DateTime.Today.ToString("yyyy MMMM dd"));
        }

        private Label label6;

        private void label6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Andoramb");
        }
    }
}