using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Elomoas.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Img = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    PL = table.Column<int>(type: "integer", nullable: false),
                    Video = table.Column<string>(type: "text", nullable: true),
                    Learn = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Img = table.Column<string>(type: "text", nullable: true),
                    PL = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Img = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_AspNetUsers_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationInMonths = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSubscriptions_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSubscriptions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSubscriptions_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupSubscriptions_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Img", "Learn", "Name", "PL", "Price", "UpdatedBy", "UpdatedDate", "Video" },
                values: new object[,]
                {
                    { 1, null, null, "Embark on a comprehensive journey into Python data science excellence. Master the fundamentals of Python for data analysis and manipulation. Dive deep into pandas, numpy, and scikit-learn libraries. Learn to build powerful machine learning models using Python. Understand data visualization techniques with matplotlib and seaborn. Explore advanced statistical analysis methods with Python. Master data cleaning and preprocessing techniques. Complete the course with real-world data science projects.", "/images/v-1.png", "Master Python for data science^Build machine learning models^Handle large datasets^Create data visualizations^Implement statistical analysis^Clean and preprocess data^Deploy machine learning models^Build data pipelines", "Python Data Science Mastery", 6, 240, null, null, "/images/video4.mp4" },
                    { 2, null, null, "Become an expert in Java development with comprehensive training. Master core Java concepts and object-oriented programming principles. Learn enterprise patterns and architectures for scalable applications. Develop robust applications using Spring Framework and Spring Boot. Understand microservices architecture and implementation with Java. Master database integration and ORM frameworks like Hibernate. Learn testing strategies and continuous integration practices. Implement security best practices in Java applications.", "/images/v-2.png", "Master Java fundamentals^Build enterprise applications^Implement Spring Framework^Create microservices^Handle database operations^Secure Java applications^Deploy enterprise systems^Implement testing strategies", "Modern Java Development", 5, 280, null, null, "/images/video4.mp4" },
                    { 3, null, null, "Transform into a professional React developer with comprehensive training. Master component-based architecture and React hooks for modern applications. Learn state management patterns with Redux and Context API. Implement responsive and accessible user interfaces with React. Explore advanced React patterns and performance optimization techniques. Build full-stack applications with React and popular backend technologies. Master testing methodologies for React applications. Deploy and scale React applications in production environments.", "/images/v-3.png", "Build modern React applications^Master Redux state management^Create responsive user interfaces^Implement React hooks effectively^Test React components^Optimize application performance^Handle authentication and routing^Deploy React applications", "React and Redux Professional", 4, 200, null, null, "/images/video4.mp4" },
                    { 4, null, null, "Master MongoDB database development and administration. Learn to design efficient and scalable database schemas. Understand MongoDB query optimization and indexing strategies. Implement data modeling patterns for different use cases. Master aggregation framework for complex data analysis. Learn database security and access control in MongoDB. Understand backup, restoration, and disaster recovery procedures. Deploy and manage MongoDB in production environments.", "/images/v-4.jpg", "Design database schemas^Optimize MongoDB queries^Implement data modeling^Use aggregation framework^Secure MongoDB databases^Handle backup and recovery^Monitor database performance^Deploy MongoDB clusters", "MongoDB Database Engineering", 7, 220, null, null, "/images/video4.mp4" },
                    { 5, null, null, "Master modern web development with HTML5 from the ground up. Learn semantic HTML5 elements and modern page structure techniques. Explore advanced features including Web Components and Custom Elements. Implement responsive design patterns for mobile-first development. Master form handling and validation techniques in HTML5. Learn accessibility best practices for inclusive web development. Understand modern SEO techniques and best practices. Create beautiful and functional web interfaces with HTML5.", "/images/v-5.jpg", "Create semantic HTML5 markup^Build responsive layouts^Implement Web Components^Handle forms and validation^Ensure web accessibility^Optimize for SEO^Create modern interfaces^Deploy web applications", "HTML5 and Modern Web", 1, 150, null, null, "/images/video4.mp4" },
                    { 6, null, null, "Master the latest version of Bootstrap framework for modern web development. Learn to create responsive and mobile-first websites with Bootstrap 5. Understand the Bootstrap grid system and flexible layout components. Implement modern UI components and interactive elements with Bootstrap. Master customization techniques and theme creation with Sass. Learn best practices for responsive web design with Bootstrap. Explore advanced Bootstrap features and plugins for enhanced functionality. Build professional websites with Bootstrap's comprehensive toolkit.", "/images/v-6.jpg", "Master Bootstrap 5 fundamentals^Create responsive layouts^Implement UI components^Customize with Sass^Build mobile-first websites^Use Bootstrap plugins^Optimize performance^Deploy Bootstrap projects", "Bootstrap 5 Framework Mastery", 0, 160, null, null, "/images/video4.mp4" },
                    { 7, null, null, "Master jQuery for modern web development and DOM manipulation. Learn advanced event handling and custom event creation. Understand jQuery animation and effects for interactive interfaces. Implement AJAX and asynchronous data handling with jQuery. Master plugin development and extension creation. Learn performance optimization techniques for jQuery applications. Understand jQuery security best practices and validation. Build professional web applications with jQuery.", "/images/v-7.jpg", "Master jQuery fundamentals^Handle events effectively^Create custom animations^Implement AJAX calls^Develop jQuery plugins^Optimize performance^Secure applications^Build interactive interfaces", "Advanced jQuery Development", 2, 170, null, null, "/images/video4.mp4" },
                    { 8, null, null, "Master modern CSS development with Sass preprocessor. Learn advanced Sass features including mixins and functions. Understand modular CSS architecture with Sass. Implement responsive design patterns using Sass capabilities. Master CSS organization and maintenance with Sass. Learn best practices for scalable stylesheet development. Explore advanced Sass features for complex styling needs. Build maintainable and efficient stylesheets with Sass.", "/images/v-8.jpg", "Master Sass fundamentals^Create reusable mixins^Build modular stylesheets^Implement responsive design^Organize CSS architecture^Optimize stylesheets^Use advanced Sass features^Deploy optimized CSS", "Sass and Modern CSS", 3, 140, null, null, "/images/video4.mp4" },
                    { 9, null, null, "Master web development using Python and Django framework. Learn to build scalable web applications with Django's powerful features. Understand MTV architecture and Django's core components. Implement authentication, authorization, and user management systems. Master database operations and ORM usage in Django. Learn REST API development with Django REST framework. Understand testing and debugging in Django applications. Deploy Django applications to production environments.", "/images/v-9.jpg", "Build Django applications^Create REST APIs^Handle authentication^Manage databases^Test Django apps^Deploy web services^Optimize performance^Implement security", "Python Web Development with Django", 6, 210, null, null, "/images/video4.mp4" },
                    { 10, null, null, "Learn game development fundamentals using Java and popular gaming frameworks. Master core concepts of game programming and design patterns. Understand game physics and collision detection implementation. Create engaging user interfaces and game mechanics. Learn sound and graphics programming in Java. Master game state management and scene transitions. Implement multiplayer functionality and networking. Deploy and publish Java games across platforms.", "/images/v-10.jpg", "Create Java games^Implement game physics^Handle user input^Manage game states^Add sound and graphics^Create multiplayer features^Optimize game performance^Deploy games", "Java Game Development", 5, 250, null, null, "/images/video4.mp4" },
                    { 11, null, null, "Master mobile app development with React Native framework. Learn to build cross-platform mobile applications efficiently. Understand native component integration and mobile-specific features. Implement responsive and adaptive mobile user interfaces. Master state management in mobile applications. Learn offline data storage and synchronization. Understand mobile app testing and debugging strategies. Deploy applications to iOS and Android platforms.", "/images/v-1.png", "Build mobile apps^Create native UIs^Handle device features^Manage app state^Implement offline storage^Test mobile apps^Optimize performance^Deploy to app stores", "React Native Mobile Development", 4, 230, null, null, "/images/video4.mp4" },
                    { 12, null, null, "Master MongoDB in microservices architecture and distributed systems. Learn to design scalable database architectures for microservices. Understand data partitioning and sharding strategies. Implement event sourcing and CQRS patterns with MongoDB. Master distributed transactions and consistency patterns. Learn monitoring and optimization in distributed databases. Understand disaster recovery in distributed systems. Deploy and manage distributed MongoDB clusters.", "/images/v-2.png", "Design microservices databases^Implement sharding^Handle distributed data^Manage transactions^Monitor performance^Scale databases^Implement security^Deploy clusters", "MongoDB for Microservices", 7, 260, null, null, "/images/video4.mp4" },
                    { 13, null, null, "Create engaging browser-based games using HTML5 and Canvas. Master game loop implementation and animation techniques. Learn sprite manipulation and character animation. Implement collision detection and physics systems. Create engaging game mechanics and user interactions. Master sound integration and effects in HTML5 games. Learn game state management and save systems. Deploy and optimize HTML5 games for web browsers.", "/images/v-3.png", "Create HTML5 games^Implement game loops^Handle animations^Add game physics^Manage game state^Integrate sound^Optimize performance^Deploy web games", "HTML5 Game Development", 1, 180, null, null, "/images/video4.mp4" },
                    { 14, null, null, "Master mobile web application development with jQuery Mobile. Learn to create responsive and touch-friendly mobile interfaces. Understand mobile-specific event handling and gestures. Implement mobile-optimized forms and validation. Master mobile navigation patterns and transitions. Learn offline capabilities and local storage. Understand mobile performance optimization techniques. Deploy and manage mobile web applications.", "/images/v-4.jpg", "Build mobile web apps^Handle touch events^Create mobile UIs^Implement navigation^Manage offline data^Optimize for mobile^Test applications^Deploy mobile apps", "jQuery Mobile App Development", 2, 190, null, null, "/images/video4.mp4" },
                    { 15, null, null, "Master enterprise-level development with Bootstrap framework. Learn to create complex enterprise UI components and systems. Understand large-scale CSS architecture with Bootstrap. Implement enterprise-grade responsive design patterns. Master theme customization for corporate branding. Learn component library development with Bootstrap. Understand accessibility compliance in enterprise systems. Deploy and maintain large-scale Bootstrap applications.", "/images/v-5.jpg", "Build enterprise UIs^Create component libraries^Implement design systems^Customize themes^Ensure accessibility^Manage large projects^Optimize performance^Deploy enterprise apps", "Enterprise Bootstrap Development", 0, 220, null, null, "/images/video4.mp4" },
                    { 16, null, null, "Master advanced web development patterns and best practices. Learn modern features and functional programming concepts.", "/images/v-1.png", "Master design patterns^Use modern features^Implement functional programming^Build scalable applications^Optimize performance^Write clean code^Test web apps^Deploy modern web apps", "Advanced Web Development", 1, 180, null, null, "/images/video4.mp4" },
                    { 17, null, null, "Create cross-platform mobile applications using React Native framework. Build native iOS and Android apps with JavaScript.", "/images/v-2.png", "Build mobile apps^Use native components^Handle device features^Create custom UI^Implement navigation^Manage state^Deploy to stores^Debug applications", "React Native Mobile Development", 4, 220, null, null, "/images/video4.mp4" },
                    { 18, null, null, "Learn cloud computing fundamentals and advanced concepts using Amazon Web Services (AWS).", "/images/v-3.png", "Setup AWS services^Deploy applications^Manage databases^Configure security^Scale services^Monitor performance^Implement DevOps^Optimize costs", "Cloud Computing with AWS", 6, 250, null, null, "/images/video4.mp4" },
                    { 19, null, null, "Deep dive into machine learning using TensorFlow framework. Build and deploy AI models.", "/images/v-4.png", "Build AI models^Train networks^Process data^Implement CNN^Use transfer learning^Deploy models^Optimize performance^Create AI apps", "Machine Learning with TensorFlow", 6, 280, null, null, "/images/video4.mp4" },
                    { 20, null, null, "Master Vue.js 3 framework with Composition API and modern best practices.", "/images/v-5.jpg", "Use Composition API^Build components^Manage state^Handle routing^Implement Vuex^Test applications^Deploy apps^Optimize performance", "Vue.js 3 Complete Guide", 4, 190, null, null, "/images/video4.mp4" },
                    { 21, null, null, "Build enterprise-grade applications with Angular framework and TypeScript.", "/images/v-6.jpg", "Master Angular^Use TypeScript^Create components^Manage state^Handle routing^Test apps^Deploy applications^Optimize performance", "Angular Enterprise Development", 5, 230, null, null, "/images/video4.mp4" },
                    { 22, null, null, "Learn modern DevOps practices and implement continuous integration/deployment pipelines.", "/images/v-7.jpg", "Setup CI/CD^Use Docker^Implement Kubernetes^Automate deployment^Monitor systems^Manage configs^Ensure security^Scale applications", "DevOps and CI/CD", 6, 260, null, null, "/images/video4.mp4" },
                    { 23, null, null, "Learn modern web development with latest technologies and frameworks.", "/images/v-1.png", "Create web apps^Use modern frameworks^Build responsive sites^Handle interactions^Implement security^Test websites^Deploy to cloud^Optimize performance", "Modern Web Development", 1, 270, null, null, "/images/video4.mp4" },
                    { 24, null, null, "Create enterprise applications using Java and Spring framework.", "/images/v-2.png", "Use Spring^Build APIs^Handle data^Implement security^Use Hibernate^Create services^Deploy apps^Debug systems", "Advanced Java Development", 5, 240, null, null, "/images/video4.mp4" },
                    { 25, null, null, "Master database development with MongoDB and modern architecture components.", "/images/v-3.png", "Use MongoDB^Build schemas^Handle data^Implement queries^Use aggregation^Create indexes^Deploy databases^Debug systems", "Database Development", 7, 230, null, null, "/images/video4.mp4" },
                    { 26, null, null, "Create 2D and 3D games using modern game engines.", "/images/v-4.png", "Use game engines^Create gameplay^Handle physics^Implement AI^Create animations^Add sound^Deploy games^Optimize performance", "Game Development Basics", 5, 250, null, null, "/images/video4.mp4" },
                    { 27, null, null, "Learn data engineering principles and build robust data pipelines.", "/images/v-5.jpg", "Build pipelines^Process data^Use databases^Handle big data^Implement ETL^Monitor systems^Ensure quality^Optimize performance", "Data Engineering", 6, 280, null, null, "/images/video4.mp4" },
                    { 28, null, null, "Master cybersecurity principles and protect systems from threats.", "/images/v-6.jpg", "Implement security^Handle threats^Use encryption^Protect systems^Monitor networks^Respond to incidents^Ensure compliance^Use best practices", "Cybersecurity Fundamentals", 6, 260, null, null, "/images/video4.mp4" },
                    { 29, null, null, "Learn user interface and user experience design principles.", "/images/v-7.jpg", "Design interfaces^Create wireframes^Build prototypes^Test usability^Handle feedback^Implement designs^Use design tools^Follow principles", "UI/UX Design", 1, 190, null, null, "/images/video4.mp4" },
                    { 30, null, null, "Become a full stack developer with modern web technologies.", "/images/v-1.png", "Build frontend^Create backend^Handle database^Implement security^Deploy apps^Monitor systems^Scale applications^Use best practices", "Full Stack Development", 5, 300, null, null, "/images/video4.mp4" },
                    { 31, null, null, "Master database administration and optimization techniques.", "/images/v-2.png", "Manage databases^Optimize queries^Ensure security^Handle backup^Monitor performance^Implement HA^Use best practices^Solve problems", "Database Administration", 7, 240, null, null, "/images/video4.mp4" },
                    { 32, null, null, "Learn software architecture patterns and best practices.", "/images/v-3.png", "Design systems^Use patterns^Handle scaling^Ensure security^Monitor performance^Make decisions^Document architecture^Lead teams", "Software Architecture", 5, 270, null, null, "/images/video4.mp4" },
                    { 33, null, null, "Build cloud-native applications using modern technologies.", "/images/v-4.png", "Use containers^Implement microservices^Handle scaling^Ensure resilience^Monitor systems^Deploy apps^Optimize performance^Follow practices", "Cloud Native Development", 5, 280, null, null, "/images/video4.mp4" },
                    { 34, null, null, "Master natural language processing with Python and modern libraries.", "/images/v-5.jpg", "Process text^Build models^Handle languages^Implement AI^Use NLTK^Create applications^Deploy solutions^Optimize performance", "Natural Language Processing", 6, 290, null, null, "/images/video4.mp4" },
                    { 35, null, null, "Learn techniques to optimize web application performance.", "/images/v-6.jpg", "Optimize loading^Handle caching^Improve rendering^Reduce size^Monitor performance^Debug issues^Use best practices^Implement PWA", "Web Performance", 1, 210, null, null, "/images/video4.mp4" },
                    { 36, null, null, "Design and implement microservices-based applications.", "/images/v-7.jpg", "Design services^Handle communication^Ensure security^Monitor systems^Scale apps^Deploy services^Handle failures^Use best practices", "Microservices Architecture", 5, 260, null, null, "/images/video4.mp4" },
                    { 37, null, null, "Master frontend development and optimization techniques.", "/images/v-1.png", "Build interfaces^Improve CSS^Handle assets^Use frameworks^Monitor performance^Debug issues^Implement PWA^Follow best practices", "Frontend Development", 1, 220, null, null, "/images/video4.mp4" },
                    { 38, null, null, "Learn backend development with modern technologies.", "/images/v-2.png", "Build APIs^Handle data^Ensure security^Monitor systems^Scale services^Deploy apps^Use best practices^Solve problems", "Backend Development", 5, 250, null, null, "/images/video4.mp4" },
                    { 39, null, null, "Create beautiful and responsive mobile user interfaces.", "/images/v-3.png", "Design UI^Handle responsive^Create animations^Implement UX^Use components^Test interfaces^Deploy apps^Follow principles", "Mobile UI Development", 4, 230, null, null, "/images/video4.mp4" },
                    { 40, null, null, "Master software testing and quality assurance practices.", "/images/v-4.png", "Write tests^Automate testing^Ensure quality^Handle CI/CD^Monitor systems^Debug issues^Use best practices^Improve processes", "Testing and Quality Assurance", 5, 240, null, null, "/images/video4.mp4" },
                    { 41, null, null, "Learn to design and develop robust APIs.", "/images/v-5.jpg", "Design APIs^Handle security^Document endpoints^Test APIs^Monitor usage^Scale services^Use best practices^Solve problems", "API Development", 5, 230, null, null, "/images/video4.mp4" },
                    { 42, null, null, "Build modern progressive web applications.", "/images/v-6.jpg", "Create PWAs^Handle offline^Use service workers^Implement push^Optimize performance^Deploy apps^Follow standards^Improve UX", "Web Applications", 1, 220, null, null, "/images/video4.mp4" },
                    { 43, null, null, "Master data visualization techniques and tools.", "/images/v-7.jpg", "Create charts^Handle data^Use libraries^Build dashboards^Implement interactivity^Deploy solutions^Follow principles^Improve UX", "Data Visualization", 6, 240, null, null, "/images/video4.mp4" },
                    { 44, null, null, "Learn to build applications using cloud architecture.", "/images/v-1.png", "Use cloud services^Handle events^Manage state^Monitor functions^Optimize costs^Deploy apps^Follow practices^Solve problems", "Cloud Architecture", 5, 260, null, null, "/images/video4.mp4" },
                    { 45, null, null, "Build cross-platform applications using modern frameworks.", "/images/v-2.png", "Use frameworks^Share code^Handle platform^Create UI^Implement features^Deploy apps^Follow practices^Solve problems", "Cross-Platform Development", 4, 250, null, null, "/images/video4.mp4" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Img", "Name", "PL", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download1.png", "Mobile Product Design", 5, null, null },
                    { 2, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download2.png", "HTML Developer", 1, null, null },
                    { 3, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download4.png", "Advanced CSS and Sass", 0, null, null },
                    { 4, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download5.png", "Modern React with Redux", 4, null, null },
                    { 5, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download6.png", "Node JS", 5, null, null },
                    { 6, null, null, "Master Python programming with advanced concepts and real-world applications!", "/images/download7.png", "Advanced Python Development", 6, null, null },
                    { 7, null, null, "Become a full-stack developer with modern JavaScript frameworks and tools!", "/images/download1.png", "Full Stack JavaScript", 1, null, null },
                    { 8, null, null, "Learn Vue.js from basics to advanced concepts with practical projects!", "/images/download2.png", "Vue.js Mastery", 4, null, null },
                    { 9, null, null, "Build enterprise-level applications with Angular framework!", "/images/download3.png", "Angular Enterprise", 5, null, null },
                    { 10, null, null, "Master DevOps practices and tools for modern software development!", "/images/download4.png", "DevOps Essentials", 6, null, null },
                    { 11, null, null, "Design and implement scalable cloud solutions!", "/images/download5.png", "Cloud Architecture", 5, null, null },
                    { 12, null, null, "Create cross-platform mobile applications with modern frameworks!", "/images/download6.png", "Mobile App Development", 4, null, null },
                    { 13, null, null, "Learn the basics of data science and machine learning!", "/images/download7.png", "Data Science Fundamentals", 6, null, null },
                    { 14, null, null, "Master web security concepts and best practices!", "/images/download1.png", "Web Security", 5, null, null },
                    { 15, null, null, "Create beautiful and user-friendly interfaces!", "/images/download2.png", "UI/UX Design", 1, null, null },
                    { 16, null, null, "Learn frontend development with modern frameworks!", "/images/download3.png", "Frontend Development", 4, null, null },
                    { 17, null, null, "Create exciting games with modern game development frameworks!", "/images/download4.png", "Game Development", 5, null, null },
                    { 18, null, null, "Deep dive into machine learning algorithms and applications!", "/images/download5.png", "Machine Learning", 6, null, null },
                    { 19, null, null, "Master database design and optimization techniques!", "/images/download6.png", "Database Design", 7, null, null },
                    { 20, null, null, "Learn to design and implement scalable software architectures!", "/images/download7.png", "Software Architecture", 5, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IdentityId",
                table: "AppUsers",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubscriptions_CourseId",
                table: "CourseSubscriptions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubscriptions_UserId",
                table: "CourseSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubscriptions_GroupId",
                table: "GroupSubscriptions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubscriptions_UserId",
                table: "GroupSubscriptions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CourseSubscriptions");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "GroupSubscriptions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
