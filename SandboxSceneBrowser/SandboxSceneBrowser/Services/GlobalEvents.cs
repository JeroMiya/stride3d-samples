namespace SandboxSceneBrowser.Services;

public class GlobalEvents
{
    public EventKey ReturnToMainMenu { get; }
        = new(category: "Global", eventName: "Return to Main Menu");

    public EventKey GameOver { get; }
        = new(category: "Global", eventName: "Game Over");

    public EventKey ReloadCurrentLevel { get; }
        = new(category: "Global", eventName: "Reload Current Level");

    public EventKey LoadLevel1 { get; set; }
        = new(category: "Global", eventName: "Load Level 1");
}
