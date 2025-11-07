namespace ScrewGameCard.Contract.DTO.CreateRoom
{
    public class CreateRoomResponse
    {
        public GameRoomDto? Room { get; set; }
        public bool IsCreated { get; set; }
    }
}
