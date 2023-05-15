namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Kategorija' entity.
/// </summary>
public class Kategorija
{
    [DisplayName("ID")]
	public int Id { get; set; }

    [DisplayName("Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }
}