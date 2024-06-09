using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Interfaces.Services;

public interface IGameService
{
    protected DataContext _dataContext { get; set; }

    public Game SetGameStart(Game game);

    public Game CheckGame(Game game);

    protected bool CheckIfBoardIsFull(Game game);

    protected bool CheckVertical(Game game);

    protected bool CheckHorizontal(Game game);
    protected bool CheckDiagnals(Game game);
    
    public Task<IActionResult> MakeMove(PlayingModel playing);
}