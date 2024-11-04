namespace SandboxSceneBrowser;

public class MainMenuLogic : StartupScript
{
    private Button mLevel1Button;
    private UIComponent mMenuUiComponent;
    private UIPage mPage;
    private GlobalEvents mGlobalEvents;


    public override void Start()
    {
        mGlobalEvents = Services.GetService<GlobalEvents>();
        mMenuUiComponent = Entity.Get<UIComponent>()
            ?? throw new InvalidOperationException("Couldn't find UIComponent");
        mPage = mMenuUiComponent.Page;
        mLevel1Button = mPage.RootElement.FindVisualChildOfType<Button>("Level1Button");
        mLevel1Button.Click += MLevel1Button_Click;
    }

    private void MLevel1Button_Click(object sender, Stride.UI.Events.RoutedEventArgs e)
    {
        mGlobalEvents.LoadLevel1.Broadcast();
    }
}
