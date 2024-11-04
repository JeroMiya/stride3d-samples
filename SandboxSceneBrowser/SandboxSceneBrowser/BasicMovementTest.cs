namespace SandboxSceneBrowser;

public class BasicMovementTest : SyncScript
{
    // Declared public member fields and properties will show in the game studio

    public override void Start()
    {
        // Initialization of the script.
    }

    public override void Update()
    {
        Entity.Transform.Position += Vector3.UnitZ * 0.1f * (float)Game.UpdateTime.Elapsed.TotalSeconds;
    }
}
