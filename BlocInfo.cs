using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class BlocInfo
    {
        /// <summary>
        /// Le nom de la texture du bloc
        /// </summary>
        public String Texture { get; set; }
        /// <summary>
        /// La durabilité du bloc
        /// </summary>
        public int Durabilite { get; set; }
        /// <summary>
        /// Les bonus
        /// </summary>
        public BonusInfo Bonus { get; set; }

        public enum TextureEnum { Blank, BriqueBleue, BriqueRouge, BriqueGrise, BriqueJaune, BriqueOrange, BriqueVerte }

        public int TextureId
        {
            get{ return (int)(Enum.Parse(typeof(TextureEnum), Texture, true)); }
        }
    }
}
