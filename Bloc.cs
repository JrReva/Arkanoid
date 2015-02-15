using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Bloc : ObjetGraphique
    {
        /// <summary>
        /// Contient les informations du bloc
        /// </summary>
        private BlocInfo _blocInfo;
        private int _durabilite = 0;

        /// <summary>
        /// Propriété de _blocInfo
        /// </summary>
        public BlocInfo BlocInfo
        {
            get { return _blocInfo; }
            private set { Durabilite = value.Durabilite; _blocInfo = value; }
        }

        /// <summary>
        /// Contient la durabilité restante du bloc
        /// </summary>
        public int Durabilite
        {
            get{ return _durabilite; }
            set
            {
                if (value == 0)
                    Visible = false;
                _durabilite = value;
            }
        }

        /// <summary>
        /// Constructeur de bloc
        /// </summary>
        /// <param name="x">La position horizontale (en bloc et non en pixel)</param>
        /// <param name="y">La position verticale (en bloc et non en pixel)</param>
        /// <param name="blocInfo">Les infos du bloc</param>
        public Bloc(int x, int y, BlocInfo blocInfo) 
            : base(
            Parametre.LARGEUR_BLOC * x,
            Parametre.HAUTEUR_BLOC * y,
            Parametre.LARGEUR_BLOC,
            Parametre.HAUTEUR_BLOC)
        {
            BlocInfo = blocInfo;
        }
    }
}
