namespace MovieTicketBooking.DTOs.Shows
{
    public class ShowSeatDetailDto
    {
        public int ShowSeatId { get; set; }
        public int ShowId { get; set; }
        public int SeatId { get; set; }
        public string RowLetter { get; set; } = string.Empty;
        public int ColumnNumber { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

