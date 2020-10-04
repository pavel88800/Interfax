using System.ServiceModel;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Helpers
{
    [ServiceContract]
    internal interface IContract
    {
        [OperationContract]
        Task<string> GetInfo();
    }
}