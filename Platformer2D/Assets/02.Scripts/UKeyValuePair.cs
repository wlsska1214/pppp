using System;

[Serializable]
public struct UKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}
