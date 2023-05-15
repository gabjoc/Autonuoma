namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Preke;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Preke' entity in list form.
/// </summary>
public class PrekeL
{
	[DisplayName("Kodas")]
	[MaxLength(11)]
	[Required]
	public int PrekesKodas { get; set; }

	[DisplayName("Pavadinimas")]
	[MaxLength(255)]
	[Required]
	public string Pavadinimas { get; set; }

	[DisplayName("Kaina")]
	[Required]
	public decimal Kaina { get; set; }
	[DisplayName("Galiojimo trukmė (mėn.)")]
	[MaxLength(11)]
	[Required]
	public int Galiojimas { get; set; }
	[DisplayName("Kategorija")]
	[MaxLength(255)]
	[Required]
	public string Kategorija { get; set; }
	[DisplayName("Gamintojas")]
	[MaxLength(255)]
	[Required]
	public string Gamintojas { get; set; }
}
/// <summary>
/// 'Preke' in create and edit forms.
/// </summary>
public class PrekeCE
{
    /// <summary>
    /// Preke.
    /// </summary>
    public class PrekeM
	{
		[DisplayName("Kodas")]
		[Required]
		public int PrekesKodas { get; set; }

		[DisplayName("Pavadinimas")]
		[MaxLength(255)]
		[Required]
		public string Pavadinimas { get; set; }
		[DisplayName("Sudėtis")]
		[MaxLength(255)]
		public string Sudetis { get; set; }

		[DisplayName("Kaina")]
		[Range(0, 3000, ErrorMessage = "Kaina must be positive number.")]
		[Required]
		public decimal Kaina { get; set; }

		[DisplayName("Aprašymas")]
		[MaxLength(255)]
		[Required]
		public string Aprasymas { get; set; }

		[DisplayName("Įspėjimai")]
		[MaxLength(255)]
		[Required]
		public string Ispejimai { get; set; }
		[DisplayName("Galiojimo trukmė (mėn.)")]
		[Range(0, 100, ErrorMessage = "Galiojimas must be positive number.")]
		[Required]
		public int Galiojimas { get; set; }

		[DisplayName("Kategorija")]
		[Required]
		public int FkKategorija { get; set; }
		[DisplayName("Gamintojas")]
		[Required]
		public int FkGamintojas { get; set; }
	} 

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Kategorijos { get; set; }
		public IList<SelectListItem> Gamintojai { get; set; }
		public IList<SelectListItem> Parduotuves { get; set; }
	}
	
	/// <summary>
	/// Preke.
	/// </summary>
	public PrekeM Preke { get ; set; } = new PrekeM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();

	public List<PrekesLikutis> Likuciai { get; set; } = new List<PrekesLikutis>();

}

