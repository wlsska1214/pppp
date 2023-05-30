using System;
using System.Collections.Generic;

public interface IState
{
    int id { get; set; }
    bool canExecute { get; }
    List<KeyValuePair<Func<bool>, int>> transitions { get; set; }
    void Execute();
    void Stop();
    int Update();
}
