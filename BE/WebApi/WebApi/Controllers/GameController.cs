using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Data;
using WebApi.Interfaces.Services;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly IGameService _gameService;

        public GameController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext, IGameService gameService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataContext = dataContext;
            _gameService = gameService;
        }

        [Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateGame()
        {
            try
            {
                var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
                var id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var name = _userManager.FindByIdAsync(id);
                var guid = name.Result.Id;

                Game game = new Game()
                {
                    Id = Guid.NewGuid(),
                    User1 = guid,
                    TurnCount = 0,
                    BoardState = new string[Game.boardSize * Game.boardSize],
                    GameState = GameState.Starting,
                    GameCreation = DateTime.Now

                };
                _dataContext.Games.Add(game);
                _dataContext.SaveChanges();
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Join")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> JoinGame([FromBody] JoinGameModel joinGame)
        {
            try
            {
                var token = await JWThandeler.GetTokenClaims(HttpContext.Request);
                var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                // Retrieve user name asynchronously
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // Find the game by GameId
                var game = await _dataContext.Games.FindAsync(joinGame.GameID);
                if (game == null)
                {
                    return BadRequest("Game not found.");
                }

                // Update game and save changes
                game.User2 = user.Id;
                game = _gameService.SetGameStart(game);
                await _dataContext.SaveChangesAsync();

                return Ok(game);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest("An error occurred while joining the game.");
            }
        }

        [Authorize]
        [HttpPost("Move")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> MakeMove([FromBody] PlayingModel playing)
        {
            if (ModelState == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return await _gameService.MakeMove(playing);
        }

        [Authorize]
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckBoard(Guid guid)
        {
            var result = await _dataContext.Games.FindAsync(guid);
            if(result == null) return NotFound();
            return Ok(result);
        }
        
    }
}
