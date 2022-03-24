using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourMoviesForum.Data.Models;

using static YourMoviesForum.Data.Common.DataValidation.Message;

namespace YourMoviesForum.Data.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> message)
        {
            message
                .Property(m => m.Content)
                .HasMaxLength(ContentMaxLength)
                .IsRequired();

            message
                .HasOne(m => m.Author)
                .WithMany(a => a.SentMessages)
                .HasForeignKey(m => m.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            message
                .HasOne(m => m.Receiver)
                .WithMany(a => a.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
  
 }
