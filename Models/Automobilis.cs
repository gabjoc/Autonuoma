namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Automobilis' in list form.
/// </summary>
public class AutomobilisL
{
	[DisplayName("Id")]
	public int Id { get; set; }

	[DisplayName("Valstybinis Nr.")]
	public string ValstybinisNr { get; set; }

	[DisplayName("Būsena")]
	public string Busena { get; set; }

	[DisplayName("Modelis")]
	public string Modelis { get; set; }

	[DisplayName("Markė")]
	public string Marke { get; set; }
}

/// <summary>
/// 'Automobilis' in create and edit forms.
/// </summary>
public class AutomobilisCE
{
	/// <summary>
	/// Automobilis.
	/// </summary>
	public class AutomobilisM
	{
		[DisplayName("Id")]
		[Required]
		public int Id { get; set; }

		[DisplayName("Valstybinis Nr.")]
		[MaxLength(6)]
		[Required]
		public string ValstybinisNr { get; set; }

		[DisplayName("Pagaminimo data")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required]
		public DateTime? PagaminimoData { get; set; }

		[DisplayName("Rida")]
		[Required]
		public int Rida { get; set; }

		[DisplayName("Radijas")]
		[Required]
		public bool Radijas { get; set; }

		[DisplayName("Grotuvas")]
		[Required]
		public bool Grotuvas { get; set; }

		[DisplayName("Kondicionierius")]
		[Required]
		public bool Kondicionierius { get; set; }

		[DisplayName("Vietų skaičius")]
		[Required]
		public int VietuSkaicius { get; set; }

		[DisplayName("Registravimo data")]
		[Required]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? RegistravimoData { get; set; }

		[DisplayName("Vertė")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal Verte { get; set; }

		[DisplayName("Būsena")]
		public int? FkBusena { get; set; }

		[DisplayName("Modelis")]
		[Required]
		public int FkModelis { get; set; }

		[DisplayName("Pavarų dėžė")]
		[Required]
		public int FkPavaruDeze { get; set; }

		[DisplayName("Degalų tipas")]
		[Required]
		public int FkDegaluTipas { get; set; }

		[DisplayName("Kėbulo tipas")]
		[Required]
		public int FkKebuloTipas { get; set; }

		[DisplayName("Bagažo dydis")]
		[Required]
		public int FkLagaminas { get; set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Busenos { get; set; }
		public IList<SelectListItem> Modeliai { get; set; }
		public IList<SelectListItem> PavaruDezes { get; set; }
		public IList<SelectListItem> KebuloTipai { get; set; }
		public IList<SelectListItem> DegaluTipai { get; set; }
		public IList<SelectListItem> Lagaminai { get; set; }
	}

	/// <summary>
	/// Automobilis.
	/// </summary>
	public AutomobilisM Automobilis { get ; set; } = new AutomobilisM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}


/// <summary>
/// 'AutoBusena' enumerator in lists.
/// </summary>
public class AutoBusena
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'PavaruDeze' enumerator in lists.
/// </summary>
public class PavaruDeze
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'KebuloTipas' enumerator in lists.
/// </summary>
public class KebuloTipas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'DegaluTipas' enumerator in lists.
/// </summary>
public class DegaluTipas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'Lagaminas' enumerator in lists.
/// </summary>
public class Lagaminas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}
