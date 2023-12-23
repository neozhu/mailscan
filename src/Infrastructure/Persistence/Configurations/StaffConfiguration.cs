// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.Property(t => t.LastName).HasMaxLength(50).IsRequired();
        builder.HasOne(t => t.Department).WithMany(x => x.Staffs).HasForeignKey(x => x.DepartmentId);
        builder.Ignore(e => e.DomainEvents);
    }
}


