namespace SandboxSceneBrowser;

public class SceneEntryPoint : SyncScript
{
    public UrlReference<Scene> MenuScene { get; set; }
    public UrlReference<Scene> PauseMenuScene { get; set; }
    public UrlReference<Scene> Level1Scene { get; set; }
    public CameraComponent Camera { get; set; }

    private UrlReference<Scene> mLastLevel { get; set; }

    private Scene mMenuSceneInstance;
    private Scene mCurrentLevelSceneInstance;
    private Scene mPauseMenuSceneInstance;

    private EventReceiver mLoadLevel1EventReceiver;
    private EventReceiver mReturnToMainMenuReceiver;
    private EventReceiver mGameOverEventReceiver;
    private EventReceiver mReloadCurrentLevelEventReceiver;

    private GlobalEvents mGlobalEvents;
    private bool mPaused = false;

    public override void Start()
    {
        SetupServices();

        mLoadLevel1EventReceiver = new EventReceiver(mGlobalEvents.LoadLevel1);
        mGameOverEventReceiver = new EventReceiver(mGlobalEvents.GameOver);
        mReturnToMainMenuReceiver = new EventReceiver(mGlobalEvents.ReturnToMainMenu);
        mReloadCurrentLevelEventReceiver = new EventReceiver(mGlobalEvents.ReloadCurrentLevel);

        mPauseMenuSceneInstance = Content.Load(PauseMenuScene);
        mMenuSceneInstance = Content.Load(MenuScene);
        mMenuSceneInstance.Parent = Entity.Scene;
    }

    public override void Update()
    {
        // this logic is really bad and needs to be rewritten


        if (mCurrentLevelSceneInstance == null
            && mMenuSceneInstance != null
            && mLoadLevel1EventReceiver.TryReceive())
        {
            LockMouse();
            mLastLevel = Level1Scene;
            mMenuSceneInstance.Parent = null;
            mCurrentLevelSceneInstance = LoadLevel(Level1Scene);
        }

        if (mCurrentLevelSceneInstance != null && mReturnToMainMenuReceiver.TryReceive())
        {
            UnlockMouse();
            Unpause();
            UnloadLevel(ref mCurrentLevelSceneInstance);
            Camera.Enabled = true;
            mMenuSceneInstance.Parent = Entity.Scene;
        }

        if (mLastLevel != null
            && mCurrentLevelSceneInstance != null
            && mReloadCurrentLevelEventReceiver.TryReceive())
        {
            Unpause();
            LockMouse();
            UnloadLevel(ref mCurrentLevelSceneInstance);
            mCurrentLevelSceneInstance = LoadLevel(mLastLevel);
        }

        if (mCurrentLevelSceneInstance != null && Input.IsKeyPressed(Keys.Escape))
        {
            if (mPaused)
            {
                LockMouse();
                Unpause();
            }
            else
            {
                UnlockMouse();
                Pause();
            }
        }
    }

    private void Pause()
    {
        if (mPaused) return;
        mPaused = true;
        mPauseMenuSceneInstance.Parent = Entity.Scene;
    }

    private void Unpause()
    {
        if (!mPaused) return;
        mPaused = false;
        mPauseMenuSceneInstance.Parent = null;
    }

    private void LockMouse()
    {
        Input.LockMousePosition();
        Input.MousePosition = new Vector2(0.5f, 0.5f);
        Game.IsMouseVisible = false;
    }

    private void UnlockMouse()
    {
        Input.UnlockMousePosition();
        Game.IsMouseVisible = true;
    }

    private Scene LoadLevel(UrlReference<Scene> sceneUrlReference)
    {
        var scene = Content.Load(sceneUrlReference);
        Camera.Enabled = false;
        scene.Parent = Entity.Scene;
        return scene;
    }

    private void UnloadLevel(ref Scene scene)
    {
        scene.Parent = null;
        Camera.Enabled = true;
        Content.Unload(scene);
        scene = null;
    }

    private void SetupServices()
    {
        mGlobalEvents = new();
        Services.AddService(mGlobalEvents);
    }

}
