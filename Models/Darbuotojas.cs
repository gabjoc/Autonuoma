namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Darbuotojas' entity in list form.
/// </summary>
public class DarbuotojasL
{
	[DisplayName("Tabelio Nr.")]
	[MaxLength(11)]
	[Required]
	public int Tabelis { get; set; }

	[DisplayName("Vardas")]
	[MaxLength(220)]
	[Required]
	public string Vardas { get; set; }

	[DisplayName("Pavardė")]
	[MaxLength(220)]
	[Required]
	public string Pavarde { get; set; }
	[DisplayName("Telefono Nr.")]
	[MaxLength(220)]
	[Required]
	public string Telefonas { get; set; }
	[DisplayName("El. paštas")]
	[MaxLength(220)]
	public string Pastas { get; set; }
	[DisplayName("Parduotuvė")]
	[Required]
	public string Parduotuve { get; set; }
}
/// <summary>
/// 'Darbuotojas' in create and edit forms.
/// </summary>
public class DarbuotojasCE
{
    /// <summary>
    /// Darbuotojas.
    /// </summary>
    public class DarbuotojasM
	{
		[DisplayName("Tabelio Nr.")]
		[Required]
		public int Tabelis { get; set; }

		[DisplayName("Vardas")]
		[MaxLength(255)]
		[Required]
		public string Vardas { get; set; }

		[DisplayName("Pavardė")]
		[MaxLength(255)]
		[Required]
		public string Pavarde { get; set; }

		[DisplayName("Telefono Nr.")]
		[MaxLength(255)]
		[Required]
		public string Telefonas { get; set; }

		[DisplayName("El. paštas")]
		[MaxLength(255)]
		
		public string Pastas { get; set; }

		[DisplayName("Parduotuvė")]
		[Required]
		public int FkParduotuve { get; set; }
	} 

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Parduotuves { get; set; }
	}

	/// <summary>
	/// Darbuotojas.
	/// </summary>
	public DarbuotojasM Darbuotojas { get ; set; } = new DarbuotojasM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}
