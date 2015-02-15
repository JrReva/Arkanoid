using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid {
    public class Joueur {
        public String Prenom { get; private set; }
        public String Nom { get; private set; }
        public int Vies { get; private set; }
        private int _pointage = 0;

        public int Pointage {
            get { return _pointage; }
            set {
                _pointage = value;
                if(ActionChangementPointage != null)
                    ActionChangementPointage();
            }
        }

        public Action ActionChangementPointage { get; set; }

        public Joueur(string prenom, string nom, int vies) {
            Prenom = prenom;
            Nom = nom;
            Vies = vies;
        }

        public Joueur(string prenom, string nom)
            : this(prenom, nom, 3) {
        }

        public bool DecrementerVie() {
            if(Vies > 0)
                Vies--;

            return (Vies != 0);
        }

        public void AjouterVies(int nb) {
            Vies += nb;
        }
    }
}
