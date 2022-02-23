
using System.ComponentModel;
using System.Reflection;
using EmailService;
using Newtonsoft.Json;

namespace AplusExtension;
public static partial class Extensions
{
    /// <summary>
    ///     A T extension method that query if '@this' is not null.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if not null, false if not.</returns>
    public static bool IsNotNullOrEmpty(this object? @this)
    {
        return @this != null && @this.ToString().Length > 0;
    }

    public static bool IsPositiveNumber(this int @this)
    {
        return @this > 0;
    }



    public static Dictionary<string, object> PropertiesFromInstance(this object @this)
    {
        if (@this == null) return null;
        Type TheType = @this.GetType();
        PropertyInfo[] Properties = TheType.GetProperties();
        Dictionary<string, object> PropertiesMap = new Dictionary<string, object>();
        foreach (PropertyInfo Prop in Properties)
        {
            try
            {
                var value = @this.GetType().GetProperty(Prop.Name).GetValue(@this, null);
                if (value != null)
                {
                    PropertiesMap.Add(Prop.Name, value);
                }
            }
            catch (Exception e)
            {

            }

        }
        return PropertiesMap;
    }

    public static List<Parameter> toParameterList(this Dictionary<string, object> dict)
    {

        return dict.Select(x => new Parameter
        {
            key = x.Key,
            value = x.Value,
            type = x.Value.GetType()
        }).ToList();
    }

  
    public static QueryContract toContract(this List<dynamic> list)
    {

        var requests = list.Select(x => new TypedQuery
        {
            request =  JsonConvert.SerializeObject(x),
            type = x.GetType()
        }).ToList();

        return new QueryContract{
            type = QueryTypes.Transaction,
            request = JsonConvert.SerializeObject(requests)
        };

    }


    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);
        return dictionary;
    }

    public static Dictionary<string, object> ToDictionary(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, object>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<object>(property, source, dictionary);
        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
    {
        object value = property.GetValue(source);
        if (value != null && IsOfType<T>(value))
            dictionary.Add(property.Name, (T)value);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
    }


    public static Dictionary<string?, object?> toDictionaryList(this List<Parameter> list)
    {
        if (list == null) return null;

        return list.ToDictionary(x => x.key, x =>
        {

            ///this is for message queue serialization on date data problem
            //we may need to add new datatype later
            if (x.type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(x.value.ToString());
            }
            return x.value;
        });
    }

    public static void AddDictionary(this Dictionary<string, object> source, Dictionary<string, object> items)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        foreach (var item in items)
        {
            source.Add(item.Key, item.Value);
        }

    }

    public static List<T> toList<T>(this List<IDictionary<string, object>> list)
    {
        return list.Select(x => x.toObject<T>()).ToList<T>();
    }

    public static T toObject<T>(this IDictionary<string, object> dict)
    {
        T obj = (T)Activator.CreateInstance(typeof(T));
        PropertyInfo[] props = obj.GetType().GetProperties();
        foreach (var prop in props)
        {
            var val = dict.Where(d => d.Key.ToLower() == prop.Name.ToLower()).Select(x => x.Value).FirstOrDefault();
            if (val != null)
            {
                var type = prop.PropertyType;
                //we may need to add new datatype later
                if (type == typeof(DateTimeOffset))
                {
                    prop.SetValue(obj, DateTimeOffset.Parse(val.ToString()));
                }
                else
                {
                    prop.SetValue(obj, val);
                }
            }
        }

        return obj;
    }



}

