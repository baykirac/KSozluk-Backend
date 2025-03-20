// using KSozluk.WebAPI.Repositories;
// using KSozluk.WebAPI;
// using KSozluk.WebAPI.DTOs;
// using KSozluk.WebAPI.DataAccess.Contexts;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Expressions;
// using System.Text;
// using System.Threading.Tasks;
// using static System.Runtime.InteropServices.JavaScript.JSType;
// using KSozluk.WebAPI.Entities;

// namespace KSozluk.WebAPI.Repositories
// {
//     public class FavoriteWordRepository : IFavoriteWordRepository
//     {
//         private readonly SozlukContext _context;
//         public FavoriteWordRepository(SozlukContext context) { 
//             _context = context;
//         }
//         public async Task CreateAsync(FavoriteWord entity)
//         {
//             await _context.AddAsync(entity);
//         }

//         public void Delete(FavoriteWord entity)
//         {
//             _context.Remove(entity);
//         }

//         public Task DeleteAsync(Guid id)
//         {
//             throw new NotImplementedException();
//         }

//         public async Task<FavoriteWord> FindAsync(Guid? id)
//         {
//             return await _context.FavoriteWords.SingleOrDefaultAsync(d => d.Id == id);
//         }

//         public Task<List<FavoriteWord>> GetAll(Expression<Func<FavoriteWord, bool>> predicate)
//         {
//             throw new NotImplementedException();
//         }

//         public async Task<FavoriteWord> GetByFavoriteWordAndUserAsync(Guid _wordId, long? _userId)
//         {
//             var favouriteWord = await _context.FavoriteWords.FirstOrDefaultAsync(x => x.WordId == _wordId && x.UserId == _userId);
//             if (favouriteWord == null)
//             {
//                 return null;
//             }
//             return favouriteWord;
//         }

//         public async Task<List<ResponseFavouriteWordContentDto>> GetFavouriteWordsByUserIdAsync(long? _userId)
//         {
         

//             var response = await _context.FavoriteWords.Where(x => x.UserId == _userId).Select(y => new ResponseFavouriteWordsDto
//             {
//                 UserId = _userId,
//                 WordId = y.WordId,
//             }).ToListAsync();   

//             var wordIds= response.Select(x => x.WordId).ToList();

//             var words = _context.Words.Where(x => wordIds.Contains(x.Id));

//             var fav = await words
//             .OrderBy(x => x.WordContent) // Alfabetik sıralama
//             .Select(x => new ResponseFavouriteWordContentDto
//             {
//                 Id = x.Id,
//                 WordContent = x.WordContent
//             }).ToListAsync();

//             return fav;
//         }

//         public Task<FavoriteWord> GetById(Expression<Func<FavoriteWord, bool>> predicate)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }
