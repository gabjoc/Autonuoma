namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Gamintojas' entity.
/// </summary>
public class Gamintojas
{
    [DisplayName("ID")]
	public int Id { get; set; }
    [DisplayName("Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }
    [DisplayName("Šalis")]
	public string Salis { get; set; }
    
}