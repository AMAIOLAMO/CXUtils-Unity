using CXUtils.CodeUtils;
using System;

namespace CXUtils.DataUtils.Infos
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

