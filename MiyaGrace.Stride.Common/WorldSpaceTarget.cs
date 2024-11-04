namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script was my first attempt at a world-space target
/// indicator. It doesn't yet support rotating the target to face
/// the camera so it pretty much only supports symetrical models.
/// Consider this experimental.
/// </summary>
public partial class WorldSpaceTarget : SyncScript
{
    /// <summary>
    /// The entity that will be moved into position
    /// </summary>
    public required Entity SourceEntity { get; set; }


    /// <summary>
    /// The maximum distance to collide with the environment. If no
    /// collisions are detected within this range, the target will be
    /// placed at this max distance from the camera.
    /// </summary>
    public float MaxDistance { get; set; } = 5f;

    /// <summary>
    /// If a collision is detected, the object will be placed in the
    /// world at the collision point, but adjusted towards the camera
    /// by this distance. Use to keep the model out of walls etc...
    /// </summary>
    public float HitAdjustmentDistance { get; set; } = 0.2f;

    /// <summary>
    /// Set this to the collision groups you want the raycast to use
    /// when detecting a collision in the environment.
    /// </summary>
    public CollisionFilterGroupFlags CollideWithGroup { get; set; }

    /// <summary>
    /// Defaults to false - set to true to have the collision test also
    /// collide with trigger volumes.
    /// </summary>
    public bool CollideWithTriggers { get; set; } = false;

    private Simulation simulation = null!;

    public override void Start()
    {
        if (SourceEntity == null) { throw new InvalidOperationException("SourceEntity is required to be set"); }

        simulation = this.GetSimulation()
            ?? throw new InvalidOperationException(
                "Couldn't get simulation - is there a physics component attached?");
    }

    public override void Update()
    {
        SourceEntity.Transform.UpdateWorldMatrix();

        var raycastStart = SourceEntity.Transform.WorldMatrix.TranslationVector;
        var direction = SourceEntity.Transform.WorldMatrix.Forward;
        direction.Normalize();
        var raycastEnd = raycastStart + direction * MaxDistance;

        int drawX = 340;
        int drawY = 80;

        // Send a raycast from the start to the endposition
        if (simulation.Raycast(raycastStart, raycastEnd, out HitResult hitResult, CollisionFilterGroups.DefaultFilter, CollideWithGroup, CollideWithTriggers))
        {
            // If we hit something, calculate the distance to the hitpoint and scale the laser to that distance
            var towardsSource = SourceEntity.Transform.WorldMatrix.TranslationVector
                - hitResult.Point;
            towardsSource.Normalize();
            towardsSource *= HitAdjustmentDistance;

            Entity.Transform.Position = hitResult.Point + towardsSource;
            var distance = Vector3.Distance(hitResult.Point, raycastStart);

            // TODO: need to manage debug text in a better way globally, maybe a service that
            // manages enabled/disabled state of debug text?
            DebugText.Print("Hit a collider", new Int2(drawX, drawY));
            DebugText.Print($"Raycast hit distance: {distance}", new Int2(drawX, drawY + 20));
            DebugText.Print($"Raycast hit entity: {hitResult.Collider.Entity.Name}", new Int2(drawX, drawY + 60));
        }
        else
        {
            Entity.Transform.Position = raycastEnd;
            DebugText.Print("No collider hit", new Int2(drawX, drawY));
        }
    }
}
