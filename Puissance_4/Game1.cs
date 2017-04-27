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

namespace Puissance_4
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    enum GameState
    {
        menu,
        gameOver,
        inGame
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /* graphic elements */
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state = GameState.menu;
        InputHelper inputHelper;
        Point mousePosition;

        /* Parameters for the game */
        const int nbLigne = 7;
        const int nbColonne = 7;
        int position;
        Player player1 = new Player(1);
        Player player2 = new Player(2);
        Player currentPlayer;
        byte[,] damier = new byte[nbLigne, nbColonne]{
            {3, 3, 3, 1, 3, 3, 3},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0}
        };
        ObjetPuissance4 cadre;
        ObjetPuissance4 pionjaune;
        ObjetPuissance4 pionrouge;
        ObjetPuissance4 background;
        ObjetPuissance4 inGame;
        
        Bouton b_start;

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
            currentPlayer = player1;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            inputHelper = new InputHelper();
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();
            // on charge un objet mur 
            cadre = new ObjetPuissance4(Content.Load<Texture2D>("images\\cadre"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            pionjaune = new ObjetPuissance4(Content.Load<Texture2D>("images\\jaune"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            pionrouge = new ObjetPuissance4(Content.Load<Texture2D>("images\\rouge"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            background = new ObjetPuissance4(Content.Load<Texture2D>("images\\background"), new Vector2(0f, 0f), new Vector2(100f, 100f));
            inGame = new ObjetPuissance4(Content.Load<Texture2D>("images\\inGame"), new Vector2(0f, 0f), new Vector2(100f, 100f));

            b_start = new Bouton(new Texture2D(GraphicsDevice, 1, 1), new Vector2(0f, 0f), new Vector2(50f, 36f));
            b_start.Texte = Content.Load<SpriteFont>("Basic");
            Vector2 label_jouer = b_start.Texte.MeasureString("Jouer");
            b_start.Rectangle = new Rectangle(graphics.PreferredBackBufferWidth / 15, graphics.PreferredBackBufferHeight / 2, (int)label_jouer.X, (int)label_jouer.Y);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            inputHelper.Update();

            switch (state)
            {
                case GameState.menu:
                    MouseState mouseState = Mouse.GetState();
                    mousePosition = new Point(mouseState.X, mouseState.Y);
                    Rectangle area = b_start.Rectangle;
                    area.Contains(mousePosition);

                    if (inputHelper.IsNewPress(Keys.Enter))
                    {
                        state = GameState.inGame;
                    }
                    if (area.Contains(mousePosition))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            state = GameState.inGame;
                        }
                        b_start.FontColor = Color.LightGreen;
                    }
                    else
                    {
                        b_start.FontColor = Color.White;
                    }
                    break;

                case GameState.inGame:
                    if (inputHelper.IsNewPress(Keys.Left))
                    {
                        gauche(damier);
                    }
                    if (inputHelper.IsNewPress(Keys.Right))
                    {
                        droite(damier);
                    }
                    if (inputHelper.IsNewPress(Keys.Enter) || inputHelper.IsNewPress(Keys.Down))
                    {
                        if (placer())
                        {
                            if (win())
                            {
                                state = GameState.gameOver;
                            }
                            if (currentPlayer == player1)
                                currentPlayer = player2;
                            else currentPlayer = player1;

                            damier[0, 3] = (byte)currentPlayer.Nb;
                        }
                    }
                    break;

                case GameState.gameOver:
                    if (inputHelper.IsNewPress(Keys.Enter))
                    {
                        currentPlayer = player1;
                        damier = new byte[nbLigne, nbColonne]{
                        {3, 3, 3, 1, 3, 3, 3},
                        {0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 0, 0}
                    };
                        state = GameState.menu;
                    }
                    break;
                    
            }
            base.Update(gameTime);
            
        }

        private void gauche(byte[,] dam)
        {
                position = checkPosition(damier);
                if ((position - 1) >= 0)
                {
                    dam[0, position - 1] = (byte)currentPlayer.Nb;
                    dam[0, position] = 3;
                }
                else
                {
                    dam[0, nbColonne - 1] = (byte)currentPlayer.Nb;
                    dam[0, position] = 3;
                }            
        }

        private void droite(byte[,] dam)
        {
            position = checkPosition(damier);
            if ((position + 1) <= (nbColonne - 1))
            {
                dam[0, position + 1] = (byte)currentPlayer.Nb;
                dam[0, position] = 3;
            }
            else
            {
                dam[0, 0] = (byte)currentPlayer.Nb;
                dam[0, position] = 3;
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();

            switch (state)
            {
                case GameState.menu:
                    Vector2 pos = new Vector2(0, 0);

                    /* Affichage du fond du menu */
                    spriteBatch.Draw(background.Texture, pos, Color.White);

                    /* Affichage du bouton Jouer */
                    string output = "Jouer";
                    Vector2 FontOrigin = b_start.Texte.MeasureString(output) / 2;
                    // Pour afficher un recatngle autour du bouton jouer
                    //spriteBatch.Draw(b_start.Texture, b_start.Rectangle, Color.Azure);
                    spriteBatch.DrawString(b_start.Texte, output, new Vector2(b_start.Rectangle.X, b_start.Rectangle.Y), b_start.FontColor);
                    break;

                case GameState.inGame:
                    drawGame();
                    break;

                case GameState.gameOver:
                    drawGame();
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);

        }
        public void drawGame()
        {
            int offsetX = 120;
            int offsetY = 150;
            Vector2 pos;
            int xpos, ypos;
            spriteBatch.Draw(inGame.Texture, new Vector2(0, 0), Color.White);

            for (int x = 0; x < nbLigne; x++)
            {
                for (int y = 0; y < nbColonne; y++)
                {
                    // Première couleur
                    if (damier[x, y] == 1)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionjaune.Texture, pos, Color.White);
                    }
                    // Deuxième couleur
                    else if (damier[x, y] == 2)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionrouge.Texture, pos, Color.White);
                    }
                    // != Première ligne (ligne deplacement)
                    if (damier[x, y] < 3 && x != 0)
                    {
                        Console.WriteLine("passe " + x + " " + y);
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(cadre.Texture, pos, Color.White);
                    }
                }
            }

            /* Affiche le score du joueur 1 */
            String output = "Player 1" + " : " + player1.Score;
            Vector2 FontOrigin = b_start.Texte.MeasureString(output) / 2;
            spriteBatch.DrawString(b_start.Texte, output, new Vector2(9 * (graphics.PreferredBackBufferWidth / 15), graphics.PreferredBackBufferHeight / 15), Color.LightGreen);

            /* Affiche le score du joueur 1 */
            output = "Player 2" + " : " + player2.Score;
            FontOrigin = b_start.Texte.MeasureString(output) / 2;
            spriteBatch.DrawString(b_start.Texte, output, new Vector2(9 * (graphics.PreferredBackBufferWidth / 15), 3 * graphics.PreferredBackBufferHeight / 15), Color.LightGreen);
        }


        public int checkPosition(byte[,] damier)
        {
            for (int i = 0; i < nbLigne; i++)
            {
                if (damier[0, i] == currentPlayer.Nb)
                {
                    return i;
                }
            }
            return 0;
        }

        private Boolean placer()
        {
            int new_posX=0;
            int y = checkPosition(damier);
            for (int x = 0; x < nbLigne; x++)
            {
                if (damier[1,y] != 0)
                {
                    return false;
                }
                else
                {
                    if (x + 1 < nbLigne)
                    {
                        if (damier[(x + 1), y] == 0)
                        {
                            new_posX++;
                        }
                        else
                        {
                            damier[new_posX, y] = (byte)currentPlayer.Nb;
                            damier[0, y] = 3;
                            return true;
                        }
                    }
                    else
                    {
                        damier[new_posX, y] = (byte)currentPlayer.Nb;
                        damier[0, y] = 3;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool win()
        {
            bool win = false;
            bool full = true;

            //La grille est pleine
            for (int x = 0; x < nbLigne; x++)
            {
                for (int y = 0; y < nbColonne; y++)
                {
                    if (damier[x, y] == 0)
                    {
                        full = false;
                    }
                }
            }
            //Si le joueur gagne
            win = cherche4();

            if (full || win)
            {
                if (!full)
                {
                    currentPlayer.Score++;
                }
                return true;
            }
            return false;
        }

        public Boolean cherche4()
        {
            for (int ligne = 1; ligne < nbLigne; ligne++)
            {
                // Vérifie les horizontales ( - )
                if (cherche4alignes(0, ligne, 1, 0))
                {
                    return true;
                }
                // Première diagonale ( \ ) inférieur
                if (cherche4alignes(0, ligne, 1, 1))
                {
                    return true;
                }
                // Deuxième diagonale ( / ) inférieur
                if (cherche4alignes(nbColonne - 1, ligne, -1, 1))
                {
                    return true;
                }
            }

            for (int col = 0; col < nbColonne; col++)
            {
                // Vérifie les verticales ( ¦ )
                if (cherche4alignes(col, 1, 0, 1))
                {
                    return true;
                }
                // Première diagonale ( / ) supérieur
                if (cherche4alignes(col, 1, -1, 1))
                {
                    return true;
                }
                // Deuxième diagonale ( \ ) supérieur
                if (cherche4alignes(col, 1, 1, 1))
                {
                    return true;
                }
            }
            // On n'a rien trouvé
            return false;
        }

        private Boolean cherche4alignes(int oCol, int oLigne, int dCol, int dLigne) {
            int couleur = 0;
            int compteur = 0;

            int curCol = oCol;
            int curRow = oLigne;

            while ((curCol >= 0) && (curCol < nbColonne) && (curRow >= 1) && (curRow < nbLigne))
            {
                if (damier[curRow, curCol] != couleur) {
                    // Si la couleur change, on réinitialise le compteur
                    couleur = damier[curRow, curCol];
                    compteur = 1;
                } else {
                    // Sinon on l'incrémente
                    compteur++;
                }

                // On sort lorsque le compteur atteint 4
                if ((couleur != 0) && (compteur == 4)) {
                    return true;
                }

                // On passe à l'itération suivante
                curCol += dCol;
                curRow += dLigne;
            }

            // Aucun alignement n'a été trouvé
            return false;
        }
    }
}
