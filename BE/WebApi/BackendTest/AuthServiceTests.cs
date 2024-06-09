using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BackendTest;

public class AuthServiceTests
{
    private Mock<UserManager<UserModel>> _mockUserManager;
    private Mock<SignInManager<UserModel>> _mockSignInManager;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<DataContext> _dataContext;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        _mockUserManager = new Mock<UserManager<UserModel>>(Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);
        _mockSignInManager = new Mock<SignInManager<UserModel>>(_mockUserManager.Object, 
            Mock.Of<IHttpContextAccessor>(), 
            Mock.Of<IUserClaimsPrincipalFactory<UserModel>>(), 
            null, null, null, null);
        _mockConfiguration = new Mock<IConfiguration>();
        _dataContext = new Mock<DataContext>(new DbContextOptions<DataContext>());
        _authService = new AuthService(_mockUserManager.Object, _mockSignInManager.Object, _mockConfiguration.Object, _dataContext.Object);
    }
    
    [TestCase]
    public async Task LoginUser_NotExcistingUser_NotFound()
    {
        var loginModel = new LoginModel() { Email = "abcd@test.com", Password = "password" };
        _mockUserManager.Setup(um => um.FindByEmailAsync(loginModel.Email)).ReturnsAsync((UserModel)null);

        var result = await _authService.LoginUser(loginModel);
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [TestCase]
    public async Task LoginUser_WrongPassword_BadRequest()
    {
        var loginModel = new LoginModel() { Email = "abcd@test.com", Password = "password" };
        var user = new UserModel() { Id = Guid.NewGuid(), Email = "abcd@test.com", UserName = "test" };
        _mockUserManager.Setup(um => um.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
        _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(user.UserName, loginModel.Password!, true, false))
            .ReturnsAsync(SignInResult.Failed);
        
        var result = await _authService.LoginUser(loginModel);
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [TestCase]
    public async Task LoginUser_CorrectInformation_OkAndGebruikerAsRole()
    {
        
        var loginModel = new LoginModel() { Email = "abcd@test.com", Password = "password" };
        var user = new UserModel() { Id = Guid.NewGuid(), Email = "abcd@test.com", UserName = "test", EmailConfirmed = true};
        var dbset = new Mock<DbSet<IdentityUserRole<Guid>>>();
        var data = new List<IdentityUserRole<Guid>>().AsQueryable();
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.Provider).Returns(data.Provider);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.Expression).Returns(data.Expression);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);
        
        _mockUserManager.Setup(um => um.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
        _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(user.UserName, loginModel.Password!, true, false))
            .ReturnsAsync(SignInResult.Success);
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("yourAudience");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("yourIssuer");
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("yourKeyForAJwtToken_yourKeyForAJwtToken");
        
        _dataContext.Object.UserRoles = dbset.Object;
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        
        var result = await _authService.LoginUser(loginModel);
        
        Assert.IsInstanceOf<OkObjectResult>(result);
        
        var okObjectResult = result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        
        var handler = new JwtSecurityTokenHandler();
        
        var model = okObjectResult.Value as string;
        var jwtSecurityToken = handler.ReadJwtToken(model);
        
        var role = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        
        Assert.That(actual: role, Is.EqualTo("Gebruiker"));
        
        
    }
    [TestCase]
    public async Task LoginAdmin_CorrectInformation_Ok()
    {
        var adminId = Guid.NewGuid();
        var loginModel = new LoginModel() { Email = "abcd@test.com", Password = "password" };
        var user = new UserModel() { Id = adminId, Email = "abcd@test.com", UserName = "test", EmailConfirmed = true};
        var dbset = new Mock<DbSet<IdentityUserRole<Guid>>>();
        List<IdentityUserRole<Guid>> queryable = new List<IdentityUserRole<Guid>>{ new IdentityUserRole<Guid > { RoleId = adminId, UserId = adminId } };
        var data = queryable.AsQueryable();
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.Provider).Returns(data.Provider);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.Expression).Returns(data.Expression);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbset.As<IQueryable<IdentityUserRole<Guid>>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);
        _dataContext.Setup(m => m.UserRoles).Returns(dbset.Object);

        List<string> roles = new List<string> { "admin" };
        _mockUserManager.Setup(um => um.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roles);
        _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(user.UserName, loginModel.Password!, true, false))
            .ReturnsAsync(SignInResult.Success);
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("yourAudience");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("yourIssuer");
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("yourKeyForAJwtToken_yourKeyForAJwtToken");
        
        _dataContext.Object.UserRoles = dbset.Object;
        
        var result = await _authService.LoginUser(loginModel);
        
        Assert.IsInstanceOf<OkObjectResult>(result);
        
        var okObjectResult = result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        
        var handler = new JwtSecurityTokenHandler();
        
        var model = okObjectResult.Value as string;
        var jwtSecurityToken = handler.ReadJwtToken(model);
        
        var role = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        Assert.That(actual: role, Is.EqualTo("admin"));
    }

    [TestCase]
    public async Task RegisterUser_DifferentPassword_BadRequest()
    {
        RegisterModel registerModel = new RegisterModel()
        {
            Password = "password",
            PasswordCheck = "password1",
        };
        var result = await _authService.RegisterUser(registerModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [TestCase]
    public async Task RegisterUser_RegisterUserWontWork_BadRequest()
    {
        RegisterModel registerModel = new RegisterModel()
        {
            Email = "test@test.com",
            Password = "password",
            PasswordCheck = "password1",
            UserName = "Test"
        };
        _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
        
        var result = await _authService.RegisterUser(registerModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
    [TestCase]
    public async Task RegisterUser()
    {
        RegisterModel registerModel = new RegisterModel()
        {
            Email = "test@test.com",
            Password = "password",
            PasswordCheck = "password",
            UserName = "Test"
        };
        
        UserModel user = new()
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email,
            EmailConfirmed = true
        };
        _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<UserModel>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mockSignInManager.Setup(m => m.SignInAsync(It.IsAny<UserModel>(), It.IsAny<bool>(), null)).Returns(Task.CompletedTask);
        var result = await _authService.RegisterUser(registerModel);
        Assert.IsInstanceOf<OkResult>(result);
    }
}