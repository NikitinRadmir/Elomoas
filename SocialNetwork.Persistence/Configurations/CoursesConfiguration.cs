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
                },
                new Course
                {
                    Id = 16,
                    Name = "Advanced Web Development",
                    Description = "Master advanced web development patterns and best practices. Learn modern features and functional programming concepts.",
                    Img = "/images/v-1.png",
                    Price = 180,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Master design patterns^Use modern features^Implement functional programming^Build scalable applications^Optimize performance^Write clean code^Test web apps^Deploy modern web apps"
                },
                new Course
                {
                    Id = 17,
                    Name = "React Native Mobile Development",
                    Description = "Create cross-platform mobile applications using React Native framework. Build native iOS and Android apps with JavaScript.",
                    Img = "/images/v-2.png",
                    Price = 220,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Build mobile apps^Use native components^Handle device features^Create custom UI^Implement navigation^Manage state^Deploy to stores^Debug applications"
                },
                new Course
                {
                    Id = 18,
                    Name = "Cloud Computing with AWS",
                    Description = "Learn cloud computing fundamentals and advanced concepts using Amazon Web Services (AWS).",
                    Img = "/images/v-3.png",
                    Price = 250,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Setup AWS services^Deploy applications^Manage databases^Configure security^Scale services^Monitor performance^Implement DevOps^Optimize costs"
                },
                new Course
                {
                    Id = 19,
                    Name = "Machine Learning with TensorFlow",
                    Description = "Deep dive into machine learning using TensorFlow framework. Build and deploy AI models.",
                    Img = "/images/v-4.png",
                    Price = 280,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Build AI models^Train networks^Process data^Implement CNN^Use transfer learning^Deploy models^Optimize performance^Create AI apps"
                },
                new Course
                {
                    Id = 20,
                    Name = "Vue.js 3 Complete Guide",
                    Description = "Master Vue.js 3 framework with Composition API and modern best practices.",
                    Img = "/images/v-5.jpg",
                    Price = 190,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Use Composition API^Build components^Manage state^Handle routing^Implement Vuex^Test applications^Deploy apps^Optimize performance"
                },
                new Course
                {
                    Id = 21,
                    Name = "Angular Enterprise Development",
                    Description = "Build enterprise-grade applications with Angular framework and TypeScript.",
                    Img = "/images/v-6.jpg",
                    Price = 230,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Master Angular^Use TypeScript^Create components^Manage state^Handle routing^Test apps^Deploy applications^Optimize performance"
                },
                new Course
                {
                    Id = 22,
                    Name = "DevOps and CI/CD",
                    Description = "Learn modern DevOps practices and implement continuous integration/deployment pipelines.",
                    Img = "/images/v-7.jpg",
                    Price = 260,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Setup CI/CD^Use Docker^Implement Kubernetes^Automate deployment^Monitor systems^Manage configs^Ensure security^Scale applications"
                },
                new Course
                {
                    Id = 23,
                    Name = "Modern Web Development",
                    Description = "Learn modern web development with latest technologies and frameworks.",
                    Img = "/images/v-1.png",
                    Price = 270,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Create web apps^Use modern frameworks^Build responsive sites^Handle interactions^Implement security^Test websites^Deploy to cloud^Optimize performance"
                },
                new Course
                {
                    Id = 24,
                    Name = "Advanced Java Development",
                    Description = "Create enterprise applications using Java and Spring framework.",
                    Img = "/images/v-2.png",
                    Price = 240,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Use Spring^Build APIs^Handle data^Implement security^Use Hibernate^Create services^Deploy apps^Debug systems"
                },
                new Course
                {
                    Id = 25,
                    Name = "Database Development",
                    Description = "Master database development with MongoDB and modern architecture components.",
                    Img = "/images/v-3.png",
                    Price = 230,
                    PL = ProgramLanguage.MONGODB,
                    Video = "/images/video4.mp4",
                    Learn = "Use MongoDB^Build schemas^Handle data^Implement queries^Use aggregation^Create indexes^Deploy databases^Debug systems"
                },
                new Course
                {
                    Id = 26,
                    Name = "Game Development Basics",
                    Description = "Create 2D and 3D games using modern game engines.",
                    Img = "/images/v-4.png",
                    Price = 250,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Use game engines^Create gameplay^Handle physics^Implement AI^Create animations^Add sound^Deploy games^Optimize performance"
                },
                new Course
                {
                    Id = 27,
                    Name = "Data Engineering",
                    Description = "Learn data engineering principles and build robust data pipelines.",
                    Img = "/images/v-5.jpg",
                    Price = 280,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Build pipelines^Process data^Use databases^Handle big data^Implement ETL^Monitor systems^Ensure quality^Optimize performance"
                },
                new Course
                {
                    Id = 28,
                    Name = "Cybersecurity Fundamentals",
                    Description = "Master cybersecurity principles and protect systems from threats.",
                    Img = "/images/v-6.jpg",
                    Price = 260,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Implement security^Handle threats^Use encryption^Protect systems^Monitor networks^Respond to incidents^Ensure compliance^Use best practices"
                },
                new Course
                {
                    Id = 29,
                    Name = "UI/UX Design",
                    Description = "Learn user interface and user experience design principles.",
                    Img = "/images/v-7.jpg",
                    Price = 190,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Design interfaces^Create wireframes^Build prototypes^Test usability^Handle feedback^Implement designs^Use design tools^Follow principles"
                },
                new Course
                {
                    Id = 30,
                    Name = "Full Stack Development",
                    Description = "Become a full stack developer with modern web technologies.",
                    Img = "/images/v-1.png",
                    Price = 300,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Build frontend^Create backend^Handle database^Implement security^Deploy apps^Monitor systems^Scale applications^Use best practices"
                },
                new Course
                {
                    Id = 31,
                    Name = "Database Administration",
                    Description = "Master database administration and optimization techniques.",
                    Img = "/images/v-2.png",
                    Price = 240,
                    PL = ProgramLanguage.MONGODB,
                    Video = "/images/video4.mp4",
                    Learn = "Manage databases^Optimize queries^Ensure security^Handle backup^Monitor performance^Implement HA^Use best practices^Solve problems"
                },
                new Course
                {
                    Id = 32,
                    Name = "Software Architecture",
                    Description = "Learn software architecture patterns and best practices.",
                    Img = "/images/v-3.png",
                    Price = 270,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Design systems^Use patterns^Handle scaling^Ensure security^Monitor performance^Make decisions^Document architecture^Lead teams"
                },
                new Course
                {
                    Id = 33,
                    Name = "Cloud Native Development",
                    Description = "Build cloud-native applications using modern technologies.",
                    Img = "/images/v-4.png",
                    Price = 280,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Use containers^Implement microservices^Handle scaling^Ensure resilience^Monitor systems^Deploy apps^Optimize performance^Follow practices"
                },
                new Course
                {
                    Id = 34,
                    Name = "Natural Language Processing",
                    Description = "Master natural language processing with Python and modern libraries.",
                    Img = "/images/v-5.jpg",
                    Price = 290,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Process text^Build models^Handle languages^Implement AI^Use NLTK^Create applications^Deploy solutions^Optimize performance"
                },
                new Course
                {
                    Id = 35,
                    Name = "Web Performance",
                    Description = "Learn techniques to optimize web application performance.",
                    Img = "/images/v-6.jpg",
                    Price = 210,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Optimize loading^Handle caching^Improve rendering^Reduce size^Monitor performance^Debug issues^Use best practices^Implement PWA"
                },
                new Course
                {
                    Id = 36,
                    Name = "Microservices Architecture",
                    Description = "Design and implement microservices-based applications.",
                    Img = "/images/v-7.jpg",
                    Price = 260,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Design services^Handle communication^Ensure security^Monitor systems^Scale apps^Deploy services^Handle failures^Use best practices"
                },
                new Course
                {
                    Id = 37,
                    Name = "Frontend Development",
                    Description = "Master frontend development and optimization techniques.",
                    Img = "/images/v-1.png",
                    Price = 220,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Build interfaces^Improve CSS^Handle assets^Use frameworks^Monitor performance^Debug issues^Implement PWA^Follow best practices"
                },
                new Course
                {
                    Id = 38,
                    Name = "Backend Development",
                    Description = "Learn backend development with modern technologies.",
                    Img = "/images/v-2.png",
                    Price = 250,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Build APIs^Handle data^Ensure security^Monitor systems^Scale services^Deploy apps^Use best practices^Solve problems"
                },
                new Course
                {
                    Id = 39,
                    Name = "Mobile UI Development",
                    Description = "Create beautiful and responsive mobile user interfaces.",
                    Img = "/images/v-3.png",
                    Price = 230,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Design UI^Handle responsive^Create animations^Implement UX^Use components^Test interfaces^Deploy apps^Follow principles"
                },
                new Course
                {
                    Id = 40,
                    Name = "Testing and Quality Assurance",
                    Description = "Master software testing and quality assurance practices.",
                    Img = "/images/v-4.png",
                    Price = 240,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Write tests^Automate testing^Ensure quality^Handle CI/CD^Monitor systems^Debug issues^Use best practices^Improve processes"
                },
                new Course
                {
                    Id = 41,
                    Name = "API Development",
                    Description = "Learn to design and develop robust APIs.",
                    Img = "/images/v-5.jpg",
                    Price = 230,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Design APIs^Handle security^Document endpoints^Test APIs^Monitor usage^Scale services^Use best practices^Solve problems"
                },
                new Course
                {
                    Id = 42,
                    Name = "Web Applications",
                    Description = "Build modern progressive web applications.",
                    Img = "/images/v-6.jpg",
                    Price = 220,
                    PL = ProgramLanguage.HTML,
                    Video = "/images/video4.mp4",
                    Learn = "Create PWAs^Handle offline^Use service workers^Implement push^Optimize performance^Deploy apps^Follow standards^Improve UX"
                },
                new Course
                {
                    Id = 43,
                    Name = "Data Visualization",
                    Description = "Master data visualization techniques and tools.",
                    Img = "/images/v-7.jpg",
                    Price = 240,
                    PL = ProgramLanguage.PYTHON,
                    Video = "/images/video4.mp4",
                    Learn = "Create charts^Handle data^Use libraries^Build dashboards^Implement interactivity^Deploy solutions^Follow principles^Improve UX"
                },
                new Course
                {
                    Id = 44,
                    Name = "Cloud Architecture",
                    Description = "Learn to build applications using cloud architecture.",
                    Img = "/images/v-1.png",
                    Price = 260,
                    PL = ProgramLanguage.JAVA,
                    Video = "/images/video4.mp4",
                    Learn = "Use cloud services^Handle events^Manage state^Monitor functions^Optimize costs^Deploy apps^Follow practices^Solve problems"
                },
                new Course
                {
                    Id = 45,
                    Name = "Cross-Platform Development",
                    Description = "Build cross-platform applications using modern frameworks.",
                    Img = "/images/v-2.png",
                    Price = 250,
                    PL = ProgramLanguage.REACT,
                    Video = "/images/video4.mp4",
                    Learn = "Use frameworks^Share code^Handle platform^Create UI^Implement features^Deploy apps^Follow practices^Solve problems"
                }
            );
        }
    }
} 