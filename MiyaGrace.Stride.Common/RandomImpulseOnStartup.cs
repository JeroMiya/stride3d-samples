namespace MiyaGrace.Stride.Common;

/// <summary>
/// If the attached entity has a RigidbodyComponent, this script will
/// apply a random impulse in a random direction on startup.
/// </summary>
public class RandomImpulseOnStartup : StartupScript
{
    /// <summary>
    /// The minimum impulse force to apply on startup. The applied
    /// force will be of a random magnitude between <see cref="MinimumImpulseForce" />
    /// and <see cref="MaximumImpulseForce" />.
    /// </summary>
    public float MinimumImpulseForce { get; set; } = 0.0f;

    /// <summary>
    /// The maximum impulse force to apply on startup. The applied
    /// force will be of a random magnitude between <see cref="MinimumImpulseForce" />
    /// and <see cref="MaximumImpulseForce" />.
    /// </summary>
    public float MaximumImpulseForce { get; set; } = 500f;

    public override void Start()
    {
        var rb = Entity.Get<RigidbodyComponent>();
        if(rb != null)
        {
            var magnitude = MaximumImpulseForce - MinimumImpulseForce;

            // this math may be cursed
            var randomDirection = new Vector3(
                Random.Shared.NextSingle() * 2 - 1,
                Random.Shared.NextSingle() * 2 - 1,
                Random.Shared.NextSingle() * 2 - 1);
            randomDirection.Normalize();
            randomDirection *= Random.Shared.NextSingle() * magnitude + MinimumImpulseForce;
            rb.ApplyImpulse(randomDirection);
        }
    }
}
