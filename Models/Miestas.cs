namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Miestas' entity.
/// </summary>
public class Miestas
{
    [DisplayName("ID")]
	public int Id { get; set; }

    [DisplayName("Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }
}