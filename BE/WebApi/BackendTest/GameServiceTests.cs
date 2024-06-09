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

    [TestCase]
    public void CheckIfStalemate()
    {
        
    }
    
    [TestCase]
    public void CheckVertical()
    {
        
    }
    [TestCase]
    public void CheckHorizontal()
    {
        
    }
    [TestCase]
    public void CheckDiagnals()
    {
        
    }
    [TestCase(1)]
    public async Task MakeMove(int cell)
    {
        var currentTurn = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var game = new Game
        {
            Id = gameId,
            User1 = currentTurn,
            User2 = Guid.NewGuid(),
            BoardState = new string[9],
            CurrentTurn = currentTurn
        };
        var playingModel = new PlayingModel { GameID = gameId, Cell = 0 };
        _dataContext.Setup(dc => dc.Games.Find(It.IsAny<int>())).Returns(game);
        //_dataContext.Setup(dc => dc.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _gameService.MakeMove(playingModel);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}