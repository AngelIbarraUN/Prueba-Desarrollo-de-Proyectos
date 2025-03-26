using System.ComponentModel.DataAnnotations;

public class GoogleFormViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
    public string Email { get; set; }

    [Required]
    public string Nombre { get; set; }
}
