using SOP.Encryption;
using SOP.Entities;

namespace SOP.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Address> Address { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemGroup> ItemGroup { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusHistory> StatusHistory { get; set; }
        public DbSet<User> User { get; set; }

        // Archive DbSets
        public DbSet<Archive_Item> Archive_Item { get; set; }
        public DbSet<Archive_ItemType> Archive_ItemType { get; set; }
        public DbSet<Archive_ItemGroup> Archive_ItemGroup { get; set; }
        public DbSet<Archive_Loan> Archive_Loan { get; set; }
        public DbSet<Archive_Request> Archive_Request { get; set; }
        public DbSet<Archive_StatusHistory> Archive_StatusHistory { get; set; }
        public DbSet<Archive_User> Archive_User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Address>()
                .Property(a => a.ZipCode)
                .ValueGeneratedNever();

            modelBuilder.Entity<Item>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ItemGroup>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ItemType>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Loan>()
               .Property(l => l.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Request>()
               .Property(r => r.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<StatusHistory>()
               .Property(s => s.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
               .Property(u => u.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Archive_Item>()
               .Property(i => i.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<Archive_ItemGroup>()
                .Property(i => i.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Archive_ItemType>()
                .Property(i => i.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Archive_Loan>()
               .Property(l => l.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<Archive_Request>()
               .Property(r => r.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<Archive_StatusHistory>()
               .Property(s => s.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<Archive_User>()
               .Property(u => u.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Id = 1,
                Name = "Admin",
                Description = "Administrator"
            },
            new Role()
            {
                Id = 2,
                Name = "Instruktør",
                Description = "Instruktør"
            },
            new Role()
            {
                Id = 3,
                Name = "Elev",
                Description = "Elev"
            },
            new Role()
            {
                Id = 4,
                Name = "Drift",
                Description = "Drift"
            });

            modelBuilder.Entity<Address>().HasData(
            new Address()
            {
                ZipCode = 2750,
                City = "Ballerup",
                Region = "Sjælland",
                Road = "Telegrafvej 9"
            },
            new Address()
            {
                ZipCode = 2650,
                City = "Hvidovre",
                Region = "Sjælland",
                Road = "Stamholmen 193, 215"
            },
            new Address()
            {
                ZipCode = 2000,
                City = "Frederiksberg",
                Region = "Sjælland",
                Road = "Stæhr Johansens Vej 7"
            },
            new Address()
            {
                ZipCode = 2860,
                City = "Gladsaxe",
                Region = "Sjælland",
                Road = "Tobaksvejen 19"
            },
            new Address()
            {
                ZipCode = 2800,
                City = "Lyngby",
                Region = "Sjælland",
                Road = "Gyrithe Lemches Vej 14"
            });

            modelBuilder.Entity<Building>().HasData(
            new Building()
            {
                Id = 1,
                BuildingName = "A",
                ZipCode = 2000,
            },
            new Building()
            {
                Id = 2,
                BuildingName = "C",
                ZipCode = 2650,

            });

            modelBuilder.Entity<Room>().HasData(
            new Room()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            },
            new Room()
            {
                Id = 2,
                BuildingId = 2,
                RoomNumber = 19,
            },
            new Room()
            {
                Id = 3,
                BuildingId = 1,
                RoomNumber = 3,
            },
            new Room()
            {
                Id = 4,
                BuildingId = 2,
                RoomNumber = 17,
            },
            new Room()
            {
                Id = 5,
                BuildingId = 1,
                RoomNumber = 4,
            },
            new Room()
            {
                Id = 6,
                BuildingId = 2,
                RoomNumber = 15,
            });

            modelBuilder.Entity<Status>().HasData(
            new Status()
            {
                Id = 1,
                Name = "Virker"
            },
            new Status()
            {
                Id = 2,
                Name = "Gik stykker"
            },
            new Status()
            {
                Id = 3,
                Name = "Under service"
            },
            new Status()
            {
                Id = 4,
                Name = "Udlånt"
            });

            modelBuilder.Entity<ItemType>().HasData(
            new ItemType()
            {
                Id = 1,
                TypeName = "Computer"
            },
            new ItemType()
            {
                Id = 2,
                TypeName = "Bord"
            },
            new ItemType()
            {
                Id = 3,
                TypeName = "Stole"
            },
            new ItemType()
            {
                Id = 4,
                TypeName = "Skærm"
            },
            new ItemType()
            {
                Id = 5,
                TypeName = "Tastatur"
            });

            modelBuilder.Entity<ItemGroup>().HasData(
            new ItemGroup()
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30
            },
            new ItemGroup()
            {
                Id = 2,
                ItemTypeId = 2,
                ModelName = "SANDSBERG ",
                Price = 299.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "2 år",
                Quantity = 20
            },
            new ItemGroup()
            {
                Id = 3,
                ItemTypeId = 1,
                ModelName = "HP Envy x360",
                Price = 11249.50m,
                Manufacturer = "HP",
                WarrantyPeriod = "2 år",
                Quantity = 15
            },
            new ItemGroup()
            {
                Id = 4,
                ItemTypeId = 3,
                ModelName = "MARKUS",
                Price = 1395.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 12
            },
            new ItemGroup()
            {
                Id = 5,
                ItemTypeId = 4,
                ModelName = "Dell UltraSharp U2723QE",
                Price = 5299.99m,
                Manufacturer = "Dell",
                WarrantyPeriod = "3 år",
                Quantity = 10
            },
            new ItemGroup()
            {
                Id = 6,
                ItemTypeId = 5,
                ModelName = "Logitech MX Keys",
                Price = 999.00m,
                Manufacturer = "Logitech",
                WarrantyPeriod = "2 år",
                Quantity = 25
            });

            modelBuilder.Entity<Item>().HasData(
            // ItemGroupId = 1 (Acer Nitro 5 - Computer)
            new Item() { Id = 1, RoomId = 1, ItemGroupId = 1, SerialNumber = "ACN5-001A" },
            new Item() { Id = 2, RoomId = 3, ItemGroupId = 1, SerialNumber = "ACN5-001B" },
            new Item() { Id = 3, RoomId = 5, ItemGroupId = 1, SerialNumber = "ACN5-001C" },
            new Item() { Id = 4, RoomId = 2, ItemGroupId = 1, SerialNumber = "ACN5-001D" },

            // ItemGroupId = 2 (SANDSBERG - Bord)
            new Item() { Id = 5, RoomId = 4, ItemGroupId = 2, SerialNumber = "SNDB-1001" },
            new Item() { Id = 6, RoomId = 6, ItemGroupId = 2, SerialNumber = "SNDB-1002" },
            new Item() { Id = 7, RoomId = 2, ItemGroupId = 2, SerialNumber = "SNDB-1003" },
            new Item() { Id = 8, RoomId = 1, ItemGroupId = 2, SerialNumber = "SNDB-1004" },
            new Item() { Id = 9, RoomId = 5, ItemGroupId = 2, SerialNumber = "SNDB-1005" },

            // ItemGroupId = 3 (HP Envy x360 - Computer)
            new Item() { Id = 10, RoomId = 1, ItemGroupId = 3, SerialNumber = "HPX360-01A" },
            new Item() { Id = 11, RoomId = 2, ItemGroupId = 3, SerialNumber = "HPX360-01B" },
            new Item() { Id = 12, RoomId = 3, ItemGroupId = 3, SerialNumber = "HPX360-01C" },
            new Item() { Id = 13, RoomId = 4, ItemGroupId = 3, SerialNumber = "HPX360-01D" },
            new Item() { Id = 14, RoomId = 6, ItemGroupId = 3, SerialNumber = "HPX360-01E" },

            // ItemGroupId = 4 (MARKUS - Chair)
            new Item() { Id = 15, RoomId = 1, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-01" },
            new Item() { Id = 16, RoomId = 2, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-02" },
            new Item() { Id = 17, RoomId = 3, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-03" },
            new Item() { Id = 18, RoomId = 4, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-04" },
            new Item() { Id = 19, RoomId = 5, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-05" },
            new Item() { Id = 20, RoomId = 6, ItemGroupId = 4, SerialNumber = "MARK-CHAIR-06" },

            // ItemGroupId = 5 (Dell UltraSharp - Monitor)
            new Item() { Id = 21, RoomId = 1, ItemGroupId = 5, SerialNumber = "DUS2723-01" },
            new Item() { Id = 22, RoomId = 2, ItemGroupId = 5, SerialNumber = "DUS2723-02" },
            new Item() { Id = 23, RoomId = 3, ItemGroupId = 5, SerialNumber = "DUS2723-03" },
            new Item() { Id = 24, RoomId = 4, ItemGroupId = 5, SerialNumber = "DUS2723-04" },

            // ItemGroupId = 6 (Logitech MX Keys - Keyboard)
            new Item() { Id = 25, RoomId = 5, ItemGroupId = 6, SerialNumber = "MXKEYS-001" },
            new Item() { Id = 26, RoomId = 6, ItemGroupId = 6, SerialNumber = "MXKEYS-002" },
            new Item() { Id = 27, RoomId = 1, ItemGroupId = 6, SerialNumber = "MXKEYS-003" },
            new Item() { Id = 28, RoomId = 2, ItemGroupId = 6, SerialNumber = "MXKEYS-004" },
            new Item() { Id = 29, RoomId = 3, ItemGroupId = 6, SerialNumber = "MXKEYS-005" },
            new Item() { Id = 30, RoomId = 4, ItemGroupId = 6, SerialNumber = "MXKEYS-006" },
            new Item() { Id = 31, RoomId = 5, ItemGroupId = 6, SerialNumber = "MXKEYS-007" },
            new Item() { Id = 32, RoomId = 6, ItemGroupId = 6, SerialNumber = "MXKEYS-008" }
            );

            modelBuilder.Entity<StatusHistory>().HasData(
            new StatusHistory()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            },
            new StatusHistory()
            {
                Id = 2,
                ItemId = 2,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            },
            new StatusHistory()
            {
                Id = 3,
                ItemId = 1,
                StatusId = 2,
                StatusUpdateDate = new DateTime(2024, 11, 28),
                Note = "Virke ikke"
            },
            new StatusHistory()
            {
                Id = 4,
                ItemId = 2,
                StatusId = 2,
                StatusUpdateDate = new DateTime(2024, 11, 30),
                Note = "Virke ikke"
            }, new StatusHistory()
            {
                Id = 5,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 12, 01),
                Note = "Virke"
            },
            new StatusHistory() { Id = 6, ItemId = 1, StatusId = 2, StatusUpdateDate = new DateTime(2024, 12, 24), Note = "Virker ikke" },
            new StatusHistory() { Id = 7, ItemId = 2, StatusId = 2, StatusUpdateDate = new DateTime(2025, 2, 18), Note = "Virker ikke" },
            new StatusHistory() { Id = 8, ItemId = 2, StatusId = 1, StatusUpdateDate = new DateTime(2024, 12, 27), Note = "Virker" },
            new StatusHistory() { Id = 9, ItemId = 2, StatusId = 3, StatusUpdateDate = new DateTime(2024, 12, 6), Note = "Til reparation" },
            new StatusHistory() { Id = 10, ItemId = 3, StatusId = 3, StatusUpdateDate = new DateTime(2025, 3, 24), Note = "Til reparation" },
            new StatusHistory() { Id = 11, ItemId = 3, StatusId = 3, StatusUpdateDate = new DateTime(2025, 3, 25), Note = "Til reparation" },
            new StatusHistory() { Id = 12, ItemId = 4, StatusId = 3, StatusUpdateDate = new DateTime(2024, 12, 10), Note = "Til reparation" },
            new StatusHistory() { Id = 13, ItemId = 5, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 3), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 14, ItemId = 5, StatusId = 3, StatusUpdateDate = new DateTime(2025, 1, 15), Note = "Til reparation" },
            new StatusHistory() { Id = 15, ItemId = 5, StatusId = 2, StatusUpdateDate = new DateTime(2024, 12, 28), Note = "Virker ikke" },
            new StatusHistory() { Id = 16, ItemId = 6, StatusId = 1, StatusUpdateDate = new DateTime(2025, 1, 8), Note = "Ny" },
            new StatusHistory() { Id = 17, ItemId = 7, StatusId = 3, StatusUpdateDate = new DateTime(2025, 1, 22), Note = "Til reparation" },
            new StatusHistory() { Id = 18, ItemId = 8, StatusId = 3, StatusUpdateDate = new DateTime(2025, 1, 12), Note = "Til reparation" },
            new StatusHistory() { Id = 19, ItemId = 8, StatusId = 4, StatusUpdateDate = new DateTime(2024, 11, 13), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 20, ItemId = 8, StatusId = 1, StatusUpdateDate = new DateTime(2024, 10, 5), Note = "Ny" },
            new StatusHistory() { Id = 21, ItemId = 9, StatusId = 1, StatusUpdateDate = new DateTime(2024, 10, 13), Note = "Ny" },
            new StatusHistory() { Id = 22, ItemId = 10, StatusId = 4, StatusUpdateDate = new DateTime(2025, 2, 6), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 23, ItemId = 10, StatusId = 3, StatusUpdateDate = new DateTime(2024, 12, 6), Note = "Til reparation" },
            new StatusHistory() { Id = 24, ItemId = 10, StatusId = 3, StatusUpdateDate = new DateTime(2025, 3, 24), Note = "Til reparation" },
            new StatusHistory() { Id = 25, ItemId = 11, StatusId = 4, StatusUpdateDate = new DateTime(2025, 3, 13), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 26, ItemId = 11, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 3), Note = "Ny" },
            new StatusHistory() { Id = 27, ItemId = 12, StatusId = 1, StatusUpdateDate = new DateTime(2024, 12, 12), Note = "Ny" },
            new StatusHistory() { Id = 28, ItemId = 13, StatusId = 2, StatusUpdateDate = new DateTime(2024, 12, 22), Note = "Virker ikke" },
            new StatusHistory() { Id = 29, ItemId = 13, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 28), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 30, ItemId = 13, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 6), Note = "Ny" },
            new StatusHistory() { Id = 31, ItemId = 14, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 9), Note = "Ny" },
            new StatusHistory() { Id = 32, ItemId = 14, StatusId = 1, StatusUpdateDate = new DateTime(2024, 10, 16), Note = "Ny" },
            new StatusHistory() { Id = 33, ItemId = 15, StatusId = 3, StatusUpdateDate = new DateTime(2025, 1, 7), Note = "Til reparation" },
            new StatusHistory() { Id = 34, ItemId = 15, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 13), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 35, ItemId = 15, StatusId = 2, StatusUpdateDate = new DateTime(2024, 12, 11), Note = "Virker ikke" },
            new StatusHistory() { Id = 36, ItemId = 15, StatusId = 1, StatusUpdateDate = new DateTime(2024, 12, 1), Note = "Virker" },
            new StatusHistory() { Id = 37, ItemId = 16, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 26), Note = "Ny" },
            new StatusHistory() { Id = 38, ItemId = 17, StatusId = 4, StatusUpdateDate = new DateTime(2024, 10, 21), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 39, ItemId = 18, StatusId = 2, StatusUpdateDate = new DateTime(2025, 1, 22), Note = "Virker ikke" },
            new StatusHistory() { Id = 40, ItemId = 18, StatusId = 3, StatusUpdateDate = new DateTime(2024, 12, 17), Note = "Til reparation" },
            new StatusHistory() { Id = 41, ItemId = 18, StatusId = 3, StatusUpdateDate = new DateTime(2025, 1, 7), Note = "Til reparation" },
            new StatusHistory() { Id = 42, ItemId = 19, StatusId = 1, StatusUpdateDate = new DateTime(2025, 3, 2), Note = "Ny" },
            new StatusHistory() { Id = 43, ItemId = 20, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 4), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 44, ItemId = 21, StatusId = 1, StatusUpdateDate = new DateTime(2024, 12, 10), Note = "Ny" },
            new StatusHistory() { Id = 45, ItemId = 22, StatusId = 3, StatusUpdateDate = new DateTime(2025, 2, 7), Note = "Til reparation" },
            new StatusHistory() { Id = 46, ItemId = 23, StatusId = 2, StatusUpdateDate = new DateTime(2025, 1, 3), Note = "Virker ikke" },
            new StatusHistory() { Id = 47, ItemId = 24, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 18), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 48, ItemId = 24, StatusId = 1, StatusUpdateDate = new DateTime(2025, 2, 6), Note = "Virker" },
            new StatusHistory() { Id = 49, ItemId = 25, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 9), Note = "Ny" },
            new StatusHistory() { Id = 50, ItemId = 25, StatusId = 4, StatusUpdateDate = new DateTime(2025, 2, 5), Note = "Udlånt til administrator" },
            new StatusHistory() { Id = 51, ItemId = 26, StatusId = 1, StatusUpdateDate = new DateTime(2024, 11, 2), Note = "Ny" },
            new StatusHistory() { Id = 52, ItemId = 27, StatusId = 3, StatusUpdateDate = new DateTime(2024, 12, 15), Note = "Til reparation" },
            new StatusHistory() { Id = 53, ItemId = 28, StatusId = 4, StatusUpdateDate = new DateTime(2025, 2, 16), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 54, ItemId = 28, StatusId = 1, StatusUpdateDate = new DateTime(2025, 3, 19), Note = "Virker" },
            new StatusHistory() { Id = 55, ItemId = 29, StatusId = 2, StatusUpdateDate = new DateTime(2025, 1, 9), Note = "Virker ikke" },
            new StatusHistory() { Id = 56, ItemId = 30, StatusId = 3, StatusUpdateDate = new DateTime(2025, 2, 1), Note = "Til reparation" },
            new StatusHistory() { Id = 57, ItemId = 31, StatusId = 1, StatusUpdateDate = new DateTime(2024, 12, 5), Note = "Ny" },
            new StatusHistory() { Id = 58, ItemId = 32, StatusId = 1, StatusUpdateDate = new DateTime(2025, 3, 11), Note = "Ny" },
            new StatusHistory() { Id = 59, ItemId = 32, StatusId = 4, StatusUpdateDate = new DateTime(2025, 1, 5), Note = "Udlånt til bruger" },
            new StatusHistory() { Id = 60, ItemId = 32, StatusId = 1, StatusUpdateDate = new DateTime(2025, 2, 2), Note = "Virker" }
            );

            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Email = EncryptionHelper.Encrypt("noree@tec.dk"),
                    Name = "Noree",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 2,
                    Email = EncryptionHelper.Encrypt("laura@tec.dk"),
                    Name = "Laura",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 3,
                    Email = EncryptionHelper.Encrypt("daniel@tec.dk"),
                    Name = "Daniel",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 4,
                    Email = EncryptionHelper.Encrypt("tobias@tec.dk"),
                    Name = "Tobias",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 5,
                    Email = EncryptionHelper.Encrypt("philip@tec.dk"),
                    Name = "Philip",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 6,
                    Email = EncryptionHelper.Encrypt("kristian@tec.dk"),
                    Name = "Kristian",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 2,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 7,
                    Email = EncryptionHelper.Encrypt("casper@tec.dk"),
                    Name = "Casper",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 2,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 8,
                    Email = EncryptionHelper.Encrypt("rami@tec.dk"),
                    Name = "Rami",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 2,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 9,
                    Email = EncryptionHelper.Encrypt("lucas@tec.dk"),
                    Name = "Lucas",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 3,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 10,
                    Email = EncryptionHelper.Encrypt("victor@tec.dk"),
                    Name = "Victor",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 3,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 11,
                    Email = EncryptionHelper.Encrypt("sebastian@tec.dk"),
                    Name = "Sebastian",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 4,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 12,
                    Email = EncryptionHelper.Encrypt("alexander@tec.dk"),
                    Name = "Alexander",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 4,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 13,
                    Email = EncryptionHelper.Encrypt("flemming@tec.dk"),
                    Name = "Flemming",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                },
                new User()
                {
                    Id = 14,
                    Email = EncryptionHelper.Encrypt("Censor@tec.dk"),
                    Name = "Censor",
                    Password = BCrypt.Net.BCrypt.HashPassword("xHBE7EWBmFL-s2L", salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = 1,
                    TwoFactorAuthentication = true,
                    TwoFactorSecretKey = "",
                }
            );

            modelBuilder.Entity<Request>().HasData(
            new Request()
            {
                Id = 1,
                UserId = 3,
                Message = "I need a laptop for my studies.",
                Date = new DateTime(2024, 11, 12, 14, 30, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Laptop",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 2,
                UserId = 4,
                Message = "Can I borrow the HP Pavilion laptop for a meeting?",
                Date = new DateTime(2024, 11, 13, 9, 00, 0),
                RecipientEmail = "noree@tec.dk",
                Item = "HP Pavilion Laptop",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 3,
                UserId = 6,
                Message = "I need a new monitor for better visibility during class.",
                Date = new DateTime(2024, 11, 15, 11, 25, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 4,
                UserId = 7,
                Message = "Can I borrow a keyboard for a workshop?",
                Date = new DateTime(2024, 11, 16, 13, 10, 0),
                RecipientEmail = "laura@tec.dk", 
                Item = "Keyboard",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 5,
                UserId = 8,
                Message = "I need a Lenovo ThinkPad for a project demonstration.",
                Date = new DateTime(2024, 11, 17, 15, 00, 0),
                RecipientEmail = "noree@tec.dk", 
                Item = "Lenovo ThinkPad",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 6,
                UserId = 9,
                Message = "Can I borrow a monitor for my exams?",
                Date = new DateTime(2024, 11, 18, 16, 45, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 7,
                UserId = 10,
                Message = "Requesting a keyboard for setup work.",
                Date = new DateTime(2024, 11, 19, 10, 30, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Keyboard",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 8,
                UserId = 11,
                Message = "Can I borrow a monitor for troubleshooting purposes?",
                Date = new DateTime(2024, 11, 20, 17, 20, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Godkent",
            },
            new Request()
            {
                Id = 9,
                UserId = 3,
                Message = "I need a laptop for my studies.",
                Date = new DateTime(2024, 11, 12, 14, 30, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Laptop",
                Status = "Sent",
            },
            new Request()
            {
                Id = 10,
                UserId = 4, 
                Message = "Can I borrow the HP Pavilion laptop for a meeting?",
                Date = new DateTime(2024, 11, 13, 9, 00, 0),
                RecipientEmail = "noree@tec.dk", 
                Item = "HP Pavilion Laptop",
                Status = "Sent",
            },
            new Request()
            {
                Id = 11,
                UserId = 6, 
                Message = "I need a new monitor for better visibility during class.",
                Date = new DateTime(2024, 11, 15, 11, 25, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Sent",
            },
            new Request()
            {
                Id = 12,
                UserId = 7, 
                Message = "Can I borrow a keyboard for a workshop?",
                Date = new DateTime(2024, 11, 16, 13, 10, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Keyboard",
                Status = "Sent",
            },
            new Request()
            {
                Id = 13,
                UserId = 8,
                Message = "I need a Lenovo ThinkPad for a project demonstration.",
                Date = new DateTime(2024, 11, 17, 15, 00, 0),
                RecipientEmail = "noree@tec.dk", 
                Item = "Lenovo ThinkPad",
                Status = "Sent",
            },
            new Request()
            {
                Id = 14,
                UserId = 9,
                Message = "Can I borrow a monitor for my exams?",
                Date = new DateTime(2024, 11, 18, 16, 45, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Sent",
            },
            new Request()
            {
                Id = 15,
                UserId = 10,
                Message = "Requesting a keyboard for setup work.",
                Date = new DateTime(2024, 11, 19, 10, 30, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Keyboard",
                Status = "Sent",
            },
            new Request()
            {
                Id = 16,
                UserId = 11,
                Message = "Can I borrow a monitor for troubleshooting purposes?",
                Date = new DateTime(2024, 11, 20, 17, 20, 0),
                RecipientEmail = "casper@tec.dk",
                Item = "Monitor",
                Status = "Sent", 
            }
            );


            modelBuilder.Entity<Loan>().HasData(
            new Loan()
            {
                Id = 1,
                ItemId = 1,
                UserId = 2,
                LoanDate = new DateTime(2024, 10, 15, 8, 59, 59),
                ReturnDate = new DateTime(2026, 06, 29, 14, 59, 59),
            },
            new Loan()
            {
                Id = 2,
                ItemId = 3,
                UserId = 2,
                LoanDate = new DateTime(2024, 10, 15, 8, 59, 59),
                ReturnDate = new DateTime(2026, 06, 29, 14, 59, 59),
            },
            new Loan()
            {
                Id = 3,
                ItemId = 5,
                UserId = 2,
                LoanDate = new DateTime(2024, 11, 5, 9, 30, 0),
                ReturnDate = new DateTime(2026, 07, 10, 12, 00, 0),
            },
            new Loan()
            {
                Id = 4,
                ItemId = 8,
                UserId = 6,
                LoanDate = new DateTime(2024, 11, 6, 9, 30, 0),
                ReturnDate = new DateTime(2026, 07, 12, 12, 00, 0),
            },
            new Loan()
            {
                Id = 5,
                ItemId = 10,
                UserId = 7,
                LoanDate = new DateTime(2024, 11, 10, 10, 00, 0),
                ReturnDate = new DateTime(2026, 07, 15, 15, 00, 0),
            },
            new Loan()
            {
                Id = 6,
                ItemId = 12,
                UserId = 8,
                LoanDate = new DateTime(2024, 11, 20, 14, 00, 0),
                ReturnDate = new DateTime(2026, 08, 1, 11, 00, 0),
            },
            new Loan()
            {
                Id = 7,
                ItemId = 15,
                UserId = 9,
                LoanDate = new DateTime(2024, 11, 25, 13, 00, 0),
                ReturnDate = new DateTime(2026, 08, 4, 16, 00, 0),
            },
            new Loan()
            {
                Id = 8,
                ItemId = 18,
                UserId = 10,
                LoanDate = new DateTime(2024, 12, 1, 8, 00, 0),
                ReturnDate = new DateTime(2026, 08, 8, 14, 00, 0),
            },
            new Loan()
            {
                Id = 9,
                ItemId = 22,
                UserId = 11,
                LoanDate = new DateTime(2024, 12, 5, 9, 30, 0),
                ReturnDate = new DateTime(2026, 08, 20, 12, 00, 0),
            },
            new Loan()
            {
                Id = 10,
                ItemId = 25,
                UserId = 12,
                LoanDate = new DateTime(2024, 12, 10, 10, 00, 0),
                ReturnDate = new DateTime(2026, 08, 25, 13, 30, 0),
            }
            );

            // Only for performance testing

           /* modelBuilder.Entity<Archive_Item>().HasData(
            new Archive_Item()
            {
                Id = 33,
                ItemGroupId = 1,
                SerialNumber = "SerieNumber-123",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Item archived due to damage.",
            }, 
            new Archive_Item()
            {
                Id = 34,
                ItemGroupId = 2,
                SerialNumber = "SerieNumber-123",
                DeleteTime = new DateTime(2024, 11, 15, 8, 59, 59),
                ArchiveNote = "Item archived due to damage.",
            }
            );

            modelBuilder.Entity<Archive_ItemGroup>().HasData(
            new Archive_ItemGroup()
            {
                Id = 7,
                ItemTypeId = 4,
                ModelName = "Dell UltraSharp++ U2723QE",
                Price = 5299.99m,
                Manufacturer = "Dell",
                WarrantyPeriod = "3 år",
                Quantity = 10,
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Fokert navn",
            },
            new Archive_ItemGroup()
            {
                Id = 8,
                ItemTypeId = 5,
                ModelName = "Logitech MX0 Keys",
                Price = 999.00m,
                Manufacturer = "Logitech",
                WarrantyPeriod = "2 år",
                Quantity = 25,
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Fokert navn",
            });

            modelBuilder.Entity<Archive_ItemType>().HasData(
            new Archive_ItemType()
            {
                Id = 6,
                TypeName = "Papir",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Fokert type",
            },
            new Archive_ItemType()
            {
                Id = 7,
                TypeName = "Dør",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Fokert type",
            }
            );

            modelBuilder.Entity<Archive_Loan>().HasData(
            new Archive_Loan()
            {
                Id = 11,
                ItemId = 4,
                UserId = 11,
                LoanDate = new DateTime(2024, 12, 5, 9, 30, 0),
                ReturnDate = new DateTime(2026, 08, 20, 12, 00, 0),
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Udløb",
            },
            new Archive_Loan()
            {
                Id = 12,
                ItemId = 20,
                UserId = 12,
                LoanDate = new DateTime(2024, 12, 10, 10, 00, 0),
                ReturnDate = new DateTime(2026, 08, 25, 13, 30, 0),
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Udløb",
            }
            );

            modelBuilder.Entity<Archive_User>().HasData(
            new Archive_User()
            {
                Id = 12,
                Email = EncryptionHelper.Encrypt("gert@tec.dk"),
                Name = "Gert",
                Password = BCrypt.Net.BCrypt.HashPassword("Passw0rd!0789", salt, true, BCrypt.Net.HashType.SHA512),
                RoleId = 2,
                TwoFactorAuthentication = true,
                TwoFactorSecretKey = "",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Sige op",
            },
            new Archive_User()
            {
                Id = 13,
                Email = EncryptionHelper.Encrypt("mick@tec.dk"),
                Name = "Mick",
                Password = BCrypt.Net.BCrypt.HashPassword("Passw0rd!0789", salt, true, BCrypt.Net.HashType.SHA512),
                RoleId = 4,
                TwoFactorAuthentication = true,
                TwoFactorSecretKey = "",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Færdig med uddannes",
            }
            );

            modelBuilder.Entity<Archive_Request>().HasData(
            new Archive_Request()
            {
                Id = 17,
                UserId = 4,
                Message = "I need a laptop for my studies.",
                Date = new DateTime(2024, 11, 12, 14, 30, 0),
                RecipientEmail = "laura@tec.dk",
                Item = "Laptop",
                Status = "Godkent",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Godkent",
            },
            new Archive_Request()
            {
                Id = 18,
                UserId = 7,
                Message = "Can I borrow the HP Pavilion laptop for a meeting?",
                Date = new DateTime(2024, 11, 13, 9, 00, 0),
                RecipientEmail = "noree@tec.dk",
                Item = "HP Pavilion Laptop",
                Status = "Godkent",
                DeleteTime = new DateTime(2024, 10, 15, 8, 59, 59),
                ArchiveNote = "Godkent",
            }
            );
           */
        }
    }
}
