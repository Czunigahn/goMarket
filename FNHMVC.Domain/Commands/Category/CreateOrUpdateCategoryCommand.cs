using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateCategoryCommand : ICommand
    {
        public CreateOrUpdateCategoryCommand(long CategoryId, string name, string description, bool activated, Category parent)
        {
            this.CategoryId = CategoryId;
            this.Name = name;
            this.Description = description;
            this.Activated = activated;
            this.Parent = parent;
        }

        public CreateOrUpdateCategoryCommand(Category category)
        {
            this.CategoryId = category.CategoryId;
            this.Name = category.Name;
            this.Description = category.Description;
            this.Parent = category.Parent;
        }

        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Activated { get; set; }
        public Category Parent { get; set; }
    }
}