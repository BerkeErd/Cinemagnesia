﻿using Application.Dtos;
using Cinemagnesia.Infrastructure.DataAccess.DbContext;
using Domain.Entities.Concrete;
using Domain.Entities.Constants;
using Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        private readonly DbSet<Genre> _dbSet;
        public GenreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = _dbContext.Set<Genre>();
        }

        public List<Genre> GetGenresWithMovies()
        {
            return _dbSet.Include(c => c.Movies.Where(m => m.Status == ApprovalStatus.Approved)).ToList();
        }


        public bool IsExistsByName(string name)
        {
            return _dbSet.Any(c => c.Name == name);
        }

        public bool HasItMovie(string id)
        {
            var genre = _dbSet.Include(g => g.Movies).FirstOrDefault(g => g.Id == id);
            if (genre != null && genre.Movies != null && genre.Movies.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetMostRatedGenre()
        {
            var result = await _dbContext.Genres
         .Select(g => new
         {
             GenreName = g.Name,
             AverageScore = g.Movies.Where(m => m.Status == ApprovalStatus.Approved && m.CinemagAvgScore > 0).Average(m => m.CinemagAvgScore)
         })
         .OrderByDescending(g => g.AverageScore)
         .FirstOrDefaultAsync();

            if (result != null)
            {
                return result.GenreName;
            }
            else
            {
                return "-";
            }
        }

        public IEnumerable<GenreScoreDto> GetGenreRankings()
        {
            var genreScores = _dbContext.Genres
                .Select(g => new GenreScoreDto
                {
                    Name = g.Name,
                    Score = g.Movies
                        .Where(m => m.Status == ApprovalStatus.Approved && m.CinemagAvgScore > 0)
                        .Any() ? g.Movies
                            .Where(m => m.Status == ApprovalStatus.Approved && m.CinemagAvgScore > 0)
                            .Average(m => m.CinemagAvgScore) : 0
                })
                .OrderByDescending(g => g.Score);

            return genreScores;
        }

    }
}
