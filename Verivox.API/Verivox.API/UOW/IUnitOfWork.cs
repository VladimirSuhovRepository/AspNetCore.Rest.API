using System;
using Verivox.API.Repositories;

namespace Verivox.API.UOW
{
    internal interface IUnitOfWork : IDisposable
    {
        IPackagedTariffRepository PackagedTariffRepository { get; }

        IBaseTariffRepository BaseTariffRepository { get; }

        void Commit();
    }
}
