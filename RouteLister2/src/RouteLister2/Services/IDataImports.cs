using RouteLister2.Data;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    public interface IDataImports
    {
        void GetParcelData();
    }
}
