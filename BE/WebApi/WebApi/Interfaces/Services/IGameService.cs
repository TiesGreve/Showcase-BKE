using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Interfaces.Services;

public interface IGameService
{
    protected DataContext _dataContext { get; set; }

    public Game SetGameStart(Game game);

    public Game CheckGame(Game game);

    protected bool CheckIfStalemate(Game game);

    protected bool CheckVertical(string[] board);

    protected bool CheckHorizontal(string[] board);
    protected bool CheckDiagnals(string[] board);
    
    public Task<IActionResult> MakeMove(PlayingModel playing);
}