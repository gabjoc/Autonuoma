namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Klientas' entity.
/// </summary>
public class Klientas
{
	[DisplayName("Pirkėjo numeris")]
	public int PirkejasNr { get; set; }
	
	[DisplayName("Vardas")]
	[Required]
	public string Vardas { get; set; }

	[DisplayName("Pavardė")]
	[Required]
	public string Pavarde { get; set; }

	[DisplayName("Banko sąskaitos Nr.")]
	[Required]
	public string Saskaita { get; set; }

	[DisplayName("Adresas")]
	[Required]
	public string Adresas { get; set; }

	[DisplayName("Telefono Nr.")]
	[Required]
	public string Telefonas { get; set; }

	[DisplayName("Elektroninis paštas")]
	[Required]
	public string Epastas { get; set; }
}
