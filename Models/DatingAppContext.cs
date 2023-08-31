using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Datingnew.Models;

public partial class DatingAppContext : DbContext
{
	public DatingAppContext()
	{
	}

	public DatingAppContext(DbContextOptions<DatingAppContext> options)
		: base(options)
	{
	}

	public virtual DbSet<User> Users { get; set; }
	public DbSet<UserLike> Likes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);


		builder.Entity<UserLike>()
			.HasKey(k => new { k.SourceId, k.TargetUserId });

		builder.Entity<UserLike>()
			.HasOne(s => s.SourceUser)
			.WithMany(l => l.LikedUsers)
			.HasForeignKey(s => s.SourceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<UserLike>()
			.HasOne(s => s.TargetUser)
			.WithMany(l => l.LikedByUsers)
			.HasForeignKey(s => s.TargetUserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
