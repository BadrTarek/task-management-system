using Application.Dtos.UserDtos;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;


namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenManager _tokenManager;
    private readonly IPasswordManager _passwordManager;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenManager tokenManager,
        IPasswordManager passwordManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenManager = tokenManager;
        _passwordManager = passwordManager;
    }

    public async Task<AuthResponseDto> SignUpAsync(SignupDto signupDto)
    {

        var existingUser = await _unitOfWork.UserRepository.GetUserByEmail(signupDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already exists");
        }
        var user = _mapper.Map<User>(signupDto);
        var createdUser = await _unitOfWork.UserRepository.CreateUser(user);
        await _unitOfWork.SaveChangesAsync();
        var token = _tokenManager.GenerateToken(createdUser.Id.ToString(), createdUser.Email, createdUser.Name);
        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(loginDto.Email);
        if (user == null || string.IsNullOrEmpty(user.Password) || !_passwordManager.VerifyPassword(loginDto.Password, user.Password))
        {
            throw new InvalidOperationException("Invalid email or password");
        }
        var token = _tokenManager.GenerateToken(user.Id.ToString(), user.Email, user.Name);
        return new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        };
    }

}