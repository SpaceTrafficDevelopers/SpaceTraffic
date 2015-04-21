/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SpaceTraffic.Entities;
using System.Data.Entity.ModelConfiguration;

 namespace SpaceTraffic.Persistence
{
    public class SpaceTrafficContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<SpaceShip> SpaceShips { get; set; }

        public DbSet<Base> Bases { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Factory> Factories { get; set; }

        public DbSet<Cargo> Cargos { get; set; }

        public DbSet<SpaceShipCargo> SpaceShipsCargos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new PlayerConfiguration());
            modelBuilder.Configurations.Add(new BaseConfiguration());
            modelBuilder.Configurations.Add(new SpaceShipConfiguration());
            modelBuilder.Configurations.Add(new CargoConfiguration());
            modelBuilder.Configurations.Add(new FactoryConfiguration());
            modelBuilder.Configurations.Add(new SpaceShipCargoConfiguration());
            base.OnModelCreating(modelBuilder); 
            
        }
    }

    //TODO: UniqueConstraint http://stackoverflow.com/questions/9363967/trying-to-create-a-hasunique-on-entitytypeconfiguration-in-entity-framework-gen
  

    #region Configuration entities

    public class PlayerConfiguration : EntityTypeConfiguration<Player>
    {
        /// <summary>
        /// Player configuration of persistent layer
        /// </summary>
        public PlayerConfiguration()
            : base()
        {
            HasKey(p => p.PlayerId);
            Property(p => p.PlayerName).HasMaxLength(50).HasColumnType("nvarchar").IsRequired();
            Property(p => p.FirstName).HasMaxLength(50).HasColumnType("varchar").IsOptional();
            Property(p => p.LastName).HasMaxLength(50).HasColumnType("varchar").IsOptional();
            Property(p => p.Email).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(p => p.PsswdHash).HasMaxLength(200).HasColumnType("varchar").IsOptional();
            Property(p => p.PsswdSalt).HasMaxLength(50).HasColumnType("varchar").IsOptional();
            Property(p => p.DateOfBirth).HasColumnType("datetime").IsRequired();
            Property(p => p.OrionEmail).HasMaxLength(50).HasColumnType("varchar").IsOptional();
            Property(p => p.IsFavStudent).IsOptional();
            Property(p => p.IsOrionEmailConfirmed).IsOptional();
            Property(p => p.IsEmailConfirmed).IsOptional();
            Property(p => p.IsAccountLocked).IsOptional();
            Property(p => p.AddedDate).HasColumnType("datetime").IsOptional();
            Property(p => p.LastVisitedDate).HasColumnType("datetime").IsOptional();
            Property(p => p.CorporationName).HasMaxLength(50).HasColumnType("varchar").IsOptional();
            Property(p => p.Credit).HasColumnType("int").IsOptional();            
            ToTable("Players");
        }

    }

    public class SpaceShipConfiguration : EntityTypeConfiguration<SpaceShip>
    {
        /// <summary>
        /// SpaceShip configuration of persistent layer
        /// </summary>
        public SpaceShipConfiguration()
            : base()
        {
            HasKey(p => p.SpaceShipId);
            Property(p => p.DamagePercent).HasColumnType("float").IsRequired();
            Property(p => p.SpaceShipName).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(p => p.SpaceShipModel).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(p => p.UserCode).HasMaxLength(200).HasColumnType("varchar").IsRequired();
            Property(p => p.TimeOfArrival).HasMaxLength(10).HasColumnType("varchar").IsRequired();
            Property(p => p.CurrentStarSystem).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(p => p.FuelTank).HasColumnType("int").IsRequired();
            Property(p => p.CurrentFuelTank).HasColumnType("int").IsRequired();
            Property(p => p.IsFlying).IsRequired();
            HasRequired(a => a.Base).WithMany(a => a.SpaceShips).HasForeignKey(a => a.DockedAtBaseId);
            HasRequired(p => p.Player).WithMany(p => p.SpaceShips).HasForeignKey(s => s.PlayerId);                    
            ToTable("SpaceShips");
        }
    }

    public class BaseConfiguration : EntityTypeConfiguration<Base>
    {
        /// <summary>
        /// SpaceShip configuration of persistent layer
        /// </summary>
        public BaseConfiguration()
            : base()
        {
            HasKey(b => b.BaseId);
            Property(b => b.Planet).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            ToTable("Bases");
        }

    }

    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        /// <summary>
        /// SpaceShip configuration of persistent layer
        /// </summary>
        public MessageConfiguration()
            : base()
        {
            HasKey(m => m.MessageId);
            Property(m => m.MetaInfo).HasMaxLength(1000).HasColumnType("varchar").IsRequired();
            Property(m => m.Type).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(p => p.Body).HasMaxLength(1000).HasColumnType("varchar").IsRequired();
            HasRequired(p => p.RecipientPlayer).WithMany().HasForeignKey(s => s.RecipientPlayerId).WillCascadeOnDelete(false);
            Property(p => p.From).IsRequired();
            ToTable("Messages");
        }
    }

    public class CargoConfiguration : EntityTypeConfiguration<Cargo>
    {
        /// <summary>
        /// Commodity configuration of persistent layer
        /// </summary>
        public CargoConfiguration()
            : base()
        {
            HasKey(p => p.CargoId);
            Property(p => p.Price).HasColumnType("int").IsRequired();
            Property(p => p.Type).HasMaxLength(50).HasColumnType("varchar").IsRequired();          ;            
            ToTable("Cargos");
        }

    }

    public class FactoryConfiguration : EntityTypeConfiguration<Factory>
    {
        /// <summary>
        /// Factory configuration of persistent layer
        /// </summary>
        public FactoryConfiguration()
            : base()
        {
            HasKey(p => p.FacotryId);
            Property(p => p.Type).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            Property(p => p.CargoCount).HasColumnType("int").IsRequired();        
            HasRequired(a => a.Cargo).WithMany(a => a.Factories).HasForeignKey(a => a.CargoId);
            HasRequired(a => a.Base).WithMany(a => a.Factories).HasForeignKey(a => a.BaseId);
            ToTable("Factories");
        }

    }

    public class SpaceShipCargoConfiguration : EntityTypeConfiguration<SpaceShipCargo>
    {
        /// <summary>
        /// Factory configuration of persistent layer
        /// </summary>
        public SpaceShipCargoConfiguration()
            : base()
        {
            HasKey(p => new { p.CargoId, p.SpaceShipId });
            Property(p => p.CargoCount).HasColumnType("int").IsRequired();            
            HasRequired(a => a.Cargo).WithMany(a => a.SpaceShipsCargos).HasForeignKey(a => a.CargoId);
            HasRequired(a => a.SpaceShip).WithMany(a => a.SpaceShipsCargos).HasForeignKey(a => a.SpaceShipId);
            ToTable("SpaceShipsCargos");
        }

    }

    

#endregion
}
