namespace Domain.Filters;

public class AccountFilter : PaginationFilter
{
    public AccountType? Type { get; set; }
}
