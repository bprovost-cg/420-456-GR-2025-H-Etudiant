using Exercice_Formulaire_Etudiant.Data;
using Exercice_Formulaire_Etudiant.Models;
using Exercice_Formulaire_Etudiant.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Formulaire_Etudiant.Services
{
    public class EmployesService
    {
        private readonly ApplicationDbContext _context;

        public EmployesService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*************** SYNCHRONE *****************/

        public Employe? Get(int id)
        {
            return _context.Employes.Find(id);
        }

        public int Create(Employe employe)

        {
            _context.Add(employe);
            return _context.SaveChanges();
        }

        public int Update(Employe employe)
        {
            return _context.SaveChanges();
        }

        public int Remove(Employe employe)
        {
            _context.Remove(employe);
            return _context.SaveChanges();
        }


        /*************** ASYNCHRONE *****************/


        public async Task<Employe?> GetAsync(int id)
        {
            return await _context.Employes.FindAsync(id);
        }

        public async Task<int> CreateAsync(Employe employe)

        {
            await _context.AddAsync(employe);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Employe employe)
        {
            //Pas de version async disponible
            _context.Update(employe);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(Employe employe)
        {
            //Pas de version async disponible
            _context.Remove(employe);
            return await _context.SaveChangesAsync();
        }
    }
}
