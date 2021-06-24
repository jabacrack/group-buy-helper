using System.ComponentModel.DataAnnotations;

namespace GroupBuyHelper.Dtos
{
    public class ImportRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ColumnSeparator { get; set; }
        [Required]
        public string NumberSeparator { get; set; }
        public string CurrencySymbol { get; set; }
        [Required]
        public string Items { get; set; }
    }
}