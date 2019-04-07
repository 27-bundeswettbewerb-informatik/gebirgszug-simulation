using System;
using System.Collections.Generic;
using System.Text;

namespace Gebirgszug_Simulation
{
    public class Gebirgszug_Aufgaben
    {
        //der Klasse Gebirgszug_Aufgaben liegen alle Methoden die die Aufgaben lösen sollen,   
        public Gebirgszug Gebirgszug;//der Gebirgszug selber,
        public ulong Anzahl_Der_Möglichkeiten = 1;//dessen Anzahl der Möglichkeiten und
        public int Mitte;//dessen Mitte zu Grunde
        Random Zufall = new Random();//Außerdem gibt es einen Zufallsgenerator
        public bool Zu_Viele_Möglichkeiten = false;//und den Bit der feststellt, ob es zu viele Möglichkeiten gäbe, die bei der Berechnung zu viel Zeit in Anspruch nehmen würden
        

        public void Springe_Zum_Ende(int N)
        {
            for (int i = 0; i <= N; i++)
            {
                //Solange nicht Alle Gebirgszugstellen betrachten wurden, werden dessen Höhen ihrer Maximalen Höhe gleich gesetzt
                Gebirgszug.Gebirgszug_Stelle[i].Höhe = Gebirgszug.Gebirgszug_Stelle[i].Maximale_Höhe;
            }
        }

        public void Erstelle_Einen_Zufälligen_Gebirgszug_Zu_N(int N)
        { 
            //Zunächst wird eine Mitte bestimmt von der wir ausgehen, welche zufallsmäßig aus allen Gebirgszugstellen ermittelt wird
            Mitte = GanzZahligerZufallZwischen(0, N);

            //Dieser gebirgszug erhält eine Zufällige Höhe, die aber auf jeden Fall zwischen 0 und der maximalen Höhe liegt
            Gebirgszug.Gebirgszug_Stelle[Mitte].Höhe = GanzZahligerZufallZwischen(0, Gebirgszug.Gebirgszug_Stelle[Mitte].Maximale_Höhe);
            
            for (int i = Mitte + 1; i <= N; i++)
            {
                //Von dieser Mitte wird nun aus nach Rechts zum Ende gestrebt und jede Stelle bekommt eine Zufällige Höhe, die aber immer nur zum Vorgänger einen Höhenunterschied von 1 aufweisen darf
                Gebirgszug.Gebirgszug_Stelle[i].Höhe = GanzZahligerZufallZwischen((Gebirgszug.Gebirgszug_Stelle[i - 1].Höhe - 1), (Gebirgszug.Gebirgszug_Stelle[i - 1].Höhe + 1));
            }
            for (int i = Mitte - 1; i >= 0; i--)
            {
                //Von der Mitte wird nun auch nach links zum Anfang gestrebt und jede Stelle bekommt eine Zufällige Höhe, die aber immer nur zum Nachfolger einen Höhenunterschied von 1 aufweisen darf
                Gebirgszug.Gebirgszug_Stelle[i].Höhe = GanzZahligerZufallZwischen((Gebirgszug.Gebirgszug_Stelle[i + 1].Höhe - 1), (Gebirgszug.Gebirgszug_Stelle[i + 1].Höhe + 1));
            }

            //Wenn innerhalb der 2 vorheriegen Schleifen ein Fehler in Sachen maximale und minimale Höhenüberschreitung vorgekommen ist, wird das durch folgende Schleife korrigiert
            for (int i = 0; i <= N; i++)
            {
                //Solangen nicht alle Stellen betrachtet wurden werden diese, wenn ihre Höhe größer ist als ihre maximale Höhe, der maximalen Höhe gleich gesetzt
                if (Gebirgszug.Gebirgszug_Stelle[i].Höhe > Gebirgszug.Gebirgszug_Stelle[i].Maximale_Höhe)
                    Gebirgszug.Gebirgszug_Stelle[i].Höhe = Gebirgszug.Gebirgszug_Stelle[i].Maximale_Höhe;
                //und wenn sie kleiner 0 geworden sind, werden sie 0 gleich gesetzt
                if (Gebirgszug.Gebirgszug_Stelle[i].Höhe < 0)
                    Gebirgszug.Gebirgszug_Stelle[i].Höhe = 0;
            }

            
        }

        //Die Funktionen zum Berechnen eines Zufalls zwischen zwei ganzen Zahlen
        public int GanzZahligerZufallZwischen(int Minimum, int Maximum)
        {
            return (int)Math.Round((Minimum + Zufall.NextDouble() * (Maximum - Minimum)));
        }

        public void Gib_Die_Anzahl_Der_Möglichkeiten_Wieder(int N)
        {
            //Diese Funktion wird nur aufgerufen wenn die Länge höchstens 16 ist, da es sonst bei der Berechnung zu einer unflüssigen Ausgabe oder sogar zu Programmabstürzen kommt
            if (N <= 16)
            {
                //Zunächst müssen alle Stellen 0 gesetzt werden, da ansonsten nicht alle Möglichkeiten betrachtet werden
                Gebirgszug.Setzte_Alle_Stellen_Auf_Null(N);
                Anzahl_Der_Möglichkeiten = 1; //Die Anzahl wird Anfangs 0 gesetzt, da es die Möglichkeit des Gebirgszuges mit allen Gebirgszügen der Stelle 0 immer gibt


                while ((Gebirgszug.Gebirgszug_Stelle[Math.Abs((N + 1) / 2) - 1].Höhe < Gebirgszug.Gebirgszug_Stelle[Math.Abs((N + 1) / 2) - 1].Maximale_Höhe) || (Gebirgszug.Gebirgszug_Stelle[Math.Abs((N + 1) / 2)].Höhe < Gebirgszug.Gebirgszug_Stelle[Math.Abs((N + 1) / 2)].Maximale_Höhe))
                {
                    //Solange die maximale Höhe der beiden Mitten oder der einen Mitte(N gerade oder ungerade) nicht erreicht ist
                    Erhöhe_den_Index(N);//Wird der Index erhöht, also eine Möglichkeit aufgerufen
                    Anzahl_Der_Möglichkeiten++;//und die Anzahl der Möglichkeiten um 1 erhöht
                }
            }
            else
            {
                //Wenn es zu viele Möglichkeiten gibt, soll nur dem Bit Zu_Viele_Möglichkeiten 1 zugewiesen bekommen
                Zu_Viele_Möglichkeiten = true;                
            }
        }

        public void Erhöhe_den_Index(int N)
        {//Die Funktion soll die nächste Möglichkeit anzeigen und somit auch alle Möglichkeiten der Länge 6 zeigen können
            //Zunächst wird solange nicht alle Stellen des Gebirgszuges betrachtet wurden folgendes gemacht:
            for (int i = N; i >= 0; i--)
            {
                //Es wird geschaut, ob die betrachtete Stelle noch erhöht werden kann, ohne die maximale Höhe zu überschreiten,
                //Wenn nicht wird gleich die nächste Stelle betrachtet
                if (((Gebirgszug.Gebirgszug_Stelle[i].Höhe) < Gebirgszug.Gebirgszug_Stelle[i].Maximale_Höhe))
                {
                    //Jetzt wird noch geschaut, ob nach einer Erhöhung der Stelle der Höhenunterschied zum Vorgänger und zum Nachfolger maximal 1 ist.
                    //Wenn nicht wird die nächste Stelle betrachtet
                    if ((Math.Abs((Gebirgszug.Gebirgszug_Stelle[i].Höhe + 1) - (Gebirgszug.Gebirgszug_Stelle[i - 1].Höhe)) <= 1) && (Math.Abs((Gebirgszug.Gebirgszug_Stelle[i].Höhe + 1) - (Gebirgszug.Gebirgszug_Stelle[i + 1].Höhe)) <= 1))
                    {
                        //Hat es die Stelle bis hierhin geschafft, wird ihre Höhe erhöht
                        Gebirgszug.Gebirgszug_Stelle[i].Höhe++;

                        //Alle Höhen nach ihr dürfen aber nicht so bleiben.
                        //Ich vergleiche diese Vorgehensweise mit dem Zählen egal in welcher Basis, aber bleiben wir am besten bei der Basis 10
                        //Mann kann bis 9 zählen, aber anschließend wird die Ziffer links von der 9 um 1 auf 1 erhöht und alle Ziffern rechts von der 1 werden 0 gesetzt
                        //In diesem Beispiel ist es genau so, nur dass sich die Basis immer wieder ändert, und dass man die Stellen rechts der neuen Zahl nicht 0 setzten darf
                        //Man muss diese Stellen dann immer um gleich der Ziffer links von ihr -1 setzten, ohne sie dabei negativ werden zu lassen
                        for (int j = i + 1; j <= N; j++)
                        {//Alle Stellen rechts der erhöhten Stelle werden betrachtet
                            
                            if ((Gebirgszug.Gebirgszug_Stelle[j - 1].Höhe - 1) >= 0)
                            {//Wenn die Höhe des Vorgängers der betrachteten Stelle - 1 noch größer ist als 0
                                //Dann wird die Stelle gleich der Höhe ihres Vorgängers - 1 gesetzt
                                Gebirgszug.Gebirgszug_Stelle[j].Höhe = (Gebirgszug.Gebirgszug_Stelle[j - 1].Höhe - 1);
                                
                            }
                            else
                            {//Wenn die Höhe des Vorgängers - 1 kleiner wird als 0 dann wird die Stelle einfach 0 gesetzt
                                Gebirgszug.Gebirgszug_Stelle[j].Höhe = 0;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public Gebirgszug_Aufgaben(int N)
        {//Der Gebirgszug muss mit jeder Änderung von N neu instanziert werden
            Gebirgszug = new Gebirgszug(N);
            //Dabei kann aber auch gleich die Anzahl_Der_Möglichkeiten ermittelt werden
            Gib_Die_Anzahl_Der_Möglichkeiten_Wieder(N);
            //Danach müssen die Stellen jedoch wieder 0 gesetzt werden
            Gebirgszug.Setzte_Alle_Stellen_Auf_Null(N);
        }
    }
}