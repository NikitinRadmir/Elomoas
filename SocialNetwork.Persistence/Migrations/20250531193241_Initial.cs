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
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
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
                    { 15, null, null, "Master enterprise-level development with Bootstrap framework. Learn to create complex enterprise UI components and systems. Understand large-scale CSS architecture with Bootstrap. Implement enterprise-grade responsive design patterns. Master theme customization for corporate branding. Learn component library development with Bootstrap. Understand accessibility compliance in enterprise systems. Deploy and maintain large-scale Bootstrap applications.", "/images/v-5.jpg", "Build enterprise UIs^Create component libraries^Implement design systems^Customize themes^Ensure accessibility^Manage large projects^Optimize performance^Deploy enterprise apps", "Enterprise Bootstrap Development", 0, 220, null, null, "/images/video4.mp4" }
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
                    { 5, null, null, "Learn new secrets to creating awesome Microsoft Access databases and VBA coding not covered in any of my other courses!", "/images/download6.png", "Node JS", 5, null, null }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

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
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
