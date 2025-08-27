namespace LibrarySystemApiV2.Dtos
{
    public class CreateBookCopyDto
    {
        public int BookId { get; set; }
        public string BookNumber{ get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty ;
    }
}
