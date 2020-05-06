using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

partial class MainForm : Form
{

	static SolidBrush white = new SolidBrush(Color.White);
	static SolidBrush black = new SolidBrush(Color.Black);
	static Pen whitePen = new Pen(Color.White);
	static Pen zastavkaPen = new Pen(Color.MediumSlateBlue);
	static SolidBrush deleteBrush = new SolidBrush(Color.DarkRed);
	static SolidBrush zastavkaBrush2 = new SolidBrush(Color.MediumSlateBlue);
	static SolidBrush zastavkaBrush = new SolidBrush(Color.AliceBlue);
	static Font drawFont1 = new Font("Arial", 12);
	static Font drawFont2 = new Font("Arial", 14);
	static Font drawFontBold = new Font("Arial", 14, FontStyle.Bold);
		
	static List<Pen> pens;
	
	static List<Zastavka> zastavky;
	static List<Linka> linky;
	static int linkaClicked = 0;
	
	static int rezim = 1;
	
	static SolidBrush background;
	
	static bool holding = false;
	static bool deletingLine = false;
	static bool deletingZast = false;
	static bool namingZast = false;
	static bool deleteMode = false;
	
	static bool[] errors = new bool[3];
	static List<string> errorMsgs = new List<string>();
	
	static int newLineX1, newLineY1, newLineX2, newLineY2;
	static Zastavka startZast;
	
	static int level = 0;
	static string question;
	static string answer;
	static int correctAnswer;
	static int body = 0;
		
	Random rnd = new Random();

	void Button1Click(object sender, EventArgs e)
	{
		rezim = 1;
		linkaClicked = 0;
		holding = false;
		deletingLine = false;
		deletingZast = false;
		level = 0;
	
		button6.Visible = true;
		button7.Visible = true;
		button8.Visible = true;
		textBox1.Visible = false;
		button9.Visible = false;
		button10.Visible = false;
		linky.Clear();
		
		for (int i = 0; i < 9; i++)
		{
			linky.Add(new Linka(i+1, pens[i]));
		}
		zastavky.Clear();
		Invalidate();
		Update();
	}
	void Button2Click(object sender, EventArgs e)
	{
		rezim = 2;
				
		button7.Visible = false;
		button8.Visible = false;
		
		linky.Clear();
		zastavky.Clear();
		StreamReader myStream;
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "txt files (*.txt)|*.txt";
		openFileDialog.FilterIndex = 2;
		openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			if ((myStream = new StreamReader(openFileDialog.OpenFile())) != null)
			{
				while(myStream.Peek() >= 0){
					var line = myStream.ReadLine();
					var variables = line.Split();
					if(variables[0] == "z"){
						zastavky.Add(new Zastavka(Convert.ToInt32(variables[1]), Convert.ToInt32(variables[2]), Convert.ToInt32(variables[3])));
						}
					else if(variables[0] == "l"){
						linky.Add(new Linka(Convert.ToInt32(variables[1]), pens[Convert.ToInt32(variables[2])]));

						if (variables.Length > 3){
							var index = Array.IndexOf(variables, "p");
							for(int m = 3; m < index; m = m + 6){
								linky[linky.Count-1].pridajUlozeneZastavky(new Zastavka(Convert.ToInt32(variables[m]), Convert.ToInt32(variables[m+1]), Convert.ToInt32(variables[m+2])),
								new Zastavka(Convert.ToInt32(variables[m+3]), Convert.ToInt32(variables[m+4]), Convert.ToInt32(variables[m+5])));
							}
							for(int m = index+1; m < variables.Length; m = m + 2){
								linky[linky.Count-1].pridajUlozenePoint(new Point(Convert.ToInt32(variables[m]), Convert.ToInt32(variables[m+1])));
							}
						}
					}
				}
				myStream.Close();
			}
			Invalidate();
			Update();
		}

	}
	void Button3Click(object sender, EventArgs e)
	{
		rezim = 3;
		button7.Visible = true;
		button8.Visible = true;
		textBox1.Visible = false;
		button9.Visible = false;
		button10.Visible = false;
		Invalidate();
		Update();
	}
/*
	class TestWriter : StreamWriter{
		
		public MyWriter(Stream stream): base(stream){}
		
	    public override void Write(string value)
	    {	
	        
	    }
	}*/
	
	void Button4Click(object sender, EventArgs e)
	{
		rezim = 4;

		StreamWriter myStream;
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "txt files (*.txt)|*.txt";
		saveFileDialog.FilterIndex = 2;
		saveFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			if ((myStream = new StreamWriter(saveFileDialog.OpenFile())) != null)
			{
				var txt = "";
				for(int i = 0; i < zastavky.Count; i++){
					txt += "z" + " " + zastavky[i].getX() + " " + zastavky[i].getY() + " " + zastavky[i].getNameNumber() + "\n";
				}
				for(int i = 0; i < linky.Count; i++){
					int[] zastavky = linky[i].getSuradniceZastavky();
					int[] point = linky[i].getPoint();
					txt += "l" + " " + linky[i].getName() + " " + linky[i].getColour();
					for(int m = 0; m < zastavky.Length; m++){
						txt += " " + zastavky[m];
					}
					for(int n = 0; n < point.Length; n++){
						if(n == 0)
							txt += " p";
						txt += " " + point[n];
					}
					txt += "\n";
				}

				myStream.Write(txt);
				myStream.Close();
			}
		}
	}
	void Button5Click(object sender, EventArgs e)
	{
		errors = checkLinky();
		bool error = false;
		foreach (var er in errors)
		{
			if (er){	
				error = true;
			}
		}
		if (!error){
			rezim = 5;
			linkaClicked = 0;
			button7.Visible = false;
			textBox1.Visible = true;
			button9.Visible = true;
			generateQuestion();
		} 
		Invalidate();
		Update();
		
	}	

	void changeButton6Name(){
		if (deletingLine){
			deletingLine = false;
			button6.Text = "Zmazať čiaru";
		} else {
			deletingLine = true;
			button6.Text = "Ukončiť mazanie";
		}
	}

	void Button6Click(object sender, EventArgs e)
	{
		changeButton6Name();
		writeDeleteModeText();
		Invalidate();
		Update();
	}

	void changeButton7Name(){
	if (deletingZast){
			deletingZast = false;
			button7.Text = "Zmazať zastávku";
		} else {
			deletingZast = true;
			button7.Text = "Ukončiť mazanie";
		}
	}

	void writeDeleteModeText(){
		if(deletingZast || deletingLine)
			deleteMode = true;
		else
			deleteMode = false;
	}

	void Button7Click(object sender, EventArgs e)
	{
		changeButton7Name();
		writeDeleteModeText();
		Invalidate();
		Update();
	}
	void Button8Click(object sender, EventArgs e)
	{
		if (background.Color == Color.White){
			background = black;
			button8.BackColor = Color.White;
		} else {
			background = white;
			button8.BackColor = Color.Black;
		}
		Invalidate();
		Update();
	}
	void Button9Click(object sender, EventArgs e)
	{
		if (level == 3){
			string[] str = answer.Split('-');
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i].ToUpper() == textBox1.Text.ToUpper()){
					correctAnswer = 1;
					body += 1;
				}
			}
			if (correctAnswer == 0){
				correctAnswer = -1;
			}
		} else if (level == 9){
			if (textBox1.Text.Split('-').OrderBy(a => a).SequenceEqual(answer.Split('-').OrderBy(a => a))){
				correctAnswer = 1;
				body += 1;
			} else {
				correctAnswer = -1;
			}
		} else if (level == 10){
			string[] cesty = answer.Split('*');
			foreach (var cesta in cesty)
			{
				if (textBox1.Text.Split('-').OrderBy(a => a).SequenceEqual(cesta.Split('-').OrderBy(a => a))){
					correctAnswer = 1;
					body += 1;
				}
			}
			if (correctAnswer == 0){
				correctAnswer = -1;
			}
			
		} else {
			if (textBox1.Text.TrimStart().TrimEnd().ToUpper() == answer.ToUpper()){				
				correctAnswer = 1;
				body += 1;
			} else {
				correctAnswer = -1;
			}
		}
		button10.Visible = true;
		Invalidate();
		Update();
	}
	void Button10Click(object sender, EventArgs e)
	{
		if (level == 10) {
			level = 0;
		}
		generateQuestion();
		Invalidate();
		Update();
	}

	void deleteModeWarning(Graphics g){
		if(deleteMode)
			g.DrawString("Si v móde mazania!", drawFont2, deleteBrush, 350, 100, null);
	}
	
	void MainFormPaint(object sender, PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		
		g.FillRectangle(background, 0,0, 865,600);

		deleteModeWarning(g);
		
		if (zastavky.Count == 0){
			g.DrawString("Kliknutím na plochu vytvoríš zastávku!", drawFont1, zastavkaBrush2, 300, 270, null);
		}
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
			g.DrawString("Upravuješ linku č." + linkaClicked + "\n\n  Trasu jej rozšíriš\n        kliknutím\n     na zastávku\na potiahnutím šípky\n  k ďalšje zastávke.\n\n     Časti trate vieš\n  zmazať po sltační\n          tlačidla\n     'Zmazať čiaru'", drawFont1, zastavkaBrush2, 698, 160, null);
			
		} else if (rezim == 1 || rezim == 3){
			g.DrawString("Tu sú tvoje linky.\nKlikni na nejakú!", drawFont1, zastavkaBrush2, 707, 160, null);		
		}
		
		if (linkaClicked != 0){
			button6.Visible = true;
		} else {
			button6.Visible = false;			
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
		
		
		for (int i = 0; i < errors.Length; i++)
		{
			if (errors[i]){
				g.DrawString(errorMsgs[i], drawFontBold, deleteBrush, 200, 200 + 30*i, null);
			}
		}
		if (errors.Length > 0){
			errors = new bool[3];
		}
		
		if (rezim == 5){
			if (level == 10 && correctAnswer != 0){
				g.DrawString("                                     Koniec testu!\n                                       Skóre: " + body + "/10\nAk si želáš test zopakovať, klikni na tlačidlo 'Ďalšia otázka'.",drawFontBold, zastavkaBrush2, 170, 200, null);
			}
			g.DrawString("Odpoveď:", drawFontBold, zastavkaBrush2, 150, 525, null);
			g.DrawString(question, drawFont1, zastavkaBrush2, 8, 40, null);
			g.DrawString("Skóre: " + body + "/10", drawFontBold, zastavkaBrush2, 710, 170, null);
			if (correctAnswer == 1){
		    	g.DrawString("Správna odpoveď!\n         +1 bod", drawFont2, zastavkaBrush2, 350, 470, null);
		    } else if (correctAnswer == -1){
		    	g.DrawString("Nesprávna odpoveď!\n        Správne: " + answer, drawFont2, zastavkaBrush2, 330, 470, null);
		    }
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
		
		background = white;
		textBox1.Visible = false;
		button9.Visible = false;
		button10.Visible = false;
		
		errorMsgs.Add("                  Trať neobsahuje žiadne zastávky!");
		errorMsgs.Add("Na spustenie otázok musíš definovať aspoň dve trate!");
		errorMsgs.Add("    Jedna alebo viac tvojich liniek sú rozkúzkované!\n        Nevieme vytvoriť otázky kým ich nespojíš.");
	}
	
	void MainFormMouseClick(object sender, MouseEventArgs e) //nahrada 
	{
		if ((rezim == 3 || rezim ==1)) {
			if (deletingLine && linky[linkaClicked-1].zmazCiaru(e.X, e.Y)){				
				Invalidate();
				Update();
			} else if (deletingZast && closeTo(e.X, e.Y) != null){
				Zastavka z = closeTo(e.X, e.Y);
				zastavky.Remove(z);
				foreach (var l in linky)
				{
					l.zmazZastavku(z);
				}
				Invalidate();
				Update();
			} else if (linkaClicked != 0 && e.X <= 140 && e.Y >= 45 && e.Y <= 230){
				linky[linkaClicked-1].changeColour(findColour(e.X, e.Y));
				Invalidate();
				Update();
			} else if (e.X >= 700 && e.Y >= 45 && e.Y <= 185){		
				linkaClicked = findClickedLinka(e.X, e.Y);
				Invalidate();
				Update();
			} else if (closeTo(e.X, e.Y) == null && !holding) {		
				int name = 65;
				List<string> names = new List<string>();
				for (int i = 0; i < zastavky.Count; i++)
				{
					names.Add(zastavky[i].name);
				}
				for (int i = 0; i < 27; i++)
				{
					if (!names.Contains((char)(65+i) + "")){
						name = 65 + i;
						break;
					}
				}
				zastavky.Add(new Zastavka(e.X, e.Y, name));
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
		if ((rezim == 3 || rezim ==1) 
		&& !deletingZast
		&& !deletingLine
		&& z != null 
		&& zastavky.Count >= 2 
		&& linkaClicked != 0){
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
	
	
	void MainFormMouseDoubleClick(object sender, MouseEventArgs e)
	{
		if ((rezim == 1 || rezim == 3) && !holding){
			Zastavka z = closeTo(e.X, e.Y);
			if (z == null){
				return;
			}
			namingZast = true;
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
	
	Pen findColour(int X, int Y){
		int xP = 20;
		int yP = 60;
		for (int i = 0; i < pens.Count; i++)
		{
			if (X >= xP && X <= xP + 30 
				&&
				Y >= yP - 10 && Y <= yP + 10){
				return pens[i];
			}
			xP += 40;
			if ((i % 3)  == 2){
				xP = 20;
				yP += 30;
			}
		}
		return null;
	}
	
	bool[] checkLinky(){
		bool[] check = new bool[3];
		if (zastavky.Count == 0){
			check[0] = true;
		}
		int emptyLinky = 0;
		foreach (var linka in linky)
		{
			if (!linka.spojita()){
				check[2] = true;
			} 
			if (linka.isEmpty()){
				emptyLinky += 1;
			}
		}
		check[1] = emptyLinky >= 8;
		return check;
	}
	
	void generateQuestion(){
		correctAnswer = 0;
		level += 1;
		textBox1.Text = "";
		button10.Visible = false;
		answer = "";
		
		switch (level)
		{
			case 1:
				question1();
				break;
			case 2:
				question2();
				break;
			case 3:
				question3();
				break;
			case 4:
				question4();
				break;
			case 5:
				question5();
				break;
			case 6:
				question6();
				break;
			case 7:
				question7();
				break;
			case 8:
				question8();
				break;
			case 9:
				question9();
				break;
			case 10:
				question10();
				break;
			default:
				level = 0;
				break;
		}
	}
	
	void question1(){
		Linka l = linky[rnd.Next(0, linky.Count)];
		while (l.isEmpty()){
			l = linky[rnd.Next(0, linky.Count)];
		}
		question = "Otázka č.1: Koľko zastávok má linka č." + l.name + "?";
		answer = new HashSet<Zastavka>(l.mojeZastavky).Count + "";
	}
	
	void question2(){
		Linka l1 = linky[rnd.Next(0, linky.Count)];
		while (l1.isEmpty()){
			l1 = linky[rnd.Next(0, linky.Count)];
		}
		Linka l2 = linky[rnd.Next(0, linky.Count)];
		while (l2.isEmpty() || l1.name == l2.name){
			l2 = linky[rnd.Next(0, linky.Count)];
		}
		question = "Otázka č.2: Má linka č." + l1.name + " viac zastávok ako linka č." + l2.name + "?\n                   (Napíšte 'áno' alebo 'nie'.)";
		if (new HashSet<Zastavka>(l1.mojeZastavky).Count > new HashSet<Zastavka>(l2.mojeZastavky).Count){
			answer = "áno";
		} else {
			answer = "nie";
		}
	}
	
	void question3(){
		List<Linka> lMax = new List<Linka>();
		lMax.Add(linky[0]);
		for (int i = 1; i < linky.Count; i++)
		{
			if (new HashSet<Zastavka>(lMax[0].mojeZastavky).Count < new HashSet<Zastavka>(linky[i].mojeZastavky).Count){
				lMax.Clear();
				lMax.Add(linky[i]);
			} else if (new HashSet<Zastavka>(lMax[0].mojeZastavky).Count == new HashSet<Zastavka>(linky[i].mojeZastavky).Count){
				lMax.Add(linky[i]);
			}
		}
		question = "Otázka č.3: Ktorá linka má najviac zastávok?\n                    (Napíšte číslo linky. Ak je možností viac, vyberte jednu.)";
		foreach (var l in lMax)
		{
			answer += "-" + l.name;
		}
		answer = answer.Remove(0,1);
	}
	
	void question4(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];
		Zastavka z2 = zastavky[rnd.Next(0, zastavky.Count)];
		while (z1.name == z2.name){
			z2 = zastavky[rnd.Next(0, zastavky.Count)];
		}
		question = "Otázka č.4: Vieme sa zo zastávky " + z1.name + " dostať na zastávku " + z2.name + " len po jednej linke?\n                    (Napíšte 'áno' alebo 'nie'.)";
		foreach (var l in linky)
		{
			if (l.mojeZastavky.Contains(z1) && l.mojeZastavky.Contains(z2)){
				answer = "áno";
				return;
			}
		}
		answer = "nie";
	}
	
	void question5(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];
		Zastavka z2 = zastavky[rnd.Next(0, zastavky.Count)];
		while (z1.name == z2.name){
			z2 = zastavky[rnd.Next(0, zastavky.Count)];
		}
		
		question =  "Otázka č.5: Vieme sa zo zastávky " + z1.name + " nejako dostať na zastávku " + z2.name + " ?\n                    (Napíšte 'áno' alebo 'nie'.)";
		if (BFS(z1.name, z2.name, linky) > 0){
			answer = "áno";
		} else {
			answer = "nie";		
		}
	}
	
	void question6(){
		Linka l = linky[rnd.Next(0, linky.Count)];
		while (l.isEmpty()){
			l = linky[rnd.Next(0, linky.Count)];
		}
		Zastavka z1 = l.mojeZastavky[rnd.Next(0, l.mojeZastavky.Count)];
		Zastavka z2 = l.mojeZastavky[rnd.Next(0, l.mojeZastavky.Count)];
		while (z1.name == z2.name){
			z2 = l.mojeZastavky[rnd.Next(0, l.mojeZastavky.Count)];
		}
		
		question = "Otázka č.6: Koľko zastávok musíme prejsť\n                    aby sme sa zo zastávky " + z1.name + " dostali na zastávku " + z2.name + " po linke " + l.name +" najrýchlejšie?\n                    (Vrátane zastávky z ktorej vychádzame.)";
		List<Linka> lines = new List<Linka>();
		lines.Add(l);
		answer = BFS(z1.name, z2.name, lines) + "";
	}
	
	void question7(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];
		Zastavka z2 = zastavky[rnd.Next(0, zastavky.Count)];
		while (z1.name == z2.name){
			z2 = zastavky[rnd.Next(0, zastavky.Count)];
		}
		question = "Otázka č.7: Koľko zastávok musíme prejsť\n                  aby sme sa zo zastávky " + z1.name + " dostali na zastávku " + z2.name + " najrýchlejšie?\n                (Vrátane zastávky z ktorej vychádzame. Ak sa na zastávku nevieme dostať, napíšte 0.)";
		answer = BFS(z1.name, z2.name, linky) + "";
	}
	
	void question8(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];
		Zastavka z2 = zastavky[rnd.Next(0, zastavky.Count)];
		while (z1.name == z2.name){
			z2 = zastavky[rnd.Next(0, zastavky.Count)];
		}
		int number = rnd.Next(2, zastavky.Count - zastavky.Count/4);
		question = "Otázka č.8: Vieme sa zo zastávky " + z1.name + " dostať na zastávku " + z2.name + " cez najviac " + number;
		switch (number)
		{
			case 2:
				question += " zastávky?\n                    (Vrátane zastávky z ktorej vychádzame. Napíšte 'áno' alebo 'nie'.)";
				break;
			case 3:
				question += " zastávky?\n                    (Vrátane zastávky z ktorej vychádzame. Napíšte 'áno' alebo 'nie'.)";
				break;
			default:
				question += " zastávok?\n                    (Vrátane zastávky z ktorej vychádzame. Napíšte 'áno' alebo 'nie'.)";
				break;
		}
		if (BFS(z1.name, z2.name, linky) <= number){
			answer = "áno";
		} else {
			answer = "nie";
		}
	}
	
	void question9(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];		
		int number = rnd.Next(2, zastavky.Count/2);
		question = "Otázka č.9: Napíšte všetky zastávky, na ktoré sa vieme zo zastávky " + z1.name + "\n                    dostať prejdením cez " + number + " alebo menej zastávok.\n                    (Vrátane zastávky z ktorej vychádzame. Názvy zastávok oddeľte pomlčkami.)";
		answer = z1.name;
		foreach (var z in zastavky)
		{
			int bfs = BFS(z1.name, z.name, linky);
			if (z.name != z1.name && bfs <= number && bfs > 0){
				answer += "-" + z.name;
			}
		}
		
	}
	
	void question10(){
		Zastavka z1 = zastavky[rnd.Next(0, zastavky.Count)];
		Zastavka z2 = zastavky[rnd.Next(0, zastavky.Count)];
		while (z1.name == z2.name){
			z2 = zastavky[rnd.Next(0, zastavky.Count)];
		}
		
		Dictionary<string, HashSet<string>> list = new Dictionary<string, HashSet<string>>();
		foreach (var l in linky)
		{
			for (int i = 0; i < l.mojeZastavky.Count; i+=2)
			{
				if (!list.ContainsKey(l.mojeZastavky[i].name)){
					list[l.mojeZastavky[i].name] = new HashSet<string>();
				} 
				list[l.mojeZastavky[i].name].Add(l.mojeZastavky[i+1].name);
				if (!list.ContainsKey(l.mojeZastavky[i+1].name)){
					list[l.mojeZastavky[i+1].name] = new HashSet<string>();
				} 
				list[l.mojeZastavky[i+1].name].Add(l.mojeZastavky[i].name);
			}			
		}
		foreach (var z in zastavky)
		{
			if (!list.ContainsKey(z.name)){
				list[z.name] = new HashSet<string>();
			} 
		}
		List<string> visited = new List<string>();
		List<Tuple<string, string>> queue = new List<Tuple<string, string>>();
		string cesta = z1.name;
		queue.Add(new Tuple<string, string>(z1.name, cesta));
		while (queue.Count > 0){
			Tuple<string, string> t = queue[0];
			queue.RemoveAt(0);
			string zast = t.Item1;
			cesta = t.Item2;
			visited.Add(zast);
			if (zast == z2.name){
				answer += "*" + cesta;
			}
			foreach (var sused in list[zast])
			{
				if (!visited.Contains(sused)){
					queue.Add(new Tuple<string, string>(sused, cesta + "-" + sused));
				}
			}			
		}
		
		question = "Otázka č.10: Ktoré zastávky musíme prejsť aby sme sa zo zastávky " + z1.name + " dostali na zastávku " + z2.name + "\n                    najrýchlejšie?\n                    (Ak sa na zastávku nevieme dostať, napíšte 0. Ak je možností viac, vyberte jednu.)";
		if (answer.Length == 0){
			answer = "0";
		} else {
			answer = answer.Remove(0,1);
		}
	}
	
	int BFS(string start, string end, List<Linka> lines){
		Dictionary<string, HashSet<string>> list = new Dictionary<string, HashSet<string>>();
		foreach (var l in lines)
		{
			for (int i = 0; i < l.mojeZastavky.Count; i+=2)
			{
				if (!list.ContainsKey(l.mojeZastavky[i].name)){
					list[l.mojeZastavky[i].name] = new HashSet<string>();
				} 
				list[l.mojeZastavky[i].name].Add(l.mojeZastavky[i+1].name);
				if (!list.ContainsKey(l.mojeZastavky[i+1].name)){
					list[l.mojeZastavky[i+1].name] = new HashSet<string>();
				} 
				list[l.mojeZastavky[i+1].name].Add(l.mojeZastavky[i].name);
			}			
		}
		foreach (var z in zastavky)
		{
			if (!list.ContainsKey(z.name)){
				list[z.name] = new HashSet<string>();
			} 
		}
		List<string> visited = new List<string>();
		List<Tuple<string, int>> queue = new List<Tuple<string, int>>();
		int depth = 1;
		queue.Add(new Tuple<string, int>(start, depth));
		while (queue.Count > 0){
			Tuple<string, int> t = queue[0];
			queue.RemoveAt(0);
			string zast = t.Item1;
			depth = t.Item2;
			visited.Add(zast);
			if (zast == end){
				return depth;
			}
			foreach (var sused in list[zast])
			{
				if (!visited.Contains(sused)){
					queue.Add(new Tuple<string, int>(sused, depth+1));
				}
			}			
		}
		return 0;
	}
	
	
	/*class FakePen : GeneralPen{
		Color colour;
		int Width;
		...
	}
	
	class Pen : GeneralPen{
		Color colour; 
		int Width;	
		...
	}
	
	[TestMethod]
	public void testGetPoint()
	{
		Pen pen = new FakePen(Color.CadetBlue, 2);
		Linka linkaA = new Linka(2, pen);
		
		Assert.AreEqual([], linka.getPoints());
		
		Point p1 = new Point(10,23);
		Point p2 = new Point(224, -14);
		
		linkaA.pridajUlozenyPoint(p1);
		linkaA.pridajUlozenyPoint(p2);
		
		Assert.AreEqual(p1, linka.getPoints()[0]);
		Assert.AreEqual(p2, linka.getPoints()[1]);
	}*/
	
	class Linka {		
		public List<Zastavka> mojeZastavky;
		public int name;
		public Pen colour;
		public List<Point> delCoords;
		
		static bool[] visited;
		static bool[][] matica;
		
		public Linka(int nName, Pen pen){
			name = nName;
			colour = pen;
			mojeZastavky = new List<Zastavka>();
			delCoords = new List<Point>();
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
			    
			    if (deletingLine && (linkaClicked == name)){
			    	g.FillEllipse(white, center.X-10, center.Y-10, 30, 30);
					g.DrawEllipse(colour, center.X-10, center.Y-10, 30, 30);
					g.DrawString("X", drawFontBold, deleteBrush, center.X-4, center.Y-4, null);	
			    }			   
			} 
					
		}
		//nahrada nedostupneho parametra
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
			Point point = getCenter(z1.X, z1.Y, z2.X, z2.Y);
			delCoords.Add(point);
			mojeZastavky.Add(z2);
			mojeZastavky.Add(z1);
		}
				
		public bool zmazCiaru(int X, int Y){
			int index = -1;
			
			for (int i = 0; i < delCoords.Count; i++)
			{
				int X2 = delCoords[i].X+5;
				int Y2 = delCoords[i].Y+5;
				int dist = (X-X2)*(X-X2) + (Y-Y2)*(Y-Y2);
				
				if (dist <= 20*20){
					index = i;
					break;
				}
			}
			
			if (index >= 0){
				delCoords.RemoveAt(index);
				mojeZastavky.RemoveAt(index*2);
				mojeZastavky.RemoveAt(index*2);
				return true;
			}			
			return false;
		}

		public void zmazZastavku(Zastavka z){
			List<int> index = new List<int>();
			for (int i = 0; i < mojeZastavky.Count; i += 2)
			{
				if ((mojeZastavky[i].X == z.X && mojeZastavky[i].Y == z.Y) 
					||
					(mojeZastavky[i+1].X == z.X && mojeZastavky[i+1].Y == z.Y)){
					index.Add(i);
				}
			}
			if (index.Count > 0){
				index.Reverse();
				foreach (var i in index)
			    {
				    mojeZastavky.RemoveAt(i+1);
				    mojeZastavky.RemoveAt(i);				
			    }
			}
		}
		
		public void changeColour(Pen p){
			if (p != null){			
				colour = p;
			}
		}
		
		public bool isEmpty(){
			return mojeZastavky.Count == 0;
		}
		
		public bool spojita(){
			if (isEmpty())
		    {
				return true;
		    }
			List<Zastavka> zast = new List<Zastavka>();
			for (int i = 0; i < mojeZastavky.Count; i++)
			{
				if (!zast.Contains(mojeZastavky[i])){
					zast.Add(mojeZastavky[i]);
				}
			}
			matica = new bool[zast.Count][];
			for (int i = 0; i < matica.Length; i++)
			{
				matica[i] = new bool[zast.Count];
				for (int j = 0; j < matica.Length; j++)
				{
					matica[i][j] = false;
				}
			}
			
			for (int i = 0; i < mojeZastavky.Count; i += 2)
			{
				matica[zast.IndexOf(mojeZastavky[i])][zast.IndexOf(mojeZastavky[i+1])] = true;
				matica[zast.IndexOf(mojeZastavky[i+1])][zast.IndexOf(mojeZastavky[i])] = true;
			}
			
			visited = new bool[zast.Count]; 
			for (int i = 0; i < zast.Count; i++)
			{
				
				visited[i] = false;
			}
			int components = 0;
	        for(int v = 0; v < zast.Count; ++v)  
	        { 
	            if(!visited[v])  
	            { 
	                DFSUtil(v); 
	                components += 1;
	            } 
	        } 	   

	        return components == 1;
		}
		
		void DFSUtil(int v)  
	    { 
	        visited[v] = true; 
	        for (int i = 0; i < matica.Length; i++)
	        {
	        	if (matica[v][i] && !visited[i]){
	            	DFSUtil(i); 	   
	        	}
	        }  
	  
	    }
		public int getName(){
			return this.name;
		}

		public int getColour(){
			Predicate<Pen> allPen = myPen;
			var actualPen = pens.FindIndex(allPen);
			return actualPen;
		}

		private bool myPen(Pen p){
			return p.Equals(colour);
		}

		public int[] getSuradniceZastavky(){
			int[] suradnice = new int[0];
			if (mojeZastavky.Count != 0){
				suradnice = new int[mojeZastavky.Count*3];
				var a = 0;
				for(int i = 0; i < mojeZastavky.Count; i++){
					suradnice[a] = mojeZastavky[i].getX();
					suradnice[a+1] = mojeZastavky[i].getY();
					suradnice[a+2] = mojeZastavky[i].getNameNumber();
					a = a+3;
				}
			}
			return suradnice;
		}

		public int[] getPoint(){
			int[] point = new int[0];
			if(delCoords.Count != 0){
				point = new int[delCoords.Count*2];
				var a = 0;
				for(int i = 0; i < delCoords.Count; i++){
					point[a] = delCoords[i].X;
					point[a+1] = delCoords[i].Y;
					a = a+2;
				}
			}
			return point;
		}

		public void pridajUlozeneZastavky(Zastavka z1, Zastavka z2){
			mojeZastavky.Add(z2);
			mojeZastavky.Add(z1);
		}

		public void pridajUlozenePoint(Point point){
			delCoords.Add(point);
		}
	}
	
	//stub
/*	class testZastavka{
		int r = 10;

		public Zastavka(){
			susedia = new Dictionary<Zastavka, int>();
			X = 20;			
			Y = 10;
			r = 10;
			name = "5";
			name_number = 5;
		}		

		public bool isClose(int X, int Y){
			return Math.Sqrt((this.X - X)*(this.X - X) + (this.Y - Y)*(this.Y - Y)) < (r + 10 + 20);
		}

		public int getX(){
			return 10;
		}

		public int getY(){
			return 20;
		}

		public int getNameNumber(){
			return 5;
		}

	}*/

	class Zastavka {	
		public int X, Y, r, name_number;
		public string name;
		public Dictionary<Zastavka, int> susedia;
	
		public Zastavka(int nX, int nY, int ord){
			susedia = new Dictionary<Zastavka, int>();
			X = nX;			
			Y = nY;
			r = 10;
			name = (char)ord + "";
			name_number = ord;
		}		
		
		public void kresli(Graphics g){
			g.DrawEllipse(zastavkaPen, X-r, Y-r, 2*r, 2*r);
			g.FillEllipse(zastavkaBrush, X-r, Y-r, 2*r, 2*r);
			g.DrawString(name, drawFont1, zastavkaBrush2, X-r+2, Y-r+1, null);
			if (deletingZast){
				g.DrawString("X", drawFontBold, deleteBrush, X-r+1, Y-r, null);
			}
		}
		
		public bool isClose(int X, int Y){
			return Math.Sqrt((this.X - X)*(this.X - X) + (this.Y - Y)*(this.Y - Y)) < (10 + 10 + 20);
		}
		
		public void spoj(Zastavka z){
			if (!susedia.ContainsKey(z)){
				susedia[z] = 1;
			} else {
				susedia[z] += 1;
			}
			
		}

		public int getX(){
			return this.X;
		}

		public int getY(){
			return this.Y;
		}

		public int getNameNumber(){
			return this.name_number;
		}
			
	}
}
