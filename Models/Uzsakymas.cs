namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Uzsakymas;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Automobilis' in list form.
/// </summary>
public class UzsakymasL
{
	[DisplayName("Užsakymo Nr.")] 
	public int Uzsakymas { get; set; }

	[DisplayName("Užsakymo data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
	[Required]
    public DateTime UzsakymoData { get; set; }

	[DisplayName("Suma")]
	public double Suma { get; set; }

	[DisplayName("Būsena")]
	public string Busena { get; set; }

	[DisplayName("Pristatymo tipas")]
	public string Pristatymas { get; set; }

    [DisplayName("Klientas")]
    public string Klientas { get; set; }
}

/// <summary>
/// 'Automobilis' in create and edit forms.
/// </summary>
public class UzsakymasCE
{
	/// <summary>
	/// Uzsakymas.
	/// </summary>
	public class UzsakymasM
	{
		[DisplayName("Užsakymo Nr.")]
		[Required]
		public int Uzsakymas { get; set; }

        [DisplayName("Užsakymo data")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
	    [Required]
        public DateTime UzsakymoData { get; set; } = DateTime.Now;

		[DisplayName("Suma")]
        [Range(0, 10000, ErrorMessage = "Suma must be positive number.")]
		[Required]
		public double Suma { get; set; }

		[DisplayName("Adresas")]
		[Required]
		public string Adresas { get; set; }

		[DisplayName("Būsena")]
		public int FkBusena { get; set; }

		[DisplayName("Pristatymo tipas")]
		[Required]
		public int FkPristatymas { get; set; }

		[DisplayName("Klientas")]
		[Required]
		public int FkKlientas { get; set; }

		[DisplayName("Darbuotojas")]
		[Required]
		public int FkDarbuotojas { get; set; }


	} 


    /// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Busenos { get; set; }
		public IList<SelectListItem> Pristatymai { get; set; }
		public IList<SelectListItem> Klientai { get; set; }
		public IList<SelectListItem> Darbuotojai { get; set; }
		public IList<SelectListItem> Prekes { get; set; }
	}

	/// <summary>
	/// Automobilis.
	/// </summary>
	public UzsakymasM Uzsakymas { get ; set; } = new UzsakymasM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();

	public List<UzsakymoPreke> Uzsakymoprekes { get; set; } = new List<UzsakymoPreke>();
}


/// <summary>
/// 'UzsakymoBusena' enumerator in lists.
/// </summary>
public class UzsakymoBusena
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'Pristatymai' enumerator in lists.
/// </summary>
public class Pristatymai
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

