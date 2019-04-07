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
        public Rectangle Viereckgröße; // Die Position und Reichweite des eingeschlossenen Vierecks

        public Button(Texture2D Übergebener_Sprite, Vector2 Übergebene_Position) 
        // Der Konstrucktor mit voreingestellten Eigenschaften
        {
            Sprite = Übergebener_Sprite;
            Position = Übergebene_Position;
            Viereckgröße = new Rectangle((int)Übergebene_Position.X, (int)Übergebene_Position.Y, (int)Übergebener_Sprite.Width, (int)Übergebener_Sprite.Height);//Die Viereckgröße wird anhand des übergebenen Sprites und der Position ermittelt
        }     
    }
}

