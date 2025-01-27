namespace MyEngine;

public static class CollectionExtensions
{
    public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> enqueued)
    {
        foreach (T each in enqueued)
            queue.Enqueue(each);
    }

    public static T GetRandomElement<T>(this List<T> list)
        => list[Random.Shared.Next(list.Count)];

    public static void SwapRemoveAt<T>(this List<T> list, int index)
    {
        list.Swap(index, list.Count - 1);
        list.RemoveAt(list.Count - 1);
    }

    private static void Swap<T>(this List<T> list, int first, int second)
        => (list[first], list[second]) = (list[second], list[first]);
}