namespace SandboxSceneBrowser;

public class BasicLookAtTest : SyncScript
{
    public Entity Player { get; set; }

    public override void Start()
    {
        if (Player == null) { throw new InvalidOperationException("Player property not set in editor"); }
    }

    public override void Update()
    {
        Entity.ModelLookAt(Player);
    }
}
