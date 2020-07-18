using UnityEngine.Rendering;
using System.Collections.Generic;

namespace CXUtils.RendereringUtils
{
    /// <summary> CX's Post processing utils </summary>
    public class PostProcessingUtils
    {
        /// <summary> Tries to get the volume Compoenent from the name </summary>
        public static bool TryGetVolumeComponent(VolumeProfile volumeProfile, string name, out VolumeComponent volumeComponent)
        {
            volumeComponent = volumeProfile.components.Find((value) => value.name.Equals(name));
            return volumeComponent != null;
        }

        /// <summary> Tries to get all the volume Compoenent from the name </summary>
        public static bool TryGetVolumeComponents(VolumeProfile volumeProfile, string name, out List<VolumeComponent> volumeComponent)
        {
            volumeComponent = volumeProfile.components.FindAll((value) => value.name.Equals(name));
            return volumeComponent != null;
        }
    }
}
