﻿//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v5.9.</auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;

namespace recipesApp.EntityClasses
{
	/// <summary>Class which represents the entity 'Recipe'.</summary>
	public partial class Recipe : CommonEntityBase
	{
		/// <summary>Method called from the constructor</summary>
		partial void OnCreated();
		private System.Int64 _id = default(System.Int64);

		/// <summary>Initializes a new instance of the <see cref="Recipe"/> class.</summary>
		public Recipe() : base()
		{
			this.Ingredients = new List<Ingredient>();
			this.Instructions = new List<Instruction>();
			this.RecipeCategories = new List<RecipeCategory>();
			this.Title = string.Empty;
			OnCreated();
		}

		/// <summary>Gets the Id field. </summary>
		public System.Int64 Id => _id;
		/// <summary>Gets or sets the IsActive field. </summary>
		public System.Boolean IsActive { get; set; }
		/// <summary>Gets or sets the Title field. </summary>
		public System.String Title { get; set; }
		/// <summary>Represents the navigator which is mapped onto the association 'Ingredient.Recipe - Recipe.Ingredients (m:1)'</summary>
		public virtual List<Ingredient> Ingredients { get; set; }
		/// <summary>Represents the navigator which is mapped onto the association 'Instruction.Recipe - Recipe.Instructions (m:1)'</summary>
		public virtual List<Instruction> Instructions { get; set; }
		/// <summary>Represents the navigator which is mapped onto the association 'RecipeCategory.Recipe - Recipe.RecipeCategories (m:1)'</summary>
		public virtual List<RecipeCategory> RecipeCategories { get; set; }
	}
}
