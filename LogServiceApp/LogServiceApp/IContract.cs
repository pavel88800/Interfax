using System.ServiceModel;
using System.Threading.Tasks;

namespace LogServiceApp
{
    [ServiceContract]
    interface IContract
    {
        [OperationContract]
        Task<string> GetInfo();
    }
}