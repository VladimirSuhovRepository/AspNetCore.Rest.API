using System;
using Verivox.API.Database;
using Verivox.API.Repositories;

namespace Verivox.API.UOW
{
    internal class ProductsDbUnitOfWork : IUnitOfWork
    {
        private readonly ProductsDbContext productsDbContext;
        private bool disposed;

        public ProductsDbUnitOfWork(ProductsDbContext productsDbContext,
            IPackagedTariffRepository packagedTariffRepository,
            IBaseTariffRepository baseTariffRepository1)
        {
            this.productsDbContext = productsDbContext;
            PackagedTariffRepository = packagedTariffRepository;
            BaseTariffRepository = baseTariffRepository1;
        }

        public IPackagedTariffRepository PackagedTariffRepository { get; }

        public IBaseTariffRepository BaseTariffRepository { get; }

        public void Commit()
        {
            productsDbContext.SaveChanges();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    productsDbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
