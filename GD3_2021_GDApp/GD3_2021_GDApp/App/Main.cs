//#define DEMO

using GDLibrary;
using GDLibrary.Collections;
using GDLibrary.Components;
using GDLibrary.Components.UI;
using GDLibrary.Core;
using GDLibrary.Core.Demo;
using GDLibrary.Graphics;
using GDLibrary.Inputs;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using GDLibrary.Renderers;
using GDLibrary.Utilities;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GDApp
{
    public class Main : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        
        /// <summary>
        /// Stores and updates all scenes (which means all game objects i.e. players, cameras, pickups, behaviours, controllers)
        /// </summary>
        private SceneManager sceneManager;

        /// <summary>
        /// Draws all game objects with an attached and enabled renderer
        /// </summary>
        private RenderManager renderManager;

        /// <summary>
        /// Updates and Draws all ui objects
        /// </summary>
        private UISceneManager uiSceneManager;

        /// <summary>
        /// Updates and Draws all menu objects
        /// </summary>
        private MyMenuManager uiMenuManager;

        /// <summary>
        /// Plays all 2D and 3D sounds
        /// </summary>
        private SoundManager soundManager;

        private MyStateManager stateManager;
        private PickingManager pickingManager;

        /// <summary>
        /// Handles all system wide events between entities
        /// </summary>
        private EventDispatcher eventDispatcher;

        /// <summary>
        /// Applies physics to all game objects with a Collider
        /// </summary>
        private PhysicsManager physicsManager;

        /// <summary>
        /// Quick lookup for all textures used within the game
        /// </summary>
        private Dictionary<string, Texture2D> textureDictionary;

        /// <summary>
        /// Quick lookup for all fonts used within the game
        /// </summary>
        private ContentDictionary<SpriteFont> fontDictionary;

        /// <summary>
        /// Quick lookup for all models used within the game
        /// </summary>
        private ContentDictionary<Model> modelDictionary;

        /// <summary>
        /// Quick lookup for all videos used within the game by texture behaviours
        /// </summary>
        private ContentDictionary<Video> videoDictionary;

        //temps
        private Scene activeScene;
        private Scene level2;

        private UITextObject nameTextObj;
        private Collider collider;

        #endregion Fields

        /// <summary>
        /// Construct the Game object
        /// </summary>
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Set application data, input, title and scene manager
        /// </summary>
        private void InitializeEngine(string gameTitle, int width, int height)
        {
            //set game title
            Window.Title = "Lava Leap";

            //the most important element! add event dispatcher for system events
            eventDispatcher = new EventDispatcher(this);

            //add physics manager to enable CD/CR and physics
            physicsManager = new PhysicsManager(this);

            //instanciate scene manager to store all scenes
            sceneManager = new SceneManager(this);

            //create the ui scene manager to update and draw all ui scenes
            uiSceneManager = new UISceneManager(this, _spriteBatch);

            //create the ui menu manager to update and draw all menu scenes
            uiMenuManager = new MyMenuManager(this, _spriteBatch);

            //add support for playing sounds
            soundManager = new SoundManager(this);

            //this will check win/lose logic
            stateManager = new MyStateManager(this);

            //picking support using physics engine
            //this predicate lets us say ignore all the other collidable objects except interactables and consumables
            Predicate<GameObject> collisionPredicate =
                (collidableObject) =>
            {
                if (collidableObject != null)
                    return collidableObject.GameObjectType
                    == GameObjectType.Interactable
                    || collidableObject.GameObjectType == GameObjectType.Consumable;

                return false;
            };
            pickingManager = new PickingManager(this, 2, 100, collisionPredicate);

            //initialize global application data
            Application.Main = this;
            Application.Content = Content;
            Application.GraphicsDevice = _graphics.GraphicsDevice;
            Application.GraphicsDeviceManager = _graphics;
            Application.SceneManager = sceneManager;
            Application.PhysicsManager = physicsManager;
            Application.StateManager = stateManager;
            Application.UISceneManager = uiSceneManager;

            //instanciate render manager to render all drawn game objects using preferred renderer (e.g. forward, backward)
            renderManager = new RenderManager(this, new ForwardRenderer(), false, true);

            //instanciate screen (singleton) and set resolution etc
            Screen.GetInstance().Set(width, height, true, true);

            //instanciate input components and store reference in Input for global access
            Input.Keys = new KeyboardComponent(this);
            Input.Mouse = new MouseComponent(this);
            Input.Mouse.Position = Screen.Instance.ScreenCentre;
            Input.Gamepad = new GamepadComponent(this);

            //************* add all input components to component list so that they will be updated and/or drawn ***********/

            //add time support
            Components.Add(Time.GetInstance(this));

            //add event dispatcher
            Components.Add(eventDispatcher);

            //add input support
            Components.Add(Input.Keys);
            Components.Add(Input.Mouse);
            Components.Add(Input.Gamepad);

            //add physics manager to enable CD/CR and physics
            Components.Add(physicsManager);

            //add support for picking using physics engine
            Components.Add(pickingManager);

            //add scene manager to update game objects
            Components.Add(sceneManager);

            //add render manager to draw objects
            Components.Add(renderManager);

            //add ui scene manager to update and drawn ui objects
            Components.Add(uiSceneManager);

            //add ui menu manager to update and drawn menu objects
            Components.Add(uiMenuManager);

            //add sound
            Components.Add(soundManager);

            //add state
            Components.Add(stateManager);
        }

        /// <summary>
        /// Not much happens in here as SceneManager, UISceneManager, MenuManager and Inputs are all GameComponents that automatically Update()
        /// Normally we use this to add some temporary demo code in class - Don't forget to remove any temp code inside this method!
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.H))
            {
                //DEMO - raise event
                //EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                  //  EventActionType.OnPause));

                object[] parameters = { nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnRemoveObject, parameters));

                //renderManager.StatusType = StatusType.Off;

            }
            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.U))
            {
                //DEMO - raise event

                //spriteBatch.DrawString(spriteFont, $"Camera [{Camera.Main.GameObject.Name}, " +
                  //  $"{translation}]", fpsTextPosition + new Vector2(0, 20), fpsTextColor);
                object[] parameters = { nameTextObj.Text, "Camera 1" };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnAddObject, parameters));

                //renderManager.StatusType = StatusType.Drawn;
                //EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                //  EventActionType.OnPlay));
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                object[] parameters = { "health", 1 };
                EventDispatcher.Raise(new EventData(EventCategoryType.UI,
                    EventActionType.OnHealthDelta, parameters));
            }
            else if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                object[] parameters = { "health", -1 };
                EventDispatcher.Raise(new EventData(EventCategoryType.UI,
                    EventActionType.OnHealthDelta, parameters));
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                          EventActionType.OnPause));
            }
            else if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.O))
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                    EventActionType.OnPlay));
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                object[] parameters = { "smokealarm" };
                EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                    EventActionType.OnPlay2D, parameters));
            }
            else if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                object[] parameters = { "smokealarm" };
                EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                    EventActionType.OnStop, parameters));
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.C))
                Application.SceneManager.ActiveScene.CycleCameras();

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                object[] parameters = { nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnRemoveObject, parameters));


                Application.SceneManager.ActiveScene.SetMainCamera("camera1");
                /*
                var camera1UI = new UIScene(AppData.UI_CAMERA_1);

                //object[] parameters = { "player name", "Camera 1" };
                //EventDispatcher.Raise(new EventData(EventCategoryType.UI, EventActionType.OnscreenText, parameters));


                nameTextObj = new UITextObject("camera name", UIObjectType.Text,
                new Transform2D(new Vector2(50, 50),
                new Vector2(10, 10), 0),
                0, fontDictionary["menu"], "CAMERA 1");

                camera1UI.Add(nameTextObj);
                uiSceneManager.SetActiveScene(AppData.UI_CAMERA_1);
                */

            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                object[] parameters = { nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnRemoveObject, parameters));


                Application.SceneManager.ActiveScene.SetMainCamera("camera2");
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                object[] parameters = { nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnRemoveObject, parameters));


                Application.SceneManager.ActiveScene.SetMainCamera("camera3");
            }

   
            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    object[] parameters = { nameTextObj };

                    EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                        EventActionType.OnRemoveObject, parameters));


                    Application.SceneManager.ActiveScene.SetMainCamera(AppData.CAMERA_FIRSTPERSON_COLLIDABLE_NAME);
                }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                InitializeIsometricCamera(activeScene);
                //InitializeIsometricCamera(level2);
            }

            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                object[] parameters = { "bounce" };
                EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                    EventActionType.OnPlay2D, parameters));
            }

            stateManager.Update(gameTime);
            //if(stateManager.GetType )

            base.Update(gameTime);
        }

        /// <summary>
        /// Not much happens in here as RenderManager, UISceneManager and MenuManager are all DrawableGameComponents that automatically Draw()
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

        }

        /******************************** Student Project-specific ********************************/
        /******************************** Student Project-specific ********************************/
        /******************************** Student Project-specific ********************************/

        #region Student/Group Specific Code

        /// <summary>
        /// Initialize engine, dictionaries, assets, level contents
        /// </summary>
        protected override void Initialize()
        {
            //move here so that UISceneManager can use!
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //data, input, scene manager
            InitializeEngine(AppData.GAME_TITLE_NAME,
                AppData.GAME_RESOLUTION_WIDTH,
                AppData.GAME_RESOLUTION_HEIGHT);

            //load structures that store assets (e.g. textures, sounds) or archetypes (e.g. Quad game object)
            InitializeDictionaries();

            //load assets into the relevant dictionary
            LoadAssets();

            //level with scenes and game objects
            InitializeLevel();

            //add menu and ui
            InitializeUI();

            //TODO - remove hardcoded mouse values - update Screen class to centre the mouse with hardcoded value - remove later
            Input.Mouse.Position = Screen.Instance.ScreenCentre;

            //turn on/off debug info
            InitializeDebugUI(true, true);

            //to show the menu we must start paused for everything else!
            EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPause));

            base.Initialize();
        }

        /******************************* Load/Unload Assets *******************************/

        private void InitializeDictionaries()
        {
            textureDictionary = new Dictionary<string, Texture2D>();

            //why not try the new and improved ContentDictionary instead of a basic Dictionary?
            fontDictionary = new ContentDictionary<SpriteFont>();
            modelDictionary = new ContentDictionary<Model>();

            //stores videos
            videoDictionary = new ContentDictionary<Video>();
        }

        private void LoadAssets()
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
        }

        /// <summary>
        /// Loads video content used by UIVideoTextureBehaviour
        /// </summary>
        private void LoadVideos()
        {
  
        }

        /// <summary>
        /// Load models to dictionary
        /// </summary>
        private void LoadModels()
        {
            
        }

        /// <summary>
        /// Load fonts to dictionary
        /// </summary>
        private void LoadFonts()
        {
            fontDictionary.Add("Assets/Fonts/ui");
            fontDictionary.Add("Assets/Fonts/menu");
            fontDictionary.Add("Assets/Fonts/debug");
        }

        /// <summary>
        /// Load sound data used by sound manager
        /// </summary>
        private void LoadSounds()
        {
            var soundEffect =
                Content.Load<SoundEffect>("Assets/Sounds/Effects/smokealarm1");

            var backgroundMusic =
                Content.Load<SoundEffect>("Assets/Sounds/music");

            var bounce =
               Content.Load<SoundEffect>("Assets/Sounds/Effects/bounce");

            var win =
               Content.Load<SoundEffect>("Assets/Sounds/Effects/win");

            var lose =
               Content.Load<SoundEffect>("Assets/Sounds/Effects/lose");

            var loseNoise =
               Content.Load<SoundEffect>("Assets/Sounds/Effects/loseNoise");


            //add the new sound effect
            soundManager.Add(new GDLibrary.Managers.Cue(
                "smokealarm",
                soundEffect,
                SoundCategoryType.Alarm,
                new Vector3(1, 0, 0),
                false));

            soundManager.Add(new GDLibrary.Managers.Cue(
                "music",
                backgroundMusic,
                SoundCategoryType.BackgroundMusic,
                new Vector3(1, 0, 0),
                false));

            soundManager.Add(new GDLibrary.Managers.Cue(
                "bounce",
                bounce,
                SoundCategoryType.Effect,
                new Vector3(1, 0, 0),
                false));

            soundManager.Add(new GDLibrary.Managers.Cue(
                "win",
                win,
                SoundCategoryType.Effect,
                new Vector3(1, 0, 0),
                false));

            soundManager.Add(new GDLibrary.Managers.Cue(
                "lose",
                lose,
                SoundCategoryType.Effect,
                new Vector3(1, 0, 0),
                false));

            soundManager.Add(new GDLibrary.Managers.Cue(
                "loseNoise",
                loseNoise,
                SoundCategoryType.Effect,
                new Vector3(1, 0, 0),
                false));
        }

        /// <summary>
        /// Load texture data from file and add to the dictionary
        /// </summary>
        private void LoadTextures()
        {
            //debug
            textureDictionary.Add("checkerboard", Content.Load<Texture2D>("Assets/Demo/Textures/checkerboard"));
            textureDictionary.Add("mona lisa", Content.Load<Texture2D>("Assets/Demo/Textures/mona lisa"));

            //skybox
            textureDictionary.Add("skybox_front", Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
            textureDictionary.Add("skybox_left", Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
            textureDictionary.Add("skybox_right", Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
            textureDictionary.Add("skybox_back", Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
            textureDictionary.Add("skybox_sky", Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));

            //environment
            textureDictionary.Add("grass", Content.Load<Texture2D>("Assets/Textures/Foliage/Ground/grass1"));
            textureDictionary.Add("lava", Content.Load<Texture2D>("Assets/Textures/Foliage/Ground/lava"));
            textureDictionary.Add("crate1", Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate1"));
            textureDictionary.Add("platform", Content.Load<Texture2D>("Assets/Textures/Props/wall"));
            textureDictionary.Add("platform_bounce", Content.Load<Texture2D>("Assets/Textures/Props/wall_bounce"));
            textureDictionary.Add("wall", Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/wall"));

            //ui
            textureDictionary.Add("ui_progress_32_8", Content.Load<Texture2D>("Assets/Textures/UI/Controls/ui_progress_32_8"));
            textureDictionary.Add("progress_white", Content.Load<Texture2D>("Assets/Textures/UI/Controls/progress_white"));

            //menu
            textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/mainmenu"));
            textureDictionary.Add("loseMenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/lose"));
            textureDictionary.Add("winMenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/win"));
            textureDictionary.Add("main_menu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/main_menu_new"));
            textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/audiomenu"));
            textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/controlsmenu"));
            textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/exitmenuwithtrans"));
            textureDictionary.Add("genericbtn", Content.Load<Texture2D>("Assets/Textures/UI/Controls/genericbtn"));
            textureDictionary.Add("generic_btn", Content.Load<Texture2D>("Assets/Textures/UI/Controls/generic_btn"));

            //reticule
            textureDictionary.Add("reticuleOpen",
            Content.Load<Texture2D>("Assets/Textures/UI/Controls/reticuleOpen"));
            textureDictionary.Add("reticuleDefault",
          Content.Load<Texture2D>("Assets/Textures/UI/Controls/reticuleDefault"));
        }

        /// <summary>
        /// Free all asset resources, dictionaries, network connections etc
        /// </summary>
        protected override void UnloadContent()
        {
            //remove all models used for the game and free RAM
            modelDictionary?.Dispose();
            fontDictionary?.Dispose();
            videoDictionary?.Dispose();

            base.UnloadContent();
        }

        /******************************* UI & Menu *******************************/

        /// <summary>
        /// Create a scene, add content, add to the scene manager, and load default scene
        /// </summary>
        private void InitializeLevel()
        {
            float worldScale = 1000;
            activeScene = new Scene("level 1");
            level2 = new Scene(AppData.MENU_LEVEL_2);

            InitializeCameras(activeScene);

            InitializeSkybox(activeScene, worldScale);


            //remove because now we are interested only in collidable things!
            //InitializeCubes(activeScene);
            //InitializeModels(activeScene);

            InitializeCollidables(activeScene, worldScale);

            sceneManager.Add(activeScene);



            InitializeCameras(level2);

            InitializeSkybox(level2, worldScale);

            //remove because now we are interested only in collidable things!
            //InitializeCubes(activeScene);
            //InitializeModels(activeScene);

            InitializeCollidablesLevel2(level2, worldScale);

            sceneManager.Add(level2);

            sceneManager.LoadScene("level 1");
            //sceneManager.LoadScene("level 2");
        }

        private void InitializeCollidablesLevel2(Scene level, float worldScale)
        {
            InitializeCollidableGround(level, worldScale);
            InitializeCollidableLava(level, worldScale);
            InitializePlatformsLevel2(level);
        }

        /// <summary>
        /// Adds menu and UI elements
        /// </summary>
        private void InitializeUI()
        {
            InitializeGameMenu();
            InitializeGameUI();
            InitializeLoseMenu();
            InitializeWinMenu();
        }

        private void InitializeLoseMenu()
        {
            UIObject menuObject = null;

            /************************** Lose Menu Scene **************************/
            //make the lose menu scene
            var loseScene = new UIScene(AppData.LOSE_SCREEN);

            /**************************** Background Image ****************************/

            var texture = textureDictionary["loseMenu"];
            //get how much we need to scale background to fit screen, then downsizes a little so we can see game behind background
            var scale = _graphics.GetScaleForTexture(texture,
                new Vector2(1f, 1f));

            menuObject = new UITextureObject("main background",
                UIObjectType.Texture,
                new Transform2D(Screen.Instance.ScreenCentre, scale, 0), //sets position as center of screen
                0,
                new Color(255, 255, 255, 215),
                texture.GetOriginAtCenter(), //if we want to position image on screen center then we need to set origin as texture center
                texture);

            //add ui object to scene
            loseScene.Add(menuObject);

            //uiMenuManager.Add(loseScene);
            uiSceneManager.Add(loseScene);

            //finally we say...where do we start
            //uiMenuManager.SetActiveScene(AppData.LOSE_SCREEN);
        }

        private void InitializeWinMenu()
        {
            UIObject menuObject = null;

            /************************** Lose Menu Scene **************************/
            //make the win menu scene
            var winScene = new UIScene(AppData.WIN_SCREEN);

            /**************************** Background Image ****************************/

            var texture = textureDictionary["winMenu"];
            //get how much we need to scale background to fit screen, then downsizes a little so we can see game behind background
            var scale = _graphics.GetScaleForTexture(texture,
                new Vector2(1f, 1f));

            menuObject = new UITextureObject("main background",
                UIObjectType.Texture,
                new Transform2D(Screen.Instance.ScreenCentre, scale, 0), //sets position as center of screen
                0,
                new Color(255, 255, 255, 215),
                texture.GetOriginAtCenter(), //if we want to position image on screen center then we need to set origin as texture center
                texture);

            //add ui object to scene
            winScene.Add(menuObject);

            //uiMenuManager.Add(winScene);
            uiSceneManager.Add(winScene);

            //finally we say...where do we start
            //uiMenuManager.SetActiveScene(AppData.WIN_SCREEN);
        }

        /// <summary>
        /// Adds main menu elements
        /// </summary>
        private void InitializeGameMenu()
        {
            //a re-usable variable for each ui object
            UIObject menuObject = null;

            #region Main Menu

            /************************** Main Menu Scene **************************/
            //make the main menu scene
            var mainMenuUIScene = new UIScene(AppData.MENU_MAIN_NAME);

            /**************************** Background Image ****************************/

            //main background
            var texture = textureDictionary["main_menu"];
            //get how much we need to scale background to fit screen, then downsizes a little so we can see game behind background
            var scale = _graphics.GetScaleForTexture(texture,
                new Vector2(0.8f, 0.8f));

            menuObject = new UITextureObject("main background",
                UIObjectType.Texture,
                new Transform2D(Screen.Instance.ScreenCentre, scale, 0), //sets position as center of screen
                0,
                new Color(255, 255, 255, 200),
                texture.GetOriginAtCenter(), //if we want to position image on screen center then we need to set origin as texture center
                texture);

            //add ui object to scene
            mainMenuUIScene.Add(menuObject);

            /**************************** Play Button ****************************/

            var btnTexture = textureDictionary["generic_btn"];
            var sourceRectangle
                = new Microsoft.Xna.Framework.Rectangle(0, 0,
                btnTexture.Width, btnTexture.Height);
            var origin = new Vector2(btnTexture.Width / 2.0f, btnTexture.Height / 2.0f);

            var playBtn = new UIButtonObject(AppData.MENU_PLAY_BTN_NAME, UIObjectType.Button,
                new Transform2D(AppData.MENU_PLAY_BTN_POSITION,
                1.5f * Vector2.One, 0),
                0.1f,
                Color.White,
                SpriteEffects.None,
                origin,
                btnTexture,
                null,
                sourceRectangle,
                "PLAY",
                fontDictionary["menu"],
                Color.Black,
                Vector2.Zero);

            //demo button color change
            var comp = new UIColorMouseOverBehaviour(Color.HotPink, Color.White);
            playBtn.AddComponent(comp);

            mainMenuUIScene.Add(playBtn);

            /**************************** Controls Button ****************************/

            //same button texture so we can re-use texture, sourceRectangle and origin

            var level2button = new UIButtonObject(AppData.MENU_PLAY_BTN_LVL_2, UIObjectType.Button,
                new Transform2D(AppData.MENU_CONTROLS_BTN_POSITION, 1.5f * Vector2.One, 0),
                0.1f,
                Color.White,
                origin,
                btnTexture,
                "PLAY LEVEL 2",
                fontDictionary["menu"],
                Color.Black);

            //demo button color change
            level2button.AddComponent(new UIColorMouseOverBehaviour(Color.Orange, Color.White));

            mainMenuUIScene.Add(level2button);

            /**************************** Exit Button ****************************/

            //same button texture so we can re-use texture, sourceRectangle and origin

            //use a simple/smaller version of the UIButtonObject constructor
            var exitBtn = new UIButtonObject(AppData.MENU_EXIT_BTN_NAME, UIObjectType.Button,
                new Transform2D(AppData.MENU_EXIT_BTN_POSITION, 1.5f * Vector2.One, 0),
                0.1f,
                Color.White,
                origin,
                btnTexture,
                "EXIT",
                fontDictionary["menu"],
                Color.Black);

            //demo button color change
            exitBtn.AddComponent(new UIColorMouseOverBehaviour(Color.Tomato, Color.White));

            mainMenuUIScene.Add(exitBtn);

            #endregion Main Menu

            //add scene to the menu manager
            uiMenuManager.Add(mainMenuUIScene);

            /************************** PLAY LEVEL 2 Menu Scene **************************/

            

            /************************** Exit Menu Scene **************************/

            //finally we say...where do we start
            uiMenuManager.SetActiveScene(AppData.MENU_MAIN_NAME);
        }

        /// <summary>
        /// Adds ui elements seen in-game (e.g. health, timer)
        /// </summary>
        private void InitializeGameUI()
        {
            //create the scene
            var mainGameUIScene = new UIScene(AppData.UI_SCENE_MAIN_NAME);

            #region Add Health Bar

            //add a health bar in the centre of the game window
            var texture = textureDictionary["progress_white"];
            var position = new Vector2(_graphics.PreferredBackBufferWidth / 2, 50);
            var origin = new Vector2(texture.Width / 2, texture.Height / 2);

            ////create the UI element
            //var healthTextureObj = new UITextureObject("health",
            //    UIObjectType.Texture,
            //    new Transform2D(position, new Vector2(2, 0.5f), 0),
            //    0,
            //    Color.White,
            //    origin,
            //    texture);

            ////add a demo time based behaviour - because we can!
            //healthTextureObj.AddComponent(new UITimeColorFlipBehaviour(Color.White, Color.Red, 1000));

            ////add a progress controller
            //healthTextureObj.AddComponent(new UIProgressBarController(5, 10));

            ////add the ui element to the scene
            //mainGameUIScene.Add(healthTextureObj);

            #endregion Add Health Bar

            #region Add Text

            var font = fontDictionary["ui"];
            var str = "level";

            //create the UI element
            nameTextObj = new UITextObject(str, UIObjectType.Text,
                new Transform2D(new Vector2(50, 50),
                new Vector2(10, 10), 0),
                0, font, "ESCAPE");

            //  nameTextObj.Origin = font.MeasureString(str) / 2;
            //  nameTextObj.AddComponent(new UIExpandFadeBehaviour());

            //add the ui element to the scene
            mainGameUIScene.Add(nameTextObj);

            #endregion Add Text

            #region Add Reticule

            var defaultTexture = textureDictionary["reticuleDefault"];
            var alternateTexture = textureDictionary["reticuleOpen"];
            origin = defaultTexture.GetOriginAtCenter();

            var reticule = new UITextureObject("reticule",
                     UIObjectType.Texture,
                new Transform2D(Vector2.Zero, Vector2.One, 0),
                0,
                Color.White,
                SpriteEffects.None,
                origin,
                defaultTexture,
                alternateTexture,
                new Microsoft.Xna.Framework.Rectangle(0, 0,
                defaultTexture.Width, defaultTexture.Height));

            reticule.AddComponent(new UIReticuleBehaviour());

            mainGameUIScene.Add(reticule);

            #endregion Add Reticule


            #region Add Scene To Manager & Set Active Scene

            //add the ui scene to the manager
            uiSceneManager.Add(mainGameUIScene);

            //set the active scene
            uiSceneManager.SetActiveScene(AppData.UI_SCENE_MAIN_NAME);

            #endregion Add Scene To Manager & Set Active Scene
        }

        /// <summary>
        /// Adds component to draw debug info to the screen
        /// </summary>
        private void InitializeDebugUI(bool showDebugInfo, bool showCollisionSkins = true)
        {
            if (showDebugInfo)
            {
                Components.Add(new GDLibrary.Utilities.GDDebug.PerfUtility(this,
                    _spriteBatch, fontDictionary["debug"],
                    new Vector2(40, _graphics.PreferredBackBufferHeight - 80),
                    Color.White));
            }

            if (showCollisionSkins)
                Components.Add(new GDLibrary.Utilities.GDDebug.PhysicsDebugDrawer(this, Color.Red));
        }

        /******************************* Non-Collidables *******************************/

        /// <summary>
        /// Set up the skybox using a QuadMesh
        /// </summary>
        /// <param name="level">Scene Stores all game objects for current...</param>
        /// <param name="worldScale">float Value used to scale skybox normally 250 - 1000</param>
        private void InitializeSkybox(Scene level, float worldScale = 500)
        {
            #region Reusable - You can copy and re-use this code elsewhere, if required

            //re-use the code on the gfx card
            var shader = new BasicShader(Application.Content, true, true);
            //re-use the vertices and indices of the primitive
            var mesh = new QuadMesh();
            //create an archetype that we can clone from
            var archetypalQuad = new GameObject("quad", GameObjectType.Skybox, true);

            #endregion Reusable - You can copy and re-use this code elsewhere, if required

            GameObject clone = null;
            //back
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_back";
            clone.Transform.Translate(0, 0, -worldScale / 2.0f);
            clone.Transform.Scale(worldScale, worldScale, 1);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_back_material", shader, Color.White, 1, textureDictionary["skybox_back"])));
            level.Add(clone);

            //left
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_left";
            clone.Transform.Translate(-worldScale / 2.0f, 0, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, 90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_left_material", shader, Color.White, 1, textureDictionary["skybox_left"])));
            level.Add(clone);

            //right
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_right";
            clone.Transform.Translate(worldScale / 2.0f, 0, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, -90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_right_material", shader, Color.White, 1, textureDictionary["skybox_right"])));
            level.Add(clone);

            //front
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_front";
            clone.Transform.Translate(0, 0, worldScale / 2.0f);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, -180, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_front_material", shader, Color.White, 1, textureDictionary["skybox_front"])));
            level.Add(clone);

            //top
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_sky";
            clone.Transform.Translate(0, worldScale / 2.0f, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(90, -90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_sky_material", shader, Color.White, 1, textureDictionary["skybox_sky"])));
            level.Add(clone);
        }


        private void InitializeIsometricCamera(Scene level)
        {
            #region Curve Camera - Non Collidable

            //add curve for camera translation
            var translationCurve = new Curve3D(CurveLoopType.Cycle);
            translationCurve.Add(new Vector3(0, 15, 30), 0);
            translationCurve.Add(new Vector3(0, 20, 0), 6000);
            translationCurve.Add(new Vector3(0, 30, -20), 10500);
            translationCurve.Add(new Vector3(0, 30, -30), 13000);
            //translationCurve.Add(new Vector3(0, 4, 25), 4000);
            //translationCurve.Add(new Vector3(0, 2, 10), 6000);

            //add camera game object
            var curveCamera = new GameObject(AppData.CAMERA_CURVE_NONCOLLIDABLE_NAME, GameObjectType.Camera);

            //add components
            curveCamera.Transform.SetRotation(new Vector3(-35f, 0f, 0f));
            curveCamera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            curveCamera.AddComponent(new CurveBehaviour(translationCurve));
            curveCamera.AddComponent(new FOVOnScrollController(MathHelper.ToRadians(2)));

            //add to level
            level.Add(curveCamera);
            level.SetMainCamera(AppData.CAMERA_CURVE_NONCOLLIDABLE_NAME);

            #endregion Curve Camera - Non Collidable
        }

        /// <summary>
        /// Initialize the camera(s) in our scene
        /// </summary>
        /// <param name="level"></param>
        private void InitializeCameras(Scene level)
        {
            #region Camera 1

            //add camera game object
            var camera = new GameObject(AppData.CAMERA_1, GameObjectType.Camera);

            //add components
            //here is where we can set a smaller viewport e.g. for split screen
            //e.g. new Viewport(0, 0, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight)
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            //add controller to actually move the noncollidable camera
            //camera.AddComponent(new FirstPersonController(0.05f, 0.025f, new Vector2(0.006f, 0.004f)));

            //set initial position
            //camera.Transform.SetTranslation(40, 10, 0);
            //camera.Transform.SetRotation(0, 90, 0);
            camera.Transform.SetTranslation(40, 10, 0);
            camera.Transform.SetRotation(0, 90, 0);



            //add to level
            level.Add(camera);

            #endregion Camera 1

            #region Camera 2

            camera = new GameObject(AppData.CAMERA_2, GameObjectType.Camera);

            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            camera.Transform.SetTranslation(40, 60, -30);
            camera.Transform.SetRotation(20f, 90f, 75f);

            level.Add(camera);

            #endregion Camera 2

            #region Camera 3

            camera = new GameObject(AppData.CAMERA_3, GameObjectType.Camera);

            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            camera.Transform.SetTranslation(0, 0, -15);
            camera.Transform.SetRotation(0, 0, 90);

            level.Add(camera);

            #endregion Camera 3

            #region Camera 4

            camera = new GameObject(AppData.CAMERA_4, GameObjectType.Camera);

            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            camera.Transform.SetTranslation(0, 15, 30);
            camera.Transform.SetRotation(-35f, 0f, 0f);

            level.Add(camera);

            #endregion Camera 4


            #region First Person Camera - Collidable

            //add camera game object
            camera = new GameObject(AppData.CAMERA_FIRSTPERSON_COLLIDABLE_NAME, GameObjectType.Camera);

            //set initial position - important to set before the collider as collider capsule feeds off this position
            camera.Transform.SetTranslation(0, 5, 10);
            //camera.Transform.SetTranslation(0, 10, 10);

            //add components
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            //adding a collidable surface that enables acceleration, jumping
            var collider = new CharacterCollider(2, 1.75f, true, false);

            camera.AddComponent(collider);
            //collider.AddPrimitive(new Capsule(camera.Transform.LocalTranslation,
            //collider.AddPrimitive(new Capsule(new Vector3(0, 5, 10),
            //Matrix.CreateRotationX(MathHelper.PiOver2), 1, 3.6f),
            //new MaterialProperties(0.2f, 0.8f, 0.7f));

            //Player
            collider.AddPrimitive(new Box(new Vector3(0, 5, 10), new Vector3(0, 0, 0), 
                new Vector3(2, 6, 2)),
              new MaterialProperties(0.2f, 0.8f, 0.7f));

            //var player = new Box(new Vector3(0, 5, 10), new Vector3(0, 0, 0),
                //new Vector3(1, 3, 1));
            

            collider.Enable(false, 2);

            //add controller to actually move the collidable camera
            camera.AddComponent(new MyCollidableFirstPersonController(12,
                       0.5f, 0.3f, new Vector2(0.006f, 0.004f)));

            //add to level
            level.Add(camera);

            #endregion First Person Camera - Collidable

            //set the main camera, if we dont call this then the first camera added will be the Main
            level.SetMainCamera(AppData.CAMERA_4);

            //allows us to scale time on all game objects that based movement on Time
            // Time.Instance.TimeScale = 0.1f;
        }

        /******************************* Collidables *******************************/

        /// <summary>
        /// Demo of the new physics manager and collidable objects
        /// </summary>
        private void InitializeCollidables(Scene level, float worldScale = 500)
        {
            InitializeCollidableGround(level, worldScale);
            InitializeCollidableLava(level, worldScale);
            InitializePlatforms(level);
            
            
            //InitializeCollidableCubes(level);

            //InitializeCollidableTriangleMeshes(level);
        }

        private void InitializeCollidableLava(Scene level, float worldScale)
        {
            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new CubeMesh();


            //create the lava floor
            var lavaFloor = new GameObject("lava floor", GameObjectType.Lava, true);
            //ground.Transform.Rotate(0, 0, 0);
            lavaFloor.Transform.SetTranslation(0, -50, 0);
            lavaFloor.Transform.SetScale(new Vector3(worldScale, 1, worldScale));
            lavaFloor.AddComponent(new MeshRenderer(mesh, new BasicMaterial("lava_material", shader, Color.White, 1, textureDictionary["lava"])));

            //add Collision Surface(s)
            //collider = new Collider();
            collider = new MyPlayerCollider();
            lavaFloor.AddComponent(collider);
            collider.AddPrimitive(new Box(
                    lavaFloor.Transform.LocalTranslation,
                    lavaFloor.Transform.LocalRotation,
                    lavaFloor.Transform.LocalScale),
                    new MaterialProperties(0f, 3f, 3f));
            collider.Enable(true, 1);

            //add To Scene Manager
            level.Add(lavaFloor);
        }

        private void InitializePlatforms(Scene level)
        {
            InitializeCollidableTetrahedron(level, new Vector3(0, 0, -10));
            InitializeCollidableTetrahedron(level, new Vector3(6, 0, -28));
            InitializeCollidableTetrahedron(level, new Vector3(-6, 0, -37), 8f);

            //InitializeCollidableHexagon(level, new Vector3(10, 3, 10));
            InitializeCollidableTrapezium(level, new Vector3(-0, 3, -65));
            //InitializeCollidableTrapezium(level, new Vector3(-0, 2, -0));
            
        }

        private void InitializePlatformsLevel2(Scene level)
        {
            InitializeCollidableTetrahedron(level, new Vector3(10, 0, -10));
            InitializeCollidableTetrahedron(level, new Vector3(-10, 0, -10));
            InitializeCollidableTetrahedron(level, new Vector3(0, 0, -37), 8f);
            InitializeCollidableTetrahedron(level, new Vector3(0, 10, -58));
            InitializeCollidableTetrahedron(level, new Vector3(12, 6, -71));

            
            InitializeCollidableTrapezium(level, new Vector3(-0, -3, -85));
            //InitializeCollidableTrapezium(level, new Vector3(-0, 2, -0));

        }

        private void InitializeCollidableTrapezium(Scene level, Vector3 translation)
        {

            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new Trapezium();

            var platform = new GameObject("trapezium", GameObjectType.Architecture, true);
            platform.Transform.Rotate(0, 0, 0);
            platform.Transform.SetTranslation(translation);
            platform.Transform.SetScale(8, 1, 13);
            platform.AddComponent(new MeshRenderer(mesh, new BasicMaterial("platform_material", shader, Color.White, 1, textureDictionary["platform"])));

            //platform.AddComponent(new PickupBehaviour("platform", 0));
            //EventDispatcher.Raise(new EventData(EventCategoryType.))

            //add Collision Surface(s)
            collider = new MyPlayerCollider2();
            platform.AddComponent(collider);
            collider.AddPrimitive(new Box(
                    new Vector3(-6f, -0.6f, -1.5f),
                    new Vector3(0f, 0f, 0f),
                   new Vector3(11f, 1.25f, 8f)),
                    new MaterialProperties(0.8f, 0.8f, 0.7f));
            collider.Enable(true, 0);

            //add To Scene Manager
            level.Add(platform);
        }

        private void InitializeCollidableHexagon(Scene level, Vector3 translation)
        {
            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new HexagonMesh();

            var platform = new GameObject("hexagon", GameObjectType.Architecture, true);
            platform.Transform.Rotate(0, 0, 0);
            platform.Transform.SetTranslation(translation);
            platform.Transform.SetScale(5, 5, 5);
            platform.AddComponent(new MeshRenderer(mesh, new BasicMaterial("platform_material", shader, Color.White, 1, textureDictionary["platform"])));

            //add Collision Surface(s)
            //collider = new Collider();
            //platform.AddComponent(collider);
            //collider.AddPrimitive(new Box(
            //        new Vector3(-3.5f, 0, -3.5f),
            //        platform.Transform.LocalRotation,
            //        platform.Transform.LocalScale),
            //        new MaterialProperties(0.8f, 0.8f, 0.7f));
            //collider.Enable(true, 0);

            //add To Scene Manager
            level.Add(platform);
        }

        private void InitializeCollidableTetrahedron(Scene level, Vector3 translation)
        {
            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new Tetrahedron();

            var platform = new GameObject("tetrahedron", GameObjectType.Architecture, true);
            platform.Transform.Rotate(180, 90, 0);
            platform.Transform.SetTranslation(translation);
            platform.Transform.SetScale(7, 7, 7);
            platform.AddComponent(new MeshRenderer(mesh, new BasicMaterial("platform_material", shader, Color.White, 1, textureDictionary["platform"])));

            //add Collision Surface(s)
            collider = new Collider();
            platform.AddComponent(collider);
            collider.AddPrimitive(new Box(
                    new Vector3(-3.5f, 0, -3.5f),
                    platform.Transform.LocalRotation,
                    platform.Transform.LocalScale),
                    new MaterialProperties(0.8f, 0.8f, 0.7f));
            collider.Enable(true, 0);

            //add To Scene Manager
            level.Add(platform);
        }

        private void InitializeCollidableTetrahedron(Scene level, Vector3 translation, float elasticty)
        {
            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new Tetrahedron();

            var platform = new GameObject("tetrahedron", GameObjectType.Architecture, true);
            platform.Transform.Rotate(180, 90, 0);
            platform.Transform.SetTranslation(translation);
            platform.Transform.SetScale(7, 7, 7);
            platform.AddComponent(new MeshRenderer(mesh, new BasicMaterial("platform_material", shader, Color.White, 1, textureDictionary["platform_bounce"])));

            //add Collision Surface(s)
            collider = new Collider();
            platform.AddComponent(collider);
            collider.AddPrimitive(new Box(
                    new Vector3(-3.5f, 0, -3.5f),
                    platform.Transform.LocalRotation,
                    platform.Transform.LocalScale),
                    new MaterialProperties(elasticty, 0.8f, 0.7f));
            collider.Enable(true, 0);

            //add To Scene Manager
            level.Add(platform);
        }

        

        private void InitializeCollidableTriangleMeshes(Scene level)
        {/*
            BasicEffect basicEffect = new BasicEffect(GraphicsDevice);

            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            vertices[0] = new VertexPositionColor(new Vector3(0.5f, 0, 0), Color.Blue);
            vertices[0] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Green);

            VertexBuffer vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);*/
           
            
           SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);

           BasicEffect basicEffect = new BasicEffect(GraphicsDevice);

            VertexPositionColor[] vertices = new VertexPositionColor[12];
            vertices[0] = new VertexPositionColor(new Vector3(0.000f, 1.000f, 0.000f), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(-0.816f, -0.333f, -0.471f), Color.Blue);
            vertices[2] = new VertexPositionColor(new Vector3(0.000f, -0.333f, 0.943f), Color.Green);
            vertices[3] = new VertexPositionColor(new Vector3(0.000f, 1.000f, 0.000f), Color.Red);
            vertices[4] = new VertexPositionColor(new Vector3(0.816f, -0.333f, -0.471f), Color.Yellow);
            vertices[5] = new VertexPositionColor(new Vector3(-0.816f, -0.333f, -0.471f), Color.Blue);
            vertices[6] = new VertexPositionColor(new Vector3(0.000f, -0.333f, 0.943f), Color.Green);
            vertices[7] = new VertexPositionColor(new Vector3(0.816f, -0.333f, -0.471f), Color.Yellow);
            vertices[8] = new VertexPositionColor(new Vector3(0.000f, 1.000f, 0.000f), Color.Red);
            vertices[9] = new VertexPositionColor(new Vector3(-0.816f, -0.333f, -0.471f), Color.Blue);
            vertices[10] = new VertexPositionColor(new Vector3(0.816f, -0.333f, -0.471f), Color.Yellow);
            vertices[11] = new VertexPositionColor(new Vector3(0.000f, -0.333f, 0.943f), Color.Green);

            VertexBuffer vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            

        }


        private void InitializeCollidableGround(Scene level, float worldScale)
        {
            #region Reusable - You can copy and re-use this code elsewhere, if required

            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the vertices and indices of the model
            var mesh = new CubeMesh();

            #endregion Reusable - You can copy and re-use this code elsewhere, if required

            //create the ground
            var ground = new GameObject("ground", GameObjectType.Architecture, true);
            //ground.Transform.Rotate(0, 0, 0);
            ground.Transform.SetTranslation(0, -1, 10);
            ground.Transform.SetScale(20, 2, 20);
            ground.AddComponent(new MeshRenderer(mesh, new BasicMaterial("platform_material", shader, Color.White, 1, textureDictionary["platform"])));

            //add Collision Surface(s)
            collider = new Collider();
            ground.AddComponent(collider);
            collider.AddPrimitive(new Box(
                    ground.Transform.LocalTranslation,
                    ground.Transform.LocalRotation,
                    ground.Transform.LocalScale),
                    new MaterialProperties(0.8f, 0.8f, 0.7f));
            collider.Enable(true, 1);

            //add To Scene Manager
            level.Add(ground);
        }

        private void InitializeCollidableCubes(Scene level)
        {
            #region Reusable - You can copy and re-use this code elsewhere, if required

            //re-use the code on the gfx card, if we want to draw multiple objects using Clone
            var shader = new BasicShader(Application.Content, false, true);
            //re-use the mesh
            var mesh = new CubeMesh();
            //clone the cube
            var cube = new GameObject("cube", GameObjectType.Consumable, false);

            #endregion Reusable - You can copy and re-use this code elsewhere, if required

            GameObject clone = null;

            for (int i = 0; i < 5; i++)
            {
                //clone the archetypal cube
                clone = cube.Clone() as GameObject;
                clone.Transform.SetRotation(0, 45, 0);
                clone.Transform.SetScale(1, 1, 1);
                clone.Name = $"cube - {i}";
                clone.Transform.Translate(5, 4f * (1 + i), 0);
                clone.AddComponent(new MeshRenderer(mesh,
                    new BasicMaterial("cube_material", shader,
                    Color.White, 0.4f, textureDictionary["crate1"])));

                //add desc and value to a pickup used when we collect/remove/collide with it
                clone.AddComponent(new PickupBehaviour("ammo pack", 15));

                //add demo alpha change behaviour
                clone.AddComponent(new ColorLerpBehaviour(Color.White, Color.Red, 0.1f));

                //add Collision Surface(s)
                collider = new MyPlayerCollider();

                clone.AddComponent(collider);
                collider.AddPrimitive(new Box(
                    clone.Transform.LocalTranslation,
                    clone.Transform.LocalRotation,
                    clone.Transform.LocalScale * 1.01f), //make the colliders a fraction larger so that transparent boxes dont sit exactly on the ground and we end up with flicker or z-fighting
                    new MaterialProperties(0.8f, 0.8f, 0.7f));
                collider.Enable(false, 10);
                //add To Scene Manager
                level.Add(clone);
            }
        }

        #endregion Student/Group Specific Code

        /******************************* Demo (Remove For Release) *******************************/

        #region Demo Code

#if DEMO

        public delegate void MyDelegate(string s, bool b);

        public List<MyDelegate> delList = new List<MyDelegate>();

        public void DoSomething(string msg, bool enableIt)
        {
        }

        private void InitializeEditorHelpers()
        {
            //a game object to record camera positions to an XML file for use in a curve later
            var curveRecorder = new GameObject("curve recorder", GameObjectType.Editor);
            curveRecorder.AddComponent(new GDLibrary.Editor.CurveRecorderController());
            activeScene.Add(curveRecorder);
        }

        private void RunDemos()
        {
            // CurveDemo();
            // SaveLoadDemo();

            EventSenderDemo();
        }

        private void EventSenderDemo()
        {
            var myDel = new MyDelegate(DoSomething);
            myDel("sdfsdfdf", true);
            delList.Add(DoSomething);
        }

        private void CurveDemo()
        {
            //var curve1D = new GDLibrary.Parameters.Curve1D(CurveLoopType.Cycle);
            //curve1D.Add(0, 0);
            //curve1D.Add(10, 1000);
            //curve1D.Add(20, 2000);
            //curve1D.Add(40, 4000);
            //curve1D.Add(60, 6000);
            //var value = curve1D.Evaluate(500, 2);
        }

        private void SaveLoadDemo()
        {
        #region Serialization Single Object Demo

            var demoSaveLoad = new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f));
            GDLibrary.Utilities.SerializationUtility.Save("DemoSingle.xml", demoSaveLoad);
            var readSingle = GDLibrary.Utilities.SerializationUtility.Load("DemoSingle.xml",
                typeof(DemoSaveLoad)) as DemoSaveLoad;

        #endregion Serialization Single Object Demo

        #region Serialization List Objects Demo

            List<DemoSaveLoad> listDemos = new List<DemoSaveLoad>();
            listDemos.Add(new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(10, 20, 30), new Vector3(4, 9, -18), new Vector3(15f, 1f, 202.5f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(100, 200, 300), new Vector3(145, 290, -80), new Vector3(6.5f, 1.1f, 8.05f)));

            GDLibrary.Utilities.SerializationUtility.Save("ListDemo.xml", listDemos);
            var readList = GDLibrary.Utilities.SerializationUtility.Load("ListDemo.xml",
                typeof(List<DemoSaveLoad>)) as List<DemoSaveLoad>;

        #endregion Serialization List Objects Demo
        }

#endif

        #endregion Demo Code
    }
}