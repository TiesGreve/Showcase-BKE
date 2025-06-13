using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }
        public Guid User1 { get; set; }
        public Guid? User2 { get; set; }
        public Guid? Winner {  get; set; }
        public Guid? FirstMove { get; set; }
        public Guid? CurrentTurn {  get; set; }
        public int TurnCount {  get; set; }

        public static int boardSize = 3;
        public string[] BoardState {  get; set; }
        public GameState GameState {  get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "timestamp")]
        public DateTime? StartedAt {  get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? FinishedAt { get; set; }
    }
    public enum GameState
    {
        Starting,
        Player1Move,
        Player2Move,
        Draw,
        Finnished
    }
}
