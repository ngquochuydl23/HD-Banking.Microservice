using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Seedworks
{
    public interface IEfRepository<TEntity, TIdKey> 
        where TEntity : class
    {
        TEntity? Find(TIdKey? key);

        bool IsExist(TIdKey? key);

		TEntity Insert(TEntity entity);

        TEntity Update(TIdKey key, TEntity entity);

        void Delete(TIdKey key);

        IQueryable<TEntity> GetQueryableNoTracking(int? limit = null, int? offset = null);

		IQueryable<TEntity> GetQueryable();

		DbContext GetDbContext();

        void SaveChanges();
    }
}
