using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Interfaces.Services;
using WebApi.Models;
using NUnit.Framework;

namespace BackendTest;

public class Tests
{

    private Mock<DataContext> _dataContext;
    private GameService _gameService;
    
    [SetUp]
    public void Setup()
    {
        _dataContext = new Mock<DataContext>(new DbContextOptions<DataContext>() );
        _gameService = new GameService(_dataContext.Object);
    }

    [TestCase]
    public void Gamestart_NoParameters_SuccesfullStart()
    {
        var game = _gameService.SetGameStart(new Game
        {
            User1 = Guid.NewGuid(),
            User2 = Guid.NewGuid()
        });
        
        Assert.IsNotNull(game.CurrentTurn);
        Assert.IsNotNull(game.FirstMove);
        Assert.IsNotNull(game.GameStart);
        Assert.That(actual: game.FirstMove, Is.EqualTo(game.CurrentTurn));
    }
    
    [TestCase(new string[]{ "x", "o", null, "x", "o", "o", "x", null, null }, GameState.Finnished)]
    [TestCase(new string[]{ "x", "x", "x", null, "o", "o", "o", null, null }, GameState.Finnished)]
    [TestCase(new string[]{ "x", "x", "x", null, "x", "o", "o", null, null }, GameState.Finnished)]
    [TestCase(new string[]{ "x", "o", null, null, "x", "o", "o", null, "x" }, GameState.Finnished)]
    [TestCase(new string[]{ "o", "x", "x", "o", "x", "o", "x", null, null }, GameState.Finnished)]
    [TestCase(new string[]{ "x", "x", "x", "x", "x", "x", "x", "x", "x" }, GameState.Finnished)]
    [TestCase(new string[]{ "o", "o", "o", "o", "o", "o", "o", "o", "o" }, GameState.Finnished)]
    [TestCase(new string[]{ null, null, null, null, null, null, null, null, null}, GameState.Starting)]
    [TestCase(new string[]{ "x", "x", "o", "o", "o", "x", "x", "x", "o" }, GameState.Draw)]
    public void CheckGame_DifferentBoardConfigurations_GameResultChanges(string[] board, GameState result)
    {
        
        var game = _gameService.CheckGame(new Game()
        {
            BoardState = board
        });
        Console.Write(game.GameState);
        Assert.That(actual: game.GameState, Is.EqualTo(result));
    }

    [TestCase(new string[]{ null, "o", null, "x", "o", "o", "x", null, null}, false)]
    [TestCase(new string[]{ null,null,null,null,null,null,null,null,null,}, false)]
    [TestCase(new string[]{ "x", "o", null, "x", "o", "o", "x", null, null}, false)]
    public void CheckIfBoardIsFull_NoStaleMateBoards_False(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckIfBoardIsFull(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ "x", "o", "x", "x", "o", "x", "o", "x", "o"}, true)]
    [TestCase(new string[]{ "x","x","x","x","x","x","x","x","x",}, true)]
    public void CheckIfBoardIsFull_FullBoards_True(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckIfBoardIsFull(game), Is.EqualTo(result));
    }
    
    [TestCase(new string[]{ "x", "x", "x", "o", "o", "o", "x", "x", "x"}, false)]
    [TestCase(new string[]{  null,null,null,null,null,null,null,null,null}, false)]
    [TestCase(new string[]{ "x", "o", "x", "o", "x", "o", "x", "o", "x"}, false)]
    public void CheckVertical_NoVerticalWins_False(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckVertical(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ "x", "o", "x", "x", "o", "o", "x", "x", "o"}, true)]
    [TestCase(new string[]{ "o", "x", "o", "o", "o", "o", "o", "x", "x"}, true)]
    public void CheckVertical_VerticalWins_True(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckVertical(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ "x", "o", "x", "x", "o", "o", "x", "x", "o"}, false)]
    [TestCase(new string[]{ null,null,null,null,null,null,null,null,null}, false)]
    [TestCase(new string[]{ "o", "x", "o", "o", "x", "o", "o", "x", "x"}, false)]
    public void CheckHorizontal_NoHorizontalWins_False(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckHorizontal(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ "x", "x", "x", "o", "o", "o", "x", "x", "x"}, true)]
    [TestCase(new string[]{ "o", "x", "o", "o", "o", "o", "o", "x", "x"}, true)]
    public void CheckHorizontal_HorizontalWins_True(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckHorizontal(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ null,null,null,null,null,null,null,null,null}, false)]
    [TestCase(new string[]{ "o", "x", "o", "x", "x", "x", "o", "x", "o"}, false)]
    public void CheckDiagnals_NoDiagnalWins_False(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckDiagnals(game), Is.EqualTo(result));
    }
    [TestCase(new string[]{ "o", "x", "o", "x", "o", "x", "o", "o", "x"}, true)]
    [TestCase(new string[]{ "o", "x", "o", "x", "o", "x", "x", "o", "o"}, true)]
    public void CheckDiagnals_DiagnalWins_True(string[] board, bool result)
    {
        var game = new Game()
        {
            BoardState = board
        };
        Assert.That(actual: _gameService.CheckDiagnals(game), Is.EqualTo(result));
    }
    [TestCase]
    public async Task MakeMove_ValidInput_Okresult()
    {
        var currentTurn = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var game = new Game
        {
            Id = gameId,
            User1 = currentTurn,
            User2 = Guid.NewGuid(),
            BoardState = new string[9],
            CurrentTurn = currentTurn,
            GameState = GameState.Player1Move
        };
        var playingModel = new PlayingModel { GameID = gameId, Cell = 0 };
        var dbSetMock = new Mock<DbSet<Game>>();
        _dataContext.Object.Games = dbSetMock.Object;
        dbSetMock.Setup(g => g.Find(It.IsAny<Guid>())).Returns(game);

        var result = await _gameService.MakeMove(playingModel);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [TestCase(-1)]
    [TestCase(9)]
    public async Task MakeMove_CellOutOfRange_BadRequestResult(int cell)
    {
        var gameId = Guid.NewGuid();
        var game = new Game
        {
            Id = gameId,
        };
        var playingModel = new PlayingModel { GameID = gameId, Cell =  cell};
        var dbSetMock = new Mock<DbSet<Game>>();
        _dataContext.Object.Games = dbSetMock.Object;
        dbSetMock.Setup(g => g.Find(It.IsAny<Guid>())).Returns(game);

        var result = await _gameService.MakeMove(playingModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
    [TestCase]
    public async Task MakeMove_NoActiveGame_BadRequestResult()
    {
        var gameId = Guid.NewGuid();
        var playingModel = new PlayingModel { GameID = gameId, Cell =  1};
        var dbSetMock = new Mock<DbSet<Game>>();
        _dataContext.Object.Games = dbSetMock.Object;
        dbSetMock.Setup(g => g.Find(It.IsAny<Guid>())).Returns(value: null);
        var result = await _gameService.MakeMove(playingModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
    [TestCase]
    public async Task MakeMove_NoSecondPlayer_BadRequestResult()
    {
        var gameId = Guid.NewGuid();
        var playingModel = new PlayingModel { GameID = gameId, Cell =  1};
        var game = new Game
        {
            Id = gameId,
            User1 = Guid.NewGuid()
        };
        var dbSetMock = new Mock<DbSet<Game>>();
        _dataContext.Object.Games = dbSetMock.Object;
        dbSetMock.Setup(g => g.Find(It.IsAny<Guid>())).Returns(value: game);
        var result = await _gameService.MakeMove(playingModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
    [TestCase]
    public async Task MakeMove_CellAlreadyFilledIn_BadRequestResult()
    {
        
        var currentTurn = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var playingModel = new PlayingModel { GameID = gameId, Cell =  1};
        string[] board = new string[9];
        board[1] = "x";
        
        var game = new Game
        {
            Id = gameId,
            User1 = currentTurn,
            User2 = Guid.NewGuid(),
            BoardState = board,
            CurrentTurn = currentTurn
        };
        
        var dbSetMock = new Mock<DbSet<Game>>();
        _dataContext.Object.Games = dbSetMock.Object;
        dbSetMock.Setup(g => g.Find(It.IsAny<Guid>())).Returns(value: game);
        var result = await _gameService.MakeMove(playingModel);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
    [TestCase]
    public async Task HandleMove_()
    {
        
        var currentTurn = Guid.NewGuid();
        var game = new Game
        {
            Id = Guid.NewGuid(),
            User1 = currentTurn,
            User2 = Guid.NewGuid(),
            BoardState = new string[9],
            CurrentTurn = currentTurn
        };
        var result = await _gameService.HandleMove(game);
        
        Assert.That(actual: result.CurrentTurn, Is.EqualTo(game.User2));
        Assert.That(actual: result.GameState, Is.EqualTo(GameState.Player2Move));
    }
}