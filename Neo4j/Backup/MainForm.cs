using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using RatingControls;

namespace StarRatingControlTester
{
	public class MainForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;

		private StarRatingControl m_starRatingControl = new StarRatingControl();

		public MainForm()
		{
			InitializeComponent();

			m_starRatingControl.Top = 45;
			m_starRatingControl.Left = 85;

			Controls.Add ( m_starRatingControl );
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

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Navy;
			this.label1.Location = new System.Drawing.Point(15, 104);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Andrey Butov - andreybutov@antair.com";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(286, 131);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Star Rating Control Tester";
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}
	}
}
