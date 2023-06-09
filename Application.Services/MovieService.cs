﻿using Application.Dtos;
using Application.Interfaces.AppInterfaces;
using AutoMapper;
using Cinemagnesia.Infrastructure.DataAccess.DbContext;
using Domain.Entities.Concrete;
using Domain.Entities.Constants;
using Domain.Interfaces.Repository;
using Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ApplicationDbContext _dbContext;
        public MovieService(IMovieRepository movieRepository, IMapper mapper, ApplicationDbContext dbContext, ICompanyRepository companyRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _movieRepository = movieRepository;
            _companyRepository = companyRepository;
        }
        public void AddMovie(AddMovieDto movieDto)
        {
            if (movieDto != null)
            {
                var directors = new List<Director>();
                foreach (var director in movieDto.Directors)
                {
                    var existingDirector = _dbContext.Directors.FirstOrDefault(d => d.Name == director.Name);
                    directors.Add(existingDirector ?? director);
                }

                var genres = new List<Genre>();
                foreach (var genre in movieDto.Genres)
                {
                    var existingGenre = _dbContext.Genres.FirstOrDefault(g => g.Name == genre.Name);
                    genres.Add(existingGenre ?? genre);
                }

                var castMembers = new List<CastMember>();
                foreach (var castMember in movieDto.CastMembers)
                {
                    var existingCastMember = _dbContext.CastMembers.FirstOrDefault(c => c.Name == castMember.Name);
                    castMembers.Add(existingCastMember ?? castMember);
                }

                var movie = new Movie
                {
                    CompanyId = movieDto.CompanyId,
                    Title = movieDto.Title,
                    Description = movieDto.Description,
                    PosterPath = movieDto.PosterPath,
                    ReleaseDate = movieDto.ReleaseDate,
                    ImdbRating = movieDto.ImdbRating,
                    Status = movieDto.Status,
                    TrailerUrl = movieDto.TrailerUrl,
                    Directors = directors,
                    Genres = genres,
                    CastMembers = castMembers,
                    MovieMinutes = movieDto.MovieMinutes,
                    Language = movieDto.Language,
                    CreatedAt = movieDto.CreatedAt,
                    UpdatedAt = movieDto.UpdatedAt
                };

                _movieRepository.CreateAsync(movie).Wait();
            }

        }

        public List<MovieDto> GetAllWaitingMovies()
        {
            var movies = _movieRepository.GetAllWaitingMovies().ToList();
            if (movies != null)
            {
                var movieDtos = _mapper.Map<List<MovieDto>>(movies);

                foreach (var movieDto in movieDtos)
                {
                    var companyId = movies.Where(m => m.Id == movieDto.Id).Select(m => m.CompanyId).FirstOrDefault();
                    var companyName = _dbContext.Companies.Where(c => c.Id == companyId).Select(c => c.Name).FirstOrDefault();
                    movieDto.CompanyName = companyName;
                }

                return movieDtos;
            }
            else
            {
                return new List<MovieDto>();
            }
        }

        public List<MovieDto> GetAllMovieswithLikes()
        {
            var movies = _movieRepository.GetAllMovieswithLikes().ToList();
            if (movies != null)
            {
                var movieDto = _mapper.Map<List<MovieDto>>(movies);
                return movieDto;
            }
            return new List<MovieDto>();
        }
        public List<MovieDto> GetMoviesByCompanyId(string companyId, bool includeMovies = false)
        {
            if (companyId != null)
            {
                var company = _companyRepository.GetByIdAsync(companyId, includeMovies).Result;

                if (company == null)
                {
                    throw new NotFoundException($"Company with ID {companyId} not found");
                }

                var movies = company.Movies;

                return _mapper.Map<List<MovieDto>>(movies);
            }
            else { return new List<MovieDto>(); }
        }
        public MovieDto GetMovieDtoById(string id)
        {
            if (id != null)
            {
                var movie = _movieRepository.GetMovieById(id);
                var movieDto = _mapper.Map<MovieDto>(movie);
                return movieDto;
            }
            else
            {
                return new MovieDto();
            }
        }
        public List<HomeMovieDto> GetAllHomeMovies()
        {
            var movies = _movieRepository.GetAllHomeMovies().ToList();
            if (movies != null)
            {
                var movieDtos = _mapper.Map<List<HomeMovieDto>>(movies);

                return movieDtos;
            }
            else
            {
                return new List<HomeMovieDto>();
            }

        }
        public Movie GetMovieById(string id)
        {
            if (id != null)
            {
                return _movieRepository.GetByIdAsync(id).Result;
            }
            else
            {
                return new Movie();
            }

        }

        public void RemoveMovie(string id)
        {
            if (id != null)
            {
                _movieRepository.DeleteAsync(id).Wait();
            }

        }

        public string ComfirmMovie(string id)
        {
            if (id != null)
            {
                var movies = _movieRepository.GetAllWaitingMovies();
                if (movies != null)
                {
                    Movie dbMovie = movies.SingleOrDefault(x => x.Id == id); // belirtilen id'ye sahip filmi bul

                    if (dbMovie != null) // film veritabanında varsa güncelleme yap
                    {
                        dbMovie.Status = ApprovalStatus.Approved;
                        dbMovie.UpdatedAt = DateTime.Now;

                        var updatedMovie = _movieRepository.Update(id, dbMovie);

                        if (updatedMovie != null)
                        {
                            return "Başarılı";
                        }
                        else
                        {
                            return "Güncelleme başarısız";
                        }
                    }
                    else // belirtilen id'ye sahip bir film bulunamazsa hata fırlat
                    {
                        return "Invalid movie id";
                    }
                }
                return "";
            }
            else
            {
                return "";
            }

        }

        public string RejectMovie(string id)
        {
            if (id != null)
            {
                var movies = _movieRepository.GetAllWaitingMovies();
                if (movies != null)
                {
                    Movie dbMovie = movies.SingleOrDefault(x => x.Id == id); // belirtilen id'ye sahip filmi bul

                    if (dbMovie != null) // film veritabanında varsa güncelleme yap
                    {
                        dbMovie.Status = ApprovalStatus.Rejected;
                        dbMovie.UpdatedAt = DateTime.Now;

                        var updatedMovie = _movieRepository.Update(id, dbMovie);

                        if (updatedMovie != null)
                        {
                            return "Başarılı";
                        }
                        else
                        {
                            return "Güncelleme başarısız";
                        }
                    }
                    else // belirtilen id'ye sahip bir film bulunamazsa hata fırlat
                    {
                        return "Invalid movie id";
                    }
                }
                return "";
            }
            else
            {
                return "";
            }

        }

        public void AddToRatedUsersList(string userId, string movieId)
        {
            if (userId != null && movieId != null)
            {
                _movieRepository.AddToRatedUsersList(userId, movieId);
            }

        }
        public int GetNumOfActiveMovies()
        {
            return _movieRepository.GetNumOfActiveMovies();
        }
        public List<MovieRankingDto> GetMovieRankings()
        {
            return _movieRepository.GetMovieRankings();
        }

        public async Task<List<LanguageStatisticDto>> GetLanguageStatistics()
        {
            return await _movieRepository.GetLanguageStatistics();
        }

        public async Task<string> GetMostRatedMovie()
        {
            return await _movieRepository.GetMostRatedMovie();
        }
    }
}
