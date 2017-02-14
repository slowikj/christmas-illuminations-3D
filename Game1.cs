using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Projekt4.Drawers;

namespace Projekt4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Model _model;
        private List<DrawableObject> _drawableObjects;
        DrawableObject _moveableObject;

        private LightingInfo _lightingInfo;
        private DrawingKit _drawingKit;

        private Drawer _drawer;
        private Effect _flatShader;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            _LoadModels();

            _drawableObjects = _GetDrawableObjects();
            _moveableObject = _drawableObjects[0];

            _lightingInfo = _GetLightingInfo(_drawableObjects);
            _drawingKit = new DrawingKit(this.GraphicsDevice, _lightingInfo);

            _LoadShaders();
            _drawer = new FlatDrawer(_drawingKit, _flatShader);

            // TODO: use this.Content to load your game content here
        }
        
        private void _LoadModels()
        {
            _model = Content.Load<Model>("Models/ball2");
        }

        private void _LoadShaders()
        {
            _flatShader = Content.Load<Effect>("Shaders/FlatShader");
        }

        private List<DrawableObject> _GetDrawableObjects()
        {
            List<DrawableObject> res = new List<DrawableObject>();

            res.Add(new DrawableObject(_model, Vector3.Zero, Color.Cyan,
                                       new ReflectanceFactors(new Vector3((float)0.1, (float)0.1, (float)0.1),
                                       new Vector3((float)1, (float)1, (float)1),
                                       new Vector3((float)1, (float)1, (float)1),
                                       5000),
                                       new RotationInfo(0, 0, 0)));
            

            return res;
        }

        private LightingInfo _GetLightingInfo(IEnumerable<DrawableObject> lights)
        {
            return new LightingInfo(new Vector3[] { new Vector3(0, 0, 10) }, new Vector3[] { new Vector3(1, 0, 0) });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch(key)
                {
                    case Keys.Left: _moveableObject.RotateAntiClockwise(); break;
                    case Keys.Right: _moveableObject.RotateClockwise(); break;
                    case Keys.Up: _moveableObject.MoveForward(); break;
                    case Keys.Down: _moveableObject.MoveBackward(); break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            foreach(DrawableObject drawableObject in _drawableObjects)
            {
                drawableObject.Draw(_drawer);
            }

            base.Draw(gameTime);
        }
    }
}
