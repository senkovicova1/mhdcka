using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

partial class MainForm : Form
{

	static SolidBrush white = new SolidBrush(Color.White);
	static Pen zastavkaPen = new Pen(Color.MediumSlateBlue);
	static SolidBrush zastavkaBrush = new SolidBrush(Color.AliceBlue);
	
	static List<Zastavka> zastavky;
	static List<Linka> linky;
	
	static int rezim = 1;
	
	static bool holding = false;
	
	static int newLineX1, newLineY1, newLineX2, newLineY2;
	static Zastavka startZast;

	void Button1Click(object sender, EventArgs e)
	{
		rezim = 1;
	}
	void Button2Click(object sender, EventArgs e)
	{
		rezim = 2;
	}
	void Button3Click(object sender, EventArgs e)
	{
		rezim = 3;
	}
	void Button4Click(object sender, EventArgs e)
	{
		rezim = 4;
	}
	void Button5Click(object sender, EventArgs e)
	{
		rezim = 5;
	}
	
	void MainFormPaint(object sender, PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		g.FillRectangle(white, 0,0, 865,600);
		
		if (newLineX1 > -1){
			g.DrawLine(zastavkaPen, newLineX1, newLineY1, newLineX2, newLineY2);
		}
		
		foreach (var l in linky){
			l.kresli(g);
		}
		
		foreach (var z in zastavky)
		{
			z.kresli(g);
		}
	}
	
	void MainFormLoad(object sender, EventArgs e)
	{
		zastavky = new List<Zastavka>();
		linky = new List<Linka>();
		zastavkaPen.Width = 5;
	}
	
	void MainFormMouseClick(object sender, MouseEventArgs e)
	{
		if (rezim == 3 && closeTo(e.X, e.Y) == null && !holding){
			zastavky.Add(new Zastavka(e.X, e.Y));
			Invalidate();
			Update();
		}

	}
	
	void MainFormMouseMove(object sender, MouseEventArgs e)
	{
		if (holding){
			newLineX2 = e.X;
			newLineY2 = e.Y;
			Invalidate();
			Update();
		}
	}
	
	void MainFormMouseDown(object sender, MouseEventArgs e)
	{				
		Zastavka z = closeTo(e.X, e.Y);
		if (rezim == 3 && z != null){
			holding = true;
			startZast = z;
			newLineX1 = z.X;
			newLineY1 = z.Y;
			newLineX2 = e.X;
			newLineY2 = e.Y;
			Invalidate();
			Update();
		}
	}
	
	void MainFormMouseUp(object sender, MouseEventArgs e)
	{	
		if (holding){
			Zastavka z = closeTo(e.X, e.Y);
			if (z != null && z.X != startZast.X && z.Y != startZast.Y){
				spoj(z, startZast);
				Linka newLinka = new Linka(startZast, z);
				linky.Add(newLinka);
			} 
			newLineX1 = -1;
			newLineX2 = -1;
			newLineY1 = -1;
			newLineY2 = -1;
			
			Invalidate();
			Update();
			holding = false;
		}
	}
	
	void spoj(Zastavka z1, Zastavka z2){
		z1.spoj(z2);
		z2.spoj(z1);
	}
	
	bool clickedZast(int X, int Y){
		return true;
	}
			
	Zastavka closeTo(int X, int Y){
	    foreach (var z in zastavky)
	    {
	    	if (z.isClose(X, Y)){
	    		return z;
	    	}
	    }
		return null;
	}

	class Linka {		
		List<Zastavka> zastavky;
		public string name;
		public Pen colour;
		
		public Linka(Zastavka z1, Zastavka z2){
			zastavky = new List<Zastavka>();
			zastavky.Add(z1);
			zastavky.Add(z2);
			colour = zastavkaPen;
		}
		
		public void kresli(Graphics g){
			for (int i = 0; i < zastavky.Count - 1; i++)
			{
				g.DrawLine(colour, zastavky[i].X, zastavky[i].Y, zastavky[i+1].X, zastavky[i+1].Y);
			} 
			
		}
		
	}
	
	class Zastavka {
	
		public int X, Y, r;
		string name;
		public Dictionary<Zastavka, int> susedia;
	
		public Zastavka(int nX, int nY){
			susedia = new Dictionary<Zastavka, int>();
			X = nX;			
			Y = nY;
			r = 10;
		}		
		
		public void kresli(Graphics g){
			g.DrawEllipse(zastavkaPen, X-r, Y-r, 2*r, 2*r);
			g.FillEllipse(zastavkaBrush, X-r, Y-r, 2*r, 2*r);
		}
		
		public bool isClose(int X, int Y){
			return Math.Sqrt((this.X - X)*(this.X - X) + (this.Y - Y)*(this.Y - Y)) < (r + 10 + 20);
		}
		
		public void spoj(Zastavka z){
			if (!susedia.ContainsKey(z)){
				susedia[z] = 1;
			} else {
				susedia[z] += 1;
			}
			
			foreach (var value in susedia.Values)
			{
				if (10 + 5 * (value - 1) > r){
					r = 10 + 5 * (value - 1);
				}
			}
		}
			
	}
}
