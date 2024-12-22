namespace MiyaGrace.Stride.Common.Extensions;

/// <summary>
/// Some potentially broken and cursed extension methods for physics.
/// Experimental and buggy - do not use.
/// </summary>
public static class BodyExtensions
{
    public static void ApplyRelativeForce(this BodyComponent rigidBody, Vector3 force, float deltaT)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyLinearImpulse(rigidBody.Entity.GetWorldVectorFromRelative(force) * deltaT);
    }

    public static void ApplyRelativeImpulse(this BodyComponent rigidBody, Vector3 force)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyLinearImpulse(rigidBody.Entity.GetWorldVectorFromRelative(force));
    }

    public static void ApplyLinearImpulse(this BodyComponent rigidBody, Vector3 torque)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyAngularImpulse(rigidBody.Entity.GetWorldVectorFromRelative(torque));
    }

    public static void ApplyRelativeTorqueImpulse(this BodyComponent rigidBody, Vector3 torque)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyAngularImpulse(rigidBody.Entity.GetWorldVectorFromRelative(torque));
    }
}
