using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionApp.Application.Utilities.Caching
{
    public interface ICustomCache
    {
        void Set<T>(string key, T value, TimeSpan expiration);
        bool TryGetValue<T>(string key, out T value);
        void RemoveByKey(string key);
    }
}
