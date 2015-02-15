using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arkanoid
{
    public class Balle : ObjetGraphiqueDeplacable
    {
        public float Vitesse { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="x">La position X</param>
        /// <param name="y">La position Y</param>
        public Balle(int x, int y) : base(
            x, 
            y, 
            Parametre.DIMENSION_BALLE, 
            Parametre.DIMENSION_BALLE)
        {
            Vitesse = 1;
        }

        public Balle(Vector2 position) : this((int)position.X, (int)position.Y)
        {}

        /// <summary>
        /// Déplacer la balle
        /// </summary>
        public override void Deplace()
        {
            DirectionCollision collision = VerifierColisionFenetre();

            //S'il est en colision avec la fenêtre du bas, on fait disparaitre la balle
            if (collision == DirectionCollision.BAS)
                Visible = false;
            else
                Rebondir(collision);

            X += DeplacementX * Vitesse * Parametre.VITESSE_JEU;
            Y += DeplacementY * Vitesse * Parametre.VITESSE_JEU;
        }
    }
}
