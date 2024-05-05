using System.Data.Common;
using System.Net;
using AutoMapper;
using Domain.DTOs.AccountDTO;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public AccountService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PagedResponse<List<GetAccountsDTO>>> GetAccountsAsync(AccountFilter filter)
    {
        try
        {
            var accounts = _context.Accounts.AsQueryable();
            if(!string.IsNullOrEmpty(filter.Type.ToString()))
                accounts = accounts.Where(a => a.AccountType.ToString().Contains(filter.Type.ToString().ToLower()));
            var result = await accounts.Skip((filter.PageNumber -1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await accounts.CountAsync();
            var response = _mapper.Map<List<GetAccountsDTO>>(result);
            return new PagedResponse<List<GetAccountsDTO>>(response,total,filter.PageNumber,filter.PageSize);
        }
        catch(DbException DBe)
        {
            return new PagedResponse<List<GetAccountsDTO>>(HttpStatusCode.InternalServerError,DBe.Message);
        }
        catch (System.Exception e)
        {
            return new PagedResponse<List<GetAccountsDTO>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }


    public async Task<Response<GetAccountsDTO>> GetAccountByIdAsync(int id)
    {
         try
        {
            var existing = await _context.Accounts.FirstOrDefaultAsync(e=>e.Id==id);
            if(existing == null) return new Response<GetAccountsDTO>(HttpStatusCode.NotFound,"Not Found");
            
            var mapped = _mapper.Map<GetAccountsDTO>(existing);
            return new Response<GetAccountsDTO>(mapped);
        }
        catch(DbException DBe)
        {
            return new Response<GetAccountsDTO>(HttpStatusCode.InternalServerError,DBe.Message);
        }
        catch (System.Exception e)
        {
            return new Response<GetAccountsDTO>(HttpStatusCode.InternalServerError,e.Message);
        }
    }


    public async Task<Response<string>> CreateAccountAsync(CreateAccountDTO create)
    {
        try
        {
            var existing = await _context.Accounts.AnyAsync(x => x.AccountNumber == create.AccountNumber);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "Account already exists");
            var newAccount = _mapper.Map<Account>(create);
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return new Response<string>("Successfully created ");
        }
        catch (DbException e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<string>> UpdateAccountAsync(UpdateAccountDTO update)
    {
        try
        {
            var existing = await _context.Accounts.AnyAsync(x => x.Id == update.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Account not found");
            var newAccount = _mapper.Map<Account>(update);
            _context.Accounts.Update(newAccount);
            await _context.SaveChangesAsync();
            return new Response<string>("Account successfully updated");
        }
        catch (DbException Dbe)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, Dbe.Message);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<bool>> RemoveAccountAsync(int id)
    {
        try
        {
            var existing = await _context.Accounts.Where(x => x.Id == id).ExecuteDeleteAsync();
           
                return existing == 0 ? new Response<bool>(HttpStatusCode.Conflict,"Accound Not Found!")
                :new Response<bool>(HttpStatusCode.OK,"Account Deleted",true);
        }
        catch (DbException e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

}
