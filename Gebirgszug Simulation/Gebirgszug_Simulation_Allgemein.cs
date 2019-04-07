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
        GraphicsDeviceManager graphics; //Allgemeine Einstellungen wie Fenstergröße, Hintergrundfarbe, etc.
        SpriteBatch spriteBatch; //Die Klasse zum Ausgeben von Sprites und Strings
        SpriteFont spriteFontgamefont; //Die benutze Schriftart für den spriteBatch
        PrimitiveBatch primitiveBatch; //Die Klasse zum ausgeben von zum Beispiel Linien

        Texture2D Maus_Texture; //Die Texture für die Maus
        MouseState Vorheriger_Maus_Status = Mouse.GetState(); //Die Position und Status(Buttons) der Maus

        public static bool N_Eingabe_Wurde_Geöffnet = false; //Ob der Dialog N_Eingabe offen ist Ja/Nein

        private static Random Zufall = new Random(); //Instanzierung der Klasse Random für Zufall zum Erzeugen von Zufallswerten

        public static int Anzahl_Der_Möglichkeiten = 1; //Die Anzahl der Möglichkeiten die für einen Gebirgszug der Länge N möglich sind
        public static int N = 2; //Die Länge N am Anfang mit 2 festgelegt

        Vector2 Gebirgszug_Zeichnung_Anfang; //Der Punkt an mit dem Zeichnen des Gebirgszuges begonnen wird
        Vector2 Gebirgszug_Zeichnung_Punkt; //Der aktuelle Punkt in der Schleife
        int Gebirgszug_Zeichnung_Längeneinheit; //Die Länge die ein Gebirgszugpunkt vom Nächsten entfernt ist.

        public Gebirgszug_Aufgaben Gebirgszug_Aufgaben; //Das Aufrufen der Klasse Gebirgszug_Aufgaben mit den Funktionen

        //Die Buttons, die die Funktionen aus Gebirgszug_Aufgaben aufrufen sollen
        Button Erstelle_Alle_6er_Gebirgszüge_Button; //Erstellt den Gebirgszug zur Länge N = 6
        Button Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button; //Erstellt den Gebirgszug zur Länge N = 16 und zeigt gleichzeitig die Anzahl der Möglichkeiten
        Button Gebe_N_Individuell_Ein_Button; //Ruft einen Dialog auf, in dem man N eingeben kann
        Button Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button; //Erstellt einen zufälligen Gebirgszug zur Länge N = 100
        Button Weiter_Button; //Erhöht den Index des Gebirgszuges
        Button Vorspulen_Button; // Erhöht bei gedrückter Maustaste den Index des Gebirgszuges (80 mal in der Sekunde)
        Button Anfang_Button; // Gibt allen Stellen die Höhe 0
        Button Ende_Button; // Gibt allen Stellen die höchstmögliche Höhe
        Button Zufall_Button; //Erstellt einen (neuen) zufälligen Gebirgszug zum vorher gewählten N
        Button Erhöhe_N_Button;
        Button Verringere_N_Button;

        public Gebirgszug_Simulation_Allgemein()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
            //Fenstergröße wird festgelegt und übernommen
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
            Erstelle_Alle_6er_Gebirgszüge_Button = new Button(Content.Load<Texture2D>("Sprites//N = 6"), new Vector2(10, 10));
            Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button = new Button(Content.Load<Texture2D>("Sprites//N = 16"), new Vector2(10, 100));
            Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button = new Button(Content.Load<Texture2D>("Sprites//N = 100"), new Vector2(10, 191));
            Gebe_N_Individuell_Ein_Button = new Button(Content.Load<Texture2D>("Sprites//N = Individuell"), new Vector2(10, 282));
            Erhöhe_N_Button = new Button(Content.Load<Texture2D>("Sprites//Erhöhe N"), new Vector2(10, 373));
            Verringere_N_Button = new Button(Content.Load<Texture2D>("Sprites//Verringere N"), new Vector2(10, 464));
            Zufall_Button = new Button(Content.Load<Texture2D>("Sprites//Zufall"), new Vector2(361, 720));
            Anfang_Button = new Button(Content.Load<Texture2D>("Sprites//Anfang"), new Vector2(220, 720));
            Vorspulen_Button = new Button(Content.Load<Texture2D>("Sprites//Vorspulen"), new Vector2(682, 720));
            Weiter_Button = new Button(Content.Load<Texture2D>("Sprites//Weiter"), new Vector2(522, 720));
            Ende_Button = new Button(Content.Load<Texture2D>("Sprites//Ende"), new Vector2(823, 720));

            //Die Maus erhält ihre Texture
            Maus_Texture = Content.Load<Texture2D>("Sprites//Gebirgszugmaus");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState Momentaner_Maus_Status = Mouse.GetState(); //Der momentane Maus-Status wird aktualisiert

            //Wenn die Maus in der vorherigen 80stel Sekunde die linkte Maustaste nicht gedrückt hatte und in dieser 80stel Sekunde doch, werden im folgenden die jeweiligen Funktionen der Buttons aufgerufen
            if ((Momentaner_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) && (Vorheriger_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
            {//Im Folgenden wird bei jedem Button abgefragt ob die Maus mit einer Viereckgröße der Buttons einen Punkt gemeinsam hat
                if (Erhöhe_N_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N++;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                }
                if (Gebe_N_Individuell_Ein_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    if (!N_Eingabe_Wurde_Geöffnet)//Das Fenster soll nur einmal geöffnet werden, daher wird ein Bit (N_Eingabe_Wurde_Geöffnet) abgefragt
                    {
                        //WennDer Dialog nicht geöffnet ist, wird der Bit auf true gesetzt und der Dialog geöffnet
                        N_Eingabe_Wurde_Geöffnet = true;
                        N_Eingabe N_Eingabe = new N_Eingabe();
                        N_Eingabe.ShowDialog(); //Das Dialogfenster für die Eingabe von N wird geössnet
                        Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
                        Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                    }
                }
                if (Verringere_N_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    if (N >= 2)
                    {
                        N--;
                        Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
                        Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                    }
                }
                if (Zufall_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                }
                if (Erstelle_Alle_6er_Gebirgszüge_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 6;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde

                }
                if (Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 16;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde

                }
                if (Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    N = 100;
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                }

                if (Zufall_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(N); //Für N wird ein zufälliger Gebirgszug erstellt
                }
                if (Anfang_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben = new Gebirgszug_Aufgaben(N); //Der Gebirgszug muss aktualisiert werden, da N verändert wurde
                }
                if (Ende_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Springe_Zum_Ende(N); //Der Index des Gebirgszuges erhält sein Maximum, da alle Stellen des Gebirgszuges die maximale Höhe bekommen
                }
                if (Weiter_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erhöhe_den_Index(N); //Der Index des Gebirgszuges wird erhöht
                }
            }
            //Wenn der Mauszeiger über dem Button ist und geklickt wird, wird die Funktion alle 80 Sekunden ausgeführt
            if ((Momentaner_Maus_Status.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (Vorspulen_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                {
                    Gebirgszug_Aufgaben.Erhöhe_den_Index(N); //Der Index des Gebirgszuges wird erhöht
                }
            }


            Vorheriger_Maus_Status = Momentaner_Maus_Status; //Der vorheriger Maus-Status wird am Ende der 80stel Sekunde aktualisiert
            //Er ist der vorherige Momentaner_Maus_Status, welcher jedoch am Anfang der nächsten 80stel Sekunde aktualisiert wird
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

            graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Graphics.Color.CornflowerBlue); //Der Hintergrund wird gefüllt

            //Alle Buttons werden gemalt unter Berücksichtigung der Alpha Textur
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(Erstelle_Alle_6er_Gebirgszüge_Button.Sprite, Erstelle_Alle_6er_Gebirgszüge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Sprite, Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Sprite, Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Weiter_Button.Sprite, Weiter_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Vorspulen_Button.Sprite, Vorspulen_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Anfang_Button.Sprite, Anfang_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Gebe_N_Individuell_Ein_Button.Sprite, Gebe_N_Individuell_Ein_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Ende_Button.Sprite, Ende_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Zufall_Button.Sprite, Zufall_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Erhöhe_N_Button.Sprite, Erhöhe_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(Verringere_N_Button.Sprite, Verringere_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.End();

            //Wenn der Mauszeiger über einem Button ist, wird dieser nochmal über sich selbst gemalt(Jedoch in einer anderen Farbe) 
            //Die Farben die übereinander liegen werden gemischt, dem Benutzer wird die aktivierung des Buttons signalisiert
            spriteBatch.Begin(SpriteBlendMode.Additive);
            if (Erstelle_Alle_6er_Gebirgszüge_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erstelle_Alle_6er_Gebirgszüge_Button.Sprite, Erstelle_Alle_6er_Gebirgszüge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Sprite, Zeige_Die_Anzahl_Aller_16er_Gebirgszüge_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Sprite, Erstelle_Einen_Zufälligen_100er_Gebirgszug_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Weiter_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Weiter_Button.Sprite, Weiter_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Vorspulen_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Vorspulen_Button.Sprite, Vorspulen_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Anfang_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Anfang_Button.Sprite, Anfang_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Gebe_N_Individuell_Ein_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Gebe_N_Individuell_Ein_Button.Sprite, Gebe_N_Individuell_Ein_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Ende_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Ende_Button.Sprite, Ende_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Zufall_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Zufall_Button.Sprite, Zufall_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Erhöhe_N_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Erhöhe_N_Button.Sprite, Erhöhe_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            if (Verringere_N_Button.Viereckgröße.Contains(new Microsoft.Xna.Framework.Point((int)Momentaner_Maus_Status.X, (int)Momentaner_Maus_Status.Y)))
                spriteBatch.Draw(Verringere_N_Button.Sprite, Verringere_N_Button.Position, Microsoft.Xna.Framework.Graphics.Color.DarkSlateBlue);
            spriteBatch.End();


            //Es erfolgt die Ausgabe von der Buchstaben N und H für Stelle und dessen Höhe und den dazugehörigen Werten der ersten stelle (0 und 0)
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFontgamefont, "N", (new Vector2(Gebirgszug_Zeichnung_Anfang.X - 20, Gebirgszug_Zeichnung_Anfang.Y)), Microsoft.Xna.Framework.Graphics.Color.Red);
            spriteBatch.DrawString(spriteFontgamefont, "H", (new Vector2(Gebirgszug_Zeichnung_Anfang.X - 20, Gebirgszug_Zeichnung_Anfang.Y + 20)), Microsoft.Xna.Framework.Graphics.Color.Red);
            spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[0].Höhe, (new Vector2(Gebirgszug_Zeichnung_Anfang.X, Gebirgszug_Zeichnung_Anfang.Y + 20)), Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.DrawString(spriteFontgamefont, "0", (Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);

            if (!Gebirgszug_Aufgaben.Zu_Viele_Möglichkeiten) //Wenn die Länge N nicht zu groß ist wird die Anzahl der Möglichkeiten ausgegeben, wenn nicht, wird angezeigt, dass es zu viele Möglichkeiten gibt
                spriteBatch.DrawString(spriteFontgamefont, "Die Anzahl der moeglichen Gebirgszuge betraegt: " + Gebirgszug_Aufgaben.Anzahl_Der_Möglichkeiten, new Vector2(300, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);
            else
                spriteBatch.DrawString(spriteFontgamefont, "Die Anzahl der moeglichen Gebirgszuge ist zu hoch, um sie zu berechnen ", new Vector2(300, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);
            spriteBatch.DrawString(spriteFontgamefont, "N= " + N, new Vector2(220, 10), Microsoft.Xna.Framework.Graphics.Color.Yellow);

            //Die Längeneinheit wird unter Abhängigkeit der zur Verfügung gestellten Pixel und der Länge N erstellt
            Gebirgszug_Zeichnung_Längeneinheit = ((768) / (N));
            Gebirgszug_Zeichnung_Anfang = new Vector2(220, 680); //Der Anfang der Zeichnung wird festgelegt
            Gebirgszug_Zeichnung_Punkt = new Vector2(0, 0); //Der momentane Punktvektor soll zum Anfang 0 betragen
            primitiveBatch.Begin(PrimitiveType.LineList);// Eine Linienlisten Zeichnung wird begonnen, 
            //von nun an werden immer 2 Punkte(Vertex) zu einer Linie verbunden
            for (int i = 1; i <= N; i++)
            {
                //Von jeder Höhe einer Stelle wird eine Linie zur nächsten Höhe gezogen
                primitiveBatch.AddVertex((Gebirgszug_Zeichnung_Punkt + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.Violet);
                Gebirgszug_Zeichnung_Punkt *= new Vector2(1, 0); //Der Y Wert wird wieder 0 gesetzt, X bleibt
                Gebirgszug_Zeichnung_Punkt = new Vector2(Gebirgszug_Zeichnung_Punkt.X + Gebirgszug_Zeichnung_Längeneinheit, (Gebirgszug_Zeichnung_Punkt.Y - (Gebirgszug_Zeichnung_Längeneinheit * Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].Höhe)));//Der Punkt verschiebt sich um eine Längeneinheit nach rechts und um die jeweilige Hähe der nächsten Stelle nach oben
                primitiveBatch.AddVertex((Gebirgszug_Zeichnung_Punkt + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.Violet);

                if (N < 35)
                {
                    // Wenn N < 35 ist, dann wird unter jeder Stelle dessen Position und Höhe angegeben
                    spriteBatch.DrawString(spriteFontgamefont, "" + i, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 0) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                    spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].Höhe, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 20) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                }
                else
                {
                    if ((i % 10) == 0)
                    {
                        //Ist N zu groß, überlagern sich die Angaben, daher wird nur noch jede 10 Stelle berücksichtigt
                        spriteBatch.DrawString(spriteFontgamefont, "" + i, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 0) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                        spriteBatch.DrawString(spriteFontgamefont, "" + Gebirgszug_Aufgaben.Gebirgszug.Gebirgszug_Stelle[i].Höhe, (new Vector2(Gebirgszug_Zeichnung_Punkt.X, 20) + Gebirgszug_Zeichnung_Anfang), Microsoft.Xna.Framework.Graphics.Color.White);
                    }
                }
                //Vom Gebirgszug_Zeichnung_Anfang Punkt + dem X Wert des Gebirgszug_Zeichnung_Punktes wird eine Linie zur jeweiligen Höhe der Stelle gezogen
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
