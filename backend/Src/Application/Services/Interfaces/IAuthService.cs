
using Application.Dtos.UserDtos;

namespace Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> SignUpAsync(SignupDto signupDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}