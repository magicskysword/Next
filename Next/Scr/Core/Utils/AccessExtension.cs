using System;

namespace SkySwordKill.Next.Utils;

public static class AccessExtension
{
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        if(obj == null)
            throw new ArgumentNullException(nameof(obj));
        var type = obj.GetType();
        var field = type.GetField(fieldName, System.Reflection.BindingFlags.Public 
                                             | System.Reflection.BindingFlags.NonPublic 
                                             | System.Reflection.BindingFlags.Instance);
        if (field == null)
            throw new Exception($"Field:{fieldName} not found");
        var value = field.GetValue(obj);
        if (value == null)
            return default;
        if(!(value is T t))
            throw new Exception($"Field:{fieldName} is not of type {typeof(T).Name}");
        return t;
    }
    
    public static T GetPropertyValue<T>(this object obj, string propertyName)
    {
        if(obj == null)
            throw new ArgumentNullException(nameof(obj));
        var type = obj.GetType();
        var property = type.GetProperty(propertyName, System.Reflection.BindingFlags.Public 
                                                 | System.Reflection.BindingFlags.NonPublic 
                                                 | System.Reflection.BindingFlags.Instance);
        if (property == null)
            throw new Exception($"Property:{propertyName} not found");
        var value = property.GetValue(obj);
        if (value == null)
            return default;
        if(!(value is T t))
            throw new Exception($"Property:{propertyName} is not of type {typeof(T).Name}");
        return t;
    }
}