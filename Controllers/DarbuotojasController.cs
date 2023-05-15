namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;


/// <summary>
/// Controller for working with 'Darbuotojas' entity.
/// </summary>
public class DarbuotojasController : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	//naudojam list nes tai reikia matyti
	public ActionResult Index()
	{
		return View(DarbuotojasRepo.ListDarbuotojas());
	}
	
	/// <summary>
	/// This is invoked when creation form is first opened in browser.
	/// </summary>
	/// <returns>Creation form view.</returns>
	[HttpGet]
	//naudoja CE nes CE yra pritaikytas create ir edit mode, t.y. naudojami skaiciukai
	//su populate selections konvertuojame skaiciuka i pavadinima
	public ActionResult Create()
	{
		var darb = new DarbuotojasCE();
		PopulateSelections(darb);
		return View(darb);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="darb">Entity model filled with latest data.</param>
	/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Create(DarbuotojasCE darb)
	{
		
		//form field validation passed?
		if( ModelState.IsValid )
		{
			DarbuotojasRepo.Insert(darb);

			//save success, go back to the entity list
			return RedirectToAction("Index");
		}
		
		//form field validation failed, go back to the form
		PopulateSelections(darb);
		return View(darb);
	}

	/// <summary>
	/// This is invoked when editing form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to edit.</param>
	/// <returns>Editing form view.</returns>
	[HttpGet]
	public ActionResult Edit(int id)
	{
		var darb = DarbuotojasRepo.FindCE(id);
		PopulateSelections(darb);
		return View(darb);
	}
	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>		
	/// <param name="darb">Entity model filled with latest data.</param>
	/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(int id, DarbuotojasCE darb)
	{
		//form field validation passed?
		if (ModelState.IsValid)
		{
			DarbuotojasRepo.Update(darb);

			//save success, go back to the entity list
			return RedirectToAction("Index");
		}
		//form field validation failed, go back to the form
		PopulateSelections(darb);
		return View(darb);
	}

	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view.</returns>
	[HttpGet]
	public ActionResult Delete(int id)
	{
		var darb = DarbuotojasRepo.FindL(id);
		return View(darb);
	}

	/// <summary>
	/// This is invoked when deletion is confirmed in deletion form
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view on error, redirects to Index on success.</returns>
	[HttpPost]
	public ActionResult DeleteConfirm(int id)
	{
		//try deleting, this will fail if foreign key constraint fails
		try 
		{
			DarbuotojasRepo.Delete(id);

			//deletion success, redired to list form
			return RedirectToAction("Index");
		}
		//entity in use, deletion not permitted
		catch( MySql.Data.MySqlClient.MySqlException )
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;

			var darb = DarbuotojasRepo.FindL(id);
			return View("Delete", darb);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="darb">'DarbuotojasCE' view model to append to.</param>
	//skirtas tam kad parduotuvesid galetume matyti kaip jos pavadinima
	public void PopulateSelections(DarbuotojasCE darb)
	{
		//load entities for the select lists
		var parduotuves = ParduotuveRepo.ListParduotuve();

		//build select lists
		darb.Lists.Parduotuves = 
			parduotuves.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Parduotuvesid), 
						Text = it.Pavadinimas + " - " + it.Adresas + " - " + it.Miestas
					};
			})
			.ToList();
	}
}
