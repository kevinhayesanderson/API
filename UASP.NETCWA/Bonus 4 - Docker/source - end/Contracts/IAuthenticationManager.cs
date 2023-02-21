using Entities.DataTransferObjects;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthenticationManager
    {
        Task<bool> ValdiateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();
    }
}
