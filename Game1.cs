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
using Projekt4.DrawableObjects;
using Projekt4.Cameras;

namespace Projekt4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Model[] _model;
        private Model _treeModel;
        private List<IDrawable> _drawableObjects;
        DrawableObject _moveableObject, _tree;

        private LightingInfo _lightingInfo;
        private DrawingKit _drawingKit;
       
        private CameraManager _cameraManager;
        private DrawerManager _drawerManager;

        private Illumination _illumination, _illumination2;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
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
            _moveableObject = _drawableObjects[0] as DrawableObject;

            _PrepareCameraManager();
            
            PatternGenerator patternGenerator = new PatternGenerator(x => x, x => (float)(Math.Sin(x)), 0, 10);
            _illumination = new Illumination(_model[2], patternGenerator.GetPoints(0, 5, 100),
                Color.Magenta, new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 20));

            _illumination2 = new Illumination(_model[3], patternGenerator.GetPoints(-5, -3, 100),
                Color.Blue, new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 20));

            _lightingInfo = new LightingInfo(new Vector3[] { Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ },
                new Vector3[] { Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ });

            //_lightingInfo = _GetLightingInfo();
            _drawingKit = new DrawingKit(
                _cameraManager.ViewMatrix,
                DrawingKit.GetDefaultProjectionMatrix(this.GraphicsDevice),
                _cameraManager.ViewerPosition,
                this.GraphicsDevice,
                _lightingInfo);

            _LoadDrawers();

            _tree = new DrawableObject(new Model[] { _treeModel }, new Vector3(-2, 0, 0 ), Color.Green,
                new ReflectanceFactors(Vector3.Zero, new Vector3((float)0.7, 0, 0), new Vector3((float)0.4, 0, 0), 1000));

            // TODO: use this.Content to load your game content here
        }
        
        private void _LoadModels()
        {
            _model = new Model[]
            {
                Content.Load<Model>("Models/cats/kitty0"), Content.Load<Model>("Models/cats/kitty1"),
                Content.Load<Model>("Models/tinyBall"),
                Content.Load<Model>("Models/smallCone")
            };

            _treeModel = Content.Load<Model>("Models/lowpolytree");
        }

        private void _LoadDrawers()
        {
            _drawerManager = new DrawerManager();

            _drawerManager.AddDrawer("Phong",
                new DefaultDrawer(_drawingKit, this.Content.Load<Effect>("Shaders/PhongShader")));

            _drawerManager.AddDrawer("Goraud",
                new DefaultDrawer(_drawingKit, this.Content.Load<Effect>("Shaders/GoraudShader")));

            _drawerManager.AddDrawer("OldFlat",
                new FlatDrawer(_drawingKit, this.Content.Load<Effect>("Shaders/OldFlatShader")));

            _drawerManager.AddDrawer("NewFlat",
                new DefaultDrawer(_drawingKit, this.Content.Load<Effect>("Shaders/NewFlatShader")));

            _drawerManager.SetDrawer("Phong");
        }

        private List<IDrawable> _GetDrawableObjects()
        {
            List<IDrawable> res = new List<IDrawable>();

            res.Add(new DrawableObject(_model.Take(2).ToArray(), Vector3.Zero, Color.Cyan,
                                       new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                                       new Vector3((float)1, (float)1, (float)1),
                                       new Vector3((float)0.1, (float)0.1, (float)0.1),
                                       500),
                                       new RotationInfo(0, 0, 0)));
            

            return res;
        }

        private void _PrepareCameraManager()
        {
            _cameraManager = new CameraManager();

            _cameraManager.AddCamera("Constant",
                new ConstantCamera(new Vector3(0,0,1), Vector3.Zero));

            _cameraManager.AddCamera("StationaryObjectCamera",
                new StationaryObjectCamera(new Vector3(0, 0, 1), _moveableObject));

            _cameraManager.AddCamera("MoveableObjectCamera",
                new MoveableObjectCamera(_moveableObject));

            _cameraManager.SetCamera("Constant");
        }

        private LightingInfo _GetLightingInfo()
        {
            return new LightingInfo(new Vector3[] { new Vector3(0, 0, 5) }, new Vector3[] { new Vector3(1, 1, 1) });
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
            
            if(keyboardState.IsKeyDown(Keys.LeftControl))
            {
                _SetCamera(keyboardState);
            }
            else if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _SetDrawer(keyboardState);
            }
            else if (keyboardState.IsKeyDown(Keys.LeftAlt))
            {
                _SetIlluminationModel(keyboardState);
            }
            else
            {
                _MoveObject(keyboardState);
            }

            base.Update(gameTime);
        }

        private void _SetCamera(KeyboardState keyboardState)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.C: _cameraManager.SetCamera("Constant"); break;
                    case Keys.M: _cameraManager.SetCamera("MoveableObjectCamera"); break;
                    case Keys.S: _cameraManager.SetCamera("StationaryObjectCamera"); break;
                }
            }
        }

        private void _SetDrawer(KeyboardState keyboardState)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.P: _drawerManager.SetDrawer("Phong"); break;
                    case Keys.G: _drawerManager.SetDrawer("Goraud"); break;
                    case Keys.F: _drawerManager.SetDrawer("NewFlat"); break;
                    case Keys.O: _drawerManager.SetDrawer("OldFlat"); break;
                }
            }
        }

        private void _SetIlluminationModel (KeyboardState keyboardState)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.P: _drawerManager.SetPhongIllumination(); break;
                    case Keys.B: _drawerManager.SetBlinnIllumination(); break;
                }
            }
        }
        private void _MoveObject(KeyboardState keyboardState)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.Left: _moveableObject.RotateAntiClockwise(); break;
                    case Keys.Right: _moveableObject.RotateClockwise(); break;
                    case Keys.Up: _moveableObject.MoveForward(); break;
                    case Keys.Down: _moveableObject.MoveBackward(); break;
                }
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _drawingKit.ViewMatrix = _cameraManager.ViewMatrix;
            _drawingKit.ViewerPosition = _cameraManager.ViewerPosition;

            foreach(DrawableObject drawableObject in _drawableObjects)
            {
                drawableObject.Draw(_drawerManager.CurrentDrawer);
            }

            _illumination.Draw(_drawerManager.CurrentDrawer);
            _illumination2.Draw(_drawerManager.CurrentDrawer);
            _tree.Draw(_drawerManager.CurrentDrawer);

            base.Draw(gameTime);
        }
    }
}
