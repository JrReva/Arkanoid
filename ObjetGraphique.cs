using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Arkanoid
{
    public class ObjetGraphique : ITexturable, IDessinable
    {
        private float _x;
        private float _y;
        public float Largeur { get; set; }
        public float Hauteur { get; set; }
        public bool Visible { get; set; }
        public Texture2D Texture { get; private set; }
        public Color Couleur { get; protected set; }

        /// <summary>
        /// Constructeur d'objet graphique
        /// </summary>
        /// <param name="x">La position X</param>
        /// <param name="y">La position Y</param>
        /// <param name="largeur">La largeur</param>
        /// <param name="hauteur">La hauteur</param>
        public ObjetGraphique(int x, int y, int largeur, int hauteur)
        {
            X = x;
            Y = y;
            Largeur = largeur;
            Hauteur = hauteur;
            Visible = true;
        }

        /// <summary>
        /// Ajuster la texture
        /// </summary>
        /// <param name="contentManager">Le contentManager du jeu</param>
        /// <param name="nom">Le nom de la texture</param>
        public void SetTexture(ContentManager contentManager, string nom)
        {
            Couleur = Color.White;

            switch(nom)
            {
                case "BriqueBleue":
                    Couleur = Color.Blue;
                    nom = "Bloc";
                    break;
                case "BriqueRouge":
                    Couleur = Color.Red;
                    nom = "Bloc";
                    break;
                case "BriqueGrise":
                    Couleur = Color.White;
                    nom = "BlocGris";
                    break;
                case "BriqueJaune":
                    Couleur = Color.White;
                    nom = "BlocJaune";
                    break;
                case "BriqueOrange":
                    Couleur = Color.Orange;
                    nom = "Bloc";
                    break;
                case "BriqueVerte":
                    Couleur = Color.Green;
                    nom = "Bloc";
                    break;
                case "Bonus":
                    Couleur = Color.White;
                    nom = "Bonus";
                    break;
            }

            Texture = contentManager.Load<Texture2D>(nom);
        }

        /// <summary>
        /// Dessine l'objet
        /// </summary>
        /// <param name="spriteBatch">Le spriteBatch du jeu</param>
        public virtual void Dessine(SpriteBatch spriteBatch)
        {
            if(this.Visible)
                spriteBatch.Draw(Texture, new Vector2(X, Y), Couleur);
        }

        /// <summary>
        /// Récupérer la position X en int
        /// </summary>
        /// <returns>La position X</returns>
        public int GetX()
        {
            return (int)X;
        }

        /// <summary>
        /// Récupérer la position Y en int
        /// </summary>
        /// <returns>La position Y</returns>
        public int GetY()
        {
            return (int)Y;
        }

        public float X
        {
            get { return _x; }
            set { _x = MathHelper.Clamp(value, 0, Parametre.LARGEUR_FENETRE - Largeur); }
        }

        public float Y
        {
            get { return _y; }
            set { _y = MathHelper.Clamp(value, 0, Parametre.HAUTEUR_FENETRE); }
        }

        /// <summary>
        /// Méthode vérifiant la colision avec un autre objetgraphique
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Collision(ObjetGraphique obj)
        {
            if (X + Largeur < obj.X || X > obj.X + obj.Largeur)
                return false;
            if (Y + Hauteur < obj.Y || Y > obj.Y + obj.Hauteur)
                return false;
            return true;
        }
    }
}
