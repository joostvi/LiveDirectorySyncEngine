using System.Collections.Generic;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public interface IDBRepository<T> where T : class
    {
        void Store(T obj);
        IList<T> GetAll();
        T Get(int id);
    }
}
