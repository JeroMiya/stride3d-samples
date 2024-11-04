namespace MiyaGrace.Stride.Common.ProjectileScripts;

/// <summary>
/// Simple script to follow a target entity by applying
/// physics forces in the direction of the target.
/// </summary>
public class ProjectileFollowTargetWithPhysics : SyncScript
{
    /// <summary>
    /// The name of the target entity. Target entity must be
    /// in the top level Scene.
    /// TODO: need to refactor this to use a better entity configuration.
    /// </summary>
    public required string FollowTargetName { get; set; }

    /// <summary>
    /// The magnitude of the follow force towards the target.
    /// </summary>
    public float FollowForce { get; set; } = 5f;

    private Entity mFollowTarget = null!;
    private RigidbodyComponent mRigidBody = null!;

    public override void Start()
    {
        mRigidBody = Entity.Get<RigidbodyComponent>()
            ?? throw new InvalidOperationException("Could not find RigidBodyComponent");

        if (string.IsNullOrEmpty(FollowTargetName))
        {
            throw new InvalidOperationException("FollowTargetName is required");
        }

        mFollowTarget = Entity.Scene.Entities.FirstOrDefault(e => e.Name == FollowTargetName)
            ?? throw new InvalidOperationException($"Couldn't find FollowTarget with name {FollowTargetName}");
    }

    public override void Update()
    {
        var thisToTarget = mFollowTarget.Transform.WorldMatrix.TranslationVector
            - Entity.Transform.WorldMatrix.TranslationVector;
        thisToTarget.Normalize();
        thisToTarget *= FollowForce;
        mRigidBody.ApplyForce(thisToTarget);
    }
}
