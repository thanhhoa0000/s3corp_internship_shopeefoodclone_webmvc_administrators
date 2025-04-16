global using System.Text;
global using System.Text.Json;
global using System.Diagnostics;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Globalization;
global using System.ComponentModel.DataAnnotations;
global using System.Reflection;

global using Microsoft.AspNetCore.DataProtection;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;

global using NLog;
global using NLog.Web;

global using ShopeeFoodClone.WebMvc.Administrators.Application;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Configurations;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Models.Dtos;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Models.Responses;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Models.ViewModels;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Models.Enums;
global using ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

global using ShopeeFoodClone.WebMvc.Administrators.Presentation.Middlewares;
global using ShopeeFoodClone.WebMvc.Administrators.Presentation.Extensions;
