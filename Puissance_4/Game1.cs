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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int VX = 7;
        const int VY = 7;
        int position;
        int player = 1;
        InputHelper inputHelper;
        Boolean fini = false;
        byte[,] damier = new byte[VX, VY]{
            {3, 3, 3, 1, 3, 3, 3},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 2, 1, 2, 0}
        };
        ObjetPuissance4 cadre;
        ObjetPuissance4 pionjaune;
        ObjetPuissance4 pionrouge;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
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

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            inputHelper.Update();

            KeyboardState newState = Keyboard.GetState();
            if (inputHelper.IsNewPress(Keys.Left))
            {
                gauche(damier);
            }
            if (inputHelper.IsNewPress(Keys.Right))
            {
                droite(damier);
            }
                base.Update(gameTime);
                

            
            // TODO: Add your update logic here

            
        }

        private void gauche(byte[,] dam)
        {
                position = checkPosition(damier);
                if ((position - 1) >= 0)
                {
                    dam[0, position - 1] = (byte)player;
                    dam[0, position] = 3;
                }
                else
                {
                    dam[0, VY - 1] = (byte)player;
                    dam[0, position] = 3;
                }
            
        }

        private void droite(byte[,] dam)
        {
            position = checkPosition(damier);
            if ((position + 1) <= (VY - 1))
            {
                dam[0, position + 1] = (byte)player;
                dam[0, position] = 3;
            }
            else
            {
                dam[0, 0] = (byte)player;
                dam[0, position] = 3;
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            int offsetX = 120;
            int offsetY = 150;
            Vector2 pos;
            int xpos, ypos;
            for (int x = 0; x < VX; x++)
            {
                for (int y = 0; y < VY; y++)
                {

                    if (damier[x, y] == 1)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionjaune.Texture, pos, Color.White);
                    }
                    else if (damier[x, y] == 2)
                    {
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pionrouge.Texture, pos, Color.White);
                    }
                    if (damier[x, y] < 3 && x!=0)
                    {
                        Console.WriteLine("passe " + x + " " +y);
                        xpos = offsetX + x * 100;
                        ypos = offsetY + y * 100;
                        pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(cadre.Texture, pos, Color.White);
                    }
                }
            }

            

            spriteBatch.End();
            base.Draw(gameTime);

        }

        public int checkPosition(byte[,] damier)
        {
            for (int i = 0; i < VX; i++)
            {
                if (damier[0, i] == player)
                {
                    return i;
                }
            }
            return 0;
        }

        private void place(ref ObjetPuissance4 pion)
        {

        }
    }
}
