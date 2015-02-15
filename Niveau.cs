using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid
{
    public class Niveau : IDessinable, IEnumerable
    {
        public delegate void Action();

        public ContentManager ContentManager { get; private set; }
        public Bloc[,] Blocs { get; private set; }

        public String Nom { get; private set; }
        public String Serie { get; private set; }
        public String Auteur { get; private set; }
        public int Difficulte { get; private set; }

        /// <summary>
        /// Retourne la liste en string des blocs
        /// </summary>
        public String BlocList
        {
            get
            {
                String retour = "";

                //Pour chaque bloc, on ajoute l'id de la texture au string
                foreach (Bloc bloc in Blocs)
                    if (bloc != null && bloc.Visible)
                        retour += bloc.BlocInfo.TextureId;
                    else
                        retour += 0;

                return retour;
            }
        }

        public Bloc this[int x, int y]
        {
            get { return Blocs[x, y]; }
        }

        /// <summary>
        /// Constructeur de niveau, récupérant le contentManager
        /// </summary>
        /// <param name="contentManager">Le contentManager du jeu</param>
        public Niveau(ContentManager contentManager)
        {
            //On crée un array à double dimension pour enrégistrer les blocs
            Blocs = new Bloc[Parametre.NB_BLOC_X, Parametre.NB_BLOC_Y];
            ContentManager = contentManager;
        }

        /// <summary>
        /// Méthode pour charger un niveau
        /// </summary>
        /// <param name="fichierXML">Le path vers le fichier XML</param>
        /// <returns>S'il a fonctionné</returns>
        public bool Charger(String fichierXML)
        {
            Blocs = new Bloc[Parametre.NB_BLOC_X, Parametre.NB_BLOC_Y];

            //On vérifie si le fichier existe
            if (fichierXML == default(String) || !File.Exists(fichierXML))
                return false;

            try
            {
                //On crée un document et on load le fichier
                XmlDocument document = new XmlDocument();
                document.Load(fichierXML);

                //On récupère les infos sur le niveau
                XmlElement info = (XmlElement)document.GetElementsByTagName("Information").Item(0);
                Nom = info.GetElementsByTagName("Nom").Item(0).InnerText;
                Serie = info.GetElementsByTagName("Série").Item(0).InnerText;
                Auteur = info.GetElementsByTagName("Auteur").Item(0).InnerText;
                Difficulte = Convert.ToInt32(info.GetElementsByTagName("Difficulté").Item(0).InnerText);

                //On récupère les infos des blocs
                XmlElement blocInfo = (XmlElement)document.GetElementsByTagName("BlocInfo").Item(0);
                Hashtable tableBlocInfo = new Hashtable();

                foreach (XmlElement bloc in blocInfo.GetElementsByTagName("Bloc"))
                {
                    BlocInfo b = new BlocInfo();
                    b.Texture = bloc.GetElementsByTagName("Texture").Item(0).InnerText;

                    String durable = bloc.GetElementsByTagName("Durabilité").Item(0).InnerText;
                    b.Durabilite = (durable == "INF") ? -1 : Convert.ToInt32(durable);

                    tableBlocInfo.Add(bloc.GetAttribute("Type"), b);

                    b.Bonus = new BonusInfo((XmlElement)bloc.GetElementsByTagName("Bonus").Item(0));
                }

                //On récupère tous les blocs
                XmlElement descriptionNiveau = (XmlElement)document.GetElementsByTagName("DescriptionNiveau").Item(0);

                foreach (XmlElement bloc in descriptionNiveau.GetElementsByTagName("Bloc"))
                {
                    int x = Convert.ToInt32(bloc.GetAttribute("PositionX"));
                    int y = Convert.ToInt32(bloc.GetAttribute("PositionY"));

                    Bloc b = new Bloc(x, y, (BlocInfo)tableBlocInfo[bloc.GetElementsByTagName("Type").Item(0).InnerText]);
                    b.SetTexture(ContentManager, b.BlocInfo.Texture);
                    Blocs[x, y] = b;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Dessine tout le niveau
        /// </summary>
        /// <param name="spriteBatch">Le spriteBatch du jeu</param>
        public void Dessine(SpriteBatch spriteBatch)
        {
            //Pour chaque bloc, s'il n'est pas nul, il faut le dessiner
            foreach (Bloc bloc in Blocs)
                if(bloc != null)
                    bloc.Dessine(spriteBatch);
        }

        public IEnumerator GetEnumerator()
        {
            return Blocs.GetEnumerator();
        }
    }
}