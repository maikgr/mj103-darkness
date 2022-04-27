using System.Collections.Generic;
using UnityEngine;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static T RandomItem<T>(this List<T> list)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
    }
}