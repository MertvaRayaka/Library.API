using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Servicers
{
    public interface IRepositoryBase2<T,TId>
    {
        Task<T> GetByIdAsync(TId id);

        Task<bool> IsExistAsync(TId id);
    }
}
