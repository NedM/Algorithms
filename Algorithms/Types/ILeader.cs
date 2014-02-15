using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public interface ILeader<T> where T : struct, IComparable, IEquatable<T>
    {
        IVertex<T> Leader { get; set; }
        int Rank { get; set; }
        bool HasSameLeader(ILeader<T> other);
    }
}
