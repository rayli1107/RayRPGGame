using ScriptableObjects;
using System;
using UnityEngine;

namespace ScriptedActions
{
    public enum PrerequisiteType
    {
        QUEST,
        NONE,
    }

    [Serializable]
    public class Prerequisite
    {
        [SerializeField]
        private PrerequisiteType _prerequisiteType;
        public PrerequisiteType prerequisiteType => _prerequisiteType;

        [SerializeField]
        private string[] _stringFields;
        public string[] stringFields => _stringFields;

        [SerializeField]
        private int[] _intFields;
        public int[] intFields => _intFields;
    }

    public class PrerequisiteUtil
    {
        public static bool Check(Prerequisite entry)
        {
            switch (entry.prerequisiteType)
            {
                case PrerequisiteType.NONE:
                    return true;
            }
            return false;
        }
    }
}