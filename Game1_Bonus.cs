using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public void ViePlus()
        {
            Joueur.AjouterVies(1);
        }

        public void PadPlus()
        {
            Pad.Largeur *= Parametre.BONUS_PAD_SCALE;
        }

        public void BalleLente()
        {
            Balle.Vitesse *= 0.8f;
        }

        public void VieMoins()
        {
            if(Joueur.Vies > 1)
                Joueur.DecrementerVie();
        }

        public void PadMoins()
        {
            Pad.Largeur /= Parametre.BONUS_PAD_SCALE;
        }

        public void BalleRapide()
        {
            Balle.Vitesse *= 1.2f;
        }

        public void BonusCancel()
        {
            foreach (Bonus bonus in InExecutionBonus)
                bonus.Cancel();

            InExecutionBonus = new List<Bonus>();
        }

        public void BonusCancelElapsed()
        {
            foreach (Bonus bonus in InExecutionBonus)
                if (bonus.Time <= 0)
                {
                    if(bonus.Cancel != null)
                        bonus.Cancel();

                    InExecutionBonus.Enlever(bonus);
                }
        }
    }
}
