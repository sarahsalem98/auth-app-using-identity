using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDataAccess.Repository
{
    public interface IRepo<T> where T : class
    {
        void Add(T entity);
        void Remove (T entity);
        void RemoveRange(IEnumerable<T> entities);
        List<T> GetAll();
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);   
    }
}
