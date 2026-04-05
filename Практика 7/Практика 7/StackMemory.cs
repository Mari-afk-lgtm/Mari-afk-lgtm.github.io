using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class StackMemory
{
    private readonly int _depth;
    private readonly List<byte[]> _list = new List<byte[]>();

    public StackMemory(int depth)
    {
        _depth = depth < 1 ? 1 : depth;
    }

    public void Push(MemoryStream stream)
    {
        if (_list.Count >= _depth)
            _list.RemoveAt(0);
        _list.Add(stream.ToArray());
    }

    public void Pop(MemoryStream stream)
    {
        if (_list.Count == 0) return;
        var data = _list[_list.Count - 1];
        stream.Write(data, 0, data.Length);
        _list.RemoveAt(_list.Count - 1);
    }

    public void Clear() => _list.Clear();
    public int Count => _list.Count;
}