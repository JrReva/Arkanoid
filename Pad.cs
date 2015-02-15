using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Arkanoid
{
    public class Pad : ObjetGraphiqueDeplacable
    {
        public Texture2D TextureLeft { get; private set; }
        public Texture2D TextureMiddle { get; private set; }
        public Texture2D TextureRight { get; private set; }

        public Pad()
            : base(
            // On centralise le Pad en enlevant la moitié du pad à la moitié de la fenêtre
            // et on enlève la hauteur du pad + 10 pour le surélever
            (int)(Parametre.LARGEUR_FENETRE / 2 - Parametre.LARGEUR_PAD / 2),
            (int)(Parametre.HAUTEUR_FENETRE - (Parametre.HAUTEUR_PAD + 10)),
            Parametre.LARGEUR_PAD,
            Parametre.HAUTEUR_PAD)
        {
        }

        /// <summary>
        /// Récupérer la position de la balle si elle est sur le pad
        /// </summary>
        /// <returns>Vecteur 2 représentant l'emplacement</returns>
        public Vector2 GetBallePosition()
        {
            return new Vector2(
                this.X + this.Largeur / 2 - Parametre.DIMENSION_BALLE / 2, 
                this.Y - Parametre.DIMENSION_BALLE);
        }

        /// <summary>
        /// Calculer l'angle qui sera donné à la balle lorsqu'ils entrent en collision
        /// </summary>
        /// <param name="balle">La balle</param>
        /// <returns>L'angle en degré</returns>
        public int calculateNewAngle(Balle balle)
        {
            // On récupère la position relative en % par rapport au milieu (de -1 à 1)
            // Milieu de la balle - le milieu du pad, divisé par la largeur du milieu du pad
            float positionRelative = (2 * (balle.X - X) + (balle.Largeur - Largeur)) / Largeur;

            // On ajoute 270 degrés, on lui ajoute 90xla position relative
            // Puis on récupère le milieu entre cet angle et celui déjà appliqué à la balle
            return (int)(315 + 45 * positionRelative - balle.GetDeplacementAngle() / 2);
        }

        /// <summary>
        /// Faire rebondir
        /// </summary>
        /// <param name="direction">La direction</param>
        public override void Rebondir(DirectionCollision direction)
        {
            //Override de la méthode rebondir pour empêcher le pad de rebondir
            return;
        }

        /// <summary>
        /// Dessine l'objet
        /// </summary>
        /// <param name="spriteBatch">Le spriteBatch du jeu</param>
        public override void Dessine(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                spriteBatch.Draw(TextureLeft, new Vector2(X, Y), Color.White);
                spriteBatch.Draw(TextureMiddle, new Vector2(X + 8, Y), new Rectangle(0, 0, (int)Largeur - 16, 15), Color.White);
                spriteBatch.Draw(TextureRight, new Vector2(X + Largeur - 8, Y), Color.White);
            }
        }

        /// <summary>
        /// Set les textures du pad
        /// </summary>
        /// <param name="contentManager">Le contentManager</param>
        public void SetTexture(ContentManager contentManager)
        {
            TextureLeft = contentManager.Load<Texture2D>("padleft");
            TextureMiddle = contentManager.Load<Texture2D>("padmiddle");
            TextureRight = contentManager.Load<Texture2D>("padright");
        }
    }
}
