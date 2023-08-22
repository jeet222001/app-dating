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

}
