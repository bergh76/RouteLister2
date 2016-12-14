using RouteLister2.Data;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    public interface IDataImports
    {
        Task GetParcelData();
        //Task GetCoordinates();
    }
}
