namespace MiyaGrace.Stride.Common;

/// <summary>
/// A simple experiment to handle game state, in this case
/// to allow an Entity to have "health" that can be damaged.
/// Tracks current health, handles damage dealing with the public
/// <see cref="DoDamage(float)" /> method, and has an event
/// (<see cref="Died" />) that fires when health reaches zero.
/// </summary>
public class HealthComponent : SyncScript
{
    /// <summary>
    /// Entity starts with this amount of health when this script
    /// starts to run.
    /// </summary>
    public float MaxHealth { get; set; } = 10.0f;

    /// <summary>
    /// Defense value - when damage is done, it is
    /// reduced by this amount. Damage cannot be reduced
    /// below zero.
    /// </summary>
    public float Defense { get; set; } = 0.0f;

    private float mCurrentHealth;

    public event EventHandler? Died;

    protected virtual void OnDied()
    {
        Died?.Invoke(this, EventArgs.Empty);
    }

    public void DoDamage(float damage)
    {
        damage = MathUtil.Clamp(damage - Defense, 0.0f, damage);
        var newHealth = mCurrentHealth - damage;
        mCurrentHealth = MathUtil.Clamp(newHealth, 0.0f, MaxHealth);
        if (newHealth < 0.0f) { OnDied(); }
    }

    public override void Start()
    {
        mCurrentHealth = MaxHealth;
    }

    public override void Update()
    {
    }
}
