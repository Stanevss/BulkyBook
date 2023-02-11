using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
       
        return View();
    }

    // GET
    public IActionResult Create()
    {

        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Company obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);

    }


    //GET
    public IActionResult Upsert(int? id)
    {
        Company company = new();
       
        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company  = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            return View(company);

            //update Company
        }

    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj)
    {

        if (ModelState.IsValid)
        {

            if (obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
                TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        return View(obj);

    }

    // GET
    //public IActionResult Delete(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }
    //    var CompanyFromDbFirst = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

    //    if (CompanyFromDbFirst == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(CompanyFromDbFirst);
    //}


    

#region API CALLS
[HttpGet]
public IActionResult GetAll()
{
    var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
}



    //POST
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }


        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });

    }


    #endregion

}