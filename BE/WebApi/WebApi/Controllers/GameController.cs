using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using WebApi.Data;
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

        public GameController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration configuration, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataContext = dataContext;
        }

        [Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateGame([FromBody]string username)
        {
            try
            {
                var name = _userManager.FindByNameAsync(username);
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
        public async Task<IActionResult> JoinGame([FromBody] Guid guid, string username)
        {
            try
            {
                var game = _dataContext.Games.Find(guid);
                var name = _userManager.FindByNameAsync(username);
                game.User2 = name.Result.Id;
                game = SetGameStart(game);
                await _dataContext.SaveChangesAsync();
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Move")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> MakeMove([FromBody] Game game)
        {
            if(ModelState == null) return BadRequest(ModelState);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            Game gameDB = _dataContext.Games.Find(game.Id);
            gameDB.GameState = game.GameState;
            gameDB = CheckGame(gameDB);
            if(gameDB.GameFinish == null)
            {
                if(gameDB.CurrentTurn == gameDB.User1)
                {
                    gameDB.CurrentTurn = gameDB.User2;
                    gameDB.GameState = GameState.Player2Move;
                }
                else
                {
                    gameDB.CurrentTurn = gameDB.User1;
                    gameDB.GameState= GameState.Player1Move;
                }
            }
            gameDB.GameUpdate = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("Check")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckBoard([FromBody] Guid guid)
        {
            return Ok(_dataContext.Games.Find(guid));
        }
        private Game SetGameStart (Game game)
        {
            var random = new Random();
            if (random.Next() > (Int32.MaxValue / 2))
            {
                game.FirstMove = game.User1;
                game.GameState = GameState.Player1Move;
            }
            else
            {
                game.FirstMove = game.User2;
                game.GameState = GameState.Player2Move;
            }
            game.GameStart = DateTime.Now;
            return game;
        }
        private Game CheckGame(Game game)
        {
            if (CheckVertical(game.BoardState) || CheckHorizontal(game.BoardState) || CheckDiagnals(game.BoardState)) { 
                game.GameState = GameState.Finnished; 
                game.GameFinish = DateTime.Now;
                game.Winner = game.CurrentTurn;
            }
            else if (CheckIfStalemate(game))
            {
                game.GameState = GameState.Draw;
                game.GameFinish = DateTime.Now;
            }
            return game;
        }
        private bool CheckIfStalemate(Game game)
        {
            foreach(var item in game.BoardState)
            {
                if(item == null) return false;
            }
            return true;
        }
        private bool CheckVertical(string[] board)
        {
            for(int i = 0; i < board.Length; i += 3)
            {
                if (board[0+i] != null && board[0+i] == board[1+i] && board[1+i] == board[2+i]) return true;
            }
            return false;
        }
        private bool CheckHorizontal(string[] board)
        {
            for (int i = 0; i < Math.Sqrt(board.Length); i ++)
            {
                if (board[i] != null && board[i] == board[3 + i] && board[3 + i] == board[6 + i]) return true;
            }
            return false;
        }
        private bool CheckDiagnals(string[] board)
        {
            if (board[0] != null && board[0] == board[4] && board[4] == board[8]) return true;
            if (board[2] != null && board[2] == board[4] && board[4] == board[6]) return true;
            return false;
        }
    }
}
