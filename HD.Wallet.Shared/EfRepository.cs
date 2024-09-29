using HD.Wallet.Shared.Seedworks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared
{
	public class EfRepository<TEntity, TIdKey> : IEfRepository<TEntity, TIdKey>
		where TEntity : class
	{
		private readonly DbContext _appDbContext;

		public EfRepository(DbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		private DbSet<TEntity> _entity => _appDbContext.Set<TEntity>();

		public void Delete(TIdKey key)
		{
			var dbEntity = _entity.Find(key);
			if (dbEntity == null)
				throw new NullReferenceException();

			if (dbEntity is IDeleteEntity)
			{
				var deletedEntity = (IDeleteEntity)dbEntity;

				deletedEntity.IsDeleted = true;
			}
			_appDbContext.Entry(dbEntity).State = EntityState.Modified;
			SaveChanges();
		}

		public TEntity? Find(TIdKey? key)
		{
			var dbEntity = _entity.Find(key);
			if (dbEntity is IDeleteEntity)
			{
				var deletedEntity = (IDeleteEntity)dbEntity;
				if (deletedEntity.IsDeleted)
				{
					return null;
				}
			}
			return dbEntity;
		}

		public DbContext GetDbContext()
		{
			return _appDbContext;
		}

		public IQueryable<TEntity> GetQueryable()
		{
			var query = _entity
				.Where(x => !((IDeleteEntity)x).IsDeleted);


			return query.AsTracking();
		}

		public IQueryable<TEntity> GetQueryableNoTracking(int? limit = null, int? offset = null)
		{
			var query = _entity
				.Where(x => !((IDeleteEntity)x).IsDeleted);

			if (limit != null && offset != null)
			{
				return query
					.Skip(offset.Value)
					.Take(limit.Value)
					.AsNoTracking();
			}

			return query.AsNoTracking();
		}

		public TEntity Insert(TEntity entity)
		{
			if (entity is IHasCreationTime)
			{
				var hasCreationTimeEntity = (IHasCreationTime)entity;
				hasCreationTimeEntity.CreatedAt = DateTime.Now;
			}

			_entity.Add(entity);
			_appDbContext.Entry(entity).State = EntityState.Added;

			SaveChanges();
			return entity;
		}

		public bool IsExist(TIdKey? key)
		{
			return Find(key) != null;
		}

		public void SaveChanges()
		{
			_appDbContext.SaveChanges();
		}

		public TEntity Update(TIdKey key, TEntity entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity is ILastUpdatedTime)
			{
				var lastUpdatedTime = (ILastUpdatedTime)entity;
				lastUpdatedTime.LastUpdated = DateTime.Now;
			}
			_appDbContext.Entry(entity).State = EntityState.Modified;
			SaveChanges();
			return entity;
		}
	}
}
