namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'ParduotuveL' entity.
/// </summary>
public class ParduotuveL
{
	[DisplayName("Identifikacijos kodas")]
	[MaxLength(11)]
	[Required]
	public int Parduotuvesid { get; set; }

	[DisplayName("Pavadinimas")]
	[MaxLength(255)]
	[Required]
	public string Pavadinimas { get; set; }

	[DisplayName("Adresas")]
	[MaxLength(255)]
	[Required]
	public string Adresas { get; set; }
	[DisplayName("Telefono Nr.")]
	[MaxLength(255)]
	[Required]
	public string Telefonas { get; set; }
	[DisplayName("El. paštas")]
	[MaxLength(255)]
	[Required]
	public string Pastas { get; set; }
	[DisplayName("Miestas")]
	[Required]
	public string Miestas {get; set; }
} 
/// <summary>
/// 'Parduotuve' in create and edit forms.
/// </summary>
public class ParduotuveCE
{
    /// <summary>
    /// Parduotuve.
    /// </summary>
    public class ParduotuveM
	{
		[DisplayName("Identifikacijos kodas")]
		[Required]
		public int Parduotuvesid { get; set; }


		[DisplayName("Pavadinimas")]
		[MaxLength(255)]
		[Required]
		public string Pavadinimas { get; set; }

		[DisplayName("Adresas")]
		[MaxLength(255)]
		[Required]
		public string Adresas { get; set; }

		[DisplayName("Telefono Nr.")]
		[MaxLength(20)]
		[Required]
		public string Telefonas { get; set; }

		[DisplayName("El. paštas")]
		[MaxLength(255)]
		[Required]
		public string Pastas { get; set; }

		[DisplayName("Miestas")]
		[Required]
		public int FkMiestas { get; set; }
	} 

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Miestai { get; set; }
	}

	/// <summary>
	/// Darbuotojas.
	/// </summary>
	public ParduotuveM Parduotuve { get ; set; } = new 	ParduotuveM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}
