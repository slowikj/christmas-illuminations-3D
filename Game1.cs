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

        Dictionary<String, Model[]> _models;

        Scene _scene;
        DrawerManager _drawerManager;
        DrawingKit _drawingKit;
        
        
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

            _drawingKit = new DrawingKit(
                Matrix.Identity,
                DrawingKit.GetDefaultProjectionMatrix(this.GraphicsDevice),
                Vector3.Zero,
                this.GraphicsDevice,
                new LightingInfo());

            _drawerManager = _GetDrawerManager(_drawingKit);

            _models = _GetModels();

            _scene = _GetScene();

            // TODO: use this.Content to load your game content here
        }
   
        private DrawerManager _GetDrawerManager(DrawingKit drawingKit)
        {
            DrawerManager res = new DrawerManager();

            res.AddDrawer("Phong",
                new DefaultDrawer(drawingKit, this.Content.Load<Effect>("Shaders/PhongShader")));

            res.AddDrawer("Goraud",
                new DefaultDrawer(drawingKit, this.Content.Load<Effect>("Shaders/GoraudShader")));

            res.AddDrawer("OldFlat",
                new FlatDrawer(drawingKit, this.Content.Load<Effect>("Shaders/OldFlatShader")));

            res.AddDrawer("NewFlat",
                new DefaultDrawer(drawingKit, this.Content.Load<Effect>("Shaders/NewFlatShader")));

            res.SetDrawer("Phong");

            return res;
        }

        private Dictionary<String, Model[]> _GetModels()
        {
            Dictionary<String, Model[]> res = new Dictionary<string, Model[]>();

            res.Add("kitty", new Model[]
            {
                _GetModel("Models/cats/kitty0"),
                _GetModel("Models/cats/kitty1")
            });

            res.Add("cone", new Model[] { _GetModel("Models/smallCone") });
            res.Add("christmasTree", new Model[] { _GetModel("Models/lowpolytree") });
            res.Add("ball", new Model[] { _GetModel("Models/tinyBall") });
            res.Add("streetLamp", new Model[] { _GetModel("Models/streetLamp") });

            return res;
        }

        private Model _GetModel(String path)
        {
            return this.Content.Load<Model>(path);
        }

        private Scene _GetScene()
        {
            Scene res = new Scene();

            DrawableObject moveableObject = new DrawableObject(_models["kitty"], Vector3.Zero, Color.Cyan,
                new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                                       new Vector3((float)0.01, (float)0.1, (float)0.1),
                                       new Vector3((float)0.001, (float)0.01, (float)0.01),
                                       1000));

            res.AddObject(moveableObject);

            _PrepareCameras(res, moveableObject);

            _AddTreesWithLamps(res, -5, 1, Color.Magenta, _models["ball"][0]);
            _AddTreesWithLamps(res, 5, -1, Color.Yellow, _models["cone"][0]);

            PatternGenerator sinOnCurve = new PatternGenerator(a => a, a => (float)Math.Sin(a), a => 10 * a * (a - 3));
            res.AddIllumination(new Illumination(_models["ball"][0],
                sinOnCurve.GetPoints(200, -3, 3, 0, 10 * (float)Math.PI, 0, 3), Color.Red,
                new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 1)));
            
            return res;
        }
        
        private void _PrepareCameras(Scene scene, DrawableObject moveableObject)
        {
            foreach (KeyValuePair<String, Camera> cameraInfo in _GetCameras(moveableObject))
            {
                scene.AddCamera(cameraInfo.Key, cameraInfo.Value);
            }

            scene.SetCamera("Constant");
        }

        private Dictionary<String, Camera> _GetCameras(DrawableObject moveableObject)
        {
            Vector3 CONSTANT_CAMERA_POSITION = new Vector3(2, 0, -8);

            Dictionary<String, Camera> res = new Dictionary<string, Camera>();

            res.Add("Constant", new ConstantCamera(CONSTANT_CAMERA_POSITION, Vector3.Zero));

            res.Add("MoveableObjectCamera", new MoveableObjectCamera(moveableObject));

            res.Add("StationaryObjectCamera", new StationaryObjectCamera(CONSTANT_CAMERA_POSITION, moveableObject));

            return res;
        }

        private void _AddTreesWithLamps(Scene scene, int xVal, int incX, Color lightColor, Model lamp)
        {
            PatternGenerator patternGenerator = new PatternGenerator(a => xVal + incX, a => (float)Math.Sin(a), a => -a);

            scene.AddIllumination(new Illumination(lamp,
                patternGenerator.GetPoints(100, 0, 1, 0, (float)Math.PI * 8, -10, 0), lightColor,
                new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 1)));

            
            foreach (IDrawable tree in _GetForest(xVal))
            {
                scene.AddObject(tree);
            }
        }

        private IEnumerable<IDrawable> _GetForest(int xVal)
        {
            IEnumerable<IDrawable> trees = new PatternGenerator(x => xVal, x => 1, z => -z)
                .GetPoints(5, 0, 1, 0, 1, -10, 0)
                .Select(point => new DrawableObject(_models["christmasTree"], point, Color.Green,
                    new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                   new Vector3((float)0.5, (float)0.5, (float)0.5),
                   new Vector3((float)0.1, (float)0.1, (float)0.1),
                   2000000)));

            return trees;
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
                    case Keys.C: _scene.SetCamera("Constant"); break;
                    case Keys.M: _scene.SetCamera("MoveableObjectCamera"); break;
                    case Keys.S: _scene.SetCamera("StationaryObjectCamera"); break;
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
                    case Keys.Left: _scene.MoveCurrentObject(Move.RotateAnticlockwise); break;
                    case Keys.Right: _scene.MoveCurrentObject(Move.RotateClockwise); break;
                    case Keys.Up: _scene.MoveCurrentObject(Move.Forward); break;
                    case Keys.Down: _scene.MoveCurrentObject(Move.Backward); break;
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

            _scene.Draw(_drawerManager.CurrentDrawer);

            base.Draw(gameTime);
        }
    }
}
