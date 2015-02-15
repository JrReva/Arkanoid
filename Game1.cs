using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

namespace Arkanoid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        //private Socket socketUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //private Socket socketTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //private EndPoint endpointUDP = new IPEndPoint(IPAddress.Broadcast, Parametre.UDP_PORT);
        public Balle Balle { get; private set; }
        public Pad Pad { get; private set; }
        public bool GameStarted { get; private set; }
        public List<Bonus> BonusList { get; private set; }
        public List<Bonus> InExecutionBonus { get; private set; }
        public Joueur Joueur { get; private set; }
        public List<String> ListeNiveau { get; private set; }
        private Texture2D stars;
        private Texture2D background;
        private SpriteFont font;
        private SpriteFont titleFont;
        private float _backgroundPosition = 0f;
        private bool opened = true;
        private int changementNiveauTimer = Parametre.TIMER_CHANGEMENT_NIVEAU;
        private String nom;
        private String prenom;

        public int BackgroundPosition
        {
            get
            {
                if (_backgroundPosition <= -2250)
                {
                    _backgroundPosition = 0;
                }
                return (int)(_backgroundPosition -= Parametre.BACKGROUND_DEPLACEMENT); }
            set { _backgroundPosition = 0; }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Niveau niveau;

        public Game1(String prenom, String nom, bool souris)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.prenom = prenom;
            this.nom = nom;
            Parametre.PLAY_WITH_MOUSE = souris;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            niveau = new Niveau(Content);
            BonusList = new List<Bonus>();
            InExecutionBonus = new List<Bonus>();
            Joueur = new Joueur(prenom, nom);
            //Joueur.ActionChangementPointage = EnvoyerUDP;
            //socketUDP.EnableBroadcast = true;
            //socketTCP.Bind(new IPEndPoint(IPAddress.Any, Parametre.TCP_PORT));

            graphics.PreferredBackBufferHeight = Parametre.HAUTEUR_FENETRE;
            graphics.PreferredBackBufferWidth = Parametre.LARGEUR_FENETRE;
            graphics.ApplyChanges();

            //new Thread(RecevoirTCP).Start();

            base.Initialize();
        }
        /*
        /// <summary>
        /// Envoit en broadcast le nom et le prénom du joueur qui joue.
        /// Est appelé lorsqu'un bloc est détruit, lorsque la balle part du pad et lorsque la partie commence
        /// </summary>
        public void EnvoyerUDP()
        {
            String send = Joueur.Prenom + " " + Joueur.Nom;
            socketUDP.SendTo(Encoding.ASCII.GetBytes(send), endpointUDP);
        }

        /// <summary>
        /// Envoit en broadcast kill, qui veut dire que la partie est terminée
        /// </summary>
        public void EnvoyerUDPEnd()
        {
            String send = "kill";
            socketUDP.SendTo(Encoding.ASCII.GetBytes(send), endpointUDP);
        }

        /// <summary>
        /// Recoit le TCP, accepte la connexion et envoit les données du niveau et le pointage
        /// </summary>
        public void RecevoirTCP()
        {
            socketTCP.Listen(3);

            while (opened)
            {
                try
                {
                    Socket client = socketTCP.Accept();
                    client.Send(Encoding.ASCII.GetBytes(niveau.BlocList + Joueur.Pointage));
                }
                catch { }
            }
        }
        */
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ListeNiveau = new List<String>();

            //On va chercher la liste des niveaux disponibles
            String[] fichiers = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.TopDirectoryOnly);

            if (fichiers.Length == 0)
                Exit();

            foreach (String fichier in fichiers)
                ListeNiveau.Ajouter(fichier);

            niveau.Charger(ListeNiveau.Pop());

            //On envoit un UDP avec les informations du niveau chargé
            //EnvoyerUDP();

            //On crée le Pad et la balle
            Pad = new Pad();
            Balle = new Balle(Pad.GetBallePosition());

            //On ajuste les textures
            Balle.SetTexture(Content, "Balle");
            Pad.SetTexture(Content);

            background = Content.Load<Texture2D>("background");
            stars = Content.Load<Texture2D>("Star");
            font = Content.Load<SpriteFont>("Font");
            titleFont = Content.Load<SpriteFont>("TitleFont");
            titleFont.LineSpacing = 130;
        }

        public void ChargerProchainNiveau()
        {
            if (!niveau.Charger(ListeNiveau.Pop()))
            {
                System.Windows.Forms.MessageBox.Show("Vous avez gagné!");
                this.Exit();
            }

            Balle.Vitesse = 1;
            GameStarted = false;
            Balle.Y = Pad.GetBallePosition().Y;
            changementNiveauTimer = Parametre.TIMER_CHANGEMENT_NIVEAU;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //EnvoyerUDPEnd();
            opened = false;
            //socketTCP.Close();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Bonus bonus in InExecutionBonus)
                bonus.Time -= gameTime.ElapsedGameTime.Milliseconds;

            BonusCancelElapsed();

            //On appèle la méthode Deplace
            Deplace();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            Vector2 niveauTitleSize = font.MeasureString("("+niveau.Nom.Trim().ToUpper()+")");

            if (changementNiveauTimer > 0)
            {
                GraphicsDevice.Clear(Color.Black);

                Vector2 titleSize = titleFont.MeasureString("super\nARKANOID BROS.");
                
                spriteBatch.DrawString(titleFont, "super\nARKANOID BROS.",  new Vector2((Parametre.LARGEUR_FENETRE - titleSize.X) / 2, 0), Color.White);
                spriteBatch.DrawString(font, "(" + niveau.Nom.Trim().ToUpper() + ")", new Vector2((Parametre.LARGEUR_FENETRE - niveauTitleSize.X) / 2, 300), Color.White);

                changementNiveauTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                spriteBatch.Draw(background, new Vector2(BackgroundPosition, 0), null, Color.White, 0f, new Vector2(0, 0), 3f, SpriteEffects.None, 1);
                spriteBatch.Draw(background, new Vector2(BackgroundPosition + 2250, 0), null, Color.White, 0f, new Vector2(0, 0), 3f, SpriteEffects.None, 1);

                niveau.Dessine(spriteBatch);
                Balle.Dessine(spriteBatch);
                Pad.Dessine(spriteBatch);

                //Pour chaque bonus qui n'est pas vide, on les dessine
                foreach (Bonus bonus in BonusList)
                    bonus.Dessine(spriteBatch);

                spriteBatch.DrawString(font, "(" + niveau.Nom.Trim().ToUpper() + ")", new Vector2(Parametre.LARGEUR_FENETRE - (niveauTitleSize.X + 10), 30), Color.White);
            }

            DessinerVies();
            spriteBatch.DrawString(font, "SCORE: " + Joueur.Pointage.ToString().PadLeft(9, '0'), new Vector2(10, 10), Color.White);

            Vector2 nomSize = font.MeasureString(Joueur.Prenom.ToUpper() + " " + Joueur.Nom.ToUpper());
            spriteBatch.DrawString(font, Joueur.Prenom.ToUpper() + " " + Joueur.Nom.ToUpper(), new Vector2(Parametre.LARGEUR_FENETRE - nomSize.X - 10, Parametre.HAUTEUR_FENETRE - nomSize.Y - 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DessinerVies()
        {
            for(int i = 0; i < Joueur.Vies; i++)
                spriteBatch.Draw(stars, new Vector2(Parametre.LARGEUR_FENETRE - (Parametre.DIMENSION_IMAGE_VIES * i) - 25, 10), Color.White);
        }

        public void Deplace()
        {
            if (changementNiveauTimer > 0)
                return;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // On récupère un compteur de nombre de bloc restant
            int compteurBloc = 0;

            // Si on joue avec la souris
            if (Parametre.PLAY_WITH_MOUSE)
            {
                // On set le X du pad avec celui de la souris
                Pad.X = Mouse.GetState().X - Pad.Largeur;

                // Si le bouton gauche est cliqué, on lance la balle si elle ne l'est pas déjà
                if (!GameStarted && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    ThrowBall();
            }
            else
            {
                // On set son déplacement vers la gauche ou la droite, selon la touche entrée
                Pad.DeplacementX = Keyboard.GetState().IsKeyDown(Keys.Left) ? 
                    -(Parametre.PAD_KEYBORD_DEPLACEMENT) : 
                    Keyboard.GetState().IsKeyDown(Keys.Right) ? 
                        Parametre.PAD_KEYBORD_DEPLACEMENT : 
                        0;

                // Si space est pesé, on lance la balle si elle ne l'est pas déjà
                if (!GameStarted && Keyboard.GetState().IsKeyDown(Keys.Space))
                    ThrowBall();
            }

            // Si la balle n'est pas lancée, on set son X sur la balle
            if (!GameStarted)
                Balle.X = Pad.GetBallePosition().X;
            else
            {
                //Sinon, on calcule les collisions avec chaque blocs
                foreach (Bloc bloc in niveau)
                {
                    if (bloc != null && bloc.Visible) //Si le bloc existe et qu'il est visible
                    {
                        //On ajoute 1 au compteur de bloc restants
                        if(bloc.Durabilite > 0)
                            compteurBloc++;

                        //On vérifie s'il y a collision
                        ObjetGraphiqueDeplacable.DirectionCollision direction = Balle.Collision(bloc);

                        //S'il y a une colision
                        if (direction != ObjetGraphiqueDeplacable.DirectionCollision.NONE)
                        {
                            //On fait rebondir la balle, on diminue la durabilité du bloc
                            Balle.Rebondir(direction);
                            bloc.Durabilite--;
                            Joueur.Pointage += 50;

                            //Si le bloc est toujours visible
                            if (!bloc.Visible)
                            {
                                //On tire un bonus
                                BonusInfo.BonusName bonus = bloc.BlocInfo.Bonus.Draw();

                                //S'il y a un bonus de tiré
                                if (bonus != BonusInfo.BonusName.none)
                                {
                                    //On crée un bonus, on lui met sa texture et on l'ajoute à la liste
                                    Bonus bonusObject = new Bonus(bloc.GetX(), bloc.GetY(), bonus);
                                    bonusObject.SetTexture(Content, "Bonus");
                                    BonusList.Ajouter(bonusObject);
                                }
                            }
                        }
                    }
                }

                //Maintenant, on vérifie la colision avec le pad
                ObjetGraphiqueDeplacable.DirectionCollision DirectionPad = Balle.Collision(Pad);

                //S'il y a une colision
                if (DirectionPad != ObjetGraphiqueDeplacable.DirectionCollision.NONE)
                {
                    //Augmentation graduelle de la vitesse de la balle
                    Balle.Vitesse *= 1.01f;

                    if (DirectionPad == ObjetGraphiqueDeplacable.DirectionCollision.HAUT || DirectionPad == ObjetGraphiqueDeplacable.DirectionCollision.BAS)
                    //Et qu'elle touche le haut ou le bas, on set son nouvel angle
                    {
                        Balle.SetDeplacementAngle(Pad.calculateNewAngle(Balle));
                        Joueur.Pointage += 10;
                    }
                    else
                        //Sinon on la fait rebondir
                        Balle.Rebondir(DirectionPad);
                }
                
                //Puis on déplace la balle
                Balle.Deplace();
            }

            //Pour chaque bonus, s'ils ne sont pas à null, on les fait déplacer
            foreach (Bonus bonus in BonusList)
                if (bonus.Visible)
                {
                    if (bonus.Collision(Pad) != ObjetGraphiqueDeplacable.DirectionCollision.NONE)
                    {
                        bonus.SetAction(this);

                        if (bonus.Timed)
                            InExecutionBonus.Ajouter(bonus);

                        if (bonus.Executer != null)
                            bonus.Executer();

                        bonus.Visible = false;
                        BonusList.Enlever(bonus);

                        Joueur.Pointage += 100;
                    }
                    else
                    {
                        if(Pad.X + (Pad.Largeur / 2) > bonus.X + (bonus.Largeur / 2) )
                            bonus.DeplacerGraduellementDroite();
                        else
                            bonus.DeplacerGraduellementGauche();

                        bonus.Deplace();
                    }
                }
                else
                    BonusList.Enlever(bonus);

            //On déplace le pad
            Pad.Deplace();

            //Si la balle n'est plus visible, c'est qu'elle a touché au bas de la fenêtre, donc on a perdu
            if (Balle.Visible == false)
            {
                GameStarted = false;

                //On enlève une vie, et on vérifit s'il en reste
                if (!Joueur.DecrementerVie())
                {
                    //EnvoyerUDPEnd();
                    System.Windows.Forms.MessageBox.Show("Vous avez perdu.");
                    this.Exit();
                }

                //On repositionne la balle, on annule les bonus et on recommence!
                BonusCancel();
                Balle.Visible = true;
                Balle.Vitesse = 1;
                GameStarted = false;
                Balle.Y = Pad.GetBallePosition().Y;
            }

            //Si le compteur de bloc est à 0, c'est qu'il n'en reste plus et qu'on a gagné
            if (GameStarted && compteurBloc == 0)
                ChargerProchainNiveau();
        }

        /// <summary>
        /// Méthode lançant la balle
        /// </summary>
        public void ThrowBall()
        {
            //On tire au hazard l'angle de la direction de départ de la balle
            Balle.SetDeplacementAngle(new Random().Next(Parametre.BALLE_MIN_ANGLE, Parametre.BALLE_MAX_ANGLE + 1));
            GameStarted = true;

            //Puis on envoit une requête UDP pour leur dire qu'on joue
            //EnvoyerUDP();
        }
    }
}
