
partial class MainForm
{
	private System.ComponentModel.IContainer components = null;
	private System.Windows.Forms.Button button1;
	private System.Windows.Forms.Button button2;
	private System.Windows.Forms.Button button3;
	private System.Windows.Forms.Button button4;
	private System.Windows.Forms.Button button5;
	private System.Windows.Forms.Button button6;
	private System.Windows.Forms.Button button7;
	private System.Windows.Forms.Button button8;
	private System.Windows.Forms.TextBox textBox1;
	private System.Windows.Forms.Button button9;
	private System.Windows.Forms.Button button10;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null) 
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.button7 = new System.Windows.Forms.Button();
		this.button8 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.button9 = new System.Windows.Forms.Button();
		this.button10 = new System.Windows.Forms.Button();
		this.SuspendLayout();
		// 
		// button1
		// 
		this.button1.Location = new System.Drawing.Point(8, 8);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(160, 25);
		this.button1.TabIndex = 0;
		this.button1.Text = "Nová mapa";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(this.Button1Click);
		// 
		// button2
		// 
		this.button2.Location = new System.Drawing.Point(176, 8);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(160, 25);
		this.button2.TabIndex = 1;
		this.button2.Text = "Načítať mapu";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(this.Button2Click);
		// 
		// button3
		// 
		this.button3.Location = new System.Drawing.Point(344, 8);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(160, 25);
		this.button3.TabIndex = 2;
		this.button3.Text = "Upraviť mapu";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(this.Button3Click);
		// 
		// button4
		// 
		this.button4.Location = new System.Drawing.Point(512, 8);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(160, 25);
		this.button4.TabIndex = 3;
		this.button4.Text = "Uložiť mapu";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(this.Button4Click);
		// 
		// button5
		// 
		this.button5.Location = new System.Drawing.Point(680, 8);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(160, 25);
		this.button5.TabIndex = 4;
		this.button5.Text = "Generovanie otázok";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(this.Button5Click);
		// 
		// button6
		// 
		this.button6.Location = new System.Drawing.Point(680, 528);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(160, 25);
		this.button6.TabIndex = 5;
		this.button6.Text = "Zmazať čiaru";
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(this.Button6Click);
		// 
		// button7
		// 
		this.button7.Location = new System.Drawing.Point(8, 528);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(160, 25);
		this.button7.TabIndex = 7;
		this.button7.Text = "Zmazať zastávku";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Click += new System.EventHandler(this.Button7Click);
		// 
		// button8
		// 
		this.button8.BackColor = System.Drawing.Color.Black;
		this.button8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.button8.Location = new System.Drawing.Point(8, 472);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(45, 45);
		this.button8.TabIndex = 8;
		this.button8.UseVisualStyleBackColor = false;
		this.button8.Click += new System.EventHandler(this.Button8Click);
		// 
		// textBox1
		// 
		this.textBox1.Location = new System.Drawing.Point(256, 528);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(352, 20);
		this.textBox1.TabIndex = 9;
		// 
		// button9
		// 
		this.button9.Location = new System.Drawing.Point(624, 528);
		this.button9.Name = "button9";
		this.button9.Size = new System.Drawing.Size(120, 23);
		this.button9.TabIndex = 10;
		this.button9.Text = "Overiť";
		this.button9.UseVisualStyleBackColor = true;
		this.button9.Click += new System.EventHandler(this.Button9Click);
		// 
		// button10
		// 
		this.button10.Location = new System.Drawing.Point(624, 496);
		this.button10.Name = "button10";
		this.button10.Size = new System.Drawing.Size(211, 23);
		this.button10.TabIndex = 12;
		this.button10.Text = "Ďalšia otázka";
		this.button10.UseVisualStyleBackColor = true;
		this.button10.Click += new System.EventHandler(this.Button10Click);
		// 
		// MainForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(849, 562);
		this.Controls.Add(this.button10);
		this.Controls.Add(this.button9);
		this.Controls.Add(this.textBox1);
		this.Controls.Add(this.button8);
		this.Controls.Add(this.button7);
		this.Controls.Add(this.button6);
		this.Controls.Add(this.button5);
		this.Controls.Add(this.button4);
		this.Controls.Add(this.button3);
		this.Controls.Add(this.button2);
		this.Controls.Add(this.button1);
		this.DoubleBuffered = true;
		this.Name = "MainForm";
		this.Text = "mhdcka";
		this.Load += new System.EventHandler(this.MainFormLoad);
		this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainFormPaint);
		this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseClick);
		this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDoubleClick);
		this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
		this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
		this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
		this.ResumeLayout(false);
		this.PerformLayout();

	}

	public MainForm()
	{
		InitializeComponent();
	}
}
