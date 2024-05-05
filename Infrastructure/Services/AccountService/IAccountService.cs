using Domain.DTOs.AccountDTO;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.AccountService;

public interface IAccountService
{
    Task<PagedResponse<List<GetAccountsDTO>>> GetAccountsAsync(AccountFilter filter);
    Task<Response<GetAccountsDTO>> GetAccountByIdAsync(int id);
    Task<Response<string>> CreateAccountAsync(CreateAccountDTO create);
    Task<Response<string>> UpdateAccountAsync(UpdateAccountDTO update);
    Task<Response<bool>> RemoveAccountAsync(int id);
}
