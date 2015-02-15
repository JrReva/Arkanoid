using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Bonus : ObjetGraphiqueDeplacable
    {
        public delegate void ActionExecuter();

        private BonusInfo.BonusName _bonusName;

        /// <summary>
        /// Les actions à exécuter lors de l'activation
        /// </summary>
        public ActionExecuter Executer { get; private set; }

        /// <summary>
        /// Les actions à exécuter lors de la fin du bonus
        /// </summary>
        public ActionExecuter Cancel { get; private set; }

        /// <summary>
        /// Booléen si le bonus a du temps
        /// </summary>
        public bool Timed { get; private set; }

        /// <summary>
        /// Le temps restant avant que le bonus soit annulé
        /// </summary>
        public int Time { get; set; }


        /// <summary>
        /// Obtenir le nom du bonus
        /// </summary>
        public BonusInfo.BonusName BonusName
        {
            get {return _bonusName;}
            private set { _bonusName = value; }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="x">La position X</param>
        /// <param name="y">La position Y</param>
        /// <param name="bonusName">Le nom du bonus</param>
        public Bonus(int x, int y, BonusInfo.BonusName bonusName) : base(x, y, Parametre.LARGEUR_BONUS, Parametre.HAUTEUR_BONUS)
        {
            DeplacementY = Parametre.DEPLACEMENT_BONUS;
            BonusName = bonusName;
            Time = Parametre.DUREE_BONUS;
            Timed = false;
        }

        /// <summary>
        /// Set les actions à exécuter lors de l'activation et lors de la fin
        /// </summary>
        /// <param name="game1">Le game1</param>
        public void SetAction(Game1 game1)
        {
            Executer = null;
            switch (BonusName)
            {
                case BonusInfo.BonusName.vieplus:
                    Executer = game1.ViePlus;
                    break;
                case BonusInfo.BonusName.padplus:
                    Executer = game1.PadPlus;
                    break;
                case BonusInfo.BonusName.balle3:
                    break;
                case BonusInfo.BonusName.ballelente:
                    Executer = game1.BalleLente;
                    Cancel = game1.BalleRapide;
                    Timed = true;
                    break;
                case BonusInfo.BonusName.ballecolle:
                    break;
                case BonusInfo.BonusName.balleexplosive:
                    break;
                case BonusInfo.BonusName.balledefeu:
                    break;
                case BonusInfo.BonusName.plancher:
                    break;
                case BonusInfo.BonusName.viemoins:
                    Executer = game1.VieMoins;
                    break;
                case BonusInfo.BonusName.padmoins:
                    Executer = game1.PadMoins;
                    break;
                case BonusInfo.BonusName.padfreeze:
                    break;
                case BonusInfo.BonusName.ballerapide:
                    Executer = game1.BalleRapide;
                    Cancel = game1.BalleLente;
                    Timed = true;
                    break;
                case BonusInfo.BonusName.ballefolle:
                    break;
                case BonusInfo.BonusName.ballefantome:
                    break;
                case BonusInfo.BonusName.nuit:
                    break;
                case BonusInfo.BonusName.bonuscancel:
                    Executer = game1.BonusCancel;
                    break;
                case BonusInfo.BonusName.none:
                    break;
                default:
                    break;
            }
        }
    }
}
