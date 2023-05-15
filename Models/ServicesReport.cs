namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// View model for a single service in services report.
/// </summary>
public class Uzsakymas
{
	[DisplayName("Klientas")]
	public string Klientas { get; set; }

	[DisplayName("Užsakymo nr.")]
	public int UzsakymoNr { get; set; }

	[DisplayName("Data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime UzsakymoData { get; set; }

	[DisplayName("Prekių kiekis")]
	public int Kiekis { get; set; }

	[DisplayName("Suma")]
	public decimal Suma { get; set; }

	[DisplayName("Aptarnavęs darbuotojas")]
	public string darbuotojas { get; set; }
}

/// <summary>
/// View model of the whole report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }

	public List<Uzsakymas> Uzsakymai { get; set; }

	public int VisoUzsakyta { get; set; }

	public decimal BendraSuma { get; set; }
}
