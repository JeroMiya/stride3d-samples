namespace MiyaGrace.Stride.Common;

/// <summary>
/// My second attempt at a flight controller. Still very
/// broken and cursed. Do not use.
/// </summary>
public class BetterFlightController : SyncScript
{
    public float MaxThrust { get; set; } = 5000f;
    public float DragForce { get; set; } = 250f;
    public float Responsiveness { get; set; } = 200f;
    public float LiftCoefficient { get; set; } = 0.01f;

    RigidbodyComponent rb = null!;

    public override void Start()
    {
        base.Start();
        rb = Entity.Get<RigidbodyComponent>()
            ?? throw new InvalidOperationException("Could not find RigidbodyComponent on Entity");
    }

    public override void Update()
    {
        var deltaT = (float)Game.UpdateTime.Elapsed.TotalSeconds;

        var relativeForward = -Vector3.UnitZ;
        var relativeLeft = -Vector3.UnitX;
        var relativeUp = Vector3.UnitY;

        var horizontalAxis = Input.GetHorizontalAxisValue();
        var verticalAxis = Input.GetVerticalAxisValue();
        var thrustAxis = Input.GetThrustAxisValue();
        var rudderAxis = Input.GetRudderAxisValue();

        // Debug Text:

        // Apply thrust
        rb.ApplyRelativeForce(relativeForward * MaxThrust * thrustAxis * deltaT);

        // horizontal torque
        rb.ApplyRelativeTorque(relativeLeft * verticalAxis * Responsiveness * deltaT);

        // vertical torque
        rb.ApplyRelativeTorque(relativeForward * horizontalAxis * Responsiveness * deltaT);

        // rudder
        rb.ApplyRelativeTorque(relativeUp * -rudderAxis * Responsiveness * deltaT);

        // drag
        // review note:
        // I believe for arcade style controls, drag force needs to be
        // relative to the angle between linear velocity and the model's
        // forward vector, and also potentially adjusted based on flight controls
        rb.ApplyForce(rb.LinearVelocity * -DragForce * deltaT);

        // lift
        // review note:
        // this is not how lift force works, even for arcade style controls.
        // needs much more research
        //rb.ApplyForce(Vector3.UnitY * rb.LinearVelocity.Length() * LiftCoefficient * deltaT);
    }
}
