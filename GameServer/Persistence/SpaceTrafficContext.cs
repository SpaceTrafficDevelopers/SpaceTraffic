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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using SpaceTraffic.Dao;
using SpaceTraffic.Entities;

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

        public DbSet<Trader> Traders { get; set; }

        public DbSet<TraderCargo> TraderCargos { get; set; }

        public DbSet<GameAction> GameActions { get; set; }

        public DbSet<GameEvent> GameEvents { get; set; }

        public DbSet<PathPlanEntity> PathPlan { get; set; }

        public DbSet<PlanItemEntity> PlanItem { get; set; }

        public DbSet<PlanAction> PlanAction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new PlayerConfiguration());
            modelBuilder.Configurations.Add(new BaseConfiguration());
            modelBuilder.Configurations.Add(new SpaceShipConfiguration());
            modelBuilder.Configurations.Add(new CargoConfiguration());
            modelBuilder.Configurations.Add(new FactoryConfiguration());
            modelBuilder.Configurations.Add(new SpaceShipCargoConfiguration());
            modelBuilder.Configurations.Add(new TraderConfiguration());
            modelBuilder.Configurations.Add(new TraderCargoConfiguration());
            modelBuilder.Configurations.Add(new GameActionConfiguration());
            modelBuilder.Configurations.Add(new GameEventConfiguration());
            modelBuilder.Configurations.Add(new PathPlanEntityConfiguration());
            modelBuilder.Configurations.Add(new PlanItemEntityConfiguration());
            modelBuilder.Configurations.Add(new PlanActionConfiguration());
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
            Property(p => p.CargoSpace).HasColumnType("int").IsRequired();
            Property(p => p.Consumption).IsRequired();
            Property(p => p.WearRate).IsRequired();
            Property(p => p.MaxSpeed).HasColumnType("int").IsRequired();
            HasOptional(a => a.Base).WithMany(a => a.SpaceShips).HasForeignKey(a => a.DockedAtBaseId);
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
            Ignore(p => p.Trader);
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
            Property(p => p.DefaultPrice).HasColumnType("int").IsRequired();
            Property(p => p.Name).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            Property(p => p.Description).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            Property(p => p.Category).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            Property(p => p.LevelToBuy).HasColumnType("int").IsRequired();
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
            HasKey(p => p.SpaceShipCargoId);
            Property(p => p.CargoId).HasColumnType("int").IsRequired();
            Property(p => p.SpaceShipId).HasColumnType("int").IsRequired();
            Property(p => p.CargoCount).HasColumnType("int").IsRequired();
            Property(p => p.CargoPrice).HasColumnType("int").IsRequired();   
            HasRequired(a => a.Cargo).WithMany(a => a.SpaceShipsCargos).HasForeignKey(a => a.CargoId);
            HasRequired(a => a.SpaceShip).WithMany(a => a.SpaceShipsCargos).HasForeignKey(a => a.SpaceShipId);
            Ignore(p => p.CargoOwnerId);
            Ignore(p => p.CargoLoadEntityId);
            ToTable("SpaceShipsCargos");
        }

    }

    public class TraderConfiguration : EntityTypeConfiguration<Trader>
    {
        /// <summary>
        /// Trader configuration of persistent layer
        /// </summary>
        public TraderConfiguration()
            : base()
        {
            HasKey(p => p.TraderId);
            HasRequired(p => p.Base).WithMany().HasForeignKey(s => s.BaseId).WillCascadeOnDelete(false);
            ToTable("Traders");
        }

    }

    public class TraderCargoConfiguration : EntityTypeConfiguration<TraderCargo>
    {
        /// <summary>
        /// Factory configuration of persistent layer
        /// </summary>
        public TraderCargoConfiguration()
            : base()
        {
            HasKey(p => p.TraderCargoId);
            Property(p => p.CargoId).HasColumnType("int").IsRequired();
            Property(p => p.TraderId).HasColumnType("int").IsRequired();
            Property(p => p.CargoCount).HasColumnType("int").IsRequired();
            Property(p => p.CargoPrice).HasColumnType("int").IsRequired();
            HasRequired(a => a.Cargo).WithMany(a => a.TraderCargos).HasForeignKey(a => a.CargoId);
            HasRequired(a => a.Trader).WithMany(a => a.TraderCargos).HasForeignKey(a => a.TraderId);
            Ignore(p => p.CargoOwnerId);
            Ignore(p => p.CargoLoadEntityId);
            ToTable("TraderCargos");
        }
    }


    public class GameActionConfiguration : EntityTypeConfiguration<GameAction>
    {
        /// <summary>
        /// Configures the persistence store for the <see cref="GameAction"/> entity.
        /// </summary>
        /// <seealso cref="GameActionDAO"/>
        /// <seealso cref="GameActionDAO.RemoveAllActions"/>
        public GameActionConfiguration()
            : base()
        {
            HasKey(p => p.ActionCode);
            Property(p => p.ActionCode).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Type).HasColumnType("varchar").HasMaxLength(1024).IsRequired();
            Property(p => p.Sequence).HasColumnType("int").IsRequired();
            Property(p => p.PlayerId).HasColumnType("int").IsRequired();
            Property(p => p.State).HasColumnType("int").IsRequired();
            Property(p => p.ActionArgs).HasColumnType("varbinary(max)").IsOptional();

            // Table renamers, beware! The following table name is used directly in a SQL command.
            // If you would like to change the name of the table, also change the SQL command in
            // SpaceTraffic.Dao.GameActionDAO::RemoveAllActions().
            // The reason for this heresy is explained in the mentioned method.
            ToTable("GameActions");
        }
    }

    public class GameEventConfiguration : EntityTypeConfiguration<GameEvent>
    {
        /// <summary>
        /// Configures the persistence store for the <see cref="GameAction"/> entity.
        /// </summary>
        /// <seealso cref="GameEventDAO"/>
        /// <seealso cref="GameEventDAO.RemoveAllEvents"/>
        public GameEventConfiguration()
            : base()
        {
            HasKey(p => p.Id);
            Property(p => p.EventType).HasColumnType("varchar").HasMaxLength(1024).IsRequired();
            Property(p => p.PlannedTime).HasColumnType("datetime2").IsRequired();
            Property(p => p.ActionType).HasColumnType("varchar").HasMaxLength(1024).IsRequired();
            Property(p => p.ActionCode).HasColumnType("int").IsOptional();
            Property(p => p.PlayerId).HasColumnType("int").IsRequired();
            Property(p => p.ActionState).HasColumnType("int").IsOptional();
            Property(p => p.ActionArgs).HasColumnType("varbinary(max)").IsOptional();

            // Table renamers, beware! The following table name is also used directly in a SQL command.
            // If you would like to change the name of the table, also change the SQL command in
            // SpaceTraffic.Dao.GameEventDAO::RemoveAllEvents().
            ToTable("GameEvents");
        }
    }

    public class PathPlanEntityConfiguration : EntityTypeConfiguration<PathPlanEntity>
    {

        public PathPlanEntityConfiguration()
            : base()
        {
            HasKey(p => p.PathPlanId);
            Property(p => p.IsPlanned).IsRequired();
            HasRequired(p => p.Player).WithMany().HasForeignKey(p => p.PlayerId).WillCascadeOnDelete(true);
            HasRequired(p => p.SpaceShip).WithMany().HasForeignKey(p => p.SpaceShipId).WillCascadeOnDelete(false);

            ToTable("PathPlan");
        }
    }

    public class PlanItemEntityConfiguration : EntityTypeConfiguration<PlanItemEntity>
    {

        public PlanItemEntityConfiguration()
            : base()
        {
            HasKey(p => p.PlanItemId);
            Property(p => p.SolarSystem).HasColumnType("varchar").HasMaxLength(256).IsRequired();
            Property(p => p.Index).HasColumnType("varchar").HasMaxLength(256).IsRequired();
            Property(p => p.SequenceNumber).HasColumnType("int").IsRequired();
            Property(p => p.IsPlanet).IsRequired();
            HasRequired(p => p.PathPlanEntity).WithMany(p => p.Items).HasForeignKey(p => p.PathPlanId);
            Ignore(p => p.Place);

            ToTable("PlanItem");
        }
    }


    public class PlanActionConfiguration : EntityTypeConfiguration<PlanAction>
    {

        public PlanActionConfiguration()
            : base()
        {
            HasKey(p => p.PlanActionId);
            Property(p => p.ActionType).HasColumnType("varchar").HasMaxLength(1024).IsRequired();
            Property(p => p.GameAction).HasColumnType("varbinary(max)").IsRequired();
            Property(p => p.SequenceNumber).HasColumnType("int").IsRequired();
            HasRequired(p => p.PlanItem).WithMany(p => p.Actions).HasForeignKey(p => p.PlanItemId);

            ToTable("PlanAction");
        }
    }

#endregion
}
