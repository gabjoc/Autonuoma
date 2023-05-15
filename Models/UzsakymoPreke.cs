namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

/// <summary>
/// 'PrekesLikutis' in create and edit forms.
/// </summary>
public class UzsakymoPreke
{
    /// <summary>
    /// Preke.
    /// </summary>
    public class UzsakymoPrekeM
	{
		public int InListId { get; set; }

		public int? FkUzsakymas { get; set; }

		[DisplayName("Kiekis")]
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Kiekis must be a positive number.")]
		public int Kiekis { get; set; }
	
		[DisplayName("Prekė")]
		[Required]
		public int FkPreke { get; set; }
		
	} 
	
	/// <summary>
	/// Likutis.
	/// </summary>
	public UzsakymoPrekeM Uzsakymopreke { get ; set; } = new UzsakymoPrekeM();
}
