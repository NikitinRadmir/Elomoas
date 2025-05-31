using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elomoas.Persistence.Configurations
{
    public class CoursesConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasData(
                new Course
                {
                    Id = 1,
                    Name = "Python Data Science Mastery",
                    Description = "Embark on a comprehensive journey into Python data science excellence. Master the fundamentals of Python for data analysis and manipulation. Dive deep into pandas, numpy, and scikit-learn libraries. Learn to build powerful machine learning models using Python. Understand data visualization techniques with matplotlib and seaborn. Explore advanced statistical analysis methods with Python. Master data cleaning and preprocessing techniques. Complete the course with real-world data science projects.",
                    Img = "/images/v-1.png",
                    Price = 240,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Master Python for data science^Build machine learning models^Handle large datasets^Create data visualizations^Implement statistical analysis^Clean and preprocess data^Deploy machine learning models^Build data pipelines"
                },
                new Course
                {
                    Id = 2,
                    Name = "Modern Java Development",
                    Description = "Become an expert in Java development with comprehensive training. Master core Java concepts and object-oriented programming principles. Learn enterprise patterns and architectures for scalable applications. Develop robust applications using Spring Framework and Spring Boot. Understand microservices architecture and implementation with Java. Master database integration and ORM frameworks like Hibernate. Learn testing strategies and continuous integration practices. Implement security best practices in Java applications.",
                    Img = "/images/v-2.png",
                    Price = 280,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Master Java fundamentals^Build enterprise applications^Implement Spring Framework^Create microservices^Handle database operations^Secure Java applications^Deploy enterprise systems^Implement testing strategies"
                },
                new Course
                {
                    Id = 3,
                    Name = "React and Redux Professional",
                    Description = "Transform into a professional React developer with comprehensive training. Master component-based architecture and React hooks for modern applications. Learn state management patterns with Redux and Context API. Implement responsive and accessible user interfaces with React. Explore advanced React patterns and performance optimization techniques. Build full-stack applications with React and popular backend technologies. Master testing methodologies for React applications. Deploy and scale React applications in production environments.",
                    Img = "/images/v-3.png",
                    Price = 200,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Build modern React applications^Master Redux state management^Create responsive user interfaces^Implement React hooks effectively^Test React components^Optimize application performance^Handle authentication and routing^Deploy React applications"
                },
                new Course
                {
                    Id = 4,
                    Name = "MongoDB Database Engineering",
                    Description = "Master MongoDB database development and administration. Learn to design efficient and scalable database schemas. Understand MongoDB query optimization and indexing strategies. Implement data modeling patterns for different use cases. Master aggregation framework for complex data analysis. Learn database security and access control in MongoDB. Understand backup, restoration, and disaster recovery procedures. Deploy and manage MongoDB in production environments.",
                    Img = "/images/v-4.jpg",
                    Price = 220,
                    PL = ProgramLanguage.MONGODB,
                    Video = "/images/video4.mp4",
                    Learn = "Design database schemas^Optimize MongoDB queries^Implement data modeling^Use aggregation framework^Secure MongoDB databases^Handle backup and recovery^Monitor database performance^Deploy MongoDB clusters"
                },
                new Course
                {
                    Id = 5,
                    Name = "HTML5 and Modern Web",
                    Description = "Master modern web development with HTML5 from the ground up. Learn semantic HTML5 elements and modern page structure techniques. Explore advanced features including Web Components and Custom Elements. Implement responsive design patterns for mobile-first development. Master form handling and validation techniques in HTML5. Learn accessibility best practices for inclusive web development. Understand modern SEO techniques and best practices. Create beautiful and functional web interfaces with HTML5.",
                    Img = "/images/v-5.jpg",
                    Price = 150,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Create semantic HTML5 markup^Build responsive layouts^Implement Web Components^Handle forms and validation^Ensure web accessibility^Optimize for SEO^Create modern interfaces^Deploy web applications"
                },
                new Course
                {
                    Id = 6,
                    Name = "Bootstrap 5 Framework Mastery",
                    Description = "Master the latest version of Bootstrap framework for modern web development. Learn to create responsive and mobile-first websites with Bootstrap 5. Understand the Bootstrap grid system and flexible layout components. Implement modern UI components and interactive elements with Bootstrap. Master customization techniques and theme creation with Sass. Learn best practices for responsive web design with Bootstrap. Explore advanced Bootstrap features and plugins for enhanced functionality. Build professional websites with Bootstrap's comprehensive toolkit.",
                    Img = "/images/v-6.jpg",
                    Price = 160,
                    PL = ProgramLanguage.BOOTSTRAP,
                    Video = "/images/video4.mp4",
                    Learn = "Master Bootstrap 5 fundamentals^Create responsive layouts^Implement UI components^Customize with Sass^Build mobile-first websites^Use Bootstrap plugins^Optimize performance^Deploy Bootstrap projects"
                },
                new Course
                {
                    Id = 7,
                    Name = "Advanced jQuery Development",
                    Description = "Master jQuery for modern web development and DOM manipulation. Learn advanced event handling and custom event creation. Understand jQuery animation and effects for interactive interfaces. Implement AJAX and asynchronous data handling with jQuery. Master plugin development and extension creation. Learn performance optimization techniques for jQuery applications. Understand jQuery security best practices and validation. Build professional web applications with jQuery.",
                    Img = "/images/v-7.jpg",
                    Price = 170,
                    PL = ProgramLanguage.JQUERY,
                    Video = "/images/video4.mp4",
                    Learn = "Master jQuery fundamentals^Handle events effectively^Create custom animations^Implement AJAX calls^Develop jQuery plugins^Optimize performance^Secure applications^Build interactive interfaces"
                },
                new Course
                {
                    Id = 8,
                    Name = "Sass and Modern CSS",
                    Description = "Master modern CSS development with Sass preprocessor. Learn advanced Sass features including mixins and functions. Understand modular CSS architecture with Sass. Implement responsive design patterns using Sass capabilities. Master CSS organization and maintenance with Sass. Learn best practices for scalable stylesheet development. Explore advanced Sass features for complex styling needs. Build maintainable and efficient stylesheets with Sass.",
                    Img = "/images/v-8.jpg",
                    Price = 140,
                    PL = ProgramLanguage.SASS,
                    Video = "/images/video4.mp4",
                    Learn = "Master Sass fundamentals^Create reusable mixins^Build modular stylesheets^Implement responsive design^Organize CSS architecture^Optimize stylesheets^Use advanced Sass features^Deploy optimized CSS"
                },
                new Course
                {
                    Id = 9,
                    Name = "Python Web Development with Django",
                    Description = "Master web development using Python and Django framework. Learn to build scalable web applications with Django's powerful features. Understand MTV architecture and Django's core components. Implement authentication, authorization, and user management systems. Master database operations and ORM usage in Django. Learn REST API development with Django REST framework. Understand testing and debugging in Django applications. Deploy Django applications to production environments.",
                    Img = "/images/v-9.jpg",
                    Price = 210,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Build Django applications^Create REST APIs^Handle authentication^Manage databases^Test Django apps^Deploy web services^Optimize performance^Implement security"
                },
                new Course
                {
                    Id = 10,
                    Name = "Java Game Development",
                    Description = "Learn game development fundamentals using Java and popular gaming frameworks. Master core concepts of game programming and design patterns. Understand game physics and collision detection implementation. Create engaging user interfaces and game mechanics. Learn sound and graphics programming in Java. Master game state management and scene transitions. Implement multiplayer functionality and networking. Deploy and publish Java games across platforms.",
                    Img = "/images/v-10.jpg",
                    Price = 250,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Create Java games^Implement game physics^Handle user input^Manage game states^Add sound and graphics^Create multiplayer features^Optimize game performance^Deploy games"
                },
                new Course
                {
                    Id = 11,
                    Name = "React Native Mobile Development",
                    Description = "Master mobile app development with React Native framework. Learn to build cross-platform mobile applications efficiently. Understand native component integration and mobile-specific features. Implement responsive and adaptive mobile user interfaces. Master state management in mobile applications. Learn offline data storage and synchronization. Understand mobile app testing and debugging strategies. Deploy applications to iOS and Android platforms.",
                    Img = "/images/v-1.png",
                    Price = 230,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Build mobile apps^Create native UIs^Handle device features^Manage app state^Implement offline storage^Test mobile apps^Optimize performance^Deploy to app stores"
                },
                new Course
                {
                    Id = 12,
                    Name = "MongoDB for Microservices",
                    Description = "Master MongoDB in microservices architecture and distributed systems. Learn to design scalable database architectures for microservices. Understand data partitioning and sharding strategies. Implement event sourcing and CQRS patterns with MongoDB. Master distributed transactions and consistency patterns. Learn monitoring and optimization in distributed databases. Understand disaster recovery in distributed systems. Deploy and manage distributed MongoDB clusters.",
                    Img = "/images/v-2.png",
                    Price = 260,
                    PL = ProgramLanguage.MONGODB,
                    Video = "/images/video4.mp4",
                    Learn = "Design microservices databases^Implement sharding^Handle distributed data^Manage transactions^Monitor performance^Scale databases^Implement security^Deploy clusters"
                },
                new Course
                {
                    Id = 13,
                    Name = "HTML5 Game Development",
                    Description = "Create engaging browser-based games using HTML5 and Canvas. Master game loop implementation and animation techniques. Learn sprite manipulation and character animation. Implement collision detection and physics systems. Create engaging game mechanics and user interactions. Master sound integration and effects in HTML5 games. Learn game state management and save systems. Deploy and optimize HTML5 games for web browsers.",
                    Img = "/images/v-3.png",
                    Price = 180,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Create HTML5 games^Implement game loops^Handle animations^Add game physics^Manage game state^Integrate sound^Optimize performance^Deploy web games"
                },
                new Course
                {
                    Id = 14,
                    Name = "jQuery Mobile App Development",
                    Description = "Master mobile web application development with jQuery Mobile. Learn to create responsive and touch-friendly mobile interfaces. Understand mobile-specific event handling and gestures. Implement mobile-optimized forms and validation. Master mobile navigation patterns and transitions. Learn offline capabilities and local storage. Understand mobile performance optimization techniques. Deploy and manage mobile web applications.",
                    Img = "/images/v-4.jpg",
                    Price = 190,
                    PL = ProgramLanguage.JQUERY,
                    Video = "/images/video4.mp4",
                    Learn = "Build mobile web apps^Handle touch events^Create mobile UIs^Implement navigation^Manage offline data^Optimize for mobile^Test applications^Deploy mobile apps"
                },
                new Course
                {
                    Id = 15,
                    Name = "Enterprise Bootstrap Development",
                    Description = "Master enterprise-level development with Bootstrap framework. Learn to create complex enterprise UI components and systems. Understand large-scale CSS architecture with Bootstrap. Implement enterprise-grade responsive design patterns. Master theme customization for corporate branding. Learn component library development with Bootstrap. Understand accessibility compliance in enterprise systems. Deploy and maintain large-scale Bootstrap applications.",
                    Img = "/images/v-5.jpg",
                    Price = 220,
                    PL = ProgramLanguage.BOOTSTRAP,
                    Video = "/images/video4.mp4",
                    Learn = "Build enterprise UIs^Create component libraries^Implement design systems^Customize themes^Ensure accessibility^Manage large projects^Optimize performance^Deploy enterprise apps"
                }
            );
        }
    }
} 