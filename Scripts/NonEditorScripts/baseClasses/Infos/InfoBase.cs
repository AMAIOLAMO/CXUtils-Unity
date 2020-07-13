using System;
using CXUtils.CodeUtils;

namespace CXUtils.DataUtils.Infos
{
    public interface IInfo : IDebugDescribable { }
    /// <summary> The ultimate base class of all the infos 
    ///(Not in use)</summary>
    [Serializable]
    public abstract class InfoBase : IInfo
    {
        public string DebugDescribe() =>
            "base class Info";

        public new string ToString() =>
            "base class Info";
    }
}

