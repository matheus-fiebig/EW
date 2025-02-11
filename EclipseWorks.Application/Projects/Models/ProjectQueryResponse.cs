﻿using EclipseWorks.Domain.Projects.Entities;

namespace EclipseWorks.Application.Projects.Models
{
    public class ProjectQueryResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public static IEnumerable<ProjectQueryResponse> ToModel(IEnumerable<ProjectEntity> entities)
        {
            return entities.Select(ToModel);
        }

        public static ProjectQueryResponse ToModel(ProjectEntity e)
        {
            return  new ProjectQueryResponse()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
            };
        }
    }
}
