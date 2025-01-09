namespace Engine;

public static class CollectionExtensions
{
    public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> enqueued)
    {
        foreach (T each in enqueued)
            queue.Enqueue(each);
    }
}