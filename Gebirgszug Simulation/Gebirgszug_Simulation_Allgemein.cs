using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gebirgszug_Simulation
{
    public class Gebirgszug_Simulation_Allgemein : Microsoft.Xna.Framework.Game
    {
        //Die Klassen GraphicsDeviceManager, SpriteBatch, SpriteFont und PrimitiveBatch vom XNA Framework werden geladen
        GraphicsDeviceManager graphics; //Allgemeine Einstellungen wie Fenstergr��e, Hintergrundfarbe, etc.
        SpriteBatch spriteBatch; //Die Klasse zum Ausgeben von Sprites und Strings
        SpriteFont spriteFontgamefont; //Die benutze Schriftart f�r den spriteBatch
        PrimitiveBatch primitiveBatch; //Die Klasse zum ausgeben von zum Beispiel Linien

        Texture2D Maus_Texture; //Die Texture f�r die Maus
        MouseState Vorheriger_Maus_Status = Mouse.GetState(); //Die Position und Status(Buttons) der Maus

        public static bool N_Eingabe_Wurde_Ge�ffnet = false; //Ob der Dialog N_Eingabe offen ist Ja/Nein

        private static Random Zufall = new Random(); //Instanzierung der Klasse Random f�r Zufall zum Erzeugen von Zufallswerten

        public static int Anzahl_Der_M�glichkeiten = 1; //Die Anzahl der M�glichkeiten die f�r einen Gebirgszug der L�nge N m�glich sind
        public static int N = 2; //Die L�nge N am Anfang mit 2 festgelegt

        Vector2 Gebirgszug_Zeichnung_Anfang; //Der Punkt an mit dem Zeichnen des Gebirgszuges begonnen wird
        Vector2 Gebirgszug_Zeichnung_Punkt; //Der aktuelle Punkt in der Schleife
        int Gebirgszug_Zeichnung_L�ngeneinheit; //Die L�nge die ein Gebirgszugpunkt vom N�chsten entfernt ist.

        public Gebirgszug_Aufgaben Gebirgszug_Aufgaben; //Das Aufrufen der Klasse Gebirgszug_Aufgaben mit den Funktionen

        //Die Buttons, die die Funktionen aus Gebirgszug_Aufgaben aufrufen sollen
        Button Erstelle_Alle_6er_Gebirgsz�ge_Button; //Erstellt den Gebirgszug zur L�nge N = 6
        Button Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button; //Erstellt den Gebirgszug zur L�nge N = 16 und zeigt gleichzeitig die Anzahl der M�glichkeiten
        Button Gebe_N_Individuell_Ein_Button; //Ruft einen Dialog auf, in dem man N eingeben kann
        Button Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button; //Erstellt einen zuf�lligen Gebirgszug zur L�nge N = 100
        Button Weiter_Button; //Erh�ht den Index des Gebirgszuges
        Button Vorspulen_Button; // Erh�ht bei gedr�ckter Maustaste den Index des Gebirgszuges (80 mal in der Sekunde)
        Button Anfang_Button; // Gibt allen Stellen die H�he 0
        Button Ende_Button; // Gibt allen Stellen die h�chstm�gliche H�he
        Button Zufall_Button; //Erstellt einen (neuen) zuf�lligen Gebirgszug zum vorher gew�hlten N
        Button Erh�he_N_Button;
        Button Verringere_N_Button;

        public Gebirgszug_Simulation_Allgemein()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
            //Fenstergr��e wird festgelegt und �bernommen
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFontgamefont = Content.Load<SpriteFont>("Sprites\\gamefont");
            primitiveBatch = new PrimitiveBatch(graphics.GraphicsDevice);

            //Alle Buttons laden hier ihre Textur und bekommen eine festgelegte Position
            Erstelle_Alle_6er_Gebirgsz�ge_Button = new Button(Content.Load<Texture2D>("Sprites//N = 6"), new Vector2(10, 10));
            Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button = new Button(Content.Load<Texture2D>("Sprites//N = 16"), new Vector2(10, 100));
            Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button = new Button(Content.Load<Texture2D>("Sprites//N = 100"), new Vector2(10, 191));
            Gebe_N_Individuell_Ein_Button = new Button(Content.Load<Texture2D>("Sprites//N = Individuell"), new Vector2(10, 282));
            Erh�he_N_Button = new Button(Content.Load<Texture2D>("Sprites//Erh�he N"), new Vector2(10, 373));
            Verringere_N_Button = new Button(Content.Load<Texture2D>("Sprites//Verringere N"), new Vector2(10, 464));
            Zufall_Button = new Button(Content.Load<Texture2D>("Sprites//Zufall"), new Vector2(361, 720));
            Anfang_Button = new Button(Content.Load<Texture2D>("Sprites//Anfang"), new Vector2(220, 720));
            Vorspulen_Button = new Button(Content.Load<Texture2D>("Sprites//Vorspulen"), new Vector2(682, 720));
            Weiter_Button = new Button(Content.Load<Texture2D>("Sprites//Weiter"), new Vector2(522, 720));
            Ende_Button = new Button(Content.Load<Texture2D>("Sprites//Ende"), new Vector2(823, 720));

            //Die Maus erh�lt ihre Texture
            Maus_Texture = Content.Load<Texture2D>("Sprites//Gebirgszugmaus");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState Momentaner_Maus_Status = Mouse.GetState(); //Der momentane Maus-Status wird aktualisiert

            //Wenn die Maus in der vorherigen 80stel Sekunde die linkte Maustaste nicht gedr�ckt hatte und in dieser 80stel Sekunde doch, werden im folgenden die jeweiligen Funktionen der Buttons aufgerufen
            if ((Momentaner_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) && (Vorheriger_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
            {//Im Folgenden wird bei jedem Button abgefragt ob die Maus mit einer Viereckgr��e der Buttons einen Punkt gemeinsam hat
                if (Erh�he_N_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N++;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                }
                if (Gebe_N_Individuell_Ein_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    if (!N_Eingabe_Wurde_Ge�ffnet)//Das Fenster soll nur einmal ge�ffnet werden, daher wird ein Bit (N_Eingabe_Wurde_Ge�ffnet) abgefragt
                    {
                        //WennDer Dialog nicht ge�ffnet ist, wird der Bit auf true gesetzt und der Dialog ge�ffnet
                        N_Eingabe_Wurde_Ge�ffnet = true;
                        N_Eingabe N_Eingabe = new N_Eingabe();
                        N_Eingabe.ShowDialog(); //Das Dialogfenster f�r die Eingabe von N wird ge�ssnet
                        Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
                        Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                    }
                }
                if (Verringere_N_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    if (N >= 2)
                    {
                        N--;
                        Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
                        Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                    }
                }
                if (Zufall_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                }
                if (Erstelle_Alle_6er_Gebirgsz�ge_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 6;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde

                }
                if (Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 16;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde

                }
                if (Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 100;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                }

                if (Zufall_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zuf�lligen_Gebirgszug_Zu_N(N); //F�r N wird ein zuf�lliger Gebirgszug erstellt
                }
                if (Anfang_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N ver�ndert wurde
                }
                if (Ende_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Springe_Zum_Ende(N); //Der Index des Gebirgszuges erh�lt sein Maximum, da alle Stellen des Gebirgszuges die maximale H�he bekommen
                }
                if (Weiter_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erh�he_den_Index(N); //Der Index des Gebirgszuges wird erh�ht
                }
            }
            //Wenn der Mauszeiger �ber dem Button ist und geklickt wird, wird die Funktion alle 80 Sekunden ausgef�hrt
            if ((Momentaner_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (Vorspulen_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erh�he_den_Index(N); //Der Index des Gebirgszuges wird erh�ht
                }
            }


            Vorheriger_Maus_Status = Momentaner_Maus_Status; //Der vorheriger Maus-Status wird am Ende der 80stel Sekunde aktualisiert
            //Er ist der vorherige Momentaner_Maus_Status, welcher jedoch am Anfang der n�chsten 80stel Sekunde aktualisiert wird
            base.Update(gameTime);
        }

        //Die Funktionen zum Berechnen eines Zufalls zwischen zwei ganzen Zahlen
        public int GanzZahligerZufallZwischen(int Minimum, int Maximum)
        {
            return (int)Math.Round((Minimum + Zufall.NextDouble() * (Maximum - Minimum)));
        }

        protected override void Draw(GameTime gameTime)
        {
            MouseState Momentaner_Maus_Status = Mouse.GetState(); //Der momentane Maus-Status wird aktualisiert

            graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Graphics.Color.CornflowerBlue); //Der Hintergrund wird gef�llt

            //Alle Buttons werden gemalt unter Ber�cksichtigung der Alpha Textur
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(Erstelle_Alle_6er_Gebirgsz�ge_Button.Sprite, Erstelle_Alle_6er_Gebirgsz�ge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Sprite, Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Sprite, Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Weiter_Button.Sprite, Weiter_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Vorspulen_Button.Sprite, Vorspulen_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Anfang_Button.Sprite, Anfang_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Gebe_N_Individuell_Ein_Button.Sprite, Gebe_N_Individuell_Ein_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Ende_Button.Sprite, Ende_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Zufall_Button.Sprite, Zufall_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Erh�he_N_Button.Sprite, Erh�he_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Verringere_N_Button.Sprite, Verringere_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.End();

            //Wenn der Mauszeiger �ber einem Button ist, wird dieser nochmal �ber sich selbst gemalt(Jedoch in einer anderen Farbe) 
            //Die Farben die �bereinander liegen werden gemischt, dem Benutzer wird die aktivierung des Buttons signalisiert
            spriteBatch.Begin(SpriteBlendMode.Additive);
            if (Erstelle_Alle_6er_Gebirgsz�ge_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erstelle_Alle_6er_Gebirgsz�ge_Button.Sprite, Erstelle_Alle_6er_Gebirgsz�ge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Sprite, Zeige_Die_Anzahl_Aller_16er_Gebirgsz�ge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Sprite, Erstelle_Einen_Zuf�lligen_100er_Gebirgszug_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Weiter_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Weiter_Button.Sprite, Weiter_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Vorspulen_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Vorspulen_Button.Sprite, Vorspulen_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Anfang_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Anfang_Button.Sprite, Anfang_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Gebe_N_Individuell_Ein_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Gebe_N_Individuell_Ein_Button.Sprite, Gebe_N_Individuell_Ein_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Ende_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Ende_Button.Sprite, Ende_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Zufall_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Zufall_Button.Sprite, Zufall_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Erh�he_N_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erh�he_N_Button.Sprite, Erh�he_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Verringere_N_Button.Viereckgr��e.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Verringere_N_Button.Sprite, Verringere_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            spriteBatch.End();


            //Es erfolgt die Ausgabe von der Buchstaben N und H f�r Stelle und dessen H�he und den dazugeh�rigen Werten der ersten stelle (0 und 0)
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFontgamefont, "N", (new Vector2(Gebirgszug_Zeichnung_Anfang.X - 20, Gebirgszug_Zeichnung_Anfang.Y)), Microsoft.Xna.Framework.Graphics.Color.Red);
            spriteBatch.DrawString(spriteFontgamefont, "H", (new Vector2(Gebirgszug_Zeichnung_Anfang.X - 20, Gebirgszug_Zeichnung_Anfang.Y + 20)), Microsoft.Xna.Framework.Graphics.Color.Red);
            spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[0].H�he, (new Vector2(Gebirgszug_Zeichnung_Anfang.X, Gebirgszug_Zeichnung_Anfang.Y + 20)), Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.DrawString(spriteFontgamefont, "0", (Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);

            if (!Gebirgszug_Aufgaben.Zu_Viele_M�glichkeiten) //Wenn die L�nge N nicht zu gro� ist wird die Anzahl der M�glichkeiten ausgegeben, wenn nicht, wird angezeigt, dass es zu viele M�glichkeiten gibt
                spriteBatch.DrawString(spriteFontgamefont, "Die Anzahl der moeglichen Gebirgszuge betraegt: " + Gebirgszug_Aufgaben.Anzahl_Der_M�glichkeiten, new Vector2(300, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);
            else
                spriteBatch.DrawString(spriteFontgamefont, "Die Anzahl der moeglichen Gebirgszuge ist zu hoch, um sie zu berechnen ", new Vector2(300, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);
            spriteBatch.DrawString(spriteFontgamefont, "N= " + N, new Vector2(220, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);

            //Die L�ngeneinheit wird unter Abh�ngigkeit der zur Verf�gung gestellten Pixel und der L�nge N erstellt
            Gebirgszug_Zeichnung_L�ngeneinheit = ((768) / (N));
            Gebirgszug_Zeichnung_Anfang = new Vector2(220, 680); //Der Anfang der Zeichnung wird festgelegt
            Gebirgszug_Zeichnung_Punkt = new Vector2(0, 0); //Der momentane Punktvektor soll zum Anfang 0 betragen
            primitiveBatch.Begin(PrimitiveType.LineList);// Eine Linienlisten Zeichnung wird begonnen, 
            //von nun an werden immer 2 Punkte(Vertex) zu einer Linie verbunden
            for (int i = 1; i <= N; i++)
            {
                //Von jeder H�he einer Stelle wird eine Linie zur n�chsten H�he gezogen
                primitiveBatch.AddVertex((Gebirgszug_Zeichnung_Punkt + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.Violet);
                Gebirgszug_Zeichnung_Punkt *= new Vector2(1, 0); //Der Y Wert wird wieder 0 gesetzt, X bleibt
                Gebirgszug_Zeichnung_Punkt = new Vector2(Gebirgszug_Zeichnung_Punkt.X + Gebirgszug_Zeichnung_L�ngeneinheit, (Gebirgszug_Zeichnung_Punkt.Y - (Gebirgszug_Zeichnung_L�ngeneinheit * Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].H�he)));//Der Punkt verschiebt sich um eine L�ngeneinheit nach rechts und um die jeweilige H�he der n�chsten Stelle nach oben
                primitiveBatch.AddVertex((Gebirgszug_Zeichnung_Punkt + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.Violet);

                if (N < 35)
                {
                    // Wenn N < 35 ist, dann wird unter jeder Stelle dessen Position und H�he angegeben
                    spriteBatch.DrawString(spriteFontgamefont, "" + i, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 0) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                    spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].H�he, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 20) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                }
                else
                {
                    if ((i % 10) == 0)
                    {
                        //Ist N zu gro�, �berlagern sich die Angaben, daher wird nur noch jede 10 Stelle ber�cksichtigt
                        spriteBatch.DrawString(spriteFontgamefont, "" + i, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 0) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                        spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].H�he, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 20) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                    }
                }
                //Vom Gebirgszug_Zeichnung_Anfang Punkt + dem X Wert des Gebirgszug_Zeichnung_Punktes wird eine Linie zur jeweiligen H�he der Stelle gezogen
                primitiveBatch.AddVertex(((new Vector2(Gebirgszug_Zeichnung_Punkt.X, 0) + Gebirgszug_Zeichnung_Anfang)), Microsoft.Xna.Framework.Graphics.Color.White);
                primitiveBatch.AddVertex((Gebirgszug_Zeichnung_Punkt + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
            }
            primitiveBatch.End();
            
            spriteBatch.Draw(Maus_Texture, new Vector2(Momentaner_Maus_Status.X, Momentaner_Maus_Status.Y), Microsoft.Xna.Framework.Graphics.Color.White);//Das Mausbild wird immer an der Mausposition gemalt
            spriteBatch.End();
            Vorheriger_Maus_Status = Momentaner_Maus_Status; //Der vorheriger Maus-Status wird am Ende der 80stel Sekunde deklariert 
            base.Draw(gameTime);
        }
    }
}
