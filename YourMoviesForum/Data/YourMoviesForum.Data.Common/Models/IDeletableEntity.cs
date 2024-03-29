﻿using System;

namespace YourMoviesForum.Data.Common.Models
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        string DeletedOn { get; set; }
    }
}
