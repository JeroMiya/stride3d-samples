using MiyaGrace.Stride.Common.Services;

namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script uses the PlayerEntityService to find
/// the player entity and rotate the attached entity
/// to point towards the player. The rotation that is
/// calculated uses the model convention of +Z forward.
/// Uses +Y as UP if Entity has no parent, otherwise it
/// will use the parent's world UP vector as the UP direction.
/// </summary>
public class LookTowardsPlayer : SyncScript
{
    private PlayerEntityService? mPlayerEntityService;

    /// <summary>
    /// Turn speed in terms of amount of ratio from current rotation
    /// to desired rotation each update. NOTE: Currently not
    /// frame-rate independent - likely buggy.
    /// </summary>
    public float TurnSpeed { get; set; } = 0.5f;

    public override void Start()
    {
        Game.Services.GetServiceLate<PlayerEntityService>(
            s => mPlayerEntityService = s);
    }

    public override void Update()
    {
        if (mPlayerEntityService == null) { return; }
        var playerEntity = mPlayerEntityService.PlayerEntity;
        if (playerEntity == null) { return; }

        var currentForward = Entity.GetModelWorldForward();
        var desiredForward = playerEntity.GetWorldPosition() - Entity.GetWorldPosition();
        desiredForward.Normalize();

        // TODO: this math is a bit cursed - and doesn't take frame rate into account.
        // But it was unstable when I tried to make it frame rate independent. Need
        // to revisit this and make it stable and frame rate independent.
        var lerpedForward = Vector3.Lerp(currentForward, desiredForward, TurnSpeed);
        var parent = Entity.GetParent();
        if(parent != null)
        {
            Entity.ModelLookAtWorld(lerpedForward, parent.Transform.WorldMatrix.Up);
        }
        else
        {
            Entity.ModelLookAtWorld(lerpedForward, Vector3.UnitY);
        }
    }
}
