﻿namespace ForumTemplate.Models
{
    public class PostQueryParameters
    {
        public string Title { get; set; } 
        public string Likes { get; set; }    
        public string MinLikes { get; set; }
        public string MaxLikes { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
