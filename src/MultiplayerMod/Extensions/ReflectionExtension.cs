using System.Reflection;

namespace MultiplayerMod.Extensions;

/// <summary>
/// Reflection Extension for Fields, Properties, and more
/// </summary>
public static class ReflectionExtension
{
    /// <summary>
    /// Get Field Value from <paramref name="fieldName"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static object GetFieldValue(this object obj, string fieldName)
    {
        return GetFieldValue<object>(obj, fieldName);
    }

    /// <summary>
    /// Get Field Value from <paramref name="fieldName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        var type = obj.GetType();
        var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (field == null)
            throw new Exception($"Field {fieldName} not found in {obj.GetType()}");
        return (T) field.GetValue(obj);
    }

    /// <summary>
    /// Set Field Value for <paramref name="fieldName"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void SetFieldValue(this object obj, string fieldName, object value)
    {
        SetFieldValue<object>(obj, fieldName, value);
    }

    /// <summary>
    /// Set Field Value for <paramref name="fieldName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static void SetFieldValue<T>(this object obj, string fieldName, T value)
    {
        var type = obj.GetType();
        var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (field == null)
            throw new Exception($"Field {fieldName} not found in {obj.GetType()}");
        field.SetValue(obj, value);
    }

    /// <summary>
    /// Get Property Value from <paramref name="propertyName"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object GetPropertyValue(this object obj, string propertyName)
    {
        return GetPropertyValue<object>(obj, propertyName);
    }

    /// <summary>
    /// Get Property Value from <paramref name="propertyName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T GetPropertyValue<T>(this object obj, string propertyName)
    {
        var type = obj.GetType();
        var field = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (field == null)
            throw new Exception($"Field {propertyName} not found in {obj.GetType()}");

        return (T) field.GetValue(obj);
    }
}
