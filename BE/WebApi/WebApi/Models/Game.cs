namespace WebApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public UserModel User1 { get; set; }
        public UserModel? User2 { get; set; }
        public UserModel? Winner {  get; set; }
        public UserModel? FirstMove { get; set; }
        public int TurnCount {  get; set; }
        public string BoardState {  get; set; }
        public string GameState {  get; set; }
        public DateTime GameCreation {  get; set; }
    }
}
