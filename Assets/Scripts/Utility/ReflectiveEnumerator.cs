using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public static class ReflectiveEnumerator
{
    public static List<Type> GetEnumerableOfType<T>() where T : class, IComparable<T>
    {
        List<Type> types = new ();

        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            types.Add(type);
        }

        return types;
    }
}