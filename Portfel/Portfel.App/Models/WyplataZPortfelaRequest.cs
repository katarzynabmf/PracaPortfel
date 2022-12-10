using System.ComponentModel.DataAnnotations;

namespace Portfel.App.Models
{
    public class WyplataZPortfelaRequest
    {
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})$", ErrorMessage = "Wpisz kwotę z dokładnością do 2 miejsc po przecinku.")]
        public string Kwota { get; set; }
    }

}
