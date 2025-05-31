using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elomoas.Persistence.Configurations
{
    public class GroupsConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasData(
                new Group
                {
                    Id = 1,
                    Name = "Mobile Product Design",
                    Description = "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!",
                    Img = "/images/download1.png",
                    PL = ProgramLanguage.JAVA
                },
                new Group
                {
                    Id = 2,
                    Name = "HTML Developer",
                    Description = "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!",
                    Img = "/images/download2.png",
                    PL = ProgramLanguage.HTML
                },
                new Group
                {
                    Id = 3,
                    Name = "Advanced CSS and Sass",
                    Description = "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!",
                    Img = "/images/download4.png",
                    PL = ProgramLanguage.BOOTSTRAP
                },
                new Group
                {
                    Id = 4,
                    Name = "Modern React with Redux",
                    Description = "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!",
                    Img = "/images/download5.png",
                    PL = ProgramLanguage.REACT
                },
                new Group
                {
                    Id = 5,
                    Name = "Node JS",
                    Description = "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!",
                    Img = "/images/download6.png",
                    PL = ProgramLanguage.JAVA
                }
            );
        }
    }
} 