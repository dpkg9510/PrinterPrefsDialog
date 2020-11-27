using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;
using System.Runtime.InteropServices;


namespace PrinterSet
{
	public class Form1 : System.Windows.Forms.Form
	{
		[DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true,
			 ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter,
			[MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
			IntPtr pDevModeOutput, ref IntPtr pDevModeInput, int fMode);
		[DllImport("kernel32.dll")]
		static extern IntPtr GlobalLock(IntPtr hMem);
		[DllImport("kernel32.dll")]
		static extern bool GlobalUnlock(IntPtr hMem);
		[DllImport("kernel32.dll")]
		static extern bool GlobalFree(IntPtr hMem);

		private void OpenPrinterPropertiesDialog(PrinterSettings printerSettings)
		{
			IntPtr hDevMode = printerSettings.GetHdevmode(printerSettings.DefaultPageSettings);
			IntPtr pDevMode = GlobalLock(hDevMode);
			int sizeNeeded = DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, pDevMode, ref pDevMode, 0);
			IntPtr devModeData = Marshal.AllocHGlobal(sizeNeeded);
			DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, devModeData, ref pDevMode, 14);
			GlobalUnlock(hDevMode);
			printerSettings.SetHdevmode(devModeData);
			printerSettings.DefaultPageSettings.SetHdevmode(devModeData);
			GlobalFree(hDevMode);
			Marshal.FreeHGlobal(devModeData);
		}

		private System.Windows.Forms.Button printSetting;
        private PictureBox pictureBox1;
        private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form
		private void InitializeComponent()
		{
            this.printSetting = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // printSetting
            // 
            this.printSetting.Location = new System.Drawing.Point(12, 150);
            this.printSetting.Name = "printSetting";
            this.printSetting.Size = new System.Drawing.Size(283, 51);
            this.printSetting.TabIndex = 0;
            this.printSetting.Text = "Change Options";
            this.printSetting.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(57, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 132);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(312, 216);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.printSetting);
            this.Name = "Form1";
            this.Text = "Printing Options";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				PrinterSettings ps = new PrinterSettings();

				OpenPrinterPropertiesDialog(ps);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Printer settings are incorrect.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			
			}		
		}

        private void Form1_Load(object sender, EventArgs e)
        {
	        Image image = Image.FromFile("D:\\0.png");
            // Set the PictureBox image property to this image.
            // ... Then, adjust its height and width properties.
            pictureBox1.Image = image;
            pictureBox1.Height = 120;
            pictureBox1.Width = 300;
		}
    }

}


