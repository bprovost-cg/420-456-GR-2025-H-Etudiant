using GestionDeProjets.Data;
using GestionDeProjets.Models;
using GestionDeProjets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly GestionDeProjetsContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly PerformanceComparisonService _performanceService;

    public HomeController(
        GestionDeProjetsContext context,
        ILogger<HomeController> logger,
        PerformanceComparisonService performanceService)
    {
        _context = context;
        _logger = logger;
        _performanceService = performanceService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Comparaison()
    {
        return View();
    }

    public async Task<IActionResult> PerformanceTest(int id = 1)
    {
        if (!await _context.Projets.AnyAsync(p => p.Id == id))
        {
            return NotFound();
        }

        var results = await _performanceService.CompareLoadingPerformance(id);

        return View(results);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}