using System;
using Verivox.API.Database;
using Verivox.API.Repositories;

namespace Verivox.API.UOW
{
    internal class ProductsDbUnitOfWork : IUnitOfWork
    {
        private readonly ProductsDbContext productsDbContext;
        private bool disposed;

        public ProductsDbUnitOfWork(
            ProductsDbContext productsDbContext,
            IPackagedTariffRepository packagedTariffRepository,
            IBaseTariffRepository baseTariffRepository)
        {
            this.productsDbContext = productsDbContext;
            this.PackagedTariffRepository = packagedTariffRepository;
            this.BaseTariffRepository = baseTariffRepository;
        }

        public IPackagedTariffRepository PackagedTariffRepository { get; }

        public IBaseTariffRepository BaseTariffRepository { get; }

        public void Commit()
        {
            this.productsDbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.productsDbContext.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
