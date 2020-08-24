using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Infrastructure.Common.BaseModels
{
    public interface IDbContextSeed
    {
        void Init();
    }
}