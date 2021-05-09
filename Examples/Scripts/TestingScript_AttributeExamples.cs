using UnityEngine;
using UnityEngine.CXExtensions;

namespace CXUtils.Test
{
    public class TestingScript_AttributeExamples : MonoBehaviour
    {
        [ForeColor( "#FF00FF", onlyThisField: true )]
        public float foregroundVal;

        [BackgroundColor( "#FFFF00", onlyThisField: true )]
        public float backgroundVal;

        [Header( "Everything below is colored!" )]
        [ForeColor( "#FFFF00" )]
        public float yey1;
        public float yey2;

        //we can stop it here
        [ForeColor( "#FFFF00", onlyThisField: true )]
        public float yey3;
    }
}
