using MiyaGrace.Stride.Common.Services;

namespace MiyaGrace.Stride.Common;

/// <summary>
/// This script fires a projectile when the attached entity is
/// pointed towards the player (found via the <see cref="PlayerEntityService" />).
/// Uses a raycast to determine if the entity can "see" the player.
/// </summary>
public class FireProjectileWhenPointedAtPlayer : SyncScript
{
    /// <summary>
    /// Sound to play when the projectile is "fired". Requires
    /// an AudioEmitterComponent with an AudioEmitterSoundController
    /// with the given name.
    /// </summary>
    public required string ProjectileSoundEmitterControllerKey { get; set; }

    /// <summary>
    /// The Prefab to spawn.
    /// </summary>
    public required Prefab ProjectilePrefab { get; set; }

    /// <summary>
    /// The fire rate of the projectile, in terms of time between fires
    /// in seconds. Example: 2 fires per second = 0.5f;
    /// </summary>
    public double FireRate { get; set; } = 0.5f;

    /// <summary>
    /// The maximum distance from the Entity that it can "see" the player
    /// to fire.
    /// </summary>
    public float MaxDistance { get; set; } = 5f;

    /// <summary>
    /// The collision group filter for the raycast.
    /// </summary>
    public CollisionFilterGroupFlags CollideWithGroup { get; set; }

    /// <summary>
    /// Defaults to false - set to true to collide with trigger volumes.
    /// </summary>
    public bool CollideWithTriggers { get; set; } = false;


    private double mLastFire = 0f;
    private Simulation simulation = null!;
    private PlayerEntityService? mPlayerEntityService = null!;
    private AudioEmitterComponent mAudioEmitter = null!;
    private AudioEmitterSoundController mLaserSoundController = null!;


    public override void Start()
    {
        if(string.IsNullOrEmpty(ProjectileSoundEmitterControllerKey))
        {
            throw new InvalidOperationException("ProjectileSoundEmitterControllerKey not defined and is required");
        }
        
        mAudioEmitter = Entity.Get<AudioEmitterComponent>()
            ?? throw new InvalidOperationException("Could not find AudioEmitterComponent");
        mLaserSoundController = mAudioEmitter[ProjectileSoundEmitterControllerKey]
            ?? throw new InvalidOperationException("Could not find laser sound controller");

        Game.Services.GetServiceLate<PlayerEntityService>(
            service => mPlayerEntityService = service);

        if (ProjectilePrefab == null)
        {
            throw new InvalidOperationException("ProjectilePrefab not set on parent Entity");
        }

        simulation = this.GetSimulation()
            ?? throw new InvalidOperationException(
                "Couldn't get simulation - is there a physics component in the scene?");
    }

    private readonly List<HitResult> mHitResults = new();
    public override void Update()
    {
        if (mPlayerEntityService == null) { return; }
        var playerEntity = mPlayerEntityService.PlayerEntity;
        if (playerEntity == null) { return; }

        var time = Game.UpdateTime.Total.TotalSeconds;

        var raycastStart = Entity.GetWorldPosition();
        var direction = Entity.GetModelWorldForward();

        direction.Normalize();
        var raycastEnd = raycastStart + (direction * MaxDistance);

        mHitResults.Clear();

        simulation.RaycastPenetrating(
            from: raycastStart,
            to: raycastEnd,
            resultsOutput: mHitResults,
            filterGroup: CollisionFilterGroups.AllFilter,
            filterFlags: CollideWithGroup,
            hitTriggers: CollideWithTriggers);

        if (mHitResults.Count == 0) { return; }

        if (time - mLastFire > FireRate)
        {
            mLaserSoundController.PlayAndForget();
            mLastFire = time;
            ProjectilePrefab.InstantiateInSceneAtEntity(Entity);
        }
    }
}
