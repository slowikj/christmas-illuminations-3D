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
using Projekt4.PatternGenerators;

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

            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

            graphics.PreferredBackBufferWidth = 1500;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            graphics.ApplyChanges();

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

            res.Add("kitty", Enumerable.Range(0, 15).Select(i => _GetModel("Models/cats/kitty" + i.ToString())).ToArray());
            
            res.Add("cone", new Model[] { _GetModel("Models/smallCone") });
            res.Add("christmasTree", new Model[] { _GetModel("Models/lowpolytree") });
            res.Add("ball", new Model[] { _GetModel("Models/tinyBall") });
            res.Add("sqrPlane", new Model[] { _GetModel("Models/sqrPlane") });
        
            return res;
        }

        private Model _GetModel(String path)
        {
            return this.Content.Load<Model>(path);
        }

        private Scene _GetScene()
        {
            Scene res = new Scene();

            SceneActor moveableObject = new SceneActor(_models["kitty"], new Vector3(0, -(float)0.4, 0), Color.Cyan,
                new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                                       new Vector3((float)0.01, (float)0.1, (float)0.1),
                                       new Vector3((float)0.001, (float)0.01, (float)0.01),
                                       500),
                new RotationInfo(0, MathHelper.ToRadians(180), 0));

            res.AddObject(moveableObject);

            _PrepareCameras(res, moveableObject);

            _AddTreesWithLamps(res, -5, 1, Color.Magenta, _models["ball"][0]);
            _AddTreesWithLamps(res, 5, -1, Color.Yellow, _models["cone"][0]);

            PatternGenerator sinOnCurve = new PatternGenerator(
                new Function2D(a => a, v => v.Y, new Range(-3, 3)),
                new Function2D(a => (float)Math.Sin(a), v => v.Y, new Range(0, 10 * (float)Math.PI)),
                new Function2D(a => 10 * a * (a - 3), v => v.Y, new Range(0, 3)));
            
            res.AddIllumination(new Illumination(_models["ball"][0],
                sinOnCurve.GetPoints(150), Color.Red,
                new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 1)));

            res.AddIllumination(new Illumination(_models["ball"][0],
                _GetFlowerPointsPattern(), Color.Violet,
                new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 5)));

            foreach (SceneActor plane in _GetPlaneMesh(_models["sqrPlane"], Color.DarkGreen, new Vector3(-3, -(float)1, 0), 8, 4))
            {
                res.AddObject(plane);
            }
            
            return res;
        }
        
        private void _PrepareCameras(Scene scene, SceneActor moveableObject)
        {
            foreach (KeyValuePair<String, Camera> cameraInfo in _GetCameras(moveableObject))
            {
                scene.AddCamera(cameraInfo.Key, cameraInfo.Value);
            }

            scene.SetCamera("Constant");
        }

        private Dictionary<String, Camera> _GetCameras(SceneActor moveableObject)
        {
            Vector3 CONSTANT_CAMERA_POSITION = new Vector3(0, 0, -20);

            Dictionary<String, Camera> res = new Dictionary<string, Camera>();

            res.Add("Constant", new Camera(CONSTANT_CAMERA_POSITION, Vector3.Zero));

            res.Add("MoveableObjectCamera", new MoveableObjectCamera(moveableObject));

            res.Add("StationaryObjectCamera", new StationaryObjectCamera(CONSTANT_CAMERA_POSITION, moveableObject));

            return res;
        }

        private void _AddTreesWithLamps(Scene scene, int xVal, int incX, Color lightColor, Model lamp)
        {
            PatternGenerator sin = new PatternGenerator(
                new Function2D(a => xVal + incX, v => v.Y, new Range(0, 1)),
                new Function2D(a => (float)Math.Sin(a), v => v.Y, new Range(0, (float)Math.PI * 8)),
                new Function2D(a => -a, v => v.Y, new Range(-10, 0)));
            
            scene.AddIllumination(new Illumination(lamp,
                sin.GetPoints(100), lightColor,
                new ReflectanceFactors(Vector3.Zero, Vector3.One, Vector3.One, 1)));

            
            foreach (IDrawable tree in _GetForest(xVal))
            {
                scene.AddObject(tree);
            }
        }

        private IEnumerable<IDrawable> _GetForest(int xVal)
        {
            PatternGenerator p = new PatternGenerator(
                new Function2D(x => xVal, v => v.Y, new Range(0, 1)),
                new Function2D(x => 1, v => v.Y, new Range(0, 1)),
                new Function2D(z => -z, v => v.Y, new Range(-10, 0)));

            IEnumerable<IDrawable> trees = p.GetPoints(5)
                .Select(point => new SceneActor(_models["christmasTree"], point, Color.Green,
                    new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                   new Vector3((float)0.5, (float)0.5, (float)0.5),
                   new Vector3((float)0.1, (float)0.1, (float)0.1),
                   2000)));

            return trees;
        }

        private IEnumerable<SceneActor> _GetPlaneMesh(Model[] planeModels, Color color, Vector3 startPos, int rows, int cols)
        {
            Func<Random, Vector3> getRandomVector = (rr => new Vector3((float)rr.NextDouble(), (float)rr.NextDouble(), (float)rr.NextDouble()));
            Random random = new Random();
            SceneActor tmpActor = new SceneActor(planeModels, Vector3.Zero, Color.Gray);

            return _GetPositionsOfMesh(startPos, rows, cols, 2, 2)
                .Select(position => new SceneActor(planeModels, position, color,
                    new ReflectanceFactors(Vector3.Zero, getRandomVector(random), getRandomVector(random), random.Next(2, 200))));                        
        }


        private IEnumerable<Vector3> _GetPositionsOfMesh(Vector3 startPos, int rows, int cols, float width, float length)
        {
            List<Vector3> res = new List<Vector3>();
            for(int r = 0; r < rows; ++r)
            {
                for(int c = 0; c < cols; ++c)
                {
                    res.Add(new Vector3(startPos.X + c * width, startPos.Y, startPos.Z + r * length));
                }
            }

            return res;
        }

        private IEnumerable<Vector3> _GetFlowerPointsPattern()
        {
            PatternGenerator flowerPattern = new PatternGenerator(
                new Function2D(a => a, v => v.Y, new Range(-(float)Math.PI, (float)Math.PI)),
                new Function2D(p => 3 + 2 * (float)Math.Sin(8 * p), v => v.X * (float)Math.Sin(v.Y), new Range(-(float)Math.PI, (float)Math.PI)),
                new Function2D(p => 18, v => v.Y, new Range(0, 1)));

            return flowerPattern.GetPoints(200);
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
