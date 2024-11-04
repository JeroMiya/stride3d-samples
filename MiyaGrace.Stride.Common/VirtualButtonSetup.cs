namespace MiyaGrace.Stride.Common;

/// <summary>
/// Configures a basic global virtual button setup.
/// NOTE: this script should probably be defined in the
/// individual game projects it will apply to as it is likely
/// to be different from game to game (possibly even scene to scene),
/// rather than common code across multiple games.
/// TODO: refactor this to support some common scenarios broken down
/// by bindings, rather than a common script that sets up everything.
/// </summary>
public class VirtualButtonSetup : StartupScript
{
    public const int MainConfigIndex = 0;
    public const string VerticalAxis = "VerticalAxis";
    public const string HorizontalAxis = "HorizontalAxis";
    public const string RudderAxis = "RudderAxis";
    public const string ThrustAxis = "ThrustAxis";

    public override void Start()
    {
        Input.VirtualButtonConfigSet = Input.VirtualButtonConfigSet
            ?? new VirtualButtonConfigSet();

        Input.VirtualButtonConfigSet.Add(
        [
            // Vertical
            new VirtualButtonBinding(VerticalAxis,
                new VirtualButtonTwoWay(VirtualButton.Keyboard.S, VirtualButton.Keyboard.W)),
            new VirtualButtonBinding(VerticalAxis, VirtualButton.GamePad.LeftThumbAxisY),
            new VirtualButtonBinding(VerticalAxis, VirtualButton.Mouse.DeltaY),

            // Horizontal
            new VirtualButtonBinding(HorizontalAxis,
                new VirtualButtonTwoWay(VirtualButton.Keyboard.A, VirtualButton.Keyboard.D)),
            new VirtualButtonBinding(HorizontalAxis, VirtualButton.GamePad.LeftThumbAxisX),
            new VirtualButtonBinding(HorizontalAxis, VirtualButton.Mouse.DeltaX),

            // Thrust
            new VirtualButtonBinding(ThrustAxis,
                new VirtualButtonTwoWay(VirtualButton.Keyboard.LeftCtrl, VirtualButton.Keyboard.Space)),
            new VirtualButtonBinding(ThrustAxis, VirtualButton.GamePad.LeftTrigger),

            // Rudder
            new VirtualButtonBinding(RudderAxis,
                new VirtualButtonTwoWay(VirtualButton.Keyboard.Q, VirtualButton.Keyboard.E)),
            new VirtualButtonBinding(RudderAxis, VirtualButton.GamePad.RightThumbAxisX),
        ]);
    }
}
