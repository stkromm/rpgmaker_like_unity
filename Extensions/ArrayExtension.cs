#region

using System;

#endregion

public static class ArrayExtension
{
    public static bool ValidIndex(this Array array, int index)
    {
        return (index >= 0 && index < array.Length);
    }
}