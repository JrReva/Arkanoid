using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Parametre
    {
        public const int VITESSE_JEU = 6;
        public const float PAD_KEYBORD_DEPLACEMENT = 0.8f;
        public static bool PLAY_WITH_MOUSE = false;
        public const int DIMENSION_BALLE = 15;
        public const int LARGEUR_FENETRE = 750;
        public const int HAUTEUR_FENETRE = 600;
        public const int HAUTEUR_PAD = 15;
        public const int LARGEUR_PAD = 120;
        public const int NB_BLOC_X = 15;
        public const int NB_BLOC_Y = 8;
        public const int LARGEUR_BLOC = (LARGEUR_FENETRE / NB_BLOC_X);
        public const int HAUTEUR_BLOC = ((HAUTEUR_FENETRE / 2) / NB_BLOC_Y);
        public const int BALLE_MIN_ANGLE = 210;
        public const int BALLE_MAX_ANGLE = 330;
        public const int LARGEUR_BONUS = 20;
        public const int HAUTEUR_BONUS = 20;
        public const float DEPLACEMENT_BONUS = 0.5f;
        public const int DIMENSION_IMAGE_VIES = 15;
        public const int DUREE_BONUS = 10000;
        public const float BONUS_PAD_SCALE = 1.3f;
        public const float BACKGROUND_DEPLACEMENT = 0.3f;
        public const int UDP_PORT = 12345;
        public const int TCP_PORT = 12346;
        public const int TIMER_CHANGEMENT_NIVEAU = 2500;
    }
}
