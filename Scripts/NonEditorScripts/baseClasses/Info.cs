using CXUtils.DebugHelper;
using System;

namespace CXUtils.Infos
{

    /// <summary> The ultimate base class of all the infos 
    ///(Not in use)</summary>
    [Serializable]
    public struct Info : IDebugDescribable
    {
        public string DebugDescribe() =>
            "New Info";

        public new string ToString() =>
            "New Info";
    }
}

