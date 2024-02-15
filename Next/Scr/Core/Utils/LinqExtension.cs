using System;
using System.Collections.Generic;

namespace SkySwordKill.Next.Utils;

public static class LinqExtension
{
    public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
    {
        return MaxBy(source, selector, Comparer<TKey>.Default);
    }
    
    public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IComparer<TKey> comparer)
    {
        T maxItem = default;
        TKey maxValue = default;
        
        foreach (var item in source)
        {
            var value = selector(item);
            if (comparer.Compare(value, maxValue) > 0)
            {
                maxItem = item;
                maxValue = value;
            }
        }
        
        return maxItem;
    }
    
    public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
    {
        return MinBy(source, selector, Comparer<TKey>.Default);
    }
    
    public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IComparer<TKey> comparer)
    {
        T minItem = default;
        TKey minValue = default;
        
        foreach (var item in source)
        {
            var value = selector(item);
            if (comparer.Compare(value, minValue) < 0)
            {
                minItem = item;
                minValue = value;
            }
        }
        
        return minItem;
    }
}