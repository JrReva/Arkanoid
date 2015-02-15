using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace Arkanoid
{
    public class BonusInfo
    {
        public enum BonusName
        {
            vieplus,
            padplus,
            balle3,
            ballelente,
            ballecolle,
            balleexplosive,
            balledefeu,
            plancher,
            viemoins,
            padmoins,
            padfreeze,
            ballerapide,
            ballefolle,
            ballefantome,
            nuit,
            bonuscancel,
            none
        }

        private int[] nombre = new int[16];

        public BonusInfo(XmlElement bonusElement)
        {
            if (bonusElement == null)
                return;

            foreach (XmlAttribute attribute in bonusElement.Attributes)
                //On convertit le name en enum pour aller chercher son index
                //Puis on entre le Value dans le tableau à l'index trouvé
                nombre[(int)Enum.Parse(typeof(BonusName), attribute.Name, true)] = Convert.ToInt32(attribute.Value);
        }

        /// <summary>
        /// Pige un bonus
        /// </summary>
        /// <returns>Le nom du bonus pigé</returns>
        public BonusName Draw()
        {
            //On pige au hasard entre 1 et 100
            int result = new Random().Next(1, 101);

            //Pour chaque index du tableau
            for (int i = 0; i < nombre.Length; i++)
            {
                //On enlève au nombre pigé la valeur du bonus
                result -= nombre[i];

                //Si le nombre pigé est maintenant plus petit que 0, on retourne le bonus actuel
                if (result <= 0)
                    return (BonusName)i;
            }

            //Sinon, on retourne rien
            return BonusName.none;
        }
    }
}
