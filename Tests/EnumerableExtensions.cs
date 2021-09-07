using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class EnumerableExtensions
    {
        public static TEnumerable ForEach<TEnumerable, TValue>(
            this TEnumerable self, Action<TValue> action) 
            where TEnumerable : IEnumerable<TValue>
        {
            foreach (var item in self)
                action.Invoke(item);
            return self;
        }

    }
}
