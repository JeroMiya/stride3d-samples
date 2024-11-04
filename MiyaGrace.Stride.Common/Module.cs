using Stride.Core.Reflection;

using System.Reflection;

namespace MiyaGrace.Stride.Common;

/// <summary>
/// This class is required to get assets (including scripts) to be referenced
/// by scenes in the main game project when this project is referenced.
/// </summary>
internal class Module
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AssemblyRegistry.Register(
            typeof(Module).GetTypeInfo().Assembly,
            AssemblyCommonCategories.Assets);
    }
}
