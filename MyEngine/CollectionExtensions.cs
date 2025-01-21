namespace MyEngine;

public static class CollectionExtensions
{
    public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> enqueued)
    {
        foreach (T each in enqueued)
            queue.Enqueue(each);
    }

    public static T GetRandomElement<T>(List<T> list)
        => list[Random.Shared.Next(list.Count)];
}