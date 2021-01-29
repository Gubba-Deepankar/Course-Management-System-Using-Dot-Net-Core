using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userMngr,
            SignInManager<User> signInMngr)
        {
            userManager = userMngr;
            signInManager = signInMngr;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);
                if (model.teachebl.Equals("Student"))
                { 
                if (result.Succeeded)
                {
                    var newClaim = new Claim(Claims.IsStudent, "true");
                    HttpContext.Session.SetString("ReqStudentId",model.Username);
                    TempData["ReqStuId"] = model.Username;
                    await userManager.AddClaimAsync(user, newClaim);
                    return RedirectToAction("studentRegister", "Student");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                }
                if (model.teachebl.Equals("Teacher"))
                {
                    if (result.Succeeded)
                    {
                        var newClaim = new Claim(Claims.IsTeacher, "true");
                        HttpContext.Session.SetString("ReqTeacherId", model.Username);
                        TempData["ReqStuId"] = model.Username;

                        await userManager.AddClaimAsync(user, newClaim);
                        return RedirectToAction("TeacherHomePage", "Teacher");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Username, model.Password, isPersistent: model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    //if(model.studentbl.Equals("Student"))
                    {
                        HttpContext.Session.SetString("ReqStudentId", model.Username);
                        HttpContext.Session.SetString("ReqTeacherId", model.Username);
                        TempData["ReqStuId"] = model.Username;
                    if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                        Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    }
                    /*if (model.studentbl.Equals("Teacher"))
                    {
                        HttpContext.Session.SetString("ReqStudentId", model.Username);
                        HttpContext.Session.SetString("ReqTeacherId", model.Username);
                        TempData["ReqTeaId"] = model.Username;
                        if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                            Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }*/
                }
            }
            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}