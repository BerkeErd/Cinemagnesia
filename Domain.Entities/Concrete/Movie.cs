﻿using Cinemagnesia.Domain.Domain.Entities.Concrete;
using Domain.Entities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    public class Movie : BaseEntity
    {
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        public string PosterPath { get; set; }
        public DateTime ReleaseDate { get; set; }
        [Range(0, 10f)]
        public float ImdbRating { get; set; }
        [Range(0, 5f)]
        public float CinemagAvgScore { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Waiting;
        public string TrailerUrl { get; set; }
        public ICollection<Director> Directors { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<CastMember> CastMembers { get; set; }
        public ICollection<MovieComment> MovieComments { get; set; }
        public ICollection<ApplicationUser> RatedUsers { get; set; }
        public ICollection<ApplicationUser> FavoritedUsers { get; set; }
        public int MovieMinutes { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        

    }
}
