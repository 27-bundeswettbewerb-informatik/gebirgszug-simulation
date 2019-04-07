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

namespace Gebirgszug_Simulation
{
    class Button
    {
        // Alle Buttons haben die folgenden Eigenschaften:
        public Texture2D Sprite; // Das auf dem Bildschirm zu malende Bild
        public Vector2 Position; // Die Position des Buttons auf dem Bildschirm
        public Rectangle Viereckgr��e; // Die Position und Reichweite des eingeschlossenen Vierecks

        public Button(Texture2D �bergebener_Sprite, Vector2 �bergebene_Position) 
        // Der Konstrucktor mit voreingestellten Eigenschaften
        {
            Sprite = �bergebener_Sprite;
            Position = �bergebene_Position;
            Viereckgr��e = new Rectangle((int)�bergebene_Position.X, (int)�bergebene_Position.Y, (int)�bergebener_Sprite.Width, (int)�bergebener_Sprite.Height);//Die Viereckgr��e wird anhand des �bergebenen Sprites und der Position ermittelt
        }     
    }
}

