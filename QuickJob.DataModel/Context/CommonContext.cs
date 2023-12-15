using System.Collections.Concurrent;
using Vostok.Context;

namespace QuickJob.DataModel.Context;

public static class CommonContext
{
    private static readonly string ContextSlotKey = Guid.NewGuid().ToString();

    public static void Initialize() => FlowingContext.Properties.Set(ContextSlotKey, new ConcurrentDictionary<Type, object>());

    public static bool HasValue<T>()
    {
        var context = GetContextObject();
        var entityType = typeof(T);
        return context.ContainsKey(entityType);
    }

    public static T Get<T>(T defaultValue = null) where T : class
    {
        var context = GetContextObject();
        var entityType = typeof(T);

        if (context.TryGetValue(entityType, out var metricsContextObject) &&
            metricsContextObject is T metricsContext)
        {
            return metricsContext;
        }

        metricsContext = defaultValue ?? throw new Exception("Impossible get context: object is not defined " + entityType.FullName);
        context[entityType] = metricsContext;
        return metricsContext;
    }

    public static void SetValue<T>(T value)
    {
        var context = GetContextObject();
        var entityType = typeof(T);
        context[entityType] = value;
    }
    
    private static ConcurrentDictionary<Type, object> GetContextObject()
    {
        if (FlowingContext.Properties.Current.TryGetValue(ContextSlotKey, out var objectContext) &&
            objectContext is ConcurrentDictionary<Type, object> context)
        {
            return context;
        }

        throw new KeyNotFoundException("CommonContext does not init");
    }
}
