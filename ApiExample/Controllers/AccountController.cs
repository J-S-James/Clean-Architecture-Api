using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Account account)
    {
        var result = await _accountService.CreateAccountAsync(account);

        if (result == 0)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ReadAsync(int id)
    {
        var result = await _accountService.GetAccountAsync(new Account { Id = id });

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(Account account)
    {
        var result = await _accountService.UpdateAccountAsync(account);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _accountService.DeleteAccountAsync(new Account { Id = id });
        return Ok(result);
    }
}
