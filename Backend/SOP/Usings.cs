global using System.Text;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;

global using SOP.Controllers;

global using Microsoft.EntityFrameworkCore;
global using SOP.Database;
global using SOP.Repositories;

global using Microsoft.OpenApi.Models;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using SOP.Entities;
global using SOP.DTOs;

global using SOP.Helpers;

global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

global using AllowAnonymousAttribute = SOP.Authorization.AllowAnonymousAttribute;
global using AuthorizeAttribute = SOP.Authentication.AuthorizeAttribute;
global using SOP.Authorization;