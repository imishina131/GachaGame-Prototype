using UnityEngine;

public static class ObjectExtensions
{
    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
}
