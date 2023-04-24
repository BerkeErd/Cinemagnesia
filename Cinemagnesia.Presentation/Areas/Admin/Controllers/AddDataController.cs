﻿using Application.Dtos;
using Application.Interfaces.AppInterfaces;
using AutoMapper;
using Cinemagnesia.Presentation.Areas.Admin.Models;
using Cinemagnesia.Presentation.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace Cinemagnesia.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AddDataController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGenreService _genreService;

        public AddDataController(IMapper mapper, IGenreService genreService)
        {
            _genreService = genreService; 
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult AddGenre(AddGenreViewModel addGenreViewModel)
        {
            if (ModelState.IsValid)
            {
                GenreDto genreDto = _mapper.Map<GenreDto>(addGenreViewModel);
                _genreService.AddGenre(genreDto);
                return RedirectToAction("Index");
            }
            return BadRequest("Eklenmedi");
        }

        [HttpGet]
        public IActionResult ListGenres()
        {
            IQueryable<GenreDto> genreDtos = _genreService.GetAllGenres().AsQueryable();
            IQueryable<GenreViewModel> genreViewModels = _mapper.ProjectTo<GenreViewModel>(genreDtos);

            return Ok(genreViewModels);
        }

    }
}
