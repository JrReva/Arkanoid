using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Arkanoid
{
    public class ObjetGraphiqueDeplacable : ObjetGraphique, ICloneable
    {
        public enum DirectionCollision { HAUT, BAS, GAUCHE, DROITE, NONE }
        public float DeplacementX { get; set; }
        public float DeplacementY { get; set; }

        /// <summary>
        /// Constructeur d'objet graphique déplaçable
        /// </summary>
        /// <param name="x">La position X</param>
        /// <param name="y">La position Y</param>
        /// <param name="largeur">La largeur</param>
        /// <param name="hauteur">La hauteur</param>
        /// <param name="deplacementX">Le déplacement X</param>
        /// <param name="deplacementY">Le déplacement Y</param>
        public ObjetGraphiqueDeplacable(int x, int y, int largeur, int hauteur, int deplacementX, int deplacementY)
            : base(x, y, largeur, hauteur)
        {
            SetDeplacement(deplacementX, deplacementY);
        }

        /// <summary>
        /// Constructeur d'objet graphique déplaçable
        /// </summary>
        /// <param name="x">La position X</param>
        /// <param name="y">La position Y</param>
        /// <param name="largeur">La largeur</param>
        /// <param name="hauteur">La hauteur</param>
        public ObjetGraphiqueDeplacable(int x, int y, int largeur, int hauteur)
            : this(x, y, largeur, hauteur, 0, 0)
        {
        }

        /// <summary>
        /// Récupérer le déplacement X en int
        /// </summary>
        /// <returns>Déplacement X</returns>
        public int GetDeplacementX()
        {
            return (int)DeplacementX;
        }

        /// <summary>
        /// Récupérer le déplacement Y en int
        /// </summary>
        /// <returns>Déplacement Y</returns>
        public int GetDeplacementY()
        {
            return (int)DeplacementY;
        }

        /// <summary>
        /// Ajuster le déplacement
        /// </summary>
        /// <param name="dx">Le déplacement X</param>
        /// <param name="dy">Le déplacement Y</param>
        public void SetDeplacement(int dx, int dy)
        {
            DeplacementX = dx;
            DeplacementY = dy;
        }

        /// <summary>
        /// Déplacer l'objet
        /// </summary>
        public virtual void Deplace()
        {
            DirectionCollision collision = VerifierColisionFenetre();

            //Si l'objet atteint le bas de la fenêtre, il disparait
            //Sinon, elle rebondit selon la direction
            if (collision == DirectionCollision.BAS)
                Visible = false;
            else
                Rebondir(collision);

            X += DeplacementX * Parametre.VITESSE_JEU;
            Y += DeplacementY * Parametre.VITESSE_JEU;
        }

        /// <summary>
        /// Vérifie s'il entre en colision avec le bord de la fenêtre
        /// </summary>
        /// <returns></returns>
        protected DirectionCollision VerifierColisionFenetre()
        {
            if (X <= 0 && DeplacementX < 0)
                return DirectionCollision.GAUCHE;
            else if (X + Largeur >= Parametre.LARGEUR_FENETRE && DeplacementX > 0)
                return DirectionCollision.DROITE;
            if (Y <= 0 && DeplacementY < 0)
                return DirectionCollision.HAUT;
            else if (Y >= Parametre.HAUTEUR_FENETRE && DeplacementY > 0)
                return DirectionCollision.BAS;
            return DirectionCollision.NONE;
        }

        /// <summary>
        /// Faire rebondir l'objet
        /// </summary>
        /// <param name="direction">La direction</param>
        public virtual void Rebondir(DirectionCollision direction)
        {
            switch (direction)
            {
                case DirectionCollision.GAUCHE:
                case DirectionCollision.DROITE:
                    DeplacementX = -DeplacementX;
                    break;

                case DirectionCollision.HAUT:
                case DirectionCollision.BAS:
                    DeplacementY = -DeplacementY;
                    break;
            }
        }

        /// <summary>
        /// Ajuster le déplacement par rapport à un angle en degré
        /// </summary>
        /// <param name="angle">L'angle en degré</param>
        public void SetDeplacementAngle(float angle)
        {
            DeplacementX = (float)Math.Cos(MathHelper.ToRadians(angle));
            DeplacementY = (float)Math.Sin(MathHelper.ToRadians(angle));
        }

        /// <summary>
        /// Retourne l'angle du déplacement en degré
        /// </summary>
        /// <returns>L'angle en degré</returns>
        public float GetDeplacementAngle()
        {
            return MathHelper.ToDegrees((float)Math.Acos(DeplacementX));
        }

        /// <summary>
        /// Déplace l'objet graduellement vers la droite
        /// </summary>
        public void DeplacerGraduellementDroite()
        {
            DeplacementX += 0.005f;
        }

        /// <summary>
        /// Déplace l'objet graduellement vers la gauche
        /// </summary>
        public void DeplacerGraduellementGauche()
        {
            DeplacementX -= 0.005f;
        }

        /// <summary>
        /// Réécriture de la fonction collision pour prendre en compte le fait qu'il se déplace
        /// </summary>
        /// <param name="obj">L'objet avec lequel il entre en colision</param>
        /// <returns></returns>
        public new ObjetGraphiqueDeplacable.DirectionCollision Collision(ObjetGraphique obj)
        {
            // On clone l'objet et on le déplace pour avoir des attribut lorsqu'il sera déplacé
            ObjetGraphiqueDeplacable newPosition = (ObjetGraphiqueDeplacable) this.Clone();
            newPosition.Deplace();

            //S'ils ne rentre pas en colision après le déplacement, on retourne none
            if(!obj.Collision(newPosition))
                return DirectionCollision.NONE;

            //Sinon, on set la nouvelle position X avec le X actuel
            newPosition.X = this.X;

            //Pour vérifier s'il rentre en colision. S'il ne rentre pas en colision, c'est qu'il a été frappé par un
            //Changement de X, sinon c'est par un changement de Y
            if(!obj.Collision(newPosition))
                return DirectionCollision.GAUCHE;
            else
                return DirectionCollision.BAS;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
