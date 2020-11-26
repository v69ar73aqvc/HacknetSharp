using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace HacknetSharp.Server.Common.Models
{
    public class FileModel : WorldMember<Guid>
    {
        public virtual SystemModel Owner { get; set; } = null!;
        public virtual string Path { get; set; } = null!;
        public virtual string Name { get; set; } = null!;
        public virtual string Content { get; set; } = null!;
        public virtual FileKind Kind { get; set; }

        public enum FileKind : byte
        {
            TextFile,
            BlobFile,
            ProgFile,
            Folder
        }

    [ModelBuilderCallback]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
#pragma warning disable 1591
        public static void ConfigureModel(ModelBuilder builder)
        {
            builder.Entity<FileModel>(x =>
            {
                x.HasKey(v => v.Key);
                x.HasOne(x => x.Owner).WithMany(x => x.Files);
            });
        }
#pragma warning restore 1591
    }
}