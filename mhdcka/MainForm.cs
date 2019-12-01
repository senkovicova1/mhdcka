using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

partial class MainForm : Form
{

	static SolidBrush white = new SolidBrush(Color.White);
	static Pen whitePen = new Pen(Color.White);
	static Pen zastavkaPen = new Pen(Color.MediumSlateBlue);
	static SolidBrush zastavkaBrush2 = new SolidBrush(Color.MediumSlateBlue);
	static SolidBrush zastavkaBrush = new SolidBrush(Color.AliceBlue);
	static Font drawFont1 = new Font("Arial", 12);
	static Font drawFontBold = new Font("Arial", 14, FontStyle.Bold);
		
	static List<Pen> pens;
	
	static List<Zastavka> zastavky;
	static List<Linka> linky;
	static int linkaClicked = 0;
	
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
		
		if (linkaClicked != 0){
			g.DrawString("Tu môžeš vybrať \nlinke novú farbu!", drawFont1, zastavkaBrush2, 10, 230, null);
			int x = 20;
			int y = 60;
			for (int i = 0; i < pens.Count; i++)
			{				
				pens[i].Width = 20;
				g.DrawLine(pens[i], x, y, x + 30, y);
				x += 40;
				if ((i % 3)  == 2){
					x = 20;
					y += 30;
				}
			}						
			g.DrawString("Upravuješ linku č." + linkaClicked, drawFont1, zastavkaBrush2, 698, 160, null);
			
		} else {
			g.DrawString("Tu sú tvoje linky!", drawFont1, zastavkaBrush2, 707, 160, null);		
		}
		
		foreach (var l in linky){
			l.kresli(g);
		}
		
		if (newLineX1 > -1 && linkaClicked != 0){
			g.DrawLine(linky[linkaClicked-1].colour, newLineX1, newLineY1, newLineX2, newLineY2);
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
				
		pens = new List<Pen>();
		zastavkaPen.Width = 5;
		
		pens.Add(new Pen(Color.Crimson));		
		pens.Add(new Pen(Color.CornflowerBlue));
		pens.Add(new Pen(Color.Coral));
		pens.Add(new Pen(Color.Orange));
		pens.Add(new Pen(Color.Gold));
		pens.Add(new Pen(Color.LightBlue));
		pens.Add(new Pen(Color.LightCoral));
		pens.Add(new Pen(Color.LightGreen));
		pens.Add(new Pen(Color.LightSeaGreen));
		pens.Add(new Pen(Color.LightGoldenrodYellow));
		pens.Add(new Pen(Color.LightPink));
		pens.Add(new Pen(Color.LightCyan));
		pens.Add(new Pen(Color.LightSalmon));
		pens.Add(new Pen(Color.LightSkyBlue));
		pens.Add(new Pen(Color.MediumTurquoise));
		pens.Add(new Pen(Color.MediumOrchid));
		pens.Add(new Pen(Color.MediumSpringGreen));
		pens.Add(new Pen(Color.DodgerBlue));
		
		for (int i = 0; i < 9; i++)
		{
			linky.Add(new Linka(i+1, pens[i]));
		}
	}
	
	void MainFormMouseClick(object sender, MouseEventArgs e)
	{
		if ((rezim == 3 || rezim ==1)) {
			if (e.X >= 700 && e.Y >= 45 && e.Y <= 185){		
				linkaClicked = findClickedLinka(e.X, e.Y);
				Invalidate();
				Update();
			} else if (closeTo(e.X, e.Y) == null && !holding) {			
				zastavky.Add(new Zastavka(e.X, e.Y));
				Invalidate();
				Update();
			}
			
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
		if ((rezim == 3 || rezim ==1) && z != null && zastavky.Count >= 2 && linkaClicked != 0){
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
				linky[linkaClicked - 1].pridajCiaru(z, startZast);
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

	int findClickedLinka(int X, int Y){
		foreach (var l in linky)
		{
			if (700 + (((l.name+2)%3)*50) <= X && 750 + (((l.name+2)%3)*50) >= X
			    &&
			    45 + (((l.name-1)/3)*40) <= Y && 75 + (((l.name-1)/3)*40) >= Y){
				return l.name;
			}
		}
		return 0;
	}

	class Linka {		
		List<Zastavka> mojeZastavky;
		public int name;
		public Pen colour;
		
		public Linka(int nName, Pen pen){
			name = nName;
			colour = pen;
			mojeZastavky = new List<Zastavka>();
		}
		
		public void kresli(Graphics g){
			colour.Width = 30;
			int x = 700 + (((name+2)%3)*50);
			int y = 60 + (((name-1)/3)*40);
			g.DrawLine(colour, x, y, x+40, y);
			g.DrawString(name + "", drawFontBold, white, x+12, y-10, null);
			
			if (linkaClicked == name){
				whitePen.Width = 5;
				g.DrawRectangle(whitePen, x+2, y-13, 36, 26);
				whitePen.Width = 1;
				g.DrawRectangle(zastavkaPen, x, y-15, 40, 30);
			}
		
			colour.Width = 5;
			Point start, center, end;
			for (int i = 0; i < mojeZastavky.Count - 1; i+=2)
			{	
				start = new Point(mojeZastavky[i].X, mojeZastavky[i].Y);
			    end = new Point(mojeZastavky[i+1].X, mojeZastavky[i+1].Y);
			    
			    center = getCenter(mojeZastavky[i].X, mojeZastavky[i].Y, mojeZastavky[i+1].X, mojeZastavky[i+1].Y);
			    
			    Point[] curvePoints = {start, center, end};
			    g.DrawCurve(colour, curvePoints);
			} 
			
		}
		
		Point getCenter(int X1, int Y1, int X2, int Y2){
			double slope = ((double)(Y2 - Y1) / (double)(X2 - X1));
			if (slope < 1 && slope > 0){
				slope = 1;
			} else if (slope < 0 && slope > -1){
				slope = -1;
			}
			slope = -1 / slope;
			
			double midpointX = (X1 + X2)/2;
			double midpointY = (Y1 + Y2)/2;
			
			double b = -slope * midpointX + midpointY;
			
			int len = (((name)/2)*8);
			if (name % 2 == 1){
				len *= -1;
			}
			int centerX = (int)midpointX + len;
			int centerY = (int)(slope * (midpointX + len) + b);
			
			return new Point(centerX, centerY);
		}
		
		public void pridajCiaru(Zastavka z1, Zastavka z2){
			mojeZastavky.Add(z2);
			mojeZastavky.Add(z1);
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
			
		}
			
	}
}
