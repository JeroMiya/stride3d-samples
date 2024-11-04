namespace SandboxSceneBrowser;

public class MoveTowardsPlayerSlowly : SyncScript
{
    Entity mPlayerEntity;
    RigidbodyComponent mRigidBody;

    public float FollowForce { get; set; } = 5f;

    public override void Start()
    {
        mPlayerEntity = Entity.Scene.Entities.Where(e => e.Name == "FirstPersonCharacter")
            .FirstOrDefault() ?? throw new InvalidOperationException("Couldn't find player entity");
        mRigidBody = Entity.Get<RigidbodyComponent>()
            ?? throw new InvalidOperationException("Missing required RigidBodyComponent");
    }

    public override void Update()
    {
        var direction = mPlayerEntity.GetWorldPosition() - Entity.GetWorldPosition();
        direction.Normalize();
        mRigidBody.ApplyForce(direction * FollowForce);
    }
}
