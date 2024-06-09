using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

public class GameService
{
    private DataContext _dataContext;

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

        public async Task<IActionResult> MakeMove(PlayingModel playing)
        {
            Game? gameDb = _dataContext.Games.Find(playing.GameID);
            if (playing.Cell > 8) return RequestService.ReturnBadRequest(nameof(MakeMove), "Illegal move");
            if (gameDb == null) return RequestService.ReturnBadRequest(nameof(MakeMove), "Game not found");
            if (gameDb.User2 == null) return RequestService.ReturnBadRequest(nameof(MakeMove), "No second user in game");
            if (gameDb.BoardState[playing.Cell] != null) return RequestService.ReturnBadRequest(nameof(MakeMove), "Cell already filled in");
            if (gameDb.CurrentTurn == gameDb.User1) gameDb.BoardState[playing.Cell] = "x";
            else gameDb.BoardState[playing.Cell] = "o";

            gameDb = CheckGame(gameDb);

            if (gameDb.GameFinish == null)
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
            }
            gameDb.GameUpdate = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return new OkResult();
        }
        
}