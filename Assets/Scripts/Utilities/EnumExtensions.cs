using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumExtensions {

    public static T NextEnumValue<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static T PreviousEnumValue<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
    }

    public static IEnumerable<T> GetEnumValues<T>() {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}