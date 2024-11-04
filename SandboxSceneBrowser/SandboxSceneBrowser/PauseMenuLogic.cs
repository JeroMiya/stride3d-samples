namespace SandboxSceneBrowser;

public class PauseMenuLogic : SyncScript
{
    private Button mReloadButton;
    private Button mReturnToMainMenuButton;
    private UIComponent mPauseMenuComponent;
    private UIPage mPauseMenuPage;
    private GlobalEvents mGlobalEvents;

    public override void Start()
    {
        mGlobalEvents = Services.GetService<GlobalEvents>();
        mPauseMenuComponent = Entity.Get<UIComponent>()
            ?? throw new InvalidOperationException("Couldn't find UIComponent");
        mPauseMenuPage = mPauseMenuComponent.Page;
        mReloadButton = mPauseMenuPage.RootElement.FindVisualChildOfType<Button>("ReloadButton");
        mReturnToMainMenuButton = mPauseMenuPage.RootElement.FindVisualChildOfType<Button>("ReturnToMainMenuButton");

        mReloadButton.Click += MReloadButton_Click;
        mReturnToMainMenuButton.Click += MReturnToMainMenuButton_Click;
    }

    private void MReturnToMainMenuButton_Click(object sender, Stride.UI.Events.RoutedEventArgs e)
    {
        mGlobalEvents.ReturnToMainMenu.Broadcast();
    }

    private void MReloadButton_Click(object sender, Stride.UI.Events.RoutedEventArgs e)
    {
        mGlobalEvents.ReloadCurrentLevel.Broadcast();
    }

    public override void Update()
    {

    }
}
