﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TareasMVC.Models;

namespace TareasMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,

            };

            var resultado = await _userManager.CreateAsync(usuario, password: model.Password);

            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, isPersistent: true);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }


        }

        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModels model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de Usuario o Password incorrectos.");

                return View(model);
            }


        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return RedirectToAction("Login", "usuarios");
        }


        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = _signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");

            var mensaje = "";

            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";

                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                mensaje = "Error cargando la data de  login externo";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoLoginExterno = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            //Yala cuenta existe:
            if (resultadoLoginExterno.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }

            string email = string.Empty;

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usaurio del preveedor!";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var usuario = new IdentityUser { Email = email, UserName = email };

            var resultadoCrearUsuaurio = await _userManager.CreateAsync(usuario);

            if (!resultadoCrearUsuaurio.Succeeded)
            {
                mensaje = resultadoCrearUsuaurio.Errors.First().Description;

                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var resultadoAgregarLogin = await _userManager.AddLoginAsync(usuario, info);

            if (resultadoAgregarLogin.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);

                return LocalRedirect(urlRetorno);
            }

            mensaje = "Ha occuriddo un error agregado el login";

            return RedirectToAction("Login", routeValues: new { mensaje });

        }

    }
}
