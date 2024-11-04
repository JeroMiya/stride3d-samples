namespace MiyaGrace.Stride.Common;

/// <summary>
/// Just a simple script that spawns a Prefab at the entity's location
/// when the user presses a key. Doesn't work with a virtual button setup
/// yet, so somewhat limited.
/// </summary>
public class FireProjectileController : SyncScript
{
    /// <summary>
    /// A sound to play when the projectile is fired. Requires an
    /// AudioEmitterComponent with an AudioEmitterSoundController with
    /// this name.
    /// </summary>
    public required string ProjectileSoundControllerKey { get; set; }

    /// <summary>
    /// The Prefab to spawn when the projectile is "fired"
    /// </summary>
    public required Prefab ProjectilePrefab { get; set; }

    /// <summary>
    /// The button that should trigger a fire. User can hold
    /// the button down to fire continuously.
    /// </summary>
    public Keys FireButton { get; set; } = Keys.Space;

    /// <summary>
    /// The rate of fire in terms of the time between fires
    /// in seconds. Example: 2 per second = 0.5f;
    /// </summary>
    public double FireRate { get; set; } = 0.5f;

    private double mLastFire = 0f;
    private AudioEmitterComponent mAudioEmitter = null!;
    private AudioEmitterSoundController mLaserSoundController = null!;

    public override void Start()
    {
        if(string.IsNullOrEmpty(ProjectileSoundControllerKey))
        {
            throw new InvalidOperationException("ProjectileSoundControllerKey is not specified and is required");
        }

        mAudioEmitter = Entity.Get<AudioEmitterComponent>()
            ?? throw new InvalidOperationException("Couldn't find AudioEmitterComponent");
        mLaserSoundController = mAudioEmitter[ProjectileSoundControllerKey]
            ?? throw new InvalidOperationException("Couldn't find laser sound controller");

        if (ProjectilePrefab == null)
        {
            throw new InvalidOperationException("ProjectilePrefab not set on parent Entity");
        }
    }

    public override void Update()
    {
        var time = Game.UpdateTime.Total.TotalSeconds;
        if (Input.IsKeyDown(FireButton) && time - mLastFire > FireRate)
        {
            mLaserSoundController.PlayAndForget();
            mLastFire = time;
            ProjectilePrefab.InstantiateInSceneAtEntity(Entity);
        }
    }
}
