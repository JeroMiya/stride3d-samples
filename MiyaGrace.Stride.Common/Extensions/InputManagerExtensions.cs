namespace MiyaGrace.Stride.Common.Extensions;

/// <summary>
/// NOTE: These extensions really belong in the game project as they'll 
/// be defined in, as likely they will vary from game to game (possibly even
/// scene to scene).
/// </summary>
public static class InputManagerVirtualButtonExtensions
{
    private const float DeadZoneAmt = 0.1f;
    private static float DeadZone(float value)
    {
        if (value > -DeadZoneAmt && value < DeadZoneAmt)
        {
            return 0.0f;
        }
        return value;
    }

    public static float GetVerticalAxisValue(this InputManager input)
        => DeadZone(input.GetVirtualButtonValue(VirtualButtonSetup.MainConfigIndex, VirtualButtonSetup.VerticalAxis));

    public static float GetHorizontalAxisValue(this InputManager input)
        => DeadZone(input.GetVirtualButtonValue(VirtualButtonSetup.MainConfigIndex, VirtualButtonSetup.HorizontalAxis));

    public static float GetThrustAxisValue(this InputManager input)
        => DeadZone(input.GetVirtualButtonValue(VirtualButtonSetup.MainConfigIndex, VirtualButtonSetup.ThrustAxis));

    public static float GetRudderAxisValue(this InputManager input)
        => DeadZone(input.GetVirtualButtonValue(VirtualButtonSetup.MainConfigIndex, VirtualButtonSetup.RudderAxis));
}
