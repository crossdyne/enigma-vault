using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Shared.Kernel.Primitives;

namespace EnigmaVault.Secret.Domain.Models
{
    public sealed class Tag : AggregateRoot<TagId>
    {
        public UserId UserId { get; private set; }
        public TagName Name { get; private set; }
        public Color Color { get; private set; }

        private Tag() { }

        private Tag(TagId id, UserId userId, TagName name, Color color) : base(id)
        {
            UserId = userId;
            Name = name;
            Color = color;
        }

        public static Tag Create(Guid userId, string name, string color)
            => new(TagId.New(), UserId.Create(userId), TagName.Create(name),  Color.FromHex(color));

        public void UpdateName(TagName tagName)
        {
            if (Name != tagName)
                Name = tagName;
        }

        public void UpdateColor(Color color)
        {
            if (Color != color)
                Color = color;
        }
    }
}