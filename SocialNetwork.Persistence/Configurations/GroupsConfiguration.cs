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
                },
                new Group
                {
                    Id = 6,
                    Name = "Advanced Python Development",
                    Description = "Master Python programming with advanced concepts and real-world applications!",
                    Img = "/images/download7.png",
                    PL = ProgramLanguage.PYTHON
                },
                new Group
                {
                    Id = 7,
                    Name = "Full Stack JavaScript",
                    Description = "Become a full-stack developer with modern JavaScript frameworks and tools!",
                    Img = "/images/download1.png",
                    PL = ProgramLanguage.HTML
                },
                new Group
                {
                    Id = 8,
                    Name = "Vue.js Mastery",
                    Description = "Learn Vue.js from basics to advanced concepts with practical projects!",
                    Img = "/images/download2.png",
                    PL = ProgramLanguage.REACT
                },
                new Group
                {
                    Id = 9,
                    Name = "Angular Enterprise",
                    Description = "Build enterprise-level applications with Angular framework!",
                    Img = "/images/download3.png",
                    PL = ProgramLanguage.JAVA
                },
                new Group
                {
                    Id = 10,
                    Name = "DevOps Essentials",
                    Description = "Master DevOps practices and tools for modern software development!",
                    Img = "/images/download4.png",
                    PL = ProgramLanguage.PYTHON
                },
                new Group
                {
                    Id = 11,
                    Name = "Cloud Architecture",
                    Description = "Design and implement scalable cloud solutions!",
                    Img = "/images/download5.png",
                    PL = ProgramLanguage.JAVA
                },
                new Group
                {
                    Id = 12,
                    Name = "Mobile App Development",
                    Description = "Create cross-platform mobile applications with modern frameworks!",
                    Img = "/images/download6.png",
                    PL = ProgramLanguage.REACT
                },
                new Group
                {
                    Id = 13,
                    Name = "Data Science Fundamentals",
                    Description = "Learn the basics of data science and machine learning!",
                    Img = "/images/download7.png",
                    PL = ProgramLanguage.PYTHON
                },
                new Group
                {
                    Id = 14,
                    Name = "Web Security",
                    Description = "Master web security concepts and best practices!",
                    Img = "/images/download1.png",
                    PL = ProgramLanguage.JAVA
                },
                new Group
                {
                    Id = 15,
                    Name = "UI/UX Design",
                    Description = "Create beautiful and user-friendly interfaces!",
                    Img = "/images/download2.png",
                    PL = ProgramLanguage.HTML
                },
                new Group
                {
                    Id = 16,
                    Name = "Frontend Development",
                    Description = "Learn frontend development with modern frameworks!",
                    Img = "/images/download3.png",
                    PL = ProgramLanguage.REACT
                },
                new Group
                {
                    Id = 17,
                    Name = "Game Development",
                    Description = "Create exciting games with modern game development frameworks!",
                    Img = "/images/download4.png",
                    PL = ProgramLanguage.JAVA
                },
                new Group
                {
                    Id = 18,
                    Name = "Machine Learning",
                    Description = "Deep dive into machine learning algorithms and applications!",
                    Img = "/images/download5.png",
                    PL = ProgramLanguage.PYTHON
                },
                new Group
                {
                    Id = 19,
                    Name = "Database Design",
                    Description = "Master database design and optimization techniques!",
                    Img = "/images/download6.png",
                    PL = ProgramLanguage.MONGODB
                },
                new Group
                {
                    Id = 20,
                    Name = "Software Architecture",
                    Description = "Learn to design and implement scalable software architectures!",
                    Img = "/images/download7.png",
                    PL = ProgramLanguage.JAVA
                }
            );
        }
    }
} 