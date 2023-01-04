using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Escape_The_Tower
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private Vector2 _positionPerso;
        private AnimatedSprite _perso;
        private TiledMapTileLayer mapLayer;
        private KeyboardState _keyboardState;
        public float _vitessePerso;
        public float _vitessePersoDebut = 100;
        private int _sensPersoX;
        private int _sensPersoY;
        //public const int LONGUEUR_ECRAN = 1400;
        //public const int LARGEUR_ECRAN = 800;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //_graphics.PreferredBackBufferWidth = LONGUEUR_ECRAN;
            //_graphics.PreferredBackBufferHeight = LARGEUR_ECRAN;
            _graphics.ApplyChanges();

            _vitessePerso = _vitessePersoDebut;

            _positionPerso = new Vector2(20,240);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _tiledMap = Content.Load<TiledMap>("mapGenerale");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("persoAnimation.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _keyboardState = Keyboard.GetState();

            // ==============================================================================
            // =============================GESTION RUN======================================
            // ==============================================================================

            if (_keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _vitessePerso = _vitessePersoDebut * 2;
            }
            else if (_keyboardState.IsKeyUp(Keys.LeftShift))
            {
                _vitessePerso = _vitessePersoDebut;
            }

            // ==============================================================================
            // =============================GESTION DEPLACEMENT==============================
            // ==============================================================================

            _sensPersoX = 0;
            _sensPersoY = 0;

            // si fleche droite enfoncé
            if (_keyboardState.IsKeyDown(Keys.Right) && !(_keyboardState.IsKeyDown(Keys.Left)))
            {
                ushort txMillieu = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                ushort tyMillieu = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);

                //ushort txHaut = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                // ushort tyHaut = (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1);

                ushort txBas = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 1);
                ushort tyBas = (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 2);

               // if (!IsCollision(txMillieu, tyMillieu) && !IsCollision(txBas, tyBas))// && !IsCollision(txHaut, tyHaut)
                //    _sensPersoX = 1;
            }


            // si fleche gauche enfoncé
            if (_keyboardState.IsKeyDown(Keys.Left) && !(_keyboardState.IsKeyDown(Keys.Right)))
            {
                ushort txMillieu = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                ushort tyMillieu = (ushort)(_positionPerso.Y / _tiledMap.TileHeight);

                //ushort txHaut = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                //ushort tyHaut = (ushort)(_positionPerso.Y / _tiledMap.TileHeight - 1);

                ushort txBas = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 1);
                ushort tyBas = (ushort)(_positionPerso.Y / _tiledMap.TileHeight + 2);

               // if (!IsCollision(txMillieu, tyMillieu) && !IsCollision(txBas, tyBas))// && !IsCollision(txHaut, tyHaut)
               //     _sensPersoX = -1;
            }

            // si fleche haut enfoncé
            if (_keyboardState.IsKeyDown(Keys.Up) && !(_keyboardState.IsKeyDown(Keys.Down)))
            {
                ushort txGauche = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 0.5);
                ushort tyGauche = (ushort)((_positionPerso.Y + 10) / _tiledMap.TileHeight - 1);

                ushort txDroite = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 0.5);
                ushort tyDroite = (ushort)((_positionPerso.Y + 10) / _tiledMap.TileHeight - 1);
              //  if (!IsCollision(txGauche, tyGauche) && !IsCollision(txDroite, tyDroite))
                  //  _sensPersoY = -1;
            }

            // si fleche bas enfoncé
            if (_keyboardState.IsKeyDown(Keys.Down) && !(_keyboardState.IsKeyDown(Keys.Up)))
            {
                ushort txGauche = (ushort)(_positionPerso.X / _tiledMap.TileWidth - 0.5);
                ushort tyGauche = (ushort)((_positionPerso.Y + 2) / _tiledMap.TileHeight + 2);

                ushort txDroite = (ushort)(_positionPerso.X / _tiledMap.TileWidth + 0.5);
                ushort tyDroite = (ushort)((_positionPerso.Y + 2) / _tiledMap.TileHeight + 2);

               // if (!IsCollision(txGauche, tyGauche) && !IsCollision(txDroite, tyDroite))
                   // _sensPersoY = 1;
            }


            // si deux touches alors divise la vitesse par racine de 2
            if (_sensPersoX != 0 && _sensPersoY != 0 && _keyboardState.IsKeyDown(Keys.LeftShift)) _vitessePerso = (_vitessePersoDebut / (float)Math.Sqrt(2)) * 2;
            else if (_sensPersoX != 0 && _sensPersoY != 0) _vitessePerso = (_vitessePersoDebut / (float)Math.Sqrt(2));
            else if (!_keyboardState.IsKeyDown(Keys.LeftShift)) _vitessePerso = _vitessePersoDebut;

            // deplace le personnage
            _positionPerso.X += _sensPersoX * _vitessePerso * deltaTime;
            _positionPerso.Y += _sensPersoY * _vitessePerso * deltaTime;

            // ==============================================================================
            // =============================GESTION ANIMATION================================
            // ==============================================================================

            // si on ne bouge pas alors on play idle
            if (_sensPersoX == 0 && _sensPersoY == 0) _perso.Play("idle"); // une des animations définies dans « persoAnimation.sf »

            // si on bouge alors on play anim
            else if (_sensPersoX == 1 && _sensPersoY == 1 || _sensPersoX == -1 && _sensPersoY == 1 || _sensPersoX == 0 && _sensPersoY == 1) _perso.Play("walkSouth");
            else if (_sensPersoX == 1 && _sensPersoY == -1 || _sensPersoX == -1 && _sensPersoY == -1 || _sensPersoX == 0 && _sensPersoY == -1) _perso.Play("walkNorth");
            else if (_sensPersoX == -1 && _sensPersoY == 0) _perso.Play("walkWest");
            else if (_sensPersoX == 1 && _sensPersoY == 0) _perso.Play("walkEast");

            _perso.Update(deltaTime); // time écoulé

            _tiledMapRenderer.Update(gameTime);



            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            _tiledMapRenderer.Draw();
            _spriteBatch.Begin();
            _spriteBatch.Draw(_perso, _positionPerso);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
       // private bool IsCollision(ushort x, ushort y)
        //{
           // if (mapLayer.GetTile(x, y).GlobalIdentifier == 129)
                //Console.WriteLine("Porte Maison Bleue");

            //if (mapLayer.GetTile(x, y).GlobalIdentifier == 135)
              //  Console.WriteLine("Porte Maison Rouge");

//            if (mapLayer.GetTile(x, y).GlobalIdentifier == 141)
  //              Console.WriteLine("Porte Maison Verte");

            // définition de tile qui peut être null (?)
          //  TiledMapTile? tile;
           // if (mapLayer.TryGetTile(x, y, out tile) == false)
            //    return false;
            //if (!tile.Value.IsBlank)
             //   return true;
           // return false;
        }
    }
    

