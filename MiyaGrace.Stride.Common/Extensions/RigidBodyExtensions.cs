namespace MiyaGrace.Stride.Common.Extensions;

/// <summary>
/// Some potentially broken and cursed extension methods for physics.
/// Experimental and buggy - do not use.
/// </summary>
public static class RibidBodyExtensions
{
    public static void ApplyRelativeForce(this RigidbodyComponent rigidBody, Vector3 force)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyForce(rigidBody.Entity.GetWorldVectorFromRelative(force));
    }

    public static void ApplyRelativeImpulse(this RigidbodyComponent rigidBody, Vector3 force)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyImpulse(rigidBody.Entity.GetWorldVectorFromRelative(force));
    }

    public static void ApplyRelativeTorque(this RigidbodyComponent rigidBody, Vector3 torque)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyTorque(rigidBody.Entity.GetWorldVectorFromRelative(torque));
    }

    public static void ApplyRelativeTorqueImpulse(this RigidbodyComponent rigidBody, Vector3 torque)
    {
        ArgumentNullException.ThrowIfNull(rigidBody);
        ArgumentNullException.ThrowIfNull(rigidBody.Entity);
        rigidBody.ApplyTorqueImpulse(rigidBody.Entity.GetWorldVectorFromRelative(torque));
    }
}
