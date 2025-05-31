using Microsoft.AspNetCore.Identity.Data;
using Presentation.Models;
using System.Net.Http;
using System.Net.Http.Json;


namespace Presentation.Services;

public interface IAuthService
{
    Task<AuthResult> AlreadyExists(string email);
    Task<AuthResult<LogInResponse>> LoginAsync(LogInRequest request);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpRequest request);
}

public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<AuthResult> AlreadyExists(string email)
    {
        var response = await _httpClient.GetFromJsonAsync<AccountServicehResult>($"https://localhost:7284/api/Accounts/exists/{email}");
        return response!.Success
             ? new AuthResult { Success = true }
             : new AuthResult { Success = false,Error = "User already exists." };
     }
    public async Task<AuthResult> SignUpAsync(SignUpRequest request)
    {
        try 
        {             
            var existsResult = await AlreadyExists(request.Email);
            if (existsResult.Success)
            {
                return new AuthResult { Success = false, Error = "User already exists." };
            }
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
        try { 
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7284/api/Accounts/create", request);
            if (response.IsSuccessStatusCode)
            {
                return new AuthResult { Success = true };
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AuthResult { Success = false, Error = error };
            }
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
      
    }

    public async Task<AuthResult<LogInResponse>> LoginAsync(LogInRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7284/api/Accounts/login", request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LogInResponse>();
                return new AuthResult<LogInResponse> { Success = true, Result = loginResponse };
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AuthResult<LogInResponse> { Success = false, Error = error };
            }
        }
        catch (Exception ex)
        {
            return new AuthResult<LogInResponse> { Success = false, Error = ex.Message };
        }
    }
    public async Task<AuthResult> SignOutAsync()
    {
        await _httpClient.PostAsync($"https://localhost:7284/api/Accounts", null);
        return new AuthResult { Success = true };
    }
}
