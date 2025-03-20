// using KSozluk.WebAPI.DTOs;
// using KSozluk.WebAPI.Entities;
// using KSozluk.WebAPI.DataAccess.Contexts;
// using KSozluk.WebAPI.SharedKernel;
// using System.Threading;
// using System.Threading.Tasks;

// namespace KSozluk.WebAPI.Repositories
// {
//     public class Unit: IUnit
//     {
//         private readonly SozlukContext _context;

//         public Unit(SozlukContext context)
//         {
//             _context = context;
//         }

//         public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//         {
//             return await _context.SaveChangesAsync(cancellationToken);
//         }
//     }
// }