using System;
using System.Collections.Generic;
using System.Text;

namespace Gebirgszug_Simulation
{
    public class Gebirgszug
    {
        public Gebirgszug_Stelle[] Gebirgszug_Stelle; //Der Gebirgszug hat eine Anzahl von Stellen die mit diesem Feld organisiert werden

        //Die Funktion zum Löschen der Stellenhöhen
        public void Setzte_Alle_Stellen_Auf_Null(int N)
        {
            for (int i = 0; i <= N; i++)
            {
                //Solange i nicht alle Stellen N betrachtet hat werden diese 0 gesetzt
                Gebirgszug_Stelle[i].Höhe = 0;
            }
        }

        public Gebirgszug(int N)
        {
            //Wird ein Gebirgszug erstellt, erhält er gleich die Menge der Stellen
            Gebirgszug_Stelle = new Gebirgszug_Stelle[N + 1];//N+1 liegt der Aufgabe zugrunde, denn es heißt es soll ein Gebirgszug der Länge N+1 generiert werden, daher auch N+1 Stellen

            for (int i = 0; i <= N; i++)
            {
                Gebirgszug_Stelle[i] = new Gebirgszug_Stelle(); // Der Index i entspricht gleichzeitig der Position der Stelle
                //Jede Stelle wird instanziert
            }

                //Es sollen vom Anfang und vom Ende des Gebirgszuges zur Mitte hin die maximale Höhe aller Stellen von 0 an immer um 1 ansteigen
                for (int i = 0; i <= Math.Round(N / 2d); i++)
                {
                    Gebirgszug_Stelle[i].Maximale_Höhe = i;
                    Gebirgszug_Stelle[N - i].Maximale_Höhe = i;
                }
        }
    }
}
