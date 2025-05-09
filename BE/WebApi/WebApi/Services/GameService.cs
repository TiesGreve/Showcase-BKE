using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Interfaces.Services;
using WebApi.Models;

namespace WebApi.Controllers;

public class GameService : IGameService
{
    public DataContext _dataContext { get; set; }

    public GameService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public Game SetGameStart (Game game)
        {
            var random = new Random();
            if (random.Next() > (Int32.MaxValue / 2))
            {
                game.FirstMove = game.User1;
                game.GameState = GameState.Player1Move;
                game.CurrentTurn = game.User1;
            }
            else
            {
                game.FirstMove = game.User2;
                game.GameState = GameState.Player2Move;
                game.CurrentTurn = game.User2;
            }
            game.GameStart = DateTime.Now;
            return game;
        }
    public Game CheckGame(Game game)
    {
        if (CheckVertical(game) || CheckHorizontal(game) || CheckDiagnals(game)) { 
            game.GameState = GameState.Finnished; 
            game.GameFinish = DateTime.Now;
            game.Winner = game.CurrentTurn;
        }
        else if (CheckIfBoardIsFull(game))
        {
            game.GameState = GameState.Draw;
            game.GameFinish = DateTime.Now;
        }
        return game;
    }
    public bool CheckIfBoardIsFull(Game game)
    {
        foreach(var item in game.BoardState)
        {
            if(string.IsNullOrEmpty(item)) return false;
        }
        return true;
    }
    public bool CheckVertical(Game game)
    {
        for(int i = 0; i < Math.Sqrt(game.BoardState.Length); i ++)
        {
            if (!string.IsNullOrEmpty(game.BoardState[i]) || game.BoardState[0+i].IsNullOrEmpty()) continue;
            if (!string.IsNullOrEmpty(game.BoardState[i]) && game.BoardState[i] == game.BoardState[3 + i] && game.BoardState[3 + i] == game.BoardState[6 + i]) return true;
        }
        return false;
    }
    public bool CheckHorizontal(Game game)
    {
        for (int i = 0; i < game.BoardState.Length; i += 3)
        {
            if (!string.IsNullOrEmpty(game.BoardState[i]) || game.BoardState[i].IsNullOrEmpty()) continue;
            if (!string.IsNullOrEmpty(game.BoardState[i]) && game.BoardState[i] == game.BoardState[1+i] && game.BoardState[1+i] == game.BoardState[2+i]) return true;
            
        }
        return false;
    }
    public bool CheckDiagnals(Game game)
    {
        if (!string.IsNullOrEmpty(game.BoardState[0]) && game.BoardState[0] == game.BoardState[4] && game.BoardState[4] == game.BoardState[8]) return true;
        if (!string.IsNullOrEmpty(game.BoardState[2]) && game.BoardState[2] == game.BoardState[4] && game.BoardState[4] == game.BoardState[6]) return true;
        return false;
    }
    public async Task<IActionResult> MakeMove(PlayingModel playing)
    {
        Game? gameDb = _dataContext.Games.Find(playing.GameID);
        if (playing.Cell is > 8 or < 0) return RequestService.ReturnBadRequest(nameof(MakeMove), "Illegal move");
        if (gameDb == null) return RequestService.ReturnBadRequest(nameof(MakeMove), "Game not found");
        if (gameDb.User2 == null) return RequestService.ReturnBadRequest(nameof(MakeMove), "No second user in game");
        if (gameDb.BoardState[playing.Cell] != null) return RequestService.ReturnBadRequest(nameof(MakeMove), "Cell already filled in");
        if (gameDb.CurrentTurn == gameDb.User1) gameDb.BoardState[playing.Cell] = "x";
        else gameDb.BoardState[playing.Cell] = "o";

        gameDb = CheckGame(gameDb);
        if (gameDb.GameFinish != null) HandleMove(gameDb);
        
        return new OkObjectResult(gameDb.GameState);
    }

    public async Task<Game> HandleMove(Game gameDb)
    {
        if (gameDb.CurrentTurn == gameDb.User1)
        {
            gameDb.CurrentTurn = gameDb.User2;
            gameDb.GameState = GameState.Player2Move;
        }
        else
        {
            gameDb.CurrentTurn = gameDb.User1;
            gameDb.GameState = GameState.Player1Move;
        }
        
        gameDb.GameUpdate = DateTime.Now;
        await _dataContext.SaveChangesAsync();
        return gameDb;
    }
}